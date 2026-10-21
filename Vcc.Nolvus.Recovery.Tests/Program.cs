using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Vcc.Nolvus.Recovery;

var tests = new (string Name, Action Body)[]
{
    ("valid XML", ValidXml),
    ("malformed XML", MalformedXml),
    ("truncated XML", TruncatedXml),
    ("all-NUL XML", AllNullXml),
    ("wrong XML root", WrongRootXml),
    ("unexpected root child", UnexpectedRootChild),
    ("DTD and XXE rejection", DtdAndXxeRejection),
    ("missing required instance nodes", MissingRequiredNodes),
    ("duplicate required instance nodes", DuplicateRequiredNodes),
    ("invalid required instance values", InvalidRequiredValues),
    ("malformed optional status fields", MalformedOptionalStatusFields),
    ("backup candidate discovery", BackupCandidateDiscovery),
    ("duplicate and out-of-order log lines", DuplicateAndOutOfOrderLogLines),
    ("exact concurrent tail semantics", ExactConcurrentTailSemantics),
    ("folder zero detection", FolderZeroDetection),
    ("dry-run no mutation", DryRunNoMutation),
    ("reconstruction refuses output inside inspected root", ReconstructionRefusesOutputInsideRoot),
    ("reconstruction refuses reparse-point output parent", ReconstructionRefusesReparsePointOutputParent),
    ("reconstruction refuses reparse-point inspected root", ReconstructionRefusesReparsePointInspectedRoot),
    ("reconstruction refuses existing output", ReconstructionRefusesExistingOutput),
    ("reconstruction refuses malformed or incomplete profile", ReconstructionRefusesMalformedOrIncompleteProfile),
    ("reconstruction refuses inconsistent stock game path", ReconstructionRefusesInconsistentStockGamePath),
    ("reconstruction refuses unpinned or misbounded evidence", ReconstructionRefusesUnpinnedOrMisboundedEvidence),
    ("reconstruction refuses unsafe checkpoints", ReconstructionRefusesUnsafeCheckpoints),
    ("reconstruction refuses invalid manifest and totals", ReconstructionRefusesInvalidManifestAndTotals),
    ("reconstruction requires single-instance confirmation for damaged ledger", ReconstructionRequiresSingleInstanceConfirmationForDamagedLedger),
    ("reconstruction preserves exact multi-instance inventory", ReconstructionPreservesExactMultiInstanceInventory),
    ("reconstruction refuses ledger inventory mismatch", ReconstructionRefusesLedgerInventoryMismatch),
    ("reconstruction enforces root-bound instance paths", ReconstructionEnforcesRootBoundInstancePaths),
    ("reconstruction adopts exact successful names only", ReconstructionAdoptsExactSuccessfulNamesOnly),
    ("reconstruction keeps queued minus completed for retry", ReconstructionKeepsQueuedMinusCompletedForRetry),
    ("reconstruction escapes XML special characters", ReconstructionEscapesXmlSpecialCharacters),
    ("reconstruction is deterministic and preserves inspected root", ReconstructionIsDeterministicAndPreservesInspectedRoot),
    ("reconstruction audit records provenance and safety", ReconstructionAuditRecordsProvenanceAndSafety),
    ("CLI inspect report safety", CliInspectReportSafety),
    ("CLI reconstruct dispatch", CliReconstructDispatch),
    ("CLI argument handling", CliArgumentHandling)
};

var failures = 0;
var skipped = 0;
foreach (var test in tests)
{
    try
    {
        test.Body();
        Console.WriteLine($"PASS {test.Name}");
    }
    catch (SkipTestException exception)
    {
        skipped++;
        Console.WriteLine($"SKIP {test.Name}: {exception.Message}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.Error.WriteLine($"FAIL {test.Name}: {exception.Message}");
    }
}

Console.WriteLine($"{tests.Length - failures - skipped} passed, {skipped} skipped, {failures} failed.");
return failures == 0 ? 0 : 1;

static void ValidXml()
{
    using var fixture = new Fixture();
    fixture.WriteLive(CompleteXml());

    var report = Inspect(fixture);
    var live = Live(report);
    Equal(XmlCandidateStatus.Valid, live.Status);
    Equal(1, live.InstanceCount);
    True(live.Sha256 is { Length: 64 }, "A valid candidate should include a SHA-256 hash.");
}

static void MalformedXml()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<Instances><Instance></Instances>");
    Equal(XmlCandidateStatus.MalformedXml, Live(Inspect(fixture)).Status);
}

static void TruncatedXml()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<Instances><Instance>");
    Equal(XmlCandidateStatus.Truncated, Live(Inspect(fixture)).Status);
}

static void AllNullXml()
{
    using var fixture = new Fixture();
    fixture.WriteLive(new byte[122_275]);
    var live = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.AllNull, live.Status);
    Equal(122_275L, live.Size);
}

static void WrongRootXml()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<NotInstances />");
    Equal(XmlCandidateStatus.WrongRoot, Live(Inspect(fixture)).Status);
}

static void UnexpectedRootChild()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<Instances><Surprise /></Instances>");
    Equal(XmlCandidateStatus.InvalidStructure, Live(Inspect(fixture)).Status);
}

static void DtdAndXxeRejection()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<!DOCTYPE Instances [<!ENTITY xxe SYSTEM 'file:///C:/Windows/win.ini'>]><Instances>&xxe;</Instances>");
    Equal(XmlCandidateStatus.DtdProhibited, Live(Inspect(fixture)).Status);
}

static void MissingRequiredNodes()
{
    using var fixture = new Fixture();
    fixture.WriteLive("<Instances><Instance><Name>Incomplete</Name></Instance></Instances>");
    var live = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.MissingRequiredNodes, live.Status);
    True(live.Evidence.Any(x => x.Contains("'Id'", StringComparison.Ordinal)), "Missing Id was not reported.");
    True(live.Evidence.Any(x => x.Contains("'Status'", StringComparison.Ordinal)), "Missing Status was not reported.");
}

static void DuplicateRequiredNodes()
{
    using var fixture = new Fixture();
    var xml = CompleteXml().Replace("<Id>test-id</Id>", "<Id>test-id</Id><Id>duplicate</Id>", StringComparison.Ordinal);
    fixture.WriteLive(xml);

    var live = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.MissingRequiredNodes, live.Status);
    True(live.Evidence.Any(x => x.Contains("2 copies", StringComparison.Ordinal)), "Duplicate required node was not reported.");
}

static void InvalidRequiredValues()
{
    using var fixture = new Fixture();
    var xml = CompleteXml()
        .Replace("<EnableArchiving>false</EnableArchiving>", "<EnableArchiving>sometimes</EnableArchiving>", StringComparison.Ordinal)
        .Replace("<TotalMods>3600</TotalMods>", "<TotalMods>999999</TotalMods>", StringComparison.Ordinal)
        .Replace("<InstallStatus>Installing</InstallStatus>", "<InstallStatus>Unknown</InstallStatus>", StringComparison.Ordinal);
    fixture.WriteLive(xml);

    var live = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.InvalidRequiredValues, live.Status);
    Equal(3, live.Evidence.Count);

    fixture.WriteLive(CompleteXml().Replace(
        "<TotalMods>3600</TotalMods>",
        "<TotalMods>-1</TotalMods>",
        StringComparison.Ordinal));
    var negative = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.InvalidRequiredValues, negative.Status);
    True(
        negative.Evidence.Any(item => item.Contains("must not be negative", StringComparison.Ordinal)),
        "A negative TotalMods value was not rejected.");
}

static void MalformedOptionalStatusFields()
{
    using var fixture = new Fixture();
    var xml = CompleteXml().Replace(
        "</InstalledMods></Status>",
        "</InstalledMods><Fields><Field><Key>Only a key</Key></Field></Fields></Status>",
        StringComparison.Ordinal);
    fixture.WriteLive(xml);

    var live = Live(Inspect(fixture));
    Equal(XmlCandidateStatus.MissingRequiredNodes, live.Status);
    True(live.Evidence.Any(x => x.Contains("'Value'", StringComparison.Ordinal)), "Missing Field/Value was not reported.");
}

static void BackupCandidateDiscovery()
{
    using var fixture = new Fixture();
    fixture.WriteLive(new byte[16]);
    fixture.WriteInstanceFile("InstancesData.backup.1.xml", CompleteXml());
    fixture.WriteInstanceFile("unrelated.xml", CompleteXml());

    var report = Inspect(fixture);
    Equal(3, report.Candidates.Count);
    var backup = report.Candidates.Single(x => x.Kind == CandidateKind.Backup);
    Equal(XmlCandidateStatus.Valid, backup.Status);
}

static void DuplicateAndOutOfOrderLogLines()
{
    using var fixture = new Fixture();
    fixture.WriteLog(
        "Mod : Alpha added to queue.",
        "Mod : Alpha added to queue.",
        "Mod : Beta installed.",
        "Mod : Beta added to queue.",
        "Mod : Alpha installed.",
        "Mod : Alpha installed.",
        "Mod : Gamma added to queue.",
        "mod : Wrong Case added to queue.",
        "Mod : Wrong suffix Installed.");

    var log = Inspect(fixture).Log;
    SequenceEqual(new[] { "Alpha", "Beta", "Gamma" }, log.Queued);
    SequenceEqual(new[] { "Beta", "Alpha" }, log.Completed);
    SequenceEqual(new[] { "Gamma" }, log.InFlight);
}

static void ExactConcurrentTailSemantics()
{
    const int completedCount = 8;
    const int inFlightCount = 32;
    using var fixture = new Fixture();
    var lines = new List<string>();

    for (var index = 1; index <= completedCount + inFlightCount; index++)
    {
        lines.Add($"Mod : Mod {index:D2} added to queue.");
    }

    for (var index = completedCount; index >= 1; index--)
    {
        lines.Add($"Mod : Mod {index:D2} installed.");
    }

    fixture.WriteLog(lines.ToArray());
    var log = Inspect(fixture).Log;
    Equal(inFlightCount, log.InFlight.Count);
    Equal("Mod 09", log.InFlight[0]);
    Equal("Mod 40", log.InFlight[^1]);
    True(log.InFlight.All(name => !log.Completed.Contains(name, StringComparer.Ordinal)), "In-flight entries must never be completed.");
}

static void FolderZeroDetection()
{
    using var fixture = new Fixture();
    var mods = fixture.CreateModsRoot();
    Directory.CreateDirectory(Path.Combine(mods, "Empty Mod"));

    var zero = Directory.CreateDirectory(Path.Combine(mods, "Zero Mod")).FullName;
    File.WriteAllBytes(Path.Combine(zero, "zero.dds"), new byte[32_768]);

    var present = Directory.CreateDirectory(Path.Combine(mods, "Present Mod")).FullName;
    File.WriteAllBytes(Path.Combine(present, "data.bin"), new byte[] { 0, 0, 1, 0 });

    var report = Inspect(fixture, true).Mods;
    Equal(ModFolderStatus.Empty, report.Folders.Single(x => x.Name == "Empty Mod").Status);
    Equal(ModFolderStatus.CorruptSampledAllNull, report.Folders.Single(x => x.Name == "Zero Mod").Status);
    Equal(ModFolderStatus.Present, report.Folders.Single(x => x.Name == "Present Mod").Status);
    Equal(2, report.SuspiciousCount);
}

static void DryRunNoMutation()
{
    using var fixture = new Fixture();
    fixture.WriteLive(CompleteXml());
    fixture.WriteInstanceFile("InstancesData.temp.xml", CompleteXml());
    fixture.WriteInstanceFile("InstancesData.backup.xml", CompleteXml());
    fixture.WriteLog("Mod : Tail added to queue.");
    var mods = fixture.CreateModsRoot();
    var mod = Directory.CreateDirectory(Path.Combine(mods, "Sample Mod")).FullName;
    File.WriteAllBytes(Path.Combine(mod, "sample.bin"), new byte[] { 1, 2, 3 });

    var before = Snapshot(fixture.Root);
    _ = RecoveryReportFormatter.Format(Inspect(fixture, true));
    var after = Snapshot(fixture.Root);

    Equal(before.Count, after.Count);
    foreach (var (path, fingerprint) in before)
    {
        True(after.TryGetValue(path, out var actual), $"Inspection removed '{path}'.");
        Equal(fingerprint, actual);
    }
}

static void ReconstructionRefusesOutputInsideRoot()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var before = Snapshot(fixture.Root);

    var insideXml = Path.Combine(fixture.Root, "recovered.xml");
    var outsideAudit = Path.Combine(fixture.OutputDirectory, "inside-xml-audit.json");
    Refused(() => Reconstruct(fixture, profile, insideXml, outsideAudit));
    False(File.Exists(insideXml), "An XML output inside the inspected root was created.");
    False(File.Exists(outsideAudit), "An audit was created for a refused XML output path.");

    var outsideXml = Path.Combine(fixture.OutputDirectory, "inside-audit.xml");
    var insideAudit = Path.Combine(fixture.Root, "recovery-audit.json");
    Refused(() => Reconstruct(fixture, profile, outsideXml, insideAudit));
    False(File.Exists(outsideXml), "XML was created when the audit path was inside the inspected root.");
    False(File.Exists(insideAudit), "An audit output inside the inspected root was created.");
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionRefusesReparsePointOutputParent()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var redirectedTarget = Path.Combine(fixture.Root, "redirected-output-target");
    var linkedOutput = Path.Combine(fixture.WorkspaceRoot, "linked-output");
    Directory.CreateDirectory(redirectedTarget);

    if (!TryCreateDirectoryLink(linkedOutput, redirectedTarget, out var skipReason))
    {
        throw new SkipTestException(skipReason);
    }

    try
    {
        var before = Snapshot(fixture.Root);
        var linkedXml = Path.Combine(linkedOutput, "candidate.xml");
        var outsideAudit = Path.Combine(fixture.OutputDirectory, "linked-output-audit.json");
        var xmlRefusal = Refused(() => Reconstruct(fixture, profile, linkedXml, outsideAudit));
        True(xmlRefusal.Message.Contains("reparse point", StringComparison.OrdinalIgnoreCase), "Linked XML parent was not refused as a reparse-point traversal.");
        False(File.Exists(Path.Combine(redirectedTarget, "candidate.xml")), "XML was written through a reparse-point parent into the inspected root.");
        False(File.Exists(outsideAudit), "Audit was created after the linked XML parent was refused.");

        var outsideXml = Path.Combine(fixture.OutputDirectory, "linked-audit.xml");
        var linkedAudit = Path.Combine(linkedOutput, "audit.json");
        var auditRefusal = Refused(() => Reconstruct(fixture, profile, outsideXml, linkedAudit));
        True(auditRefusal.Message.Contains("reparse point", StringComparison.OrdinalIgnoreCase), "Linked audit parent was not refused as a reparse-point traversal.");
        False(File.Exists(outsideXml), "XML was created after the linked audit parent was refused.");
        False(File.Exists(Path.Combine(redirectedTarget, "audit.json")), "Audit was written through a reparse-point parent into the inspected root.");
        SnapshotEqual(before, Snapshot(fixture.Root));
    }
    finally
    {
        DeleteDirectoryLink(linkedOutput);
    }
}

static void ReconstructionRefusesReparsePointInspectedRoot()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var linkedRoot = Path.Combine(fixture.WorkspaceRoot, "linked-inspected-root");

    if (!TryCreateDirectoryLink(linkedRoot, fixture.Root, out var skipReason))
    {
        throw new SkipTestException(skipReason);
    }

    try
    {
        var before = Snapshot(fixture.Root);
        var xml = Path.Combine(fixture.OutputDirectory, "linked-root.xml");
        var audit = Path.Combine(fixture.OutputDirectory, "linked-root.json");
        var refusal = Refused(() => InstancesDataReconstructor.Reconstruct(
            new ReconstructionRequest(linkedRoot, profile, xml, audit)));

        True(refusal.Message.Contains("reparse point", StringComparison.OrdinalIgnoreCase), "Linked inspected root was not refused as a reparse-point traversal.");
        False(File.Exists(xml), "XML was created for a reparse-point inspected root.");
        False(File.Exists(audit), "Audit was created for a reparse-point inspected root.");
        SnapshotEqual(before, Snapshot(fixture.Root));
    }
    finally
    {
        DeleteDirectoryLink(linkedRoot);
    }
}

static void ReconstructionRefusesExistingOutput()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var before = Snapshot(fixture.Root);

    var existingXml = Path.Combine(fixture.OutputDirectory, "existing.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "existing-xml-audit.json");
    File.WriteAllText(existingXml, "sentinel-xml", new UTF8Encoding(false));
    Refused(() => Reconstruct(fixture, profile, existingXml, audit));
    Equal("sentinel-xml", File.ReadAllText(existingXml));
    False(File.Exists(audit), "Audit output was created after an existing XML output was refused.");

    var xml = Path.Combine(fixture.OutputDirectory, "existing-audit.xml");
    var existingAudit = Path.Combine(fixture.OutputDirectory, "existing-audit.json");
    File.WriteAllText(existingAudit, "sentinel-audit", new UTF8Encoding(false));
    Refused(() => Reconstruct(fixture, profile, xml, existingAudit));
    False(File.Exists(xml), "XML output was created after an existing audit output was refused.");
    Equal("sentinel-audit", File.ReadAllText(existingAudit));
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionRefusesMalformedOrIncompleteProfile()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var before = Snapshot(fixture.Root);
    var complete = CompleteProfileJson(fixture);
    var duplicateConfirmed = complete.Replace(
        "\"confirmed\": true,",
        $"\"confirmed\": true,{Environment.NewLine}  \"confirmed\": true,",
        StringComparison.Ordinal);
    var cases = new (string Name, string Json, string? ExpectedMessage)[]
    {
        ("malformed", "{ this is not JSON", null),
        ("incomplete", "{\"profileVersion\":2,\"instance\":{\"id\":\"only-id\"}}", null),
        ("missing-evidence", MutateProfile(complete, root => root.Remove("evidence")), "evidence"),
        ("missing-ledger-safety", MutateProfile(complete, root => root.Remove("ledgerSafety")), "ledgerSafety"),
        ("missing-log-hash", MutateProfile(complete, root => root["evidence"]!.AsObject().Remove("logSha256")), "logSha256"),
        ("missing-session-start", MutateProfile(complete, root => root["evidence"]!.AsObject().Remove("sessionStartLine")), "sessionStartLine"),
        ("missing-session-end", MutateProfile(complete, root => root["evidence"]!.AsObject().Remove("sessionEndLine")), "sessionEndLine"),
        ("missing-instance-inventory", MutateProfile(complete, root => root["ledgerSafety"]!.AsObject().Remove("expectedInstanceIds")), "expectedInstanceIds"),
        ("missing-enable-archiving", MutateProfile(complete, root => root["settings"]!.AsObject().Remove("enableArchiving")), "enableArchiving"),
        ("missing-total-mods", MutateProfile(complete, root => root["status"]!.AsObject().Remove("totalMods")), "totalMods"),
        ("unconfirmed", MutateProfile(complete, root => root["confirmed"] = false), "confirmed: true"),
        ("version-mismatch", MutateProfile(complete, root => root["instance"]!.AsObject()["version"] = "6.0.16"), "instance.version"),
        ("unknown-key", MutateProfile(complete, root => root["unexpected"] = "not allowed"), "unexpected"),
        ("case-mismatched-key", MutateProfile(complete, root => root["Confirmed"] = true), "Confirmed"),
        ("duplicate-key", duplicateConfirmed, "duplicate property"),
        ("relative-install-path", MutateProfile(complete, root => root["instance"]!.AsObject()["installPath"] = "relative-install"), "instance.installPath"),
        ("negative-total-mods", MutateProfile(complete, root => root["status"]!.AsObject()["totalMods"] = -1), "status.totalMods"),
        ("oversized-total-mods", MutateProfile(complete, root => root["status"]!.AsObject()["totalMods"] = short.MaxValue + 1), "status.totalMods")
    };

    foreach (var testCase in cases)
    {
        var profile = fixture.WriteProfile($"{testCase.Name}.json", testCase.Json);
        var xml = Path.Combine(fixture.OutputDirectory, $"{testCase.Name}.xml");
        var audit = Path.Combine(fixture.OutputDirectory, $"{testCase.Name}.audit.json");
        var refusal = Refused(() => Reconstruct(fixture, profile, xml, audit));

        if (testCase.ExpectedMessage is not null)
        {
            True(
                refusal.Message.Contains(testCase.ExpectedMessage, StringComparison.OrdinalIgnoreCase),
                $"Profile case '{testCase.Name}' did not identify '{testCase.ExpectedMessage}': {refusal.Message}");
        }

        False(File.Exists(xml), $"Invalid profile case '{testCase.Name}' created XML output.");
        False(File.Exists(audit), $"Invalid profile case '{testCase.Name}' created audit output.");
    }

    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionRefusesInconsistentStockGamePath()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Confirmed added to queue.",
        "Mod : Confirmed installed.");
    var profile = fixture.WriteProfile(
        "inconsistent-stock-game.json",
        CompleteProfileJson(fixture, stockGame: Path.Combine(fixture.Root, "Different Stock Game")));
    var xml = Path.Combine(fixture.OutputDirectory, "inconsistent-stock-game.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "inconsistent-stock-game.json");
    var before = Snapshot(fixture.Root);

    var refusal = Refused(() => Reconstruct(fixture, profile, xml, audit));

    True(refusal.Message.Contains("instance.stockGame", StringComparison.Ordinal), "Inconsistent stockGame rejection did not identify the invalid profile field.");
    False(File.Exists(xml), "XML was created for an inconsistent stockGame path.");
    False(File.Exists(audit), "Audit was created for an inconsistent stockGame path.");
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionRefusesUnpinnedOrMisboundedEvidence()
{
    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, logSha256: new string('0', 64));
        AssertReconstructionRefused(fixture, "hash-mismatch", profile, "SHA-256");
    }

    using (var fixture = new Fixture())
    {
        fixture.WriteLog(
            "Start installing Wrong Instance - v 6.0.15...",
            "Mod : Alpha added to queue.",
            "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, sessionStartLine: 1);
        AssertReconstructionRefused(fixture, "wrong-marker", profile, "does not exactly match");
    }

    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, sessionEndLine: 2);
        AssertReconstructionRefused(fixture, "excluded-completion", profile, "completion checkpoint");
    }

    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, sessionEndLine: 4);
        AssertReconstructionRefused(fixture, "past-end-bound", profile, "has only 3 lines");
    }

    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(
            fixture,
            "Mod : Alpha added to queue.",
            "Mod : Alpha installed.",
            "Start installing Another Instance - v 6.0.15...");
        var profile = CompleteProfileJson(fixture);
        AssertReconstructionRefused(fixture, "crossed-session", profile, "crosses another install session marker");
    }
}

static void ReconstructionRefusesUnsafeCheckpoints()
{
    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(
            fixture,
            "Mod : Alpha added to queue.",
            "Mod : Alpha installed.",
            "Mod : Orphan installed.");
        var profile = CompleteProfileJson(fixture);
        AssertReconstructionRefused(fixture, "completed-not-queued", profile, "no matching queue checkpoint");
    }

    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, packageModNames: new[] { "Different Mod" });
        AssertReconstructionRefused(
            fixture,
            "checkpoint-not-allowlisted",
            profile,
            "not present in the reviewed exact-package mod manifest");
    }
}

static void ReconstructionRefusesInvalidManifestAndTotals()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
    var complete = CompleteProfileJson(fixture);
    var cases = new (string Name, string Json, string ExpectedMessage)[]
    {
        ("missing-manifest", MutateProfile(complete, root => root["evidence"]!.AsObject().Remove("packageModNames")), "packageModNames"),
        ("empty-manifest", MutateProfile(complete, root => root["evidence"]!.AsObject()["packageModNames"] = new JsonArray()), "must contain"),
        ("duplicate-manifest", MutateProfile(complete, root => root["evidence"]!.AsObject()["packageModNames"] = JsonNode.Parse("[\"Alpha\",\"Alpha\"]")), "duplicate exact name"),
        ("total-mismatch", MutateProfile(complete, root => root["status"]!.AsObject()["totalMods"] = 2), "exactly match")
    };

    foreach (var testCase in cases)
    {
        AssertReconstructionRefused(fixture, testCase.Name, testCase.Json, testCase.ExpectedMessage);
    }
}

static void ReconstructionRequiresSingleInstanceConfirmationForDamagedLedger()
{
    using (var fixture = new Fixture())
    {
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, singleInstanceOnlyConfirmed: false);
        AssertReconstructionRefused(fixture, "missing-ledger-unconfirmed", profile, "only the target instance");
    }

    using (var fixture = new Fixture())
    {
        fixture.WriteLive(new byte[128]);
        WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
        var profile = CompleteProfileJson(fixture, singleInstanceOnlyConfirmed: false);
        AssertReconstructionRefused(fixture, "corrupt-ledger-unconfirmed", profile, "only the target instance");
    }
}

static void ReconstructionPreservesExactMultiInstanceInventory()
{
    using var fixture = new Fixture();
    fixture.WriteLive(MultiInstanceXml(
        ("other-instance-id", "Other Instance"),
        ("recovered-instance-id", "Old Target")));
    WriteRecoveryLog(
        fixture,
        "Mod : Alpha added to queue.",
        "Mod : Retry added to queue.",
        "Mod : Alpha installed.");
    var profile = fixture.WriteProfile(
        "multi-instance-profile.json",
        CompleteProfileJson(
            fixture,
            singleInstanceOnlyConfirmed: false,
            expectedInstanceIds: new[] { "other-instance-id", "recovered-instance-id" }));
    var xml = Path.Combine(fixture.OutputDirectory, "multi-instance.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "multi-instance.audit.json");
    var livePath = Path.Combine(fixture.Root, "Instances", "InstancesData.xml");
    var liveDocument = XDocument.Load(livePath);
    var preservedBefore = new XElement(liveDocument.Root!.Elements("Instance").First());
    var before = Snapshot(fixture.Root);

    var result = Reconstruct(fixture, profile, xml, audit);

    var candidate = XDocument.Load(xml);
    var instances = candidate.Root!.Elements("Instance").ToArray();
    SequenceEqual(
        new[] { "other-instance-id", "recovered-instance-id" },
        instances.Select(instance => instance.Element("Id")!.Value).ToArray());
    True(XNode.DeepEquals(preservedBefore, instances[0]), "The non-target instance was not preserved exactly.");
    Equal("Recovered Instance", instances[1].Element("Name")?.Value);
    SequenceEqual(new[] { "Alpha" }, InstalledNames(xml, "recovered-instance-id"));
    SequenceEqual(new[] { "Retry" }, result.RetryMods);

    using var auditDocument = JsonDocument.Parse(File.ReadAllBytes(audit));
    var ledger = auditDocument.RootElement.GetProperty("ledger");
    Equal("Valid", ledger.GetProperty("liveLedgerStatus").GetString());
    Equal(FileSha256(livePath), ledger.GetProperty("liveLedgerSha256").GetString());
    SequenceEqual(
        new[] { "other-instance-id", "recovered-instance-id" },
        ledger.GetProperty("expectedInstanceIds").EnumerateArray().Select(value => value.GetString()!).ToArray());
    False(ledger.GetProperty("singleInstanceOnlyConfirmed").GetBoolean(), "Multi-instance recovery claimed single-instance confirmation.");
    False(ledger.GetProperty("singleInstanceEvidenceUsed").GetBoolean(), "Multi-instance recovery claimed damaged-ledger evidence.");
    Equal(1, ledger.GetProperty("preservedInstanceCount").GetInt32());
    Equal(2, auditDocument.RootElement.GetProperty("candidate").GetProperty("instanceCount").GetInt32());
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionRefusesLedgerInventoryMismatch()
{
    using var fixture = new Fixture();
    fixture.WriteLive(MultiInstanceXml(
        ("other-instance-id", "Other Instance"),
        ("recovered-instance-id", "Old Target")));
    WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
    var profile = CompleteProfileJson(
        fixture,
        singleInstanceOnlyConfirmed: false,
        expectedInstanceIds: new[] { "recovered-instance-id", "other-instance-id" });
    AssertReconstructionRefused(fixture, "ledger-inventory-mismatch", profile, "order/ids do not exactly match");
}

static void ReconstructionEnforcesRootBoundInstancePaths()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(fixture, "Mod : Alpha added to queue.", "Mod : Alpha installed.");
    var cases = new (string Name, string Profile, string ExpectedMessage)[]
    {
        ("install-outside-root", CompleteProfileJson(fixture, installPath: Path.Combine(fixture.WorkspaceRoot, "outside-install")), "instance.installPath"),
        ("archive-outside-root", CompleteProfileJson(fixture, archivePath: Path.Combine(fixture.WorkspaceRoot, "outside-archive")), "instance.archivePath")
    };

    foreach (var testCase in cases)
    {
        AssertReconstructionRefused(fixture, testCase.Name, testCase.Profile, testCase.ExpectedMessage);
    }
}

static void ReconstructionAdoptsExactSuccessfulNamesOnly()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Alpha added to queue.",
        "Mod : Folder Evidence added to queue.",
        "Mod : Beta added to queue.",
        "Mod : Alpha installed.",
        "Mod : Beta installed.",
        "Mod : Alpha installed.",
        "Mod : Folder Evidence removed from queue.",
        "Installing mod Generic Installer Text",
        "mod : Wrong Case installed.",
        "Mod : Wrong Suffix Installed.",
        "Mod : Trailing Text installed. ignored");
    var mods = fixture.CreateModsRoot();
    Directory.CreateDirectory(Path.Combine(mods, "Folder Evidence"));

    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var xml = Path.Combine(fixture.OutputDirectory, "success-only.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "success-only.json");
    var result = Reconstruct(fixture, profile, xml, audit);

    SequenceEqual(new[] { "Alpha", "Beta" }, result.CompletedMods);
    SequenceEqual(new[] { "Folder Evidence" }, result.RetryMods);
    SequenceEqual(new[] { "Alpha", "Beta" }, InstalledNames(xml));
    SequenceEqual(new[] { "Alpha", "Beta" }, AuditCheckpointNames(audit, "completed"));
    SequenceEqual(new[] { "Folder Evidence" }, AuditCheckpointNames(audit, "retry"));
}

static void ReconstructionKeepsQueuedMinusCompletedForRetry()
{
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        "Mod : Alpha added to queue.",
        "Mod : Beta added to queue.",
        "Mod : Alpha added to queue.",
        "Mod : Gamma added to queue.",
        "Mod : Delta installed.",
        "Mod : Beta installed.",
        "Mod : Delta added to queue.",
        "Mod : Gamma removed from queue.");

    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var xml = Path.Combine(fixture.OutputDirectory, "retry.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "retry.json");
    var result = Reconstruct(fixture, profile, xml, audit);

    SequenceEqual(new[] { "Delta", "Beta" }, result.CompletedMods);
    SequenceEqual(new[] { "Alpha", "Gamma" }, result.RetryMods);
    SequenceEqual(result.CompletedMods, InstalledNames(xml));
    SequenceEqual(new[] { "Alpha", "Gamma" }, AuditCheckpointNames(audit, "retry"));
}

static void ReconstructionEscapesXmlSpecialCharacters()
{
    const string special = "Ampersand & less < greater > quote \" apostrophe '";
    const string specialMod = "Mod & <Patch> \"Quoted\" 'Apostrophe'";
    using var fixture = new Fixture();
    WriteRecoveryLog(
        fixture,
        $"Mod : {specialMod} added to queue.",
        $"Mod : {specialMod} installed.");
    var profile = fixture.WriteProfile(
        "special-profile.json",
        CompleteProfileJson(fixture, description: special, fieldKey: special, fieldValue: special));
    var xml = Path.Combine(fixture.OutputDirectory, "special.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "special.json");

    Reconstruct(fixture, profile, xml, audit);
    var document = XDocument.Load(xml, LoadOptions.PreserveWhitespace);
    Equal("Recovered Instance", RequiredInstance(document).Element("Name")?.Value);
    Equal(special, RequiredInstance(document).Element("Description")?.Value);
    Equal(special, RequiredInstance(document).Element("Status")?.Element("Fields")?.Element("Field")?.Element("Key")?.Value);
    SequenceEqual(new[] { specialMod }, InstalledNames(xml));

    var raw = File.ReadAllText(xml);
    True(raw.Contains("&amp;", StringComparison.Ordinal), "Ampersand was not escaped in reconstructed XML.");
    True(raw.Contains("&lt;", StringComparison.Ordinal), "Less-than was not escaped in reconstructed XML.");
    True(raw.Contains("&gt;", StringComparison.Ordinal), "Greater-than was not escaped in reconstructed XML.");
}

static void ReconstructionIsDeterministicAndPreservesInspectedRoot()
{
    using var fixture = new Fixture();
    fixture.WriteLive(new byte[122_275]);
    WriteRecoveryLog(
        fixture,
        "Mod : First added to queue.",
        "Mod : Second added to queue.",
        "Mod : Second installed.");
    var profile = fixture.WriteProfile("valid-profile.json", CompleteProfileJson(fixture));
    var before = Snapshot(fixture.Root);

    var firstRoot = Path.Combine(fixture.OutputDirectory, "first");
    var secondRoot = Path.Combine(fixture.OutputDirectory, "second");
    Directory.CreateDirectory(Path.Combine(firstRoot, "Instances"));
    Directory.CreateDirectory(Path.Combine(secondRoot, "Instances"));
    var firstXml = Path.Combine(firstRoot, "Instances", "InstancesData.xml");
    var secondXml = Path.Combine(secondRoot, "Instances", "InstancesData.xml");

    var first = Reconstruct(fixture, profile, firstXml, Path.Combine(firstRoot, "audit.json"));
    var second = Reconstruct(fixture, profile, secondXml, Path.Combine(secondRoot, "audit.json"));

    True(File.ReadAllBytes(firstXml).SequenceEqual(File.ReadAllBytes(secondXml)), "Equivalent reconstruction inputs produced different XML bytes.");
    Equal(XmlCandidateStatus.Valid, first.Candidate.Status);
    Equal(XmlCandidateStatus.Valid, second.Candidate.Status);
    Equal(XmlCandidateStatus.Valid, Live(new RecoveryInspector().Inspect(firstRoot)).Status);
    var reconstructedStatus = RequiredInstance(XDocument.Load(firstXml)).Element("Status");
    Equal("Installing", reconstructedStatus?.Element("InstallStatus")?.Value);
    Equal("2", reconstructedStatus?.Element("TotalMods")?.Value);
    SequenceEqual(new[] { "Second" }, InstalledNames(firstXml));
    SequenceEqual(new[] { "First" }, first.RetryMods);
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void ReconstructionAuditRecordsProvenanceAndSafety()
{
    using var fixture = new Fixture();
    fixture.WriteLive(new byte[122_275]);
    WriteRecoveryLog(
        fixture,
        "Mod : Completed added to queue.",
        "Mod : Retry added to queue.",
        "Mod : Completed installed.");
    var profile = fixture.WriteProfile("audit-profile.json", CompleteProfileJson(fixture));
    var xml = Path.Combine(fixture.OutputDirectory, "audit-candidate.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "audit-provenance.json");
    var log = Path.Combine(fixture.Root, "Log.txt");
    var before = Snapshot(fixture.Root);

    var result = Reconstruct(fixture, profile, xml, audit);

    using var document = JsonDocument.Parse(File.ReadAllBytes(audit));
    var root = document.RootElement;
    Equal(2, root.GetProperty("auditFormatVersion").GetInt32());

    var source = root.GetProperty("source");
    Equal(Path.GetFullPath(fixture.Root), source.GetProperty("inspectedRootPath").GetString());
    Equal(Path.GetFullPath(log), source.GetProperty("logPath").GetString());
    Equal(FileSha256(log), source.GetProperty("logSha256").GetString());
    Equal(4L, source.GetProperty("logLines").GetInt64());
    Equal(1L, source.GetProperty("sessionStartLine").GetInt64());
    Equal(4L, source.GetProperty("sessionEndLine").GetInt64());
    Equal("Start installing Recovered Instance - v 6.0.15...", source.GetProperty("sessionStartMarker").GetString());
    Equal(Path.GetFullPath(profile), source.GetProperty("profilePath").GetString());
    Equal(FileSha256(profile), source.GetProperty("profileSha256").GetString());
    Equal(2, source.GetProperty("profileVersion").GetInt32());
    True(source.GetProperty("confirmed").GetBoolean(), "Audit did not record a confirmed profile.");
    Equal("6.0.15", source.GetProperty("packageVersion").GetString());
    Equal(
        Convert.ToHexString(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(new[] { "Completed", "Retry" }))),
        source.GetProperty("packageManifestSha256").GetString());
    Equal(2, source.GetProperty("packageManifestCount").GetInt32());

    var ledger = root.GetProperty("ledger");
    Equal(Path.Combine(fixture.Root, "Instances", "InstancesData.xml"), ledger.GetProperty("liveLedgerPath").GetString());
    Equal("AllNull", ledger.GetProperty("liveLedgerStatus").GetString());
    Equal(FileSha256(Path.Combine(fixture.Root, "Instances", "InstancesData.xml")), ledger.GetProperty("liveLedgerSha256").GetString());
    SequenceEqual(
        new[] { "recovered-instance-id" },
        ledger.GetProperty("expectedInstanceIds").EnumerateArray().Select(value => value.GetString()!).ToArray());
    True(ledger.GetProperty("singleInstanceOnlyConfirmed").GetBoolean(), "Audit omitted single-instance confirmation.");
    True(ledger.GetProperty("singleInstanceEvidenceUsed").GetBoolean(), "Audit did not record damaged-ledger evidence use.");
    Equal(0, ledger.GetProperty("preservedInstanceCount").GetInt32());

    var candidate = root.GetProperty("candidate");
    Equal(Path.GetFullPath(xml), candidate.GetProperty("outputXmlPath").GetString());
    Equal(FileSha256(xml), candidate.GetProperty("sha256").GetString());
    Equal(new FileInfo(xml).Length, candidate.GetProperty("size").GetInt64());
    Equal(result.Candidate.Sha256, candidate.GetProperty("sha256").GetString());
    Equal(result.Candidate.Size, candidate.GetProperty("size").GetInt64());
    Equal(1, candidate.GetProperty("instanceCount").GetInt32());
    Equal("recovered-instance-id", candidate.GetProperty("instanceId").GetString());
    Equal("6.0.15", candidate.GetProperty("instanceVersion").GetString());
    Equal("Installing", candidate.GetProperty("installStatus").GetString());
    Equal(2, candidate.GetProperty("totalMods").GetInt32());

    var checkpoints = root.GetProperty("checkpoints");
    Equal("hash-pinned-session-range-plus-reviewed-package-allowlist", checkpoints.GetProperty("policy").GetString());
    False(checkpoints.GetProperty("folderEvidenceUsedForCompletion").GetBoolean(), "Audit claimed folder evidence was used for completion.");
    True(checkpoints.GetProperty("completedMustBeQueuedInSession").GetBoolean(), "Audit omitted the completion/queue invariant.");
    Equal(2, checkpoints.GetProperty("queuedCount").GetInt32());
    Equal(1, checkpoints.GetProperty("completedCount").GetInt32());
    Equal(1, checkpoints.GetProperty("retryCount").GetInt32());
    SequenceEqual(new[] { "Completed", "Retry" }, AuditCheckpointNames(audit, "queued"));
    SequenceEqual(new[] { "Completed" }, AuditCheckpointNames(audit, "completed"));
    SequenceEqual(new[] { "Retry" }, AuditCheckpointNames(audit, "retry"));

    var safety = root.GetProperty("safety");
    False(safety.GetProperty("activationPerformed").GetBoolean(), "Audit claimed activation was performed.");
    False(safety.GetProperty("inspectedRootWritten").GetBoolean(), "Audit claimed the inspected root was written.");
    True(safety.GetProperty("createNewNoOverwrite").GetBoolean(), "Audit did not record CreateNew/no-overwrite safety.");
    True(safety.GetProperty("strictSelfParseValidated").GetBoolean(), "Audit did not record strict self-parse validation.");
    True(safety.GetProperty("writtenCandidateHashAndLengthReverified").GetBoolean(), "Audit did not record written-byte hash/length re-verification.");
    True(safety.GetProperty("multiInstanceSafetyChecked").GetBoolean(), "Audit did not record multi-instance safety validation.");
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void CliInspectReportSafety()
{
    using var fixture = new Fixture();
    fixture.WriteLive(CompleteXml());
    fixture.WriteLog("Mod : Confirmed installed.");
    var before = Snapshot(fixture.Root);

    var insideReport = Path.Combine(fixture.Root, "inspection-report.txt");
    var inside = RunCli("inspect", "--root", fixture.Root, "--report", insideReport);
    Equal(1, inside.ExitCode);
    True(inside.StandardError.Contains("Recovery command refused", StringComparison.Ordinal), "CLI did not standardize the inside-root report refusal.");
    True(inside.StandardError.Contains("outside the inspected", StringComparison.Ordinal), "CLI did not explain the report safety boundary.");
    False(File.Exists(insideReport), "CLI wrote an inspection report inside the inspected root.");

    var existingReport = Path.Combine(fixture.OutputDirectory, "existing-report.txt");
    File.WriteAllText(existingReport, "sentinel-report", new UTF8Encoding(false));
    var existing = RunCli("inspect", "--root", fixture.Root, "--report", existingReport);
    Equal(1, existing.ExitCode);
    True(existing.StandardError.Contains("Recovery command refused", StringComparison.Ordinal), "CLI did not standardize the existing-report refusal.");
    Equal("sentinel-report", File.ReadAllText(existingReport));
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void CliReconstructDispatch()
{
    using var fixture = new Fixture();
    fixture.WriteLive(new byte[122_275]);
    WriteRecoveryLog(
        fixture,
        "Mod : Exact Success added to queue.",
        "Mod : Retry Me added to queue.",
        "Mod : Exact Success installed.");
    var profile = fixture.WriteProfile("cli-profile.json", CompleteProfileJson(fixture));
    var xml = Path.Combine(fixture.OutputDirectory, "cli-candidate.xml");
    var audit = Path.Combine(fixture.OutputDirectory, "cli-audit.json");
    var before = Snapshot(fixture.Root);

    var success = RunCli(
        "reconstruct",
        "--root", fixture.Root,
        "--profile", profile,
        "--output", xml,
        "--audit", audit);

    Equal(0, success.ExitCode);
    Equal(string.Empty, success.StandardError);
    True(success.StandardOutput.Contains("Candidate status: Valid", StringComparison.Ordinal), "CLI did not report a valid candidate.");
    True(success.StandardOutput.Contains("Exact completed checkpoints adopted: 1", StringComparison.Ordinal), "CLI reported the wrong completed count.");
    True(success.StandardOutput.Contains("Queued entries retained for retry: 1", StringComparison.Ordinal), "CLI reported the wrong retry count.");
    True(success.StandardOutput.Contains("Legacy TotalMods preserved: 2", StringComparison.Ordinal), "CLI did not report the manifest-aligned TotalMods value.");
    SequenceEqual(new[] { "Exact Success" }, InstalledNames(xml));
    SequenceEqual(new[] { "Retry Me" }, AuditCheckpointNames(audit, "retry"));

    var insideXml = Path.Combine(fixture.Root, "forbidden-candidate.xml");
    var insideAudit = Path.Combine(fixture.Root, "forbidden-audit.json");
    var refused = RunCli(
        "reconstruct",
        "--root", fixture.Root,
        "--profile", profile,
        "--output", insideXml,
        "--audit", insideAudit);

    Equal(1, refused.ExitCode);
    True(refused.StandardError.Contains("Recovery command refused", StringComparison.Ordinal), "CLI did not standardize InvalidOperationException path refusal.");
    False(refused.StandardError.Contains("Unhandled exception", StringComparison.OrdinalIgnoreCase), "CLI leaked an unhandled exception for a safety refusal.");
    False(File.Exists(insideXml), "CLI created a forbidden candidate inside the inspected root.");
    False(File.Exists(insideAudit), "CLI created a forbidden audit inside the inspected root.");
    SnapshotEqual(before, Snapshot(fixture.Root));
}

static void CliArgumentHandling()
{
    var noArguments = RunCli();
    Equal(2, noArguments.ExitCode);
    True(noArguments.StandardError.Contains("Usage:", StringComparison.Ordinal), "No-argument invocation did not print usage.");

    var unknown = RunCli("unknown-command");
    Equal(2, unknown.ExitCode);
    True(unknown.StandardError.Contains("Unknown command", StringComparison.Ordinal), "Unknown command was not diagnosed.");

    var missingValue = RunCli("inspect", "--root");
    Equal(2, missingValue.ExitCode);
    True(missingValue.StandardError.Contains("--root requires a value", StringComparison.Ordinal), "Missing inspect value was not diagnosed.");

    var optionInsteadOfValue = RunCli("inspect", "--root", "--audit-mods");
    Equal(2, optionInsteadOfValue.ExitCode);
    True(optionInsteadOfValue.StandardError.Contains("--root requires a value", StringComparison.Ordinal), "An option token was incorrectly accepted as the preceding option's value.");

    var duplicateInspect = RunCli("inspect", "--root", "first", "--root", "second");
    Equal(2, duplicateInspect.ExitCode);
    True(duplicateInspect.StandardError.Contains("Duplicate option: --root", StringComparison.Ordinal), "Duplicate inspect option was not rejected.");

    var duplicateReconstruct = RunCli(
        "reconstruct",
        "--root", "first",
        "--root", "second",
        "--profile", "profile.json",
        "--output", "candidate.xml",
        "--audit", "audit.json");
    Equal(2, duplicateReconstruct.ExitCode);
    True(duplicateReconstruct.StandardError.Contains("Duplicate option: --root", StringComparison.Ordinal), "Duplicate reconstruct option was not rejected.");

    var help = RunCli("--help");
    Equal(0, help.ExitCode);
    True(help.StandardError.Contains("Usage:", StringComparison.Ordinal), "Help did not print usage.");
}

static CliProcessResult RunCli(params string[] arguments)
{
    var cliAssembly = FindCliAssembly();
    var startInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
    };
    startInfo.ArgumentList.Add(cliAssembly);
    foreach (var argument in arguments)
    {
        startInfo.ArgumentList.Add(argument);
    }

    using var process = Process.Start(startInfo)
        ?? throw new InvalidOperationException("Could not start the recovery CLI subprocess.");
    var standardOutput = process.StandardOutput.ReadToEndAsync();
    var standardError = process.StandardError.ReadToEndAsync();
    if (!process.WaitForExit(30_000))
    {
        process.Kill(entireProcessTree: true);
        throw new TimeoutException("Recovery CLI subprocess did not exit within 30 seconds.");
    }

    process.WaitForExit();
    return new CliProcessResult(
        process.ExitCode,
        standardOutput.GetAwaiter().GetResult(),
        standardError.GetAwaiter().GetResult());
}

static string FindCliAssembly()
{
    var targetFrameworkDirectory = new DirectoryInfo(AppContext.BaseDirectory);
    var configurationDirectory = targetFrameworkDirectory.Parent
        ?? throw new InvalidOperationException("Could not resolve the test configuration directory.");
    var repositoryRoot = configurationDirectory.Parent?.Parent?.Parent
        ?? throw new InvalidOperationException("Could not resolve the recovery repository root.");
    var path = Path.Combine(
        repositoryRoot.FullName,
        "Vcc.Nolvus.Recovery.Cli",
        "bin",
        configurationDirectory.Name,
        targetFrameworkDirectory.Name,
        "Vcc.Nolvus.Recovery.Cli.dll");
    if (!File.Exists(path))
    {
        throw new FileNotFoundException("The recovery CLI was not built for subprocess testing.", path);
    }

    return path;
}

static ReconstructionResult Reconstruct(Fixture fixture, string profile, string xml, string audit) =>
    InstancesDataReconstructor.Reconstruct(new ReconstructionRequest(fixture.Root, profile, xml, audit));

static IReadOnlyList<string> InstalledNames(string path, string? instanceId = null)
{
    var document = XDocument.Load(path);
    var instance = instanceId is null
        ? RequiredInstance(document)
        : document.Root?.Elements("Instance").Single(element =>
            string.Equals(element.Element("Id")?.Value, instanceId, StringComparison.Ordinal))
          ?? throw new InvalidOperationException($"Reconstructed XML has no instance '{instanceId}'.");
    return instance
        .Element("Status")?
        .Element("InstalledMods")?
        .Elements("InstalledMod")
        .Select(element => element.Value)
        .ToArray()
    ?? Array.Empty<string>();
}

static IReadOnlyList<string> AuditCheckpointNames(string path, string propertyName)
{
    using var document = JsonDocument.Parse(File.ReadAllBytes(path));
    return document.RootElement
        .GetProperty("checkpoints")
        .GetProperty(propertyName)
        .EnumerateArray()
        .Select(element => element.GetString() ?? throw new InvalidDataException($"Audit '{propertyName}' contains null."))
        .ToArray();
}

static XElement RequiredInstance(XDocument document) =>
    document.Root?.Element("Instance")
    ?? throw new InvalidOperationException("Reconstructed XML has no Instances/Instance element.");

static string MutateProfile(string json, Action<JsonObject> mutation)
{
    var root = JsonNode.Parse(json) as JsonObject
        ?? throw new InvalidOperationException("Complete recovery profile fixture is not a JSON object.");
    mutation(root);
    return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
}

static string CompleteProfileJson(
    Fixture fixture,
    string instanceName = "Recovered Instance",
    string description = "Recovery test fixture",
    string fieldKey = "Phase",
    string fieldValue = "Packages",
    string? stockGame = null,
    string? installPath = null,
    string? archivePath = null,
    IReadOnlyList<string>? packageModNames = null,
    int? totalMods = null,
    bool singleInstanceOnlyConfirmed = true,
    IReadOnlyList<string>? expectedInstanceIds = null,
    long? sessionStartLine = null,
    long? sessionEndLine = null,
    string? logSha256 = null)
{
    const string packageVersion = "6.0.15";
    const string instanceId = "recovered-instance-id";
    var logPath = Path.Combine(fixture.Root, "Log.txt");
    var logLines = File.ReadAllLines(logPath);
    var sessionMarker = $"Start installing {instanceName} - v {packageVersion}...";
    var detectedStart = Array.FindIndex(
        logLines,
        line => string.Equals(line, sessionMarker, StringComparison.Ordinal)) + 1;
    var manifest = packageModNames?.ToArray() ?? ExactCheckpointNames(logLines).ToArray();
    var instanceDirectory = installPath ?? Path.Combine(fixture.Root, "Instances", instanceName);

    return JsonSerializer.Serialize(
        new
        {
            profileVersion = 2,
            confirmed = true,
            packageVersion,
            evidence = new
            {
                logSha256 = logSha256 ?? FileSha256(logPath),
                sessionStartLine = sessionStartLine ?? detectedStart,
                sessionEndLine = sessionEndLine ?? logLines.LongLength,
                packageModNames = manifest
            },
            ledgerSafety = new
            {
                singleInstanceOnlyConfirmed,
                expectedInstanceIds = expectedInstanceIds?.ToArray() ?? new[] { instanceId }
            },
            instance = new
            {
                id = instanceId,
                name = instanceName,
                tag = string.Empty,
                description,
                version = packageVersion,
                installPath = instanceDirectory,
                archivePath = archivePath ?? Path.Combine(fixture.Root, "Instances", "ARCHIVE"),
                stockGame = stockGame ?? Path.Combine(instanceDirectory, "STOCK GAME")
            },
            settings = new
            {
                ratio = "16:9",
                height = "1080",
                width = "1920",
                enableArchiving = true,
                cdn = "Nexus CDN",
                lgCode = "EN",
                lgName = "English"
            },
            performance = new
            {
                advancedPhysics = "FALSE",
                downScaling = "FALSE",
                downHeight = "1080",
                downWidth = "1920",
                iniSettings = "1",
                antiAliasing = "TAA",
                variant = "Redux",
                lods = "Performance",
                rayTracing = "FALSE",
                fpsStabilizer = "FALSE",
                srex = "FALSE",
                frameGeneration = "FALSE"
            },
            options = new
            {
                nudity = "FALSE",
                alternateEnb = "PICHO",
                fantasyMode = "FALSE",
                hardcoreMode = "FALSE",
                alternateLeveling = "FALSE",
                skinType = "Smooth",
                alternateStart = "FALSE",
                combatAnimation = "Fantasy",
                stancesPerksTree = "TRUE",
                deleveledEnemies = "TRUE",
                exhaustion = "TRUE",
                nerfPa = "Player Only",
                enemiesResistance = "TRUE",
                boss = "TRUE",
                poise = "TRUE",
                gore = "FALSE",
                combatScaling = "Hard",
                controller = "FALSE",
                ui = "Untarnished UI"
            },
            status = new
            {
                totalMods = totalMods ?? manifest.Length,
                fields = new[] { new { key = fieldKey, value = fieldValue } }
            }
        },
        new JsonSerializerOptions { WriteIndented = true });
}

static void WriteRecoveryLog(Fixture fixture, params string[] checkpointLines)
{
    var lines = new[] { "Start installing Recovered Instance - v 6.0.15..." }
        .Concat(checkpointLines)
        .ToArray();
    fixture.WriteLog(lines);
}

static IReadOnlyList<string> ExactCheckpointNames(IEnumerable<string> lines)
{
    const string prefix = "Mod : ";
    var names = new List<string>();
    var seen = new HashSet<string>(StringComparer.Ordinal);

    foreach (var line in lines)
    {
        string? name = null;
        foreach (var suffix in new[] { " added to queue.", " installed." })
        {
            if (line.StartsWith(prefix, StringComparison.Ordinal) &&
                line.EndsWith(suffix, StringComparison.Ordinal) &&
                line.Length > prefix.Length + suffix.Length)
            {
                name = line.Substring(prefix.Length, line.Length - prefix.Length - suffix.Length);
                break;
            }
        }

        if (name is not null && seen.Add(name))
        {
            names.Add(name);
        }
    }

    return names;
}

static void SnapshotEqual(
    IReadOnlyDictionary<string, string> expected,
    IReadOnlyDictionary<string, string> actual)
{
    Equal(expected.Count, actual.Count);
    foreach (var (path, fingerprint) in expected)
    {
        True(actual.TryGetValue(path, out var actualFingerprint), $"Inspected root lost '{path}'.");
        Equal(fingerprint, actualFingerprint);
    }
}

static Exception Refused(Action action)
{
    try
    {
        action();
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidDataException or InvalidOperationException or IOException or UnauthorizedAccessException)
    {
        return exception;
    }

    throw new InvalidOperationException("Expected reconstruction to refuse the request.");
}

static Exception AssertReconstructionRefused(
    Fixture fixture,
    string caseName,
    string profileJson,
    string expectedMessage)
{
    var profile = fixture.WriteProfile($"{caseName}.profile.json", profileJson);
    var xml = Path.Combine(fixture.OutputDirectory, $"{caseName}.xml");
    var audit = Path.Combine(fixture.OutputDirectory, $"{caseName}.audit.json");
    var before = Snapshot(fixture.Root);
    var refusal = Refused(() => Reconstruct(fixture, profile, xml, audit));

    True(
        refusal.Message.Contains(expectedMessage, StringComparison.OrdinalIgnoreCase),
        $"Refusal case '{caseName}' did not identify '{expectedMessage}': {refusal.Message}");
    False(File.Exists(xml), $"Refusal case '{caseName}' created an XML candidate.");
    False(File.Exists(audit), $"Refusal case '{caseName}' created an audit.");
    SnapshotEqual(before, Snapshot(fixture.Root));
    return refusal;
}

static bool TryCreateDirectoryLink(string linkPath, string targetPath, out string skipReason)
{
    try
    {
        Directory.CreateSymbolicLink(linkPath, targetPath);
        RequireReparsePoint(linkPath);
        skipReason = string.Empty;
        return true;
    }
    catch (PlatformNotSupportedException exception)
    {
        skipReason = exception.Message;
        return false;
    }
    catch (UnauthorizedAccessException exception)
    {
        return TryCreateWindowsJunction(linkPath, targetPath, exception.Message, out skipReason);
    }
    catch (IOException exception) when (IsLinkPrivilegeFailure(exception))
    {
        return TryCreateWindowsJunction(linkPath, targetPath, exception.Message, out skipReason);
    }
}

static bool TryCreateWindowsJunction(
    string linkPath,
    string targetPath,
    string symbolicLinkFailure,
    out string skipReason)
{
    if (!OperatingSystem.IsWindows())
    {
        skipReason = symbolicLinkFailure;
        return false;
    }

    using var process = new Process
    {
        StartInfo = new ProcessStartInfo("cmd.exe")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        }
    };
    process.StartInfo.ArgumentList.Add("/d");
    process.StartInfo.ArgumentList.Add("/c");
    process.StartInfo.ArgumentList.Add("mklink");
    process.StartInfo.ArgumentList.Add("/J");
    process.StartInfo.ArgumentList.Add(linkPath);
    process.StartInfo.ArgumentList.Add(targetPath);

    True(process.Start(), "Failed to start cmd.exe for junction creation.");
    var standardOutput = process.StandardOutput.ReadToEnd();
    var standardError = process.StandardError.ReadToEnd();
    True(process.WaitForExit(10_000), "Timed out while creating a test junction.");
    if (process.ExitCode != 0)
    {
        throw new InvalidOperationException(
            $"Could not create the test junction after symbolic-link creation failed: {symbolicLinkFailure} " +
            $"mklink exit {process.ExitCode}: {standardError}{standardOutput}");
    }

    RequireReparsePoint(linkPath);
    skipReason = string.Empty;
    return true;
}

static bool IsLinkPrivilegeFailure(IOException exception)
{
    const int errorAccessDenied = 5;
    const int errorPrivilegeNotHeld = 1314;
    var errorCode = exception.HResult & 0xffff;
    return errorCode is errorAccessDenied or errorPrivilegeNotHeld;
}

static void RequireReparsePoint(string linkPath)
{
    var info = new DirectoryInfo(linkPath);
    info.Refresh();
    True(info.Exists, $"Test link '{linkPath}' was not created.");
    True((info.Attributes & FileAttributes.ReparsePoint) != 0, "The test link was created without the reparse-point attribute.");
}

static void DeleteDirectoryLink(string linkPath)
{
    var info = new DirectoryInfo(linkPath);
    info.Refresh();
    if (!info.Exists)
    {
        return;
    }

    True((info.Attributes & FileAttributes.ReparsePoint) != 0, $"Refusing to delete non-link test directory '{linkPath}'.");
    Directory.Delete(linkPath, recursive: false);
}

static void False(bool condition, string message) => True(!condition, message);

static RecoveryInspectionReport Inspect(Fixture fixture, bool auditMods = false) =>
    new RecoveryInspector().Inspect(fixture.Root, new RecoveryInspectionOptions { AuditMods = auditMods });

static XmlCandidateReport Live(RecoveryInspectionReport report) =>
    report.Candidates.Single(x => x.Kind == CandidateKind.Live);

static string FileSha256(string path) =>
    Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)));

static SortedDictionary<string, string> Snapshot(string root)
{
    var snapshot = new SortedDictionary<string, string>(StringComparer.Ordinal);
    var rootInfo = new DirectoryInfo(root);
    snapshot["D:."] = $"{rootInfo.CreationTimeUtc.Ticks}:{rootInfo.LastWriteTimeUtc.Ticks}:{(int)rootInfo.Attributes}";

    foreach (var directory in Directory.GetDirectories(root, "*", SearchOption.AllDirectories).OrderBy(x => x, StringComparer.Ordinal))
    {
        var info = new DirectoryInfo(directory);
        snapshot[$"D:{Path.GetRelativePath(root, directory)}"] = $"{info.CreationTimeUtc.Ticks}:{info.LastWriteTimeUtc.Ticks}:{(int)info.Attributes}";
    }

    foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories).OrderBy(x => x, StringComparer.Ordinal))
    {
        var info = new FileInfo(file);
        var hash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file)));
        snapshot[$"F:{Path.GetRelativePath(root, file)}"] = $"{info.Length}:{info.CreationTimeUtc.Ticks}:{info.LastWriteTimeUtc.Ticks}:{(int)info.Attributes}:{hash}";
    }

    return snapshot;
}

static string CompleteXml() =>
    """
    <?xml version="1.0" encoding="utf-8"?>
    <Instances>
      <Instance>
        <Id>test-id</Id><Name>Test</Name><Description>Fixture</Description><Version>6.0</Version>
        <InstallPath>C:\Nolvus\Instances\Test</InstallPath><ArchivePath>C:\Nolvus\Instances\ARCHIVE</ArchivePath><StockGame>C:\Stock</StockGame>
        <Settings><Ratio>16:9</Ratio><Height>1080</Height><Width>1920</Width><EnableArchiving>false</EnableArchiving><CDN>cdn</CDN><LgCode>en</LgCode><LgName>English</LgName></Settings>
        <Performance><AdvancedPhysics>FALSE</AdvancedPhysics><DownScaling>FALSE</DownScaling><DownHeight>1080</DownHeight><DownWidth>1920</DownWidth><IniSettings>Medium</IniSettings><AntiAliasing>TAA</AntiAliasing><Variant>Redux</Variant><LODs>Medium</LODs><RayTracing>FALSE</RayTracing></Performance>
        <Options><Nudity>FALSE</Nudity><AlternateENB>PICHO</AlternateENB><FantasyMode>FALSE</FantasyMode><HardcoreMode>FALSE</HardcoreMode><AlternateLeveling>FALSE</AlternateLeveling><SkinType>Default</SkinType></Options>
        <Status><TotalMods>3600</TotalMods><InstallStatus>Installing</InstallStatus><InstalledMods><InstalledMod>First Mod</InstalledMod></InstalledMods></Status>
      </Instance>
    </Instances>
    """;

static string MultiInstanceXml(params (string Id, string Name)[] instances)
{
    var template = XDocument.Parse(CompleteXml()).Root!.Element("Instance")!;
    var root = new XElement(
        "Instances",
        instances.Select(specification =>
        {
            var instance = new XElement(template);
            instance.Element("Id")!.Value = specification.Id;
            instance.Element("Name")!.Value = specification.Name;
            instance.Element("Description")!.Value = $"Fixture for {specification.Name}";
            return instance;
        }));
    return new XDocument(new XDeclaration("1.0", "utf-8", null), root).ToString();
}

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
    }
}

static void SequenceEqual(IReadOnlyList<string> expected, IReadOnlyList<string> actual)
{
    if (!expected.SequenceEqual(actual, StringComparer.Ordinal))
    {
        throw new InvalidOperationException($"Expected [{string.Join(", ", expected)}], got [{string.Join(", ", actual)}].");
    }
}

static void True(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

internal sealed class SkipTestException : Exception
{
    public SkipTestException(string message)
        : base(message)
    {
    }
}

internal sealed class Fixture : IDisposable
{
    public Fixture()
    {
        WorkspaceRoot = Path.Combine(Path.GetTempPath(), "NolvusRecoveryTests", Guid.NewGuid().ToString("N"));
        Root = Path.Combine(WorkspaceRoot, "inspected");
        OutputDirectory = Path.Combine(WorkspaceRoot, "output");
        Directory.CreateDirectory(Path.Combine(Root, "Instances"));
        Directory.CreateDirectory(OutputDirectory);
    }

    public string WorkspaceRoot { get; }

    public string Root { get; }

    public string OutputDirectory { get; }

    public void WriteLive(string text) => WriteInstanceFile("InstancesData.xml", text);

    public void WriteLive(byte[] bytes) => File.WriteAllBytes(Path.Combine(Root, "Instances", "InstancesData.xml"), bytes);

    public void WriteInstanceFile(string name, string text) =>
        File.WriteAllText(Path.Combine(Root, "Instances", name), text, new UTF8Encoding(false));

    public void WriteLog(params string[] lines) =>
        File.WriteAllLines(Path.Combine(Root, "Log.txt"), lines, new UTF8Encoding(false));

    public string WriteProfile(string name, string json)
    {
        var path = Path.Combine(WorkspaceRoot, name);
        File.WriteAllText(path, json, new UTF8Encoding(false));
        return path;
    }

    public string CreateModsRoot()
    {
        var path = Path.Combine(Root, "Instances", "Test Instance", "MODS", "mods");
        Directory.CreateDirectory(path);
        return path;
    }

    public void Dispose()
    {
        if (Directory.Exists(WorkspaceRoot))
        {
            Directory.Delete(WorkspaceRoot, true);
        }
    }
}


internal sealed record CliProcessResult(
    int ExitCode,
    string StandardOutput,
    string StandardError);
