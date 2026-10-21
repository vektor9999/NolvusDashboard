using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace Vcc.Nolvus.Recovery;

public sealed record ReconstructionRequest(
    string InspectedRootPath,
    string ProfilePath,
    string OutputXmlPath,
    string AuditJsonPath);

public sealed record ReconstructionResult(
    string RootPath,
    string ProfilePath,
    string OutputXmlPath,
    string AuditJsonPath,
    string ProfileSha256,
    string LogSha256,
    long LogLines,
    int TotalMods,
    IReadOnlyList<string> QueuedMods,
    IReadOnlyList<string> CompletedMods,
    IReadOnlyList<string> RetryMods,
    XmlCandidateReport Candidate,
    IReadOnlyList<string> Evidence);

public static class InstancesDataReconstructor
{
    private const int MaximumProfileBytes = 4 * 1024 * 1024;
    private const int MaximumLogBytes = 128 * 1024 * 1024;

    private static readonly JsonSerializerOptions AuditSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private sealed record LedgerPlan(
        string LiveLedgerPath,
        XmlCandidateStatus LiveLedgerStatus,
        string? LiveLedgerSha256,
        IReadOnlyList<XElement> ExistingInstances,
        IReadOnlyList<string> InstanceIds,
        bool SingleInstanceEvidenceUsed)
    {
        public int PreservedInstanceCount =>
            ExistingInstances.Count == 0 ? 0 : ExistingInstances.Count - 1;
    }

    public static ReconstructionResult Reconstruct(ReconstructionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rootPath = NormalizeRequiredPath(request.InspectedRootPath, nameof(request.InspectedRootPath));
        var profilePath = NormalizeRequiredPath(request.ProfilePath, nameof(request.ProfilePath));
        var outputXmlPath = NormalizeRequiredPath(request.OutputXmlPath, nameof(request.OutputXmlPath));
        var auditJsonPath = NormalizeRequiredPath(request.AuditJsonPath, nameof(request.AuditJsonPath));

        ValidatePaths(rootPath, profilePath, outputXmlPath, auditJsonPath);

        var profileBytes = ReadFileWithLimit(profilePath, MaximumProfileBytes, "Recovery profile");
        var profile = ReconstructionProfileLoader.Parse(profileBytes);
        ValidateProfileConsistency(profile, rootPath);
        var profileSha256 = Sha256(profileBytes);
        var packageManifestSha256 = Sha256(
            JsonSerializer.SerializeToUtf8Bytes(profile.Evidence.PackageModNames));

        var logPath = Path.Combine(rootPath, "Log.txt");
        var logBytesBefore = ReadFileWithLimit(logPath, MaximumLogBytes, "Log.txt");
        var logSha256 = Sha256(logBytesBefore);
        if (!string.Equals(logSha256, profile.Evidence.LogSha256, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "Log.txt SHA-256 does not match evidence.logSha256; no candidate was written.");
        }

        var expectedSessionMarker =
            $"Start installing {profile.Instance.Name} - v {profile.PackageVersion}...";
        var log = LogInspector.Inspect(
            logPath,
            profile.Evidence.SessionStartLine,
            profile.Evidence.SessionEndLine,
            expectedSessionMarker);
        var logBytesAfter = ReadFileWithLimit(logPath, MaximumLogBytes, "Log.txt");

        if (!logBytesBefore.AsSpan().SequenceEqual(logBytesAfter))
        {
            throw new IOException("Log.txt changed while it was being inspected; no candidate was written.");
        }

        ValidateLogInspection(log, profile);
        var ledgerPlan = PrepareLedgerPlan(rootPath, profile);
        var candidateBytes = BuildCandidate(profile, log.Completed, ledgerPlan);
        ValidateCandidateRoundTrip(candidateBytes, profile, log.Completed, ledgerPlan);
        var candidateSha256 = Sha256(candidateBytes);

        var candidateCreated = false;
        var auditCreated = false;

        try
        {
            RejectReparsePointTraversals(rootPath, outputXmlPath, auditJsonPath);
            WriteCreateNew(outputXmlPath, candidateBytes, ref candidateCreated);

            var candidateReport = XmlCandidateInspector.Inspect(CandidateKind.Backup, outputXmlPath);
            if (!candidateReport.IsValid)
            {
                throw new InvalidDataException(
                    $"The generated candidate failed the independent XML inspection with status '{candidateReport.Status}'.");
            }

            if (candidateReport.Size != candidateBytes.LongLength ||
                !string.Equals(candidateReport.Sha256, candidateSha256, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "The written candidate bytes do not match the exact generated length and SHA-256.");
            }

            var auditBytes = BuildAudit(
                rootPath,
                profilePath,
                outputXmlPath,
                logPath,
                profile,
                profileSha256,
                logSha256,
                packageManifestSha256,
                candidateSha256,
                candidateBytes.LongLength,
                expectedSessionMarker,
                ledgerPlan,
                log);
            RejectReparsePointTraversals(rootPath, outputXmlPath, auditJsonPath);
            WriteCreateNew(auditJsonPath, auditBytes, ref auditCreated);

            return new ReconstructionResult(
                rootPath,
                profilePath,
                outputXmlPath,
                auditJsonPath,
                profileSha256,
                logSha256,
                log.LinesRead,
                profile.Status.TotalMods,
                log.Queued,
                log.Completed,
                log.InFlight,
                candidateReport,
                new[]
                {
                    "InstalledMods contains only exact, ordered completion checkpoints from the reviewed, hash-pinned session range.",
                    "Every queued and completed name was allowlisted by the reviewed exact-package manifest; every completed name was queued in-session.",
                    "RetryMods is the ordered set difference queued minus completed; mod folders were not used as completion evidence.",
                    ledgerPlan.SingleInstanceEvidenceUsed
                        ? "The damaged or missing live ledger was accepted only because the profile explicitly confirms a single-instance root."
                        : $"The valid live ledger inventory was verified and {ledgerPlan.PreservedInstanceCount} non-target instance record(s) were preserved.",
                    "InstallStatus is fixed to Installing. TotalMods exactly matches the reviewed package manifest count.",
                    "The candidate and audit were created outside the inspected root with CreateNew."
                });
        }
        catch
        {
            if (auditCreated)
            {
                TryDeleteCreatedArtifact(auditJsonPath);
            }

            if (candidateCreated)
            {
                TryDeleteCreatedArtifact(outputXmlPath);
            }

            throw;
        }
    }

    private static void ValidatePaths(
        string rootPath,
        string profilePath,
        string outputXmlPath,
        string auditJsonPath)
    {
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"Inspected root does not exist: {rootPath}");
        }

        if (!File.Exists(profilePath))
        {
            throw new FileNotFoundException("Recovery profile does not exist.", profilePath);
        }

        if (PathsEqual(outputXmlPath, auditJsonPath))
        {
            throw new ArgumentException("The XML candidate and JSON audit paths must be different.");
        }

        if (IsWithin(rootPath, outputXmlPath))
        {
            throw new InvalidOperationException("The XML candidate path must be outside the inspected root.");
        }

        if (IsWithin(rootPath, auditJsonPath))
        {
            throw new InvalidOperationException("The JSON audit path must be outside the inspected root.");
        }

        if (!string.Equals(Path.GetExtension(outputXmlPath), ".xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The XML candidate path must use the .xml extension.");
        }

        if (!string.Equals(Path.GetExtension(auditJsonPath), ".json", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The audit path must use the .json extension.");
        }

        RequireExistingParent(outputXmlPath, "XML candidate");
        RequireExistingParent(auditJsonPath, "JSON audit");
        RejectReparsePointTraversals(rootPath, outputXmlPath, auditJsonPath);

        if (File.Exists(outputXmlPath) || Directory.Exists(outputXmlPath))
        {
            throw new IOException($"XML candidate path already exists: {outputXmlPath}");
        }

        if (File.Exists(auditJsonPath) || Directory.Exists(auditJsonPath))
        {
            throw new IOException($"JSON audit path already exists: {auditJsonPath}");
        }
    }

    private static void ValidateLogInspection(
        LogInspectionReport log,
        ReconstructionProfile profile)
    {
        if (!log.Exists)
        {
            throw new FileNotFoundException("Log.txt does not exist.", log.Path);
        }

        if (log.Evidence.Any(item => item.StartsWith("Log read stopped:", StringComparison.Ordinal)))
        {
            throw new IOException("Log.txt could not be read completely; no candidate was written.");
        }

        if (log.LinesRead == 0)
        {
            throw new InvalidDataException("Log.txt is empty; there are no recovery checkpoints.");
        }

        if (log.Queued.Count == 0 || log.Completed.Count == 0)
        {
            throw new InvalidDataException(
                "The reviewed session range must contain at least one exact queue checkpoint and one exact completion checkpoint.");
        }

        var allowedNames = new HashSet<string>(
            profile.Evidence.PackageModNames,
            StringComparer.Ordinal);
        var queuedNames = new HashSet<string>(log.Queued, StringComparer.Ordinal);

        foreach (var name in log.Queued.Concat(log.Completed))
        {
            if (name.Length == 0 || !string.Equals(name, name.Trim(), StringComparison.Ordinal))
            {
                throw new InvalidDataException("A log checkpoint contains a blank or boundary-whitespace mod name.");
            }

            try
            {
                XmlConvert.VerifyXmlChars(name);
            }
            catch (XmlException exception)
            {
                throw new InvalidDataException("A log checkpoint contains characters that cannot be represented in XML.", exception);
            }

            if (!allowedNames.Contains(name))
            {
                throw new InvalidDataException(
                    $"Log checkpoint '{name}' is not present in the reviewed exact-package mod manifest.");
            }
        }

        foreach (var name in log.Completed)
        {
            if (!queuedNames.Contains(name))
            {
                throw new InvalidDataException(
                    $"Completed checkpoint '{name}' has no matching queue checkpoint inside the reviewed session range.");
            }
        }
    }

    private static LedgerPlan PrepareLedgerPlan(
        string rootPath,
        ReconstructionProfile profile)
    {
        var liveLedgerPath = Path.Combine(rootPath, "Instances", "InstancesData.xml");
        byte[]? liveBytesBefore = null;
        byte[]? liveBytesAfter = null;

        if (File.Exists(liveLedgerPath))
        {
            liveBytesBefore = ReadFileWithLimit(
                liveLedgerPath,
                64 * 1024 * 1024,
                "Live InstancesData.xml");
        }

        var liveReport = XmlCandidateInspector.Inspect(CandidateKind.Live, liveLedgerPath);

        if (File.Exists(liveLedgerPath))
        {
            liveBytesAfter = ReadFileWithLimit(
                liveLedgerPath,
                64 * 1024 * 1024,
                "Live InstancesData.xml");
        }

        if (liveBytesBefore is not null || liveBytesAfter is not null)
        {
            if (liveBytesBefore is null ||
                liveBytesAfter is null ||
                !liveBytesBefore.AsSpan().SequenceEqual(liveBytesAfter))
            {
                throw new IOException(
                    "InstancesData.xml changed while its instance inventory was being inspected.");
            }
        }

        if (liveReport.Status == XmlCandidateStatus.IoError)
        {
            throw new IOException(
                "InstancesData.xml could not be read safely; multi-instance safety cannot be established.");
        }

        if (!liveReport.IsValid)
        {
            if (!profile.LedgerSafety.SingleInstanceOnlyConfirmed ||
                profile.LedgerSafety.ExpectedInstanceIds.Count != 1 ||
                !string.Equals(
                    profile.LedgerSafety.ExpectedInstanceIds[0],
                    profile.Instance.Id,
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Live InstancesData.xml is '{liveReport.Status}'. " +
                    "Reconstruction fails closed unless the reviewed profile explicitly confirms " +
                    "that this root contains only the target instance.");
            }

            return new LedgerPlan(
                liveLedgerPath,
                liveReport.Status,
                liveReport.Sha256,
                Array.Empty<XElement>(),
                profile.LedgerSafety.ExpectedInstanceIds.ToArray(),
                SingleInstanceEvidenceUsed: true);
        }

        if (liveBytesBefore is null)
        {
            throw new InvalidDataException(
                "The live ledger was reported valid but no stable bytes were available.");
        }

        if (liveReport.Size != liveBytesBefore.LongLength ||
            !string.Equals(liveReport.Sha256, Sha256(liveBytesBefore), StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "The live ledger report does not match the exact stable bytes that were reviewed.");
        }

        var document = LoadDocument(liveBytesBefore, "Live InstancesData.xml");
        var instances = document.Root!.Elements("Instance")
            .Select(instance => new XElement(instance))
            .ToArray();
        var instanceIds = instances
            .Select(instance => instance.Element("Id")!.Value.Trim())
            .ToArray();

        if (instanceIds.Distinct(StringComparer.Ordinal).Count() != instanceIds.Length)
        {
            throw new InvalidDataException(
                "Live InstancesData.xml contains duplicate instance ids.");
        }

        if (!instanceIds.SequenceEqual(
                profile.LedgerSafety.ExpectedInstanceIds,
                StringComparer.Ordinal))
        {
            throw new InvalidDataException(
                "The valid live ledger instance order/ids do not exactly match " +
                "ledgerSafety.expectedInstanceIds.");
        }

        if (instanceIds.Count(id => string.Equals(
                id,
                profile.Instance.Id,
                StringComparison.Ordinal)) != 1)
        {
            throw new InvalidDataException(
                "The valid live ledger must contain exactly one instance matching instance.id.");
        }

        return new LedgerPlan(
            liveLedgerPath,
            liveReport.Status,
            liveReport.Sha256,
            instances,
            instanceIds,
            SingleInstanceEvidenceUsed: false);
    }

    private static XDocument LoadDocument(byte[] bytes, string description)
    {
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = 64L * 1024L * 1024L,
            IgnoreWhitespace = true
        };

        try
        {
            using var input = new MemoryStream(bytes, writable: false);
            using var reader = XmlReader.Create(input, settings);
            return XDocument.Load(reader, LoadOptions.None);
        }
        catch (XmlException exception)
        {
            throw new InvalidDataException($"{description} changed validity during reconstruction.", exception);
        }
    }

    private static byte[] BuildCandidate(
        ReconstructionProfile profile,
        IReadOnlyList<string> completedMods,
        LedgerPlan ledgerPlan)
    {
        var recoveredInstance = BuildRecoveredInstance(profile, completedMods);
        IReadOnlyList<XElement> candidateInstances;

        if (ledgerPlan.ExistingInstances.Count == 0)
        {
            candidateInstances = new[] { recoveredInstance };
        }
        else
        {
            candidateInstances = ledgerPlan.ExistingInstances
                .Select(instance =>
                    string.Equals(
                        instance.Element("Id")!.Value.Trim(),
                        profile.Instance.Id,
                        StringComparison.Ordinal)
                        ? recoveredInstance
                        : new XElement(instance))
                .ToArray();
        }

        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XComment("Warning DO NOT MODIFY this file yourself, you will break your installation!!!"),
            new XElement("Instances", candidateInstances));

        using var output = new MemoryStream();
        var writerSettings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true),
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            CloseOutput = false
        };

        using (var writer = XmlWriter.Create(output, writerSettings))
        {
            document.Save(writer);
        }

        return output.ToArray();
    }

    private static XElement BuildRecoveredInstance(
        ReconstructionProfile profile,
        IReadOnlyList<string> completedMods)
    {
        var instance = profile.Instance;
        var settings = profile.Settings;
        var performance = profile.Performance;
        var options = profile.Options;
        var status = profile.Status;

        return new XElement("Instance",
            Element("Id", instance.Id),
            Element("Name", instance.Name),
            Element("Tag", instance.Tag),
            Element("Description", instance.Description),
            Element("Version", instance.Version),
            Element("InstallPath", instance.InstallPath),
            Element("ArchivePath", instance.ArchivePath),
            Element("StockGame", instance.StockGame),
            new XElement("Settings",
                Element("Ratio", settings.Ratio),
                Element("Height", settings.Height),
                Element("Width", settings.Width),
                Element("EnableArchiving", settings.EnableArchiving.ToString()),
                Element("CDN", settings.Cdn),
                Element("LgCode", settings.LgCode),
                Element("LgName", settings.LgName)),
            new XElement("Performance",
                Element("AdvancedPhysics", performance.AdvancedPhysics),
                Element("DownScaling", performance.DownScaling),
                Element("DownHeight", performance.DownHeight),
                Element("DownWidth", performance.DownWidth),
                Element("IniSettings", performance.IniSettings),
                Element("AntiAliasing", performance.AntiAliasing),
                Element("Variant", performance.Variant),
                Element("LODs", performance.Lods),
                Element("RayTracing", performance.RayTracing),
                Element("FPSStabilizer", performance.FpsStabilizer),
                Element("SREX", performance.Srex),
                Element("FrameGeneration", performance.FrameGeneration)),
            new XElement("Options",
                Element("Nudity", options.Nudity),
                Element("AlternateENB", options.AlternateEnb),
                Element("FantasyMode", options.FantasyMode),
                Element("HardcoreMode", options.HardcoreMode),
                Element("AlternateLeveling", options.AlternateLeveling),
                Element("SkinType", options.SkinType),
                Element("AlternateStart", options.AlternateStart),
                Element("CombatAnimation", options.CombatAnimation),
                Element("StancesPerksTree", options.StancesPerksTree),
                Element("DeleveledEnemies", options.DeleveledEnemies),
                Element("Exhaustion", options.Exhaustion),
                Element("NerfPA", options.NerfPa),
                Element("EnemiesResistance", options.EnemiesResistance),
                Element("Boss", options.Boss),
                Element("Poise", options.Poise),
                Element("Gore", options.Gore),
                Element("CombatScaling", options.CombatScaling),
                Element("Controller", options.Controller),
                Element("UI", options.Ui)),
            new XElement("Status",
                Element("InstallStatus", "Installing"),
                Element("TotalMods", status.TotalMods.ToString(CultureInfo.InvariantCulture)),
                new XElement("InstalledMods",
                    completedMods.Select(name => Element("InstalledMod", name))),
                new XElement("Fields",
                    status.Fields.Select(field =>
                        new XElement("Field",
                            Element("Key", field.Key),
                            Element("Value", field.Value))))));
    }

    private static void ValidateCandidateRoundTrip(
        byte[] candidateBytes,
        ReconstructionProfile profile,
        IReadOnlyList<string> completedMods,
        LedgerPlan ledgerPlan)
    {
        XDocument document;
        var readerSettings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = 64L * 1024L * 1024L,
            IgnoreWhitespace = true
        };

        using (var input = new MemoryStream(candidateBytes, writable: false))
        using (var reader = XmlReader.Create(input, readerSettings))
        {
            document = XDocument.Load(reader, LoadOptions.None);
        }

        var root = document.Root
            ?? throw new InvalidDataException("Generated candidate has no root element.");
        RequireElementName(root, "Instances");
        if (root.HasAttributes || root.Elements().Any(element => element.Name != "Instance"))
        {
            throw new InvalidDataException("Generated Instances root has attributes or unexpected child elements.");
        }

        var instances = root.Elements("Instance").ToArray();
        if (instances.Length != ledgerPlan.InstanceIds.Count)
        {
            throw new InvalidDataException(
                "Generated candidate instance count does not match the reviewed ledger inventory.");
        }

        var actualIds = instances.Select(instance => instance.Element("Id")?.Value.Trim()).ToArray();
        if (actualIds.Any(string.IsNullOrEmpty) ||
            !actualIds.SequenceEqual(ledgerPlan.InstanceIds, StringComparer.Ordinal))
        {
            throw new InvalidDataException(
                "Generated candidate instance order/ids do not match the reviewed ledger inventory.");
        }

        var targetMatches = instances
            .Where(instance => string.Equals(
                instance.Element("Id")!.Value.Trim(),
                profile.Instance.Id,
                StringComparison.Ordinal))
            .ToArray();
        if (targetMatches.Length != 1)
        {
            throw new InvalidDataException("Generated candidate must contain exactly one recovered target instance.");
        }

        if (ledgerPlan.ExistingInstances.Count > 0)
        {
            for (var index = 0; index < ledgerPlan.ExistingInstances.Count; index++)
            {
                if (string.Equals(
                        ledgerPlan.InstanceIds[index],
                        profile.Instance.Id,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                if (!XNode.DeepEquals(ledgerPlan.ExistingInstances[index], instances[index]))
                {
                    throw new InvalidDataException(
                        $"Generated candidate modified preserved instance '{ledgerPlan.InstanceIds[index]}'.");
                }
            }
        }

        var instance = targetMatches[0];
        RequireExactChildren(
            instance,
            "Id", "Name", "Tag", "Description", "Version", "InstallPath", "ArchivePath", "StockGame",
            "Settings", "Performance", "Options", "Status");
        RequireExactChildren(
            instance.Element("Settings")!,
            "Ratio", "Height", "Width", "EnableArchiving", "CDN", "LgCode", "LgName");
        RequireExactChildren(
            instance.Element("Performance")!,
            "AdvancedPhysics", "DownScaling", "DownHeight", "DownWidth", "IniSettings", "AntiAliasing",
            "Variant", "LODs", "RayTracing", "FPSStabilizer", "SREX", "FrameGeneration");
        RequireExactChildren(
            instance.Element("Options")!,
            "Nudity", "AlternateENB", "FantasyMode", "HardcoreMode", "AlternateLeveling", "SkinType",
            "AlternateStart", "CombatAnimation", "StancesPerksTree", "DeleveledEnemies", "Exhaustion",
            "NerfPA", "EnemiesResistance", "Boss", "Poise", "Gore", "CombatScaling", "Controller", "UI");

        var status = instance.Element("Status")!;
        RequireExactChildren(status, "InstallStatus", "TotalMods", "InstalledMods", "Fields");
        if (!string.Equals(status.Element("InstallStatus")!.Value, "Installing", StringComparison.Ordinal))
        {
            throw new InvalidDataException("Generated candidate InstallStatus must be 'Installing'.");
        }

        if (!string.Equals(
                status.Element("TotalMods")!.Value,
                profile.Status.TotalMods.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal))
        {
            throw new InvalidDataException("Generated candidate TotalMods does not match the recovery profile.");
        }

        var installed = status.Element("InstalledMods")!.Elements("InstalledMod")
            .Select(element => element.Value)
            .ToArray();
        if (!installed.SequenceEqual(completedMods, StringComparer.Ordinal))
        {
            throw new InvalidDataException("Generated InstalledMods does not exactly match ordered success checkpoints.");
        }

        if (status.Element("InstalledMods")!.Elements().Any(element => element.Name != "InstalledMod"))
        {
            throw new InvalidDataException("Generated InstalledMods contains an unexpected element.");
        }

        var fields = status.Element("Fields")!.Elements("Field").ToArray();
        if (fields.Length != profile.Status.Fields.Count)
        {
            throw new InvalidDataException("Generated Fields count does not match the recovery profile.");
        }

        for (var index = 0; index < fields.Length; index++)
        {
            RequireExactChildren(fields[index], "Key", "Value");
            if (!string.Equals(fields[index].Element("Key")!.Value, profile.Status.Fields[index].Key, StringComparison.Ordinal) ||
                !string.Equals(fields[index].Element("Value")!.Value, profile.Status.Fields[index].Value, StringComparison.Ordinal))
            {
                throw new InvalidDataException($"Generated status Field[{index + 1}] does not match the recovery profile.");
            }
        }

        if (document.Descendants().Any(element => element.HasAttributes))
        {
            throw new InvalidDataException("Generated candidate unexpectedly contains XML attributes.");
        }
    }

    private static byte[] BuildAudit(
        string rootPath,
        string profilePath,
        string outputXmlPath,
        string logPath,
        ReconstructionProfile profile,
        string profileSha256,
        string logSha256,
        string packageManifestSha256,
        string candidateSha256,
        long candidateSize,
        string expectedSessionMarker,
        LedgerPlan ledgerPlan,
        LogInspectionReport log)
    {
        var audit = new
        {
            auditFormatVersion = 2,
            source = new
            {
                inspectedRootPath = rootPath,
                logPath,
                logSha256,
                logLines = log.LinesRead,
                sessionStartLine = profile.Evidence.SessionStartLine,
                sessionEndLine = profile.Evidence.SessionEndLine,
                sessionStartMarker = expectedSessionMarker,
                profilePath,
                profileSha256,
                profileVersion = profile.ProfileVersion,
                confirmed = profile.Confirmed,
                packageVersion = profile.PackageVersion,
                packageManifestSha256,
                packageManifestCount = profile.Evidence.PackageModNames.Count
            },
            ledger = new
            {
                liveLedgerPath = ledgerPlan.LiveLedgerPath,
                liveLedgerStatus = ledgerPlan.LiveLedgerStatus.ToString(),
                liveLedgerSha256 = ledgerPlan.LiveLedgerSha256,
                expectedInstanceIds = ledgerPlan.InstanceIds,
                singleInstanceOnlyConfirmed = profile.LedgerSafety.SingleInstanceOnlyConfirmed,
                singleInstanceEvidenceUsed = ledgerPlan.SingleInstanceEvidenceUsed,
                preservedInstanceCount = ledgerPlan.PreservedInstanceCount
            },
            candidate = new
            {
                outputXmlPath,
                sha256 = candidateSha256,
                size = candidateSize,
                instanceCount = ledgerPlan.InstanceIds.Count,
                instanceId = profile.Instance.Id,
                instanceVersion = profile.Instance.Version,
                installStatus = "Installing",
                totalMods = profile.Status.TotalMods
            },
            checkpoints = new
            {
                policy = "hash-pinned-session-range-plus-reviewed-package-allowlist",
                folderEvidenceUsedForCompletion = false,
                completedMustBeQueuedInSession = true,
                queuedCount = log.Queued.Count,
                completedCount = log.Completed.Count,
                retryCount = log.InFlight.Count,
                queued = log.Queued,
                completed = log.Completed,
                retry = log.InFlight
            },
            safety = new
            {
                activationPerformed = false,
                inspectedRootWritten = false,
                createNewNoOverwrite = true,
                strictSelfParseValidated = true,
                writtenCandidateHashAndLengthReverified = true,
                multiInstanceSafetyChecked = true
            }
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(audit, AuditSerializerOptions);
        var withNewline = new byte[bytes.Length + 1];
        bytes.CopyTo(withNewline, 0);
        withNewline[^1] = (byte)'\n';
        return withNewline;
    }

    private static XElement Element(string name, string value) => new(name, value.Trim());

    private static void RequireExactChildren(XElement parent, params string[] expectedNames)
    {
        if (parent.HasAttributes)
        {
            throw new InvalidDataException($"Generated element '{parent.Name}' unexpectedly contains attributes.");
        }

        var actualNames = parent.Elements().Select(element => element.Name.ToString()).ToArray();
        if (!actualNames.SequenceEqual(expectedNames, StringComparer.Ordinal))
        {
            throw new InvalidDataException(
                $"Generated element '{parent.Name}' has unexpected children. " +
                $"Expected [{string.Join(", ", expectedNames)}], found [{string.Join(", ", actualNames)}].");
        }
    }

    private static void RequireElementName(XElement element, string expectedName)
    {
        if (element.Name != expectedName)
        {
            throw new InvalidDataException(
                $"Generated root element must be '{expectedName}', found '{element.Name}'.");
        }
    }

    private static byte[] ReadFileWithLimit(string path, int maximumBytes, string description)
    {
        using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite | FileShare.Delete,
            81920,
            FileOptions.SequentialScan);

        if (stream.Length > maximumBytes)
        {
            throw new InvalidDataException($"{description} exceeds the {maximumBytes}-byte safety limit.");
        }

        var bytes = new byte[(int)stream.Length];
        stream.ReadExactly(bytes);
        return bytes;
    }

    private static void WriteCreateNew(string path, byte[] bytes, ref bool created)
    {
        using var stream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            FileOptions.WriteThrough);
        created = true;
        stream.Write(bytes);
        stream.Flush(flushToDisk: true);
    }

    private static void TryDeleteCreatedArtifact(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            // Preserve the original failure; this path was created by this reconstruction attempt.
        }
        catch (UnauthorizedAccessException)
        {
            // Preserve the original failure; this path was created by this reconstruction attempt.
        }
    }

    private static string NormalizeRequiredPath(string? path, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, parameterName);
        return Path.GetFullPath(path);
    }

    private static void ValidateProfileConsistency(
        ReconstructionProfile profile,
        string rootPath)
    {
        var canonicalRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
        var instancesRoot = Path.TrimEndingDirectorySeparator(
            Path.Combine(canonicalRoot, "Instances"));
        var expectedInstallFolder = profile.Instance.Tag.Length == 0
            ? profile.Instance.Name
            : $"{profile.Instance.Name} - {profile.Instance.Tag}";
        var expectedInstallPath = Path.TrimEndingDirectorySeparator(
            Path.Combine(instancesRoot, expectedInstallFolder));
        var expectedArchivePath = Path.TrimEndingDirectorySeparator(
            Path.Combine(instancesRoot, "ARCHIVE"));
        var expectedStockGamePath = Path.TrimEndingDirectorySeparator(
            Path.Combine(expectedInstallPath, "STOCK GAME"));

        var installPath = Path.TrimEndingDirectorySeparator(
            Path.GetFullPath(profile.Instance.InstallPath));
        var archivePath = Path.TrimEndingDirectorySeparator(
            Path.GetFullPath(profile.Instance.ArchivePath));
        var stockGamePath = Path.TrimEndingDirectorySeparator(
            Path.GetFullPath(profile.Instance.StockGame));

        if (!PathsEqual(installPath, expectedInstallPath))
        {
            throw new InvalidDataException(
                "instance.installPath must be the canonical instance directory directly beneath the inspected root's Instances directory.");
        }

        if (!PathsEqual(archivePath, expectedArchivePath))
        {
            throw new InvalidDataException(
                "instance.archivePath must be the inspected root's canonical Instances/ARCHIVE directory.");
        }

        if (!PathsEqual(stockGamePath, expectedStockGamePath))
        {
            throw new InvalidDataException(
                "instance.stockGame must be the canonical STOCK GAME directory directly beneath instance.installPath.");
        }
    }

    private static void RejectReparsePointTraversals(
        string rootPath,
        string outputXmlPath,
        string auditJsonPath)
    {
        RejectReparsePointTraversal(rootPath, "Inspected root");
        RejectReparsePointTraversal(
            Path.GetDirectoryName(outputXmlPath)
                ?? throw new InvalidOperationException("XML candidate path has no parent directory."),
            "XML candidate parent");
        RejectReparsePointTraversal(
            Path.GetDirectoryName(auditJsonPath)
                ?? throw new InvalidOperationException("JSON audit path has no parent directory."),
            "JSON audit parent");
    }

    private static void RejectReparsePointTraversal(string directoryPath, string description)
    {
        DirectoryInfo? current = new(Path.GetFullPath(directoryPath));

        while (current is not null)
        {
            current.Refresh();
            if (!current.Exists)
            {
                throw new DirectoryNotFoundException(
                    $"{description} traversal component does not exist: {current.FullName}");
            }

            if ((current.Attributes & FileAttributes.ReparsePoint) != 0 || current.LinkTarget is not null)
            {
                throw new InvalidOperationException(
                    $"{description} must not traverse a symbolic link, junction, mount point, or other reparse point: {current.FullName}");
            }

            current = current.Parent;
        }
    }

    private static void RequireExistingParent(string path, string description)
    {
        var parent = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(parent) || !Directory.Exists(parent))
        {
            throw new DirectoryNotFoundException($"{description} parent directory does not exist: {parent}");
        }
    }

    private static bool IsWithin(string rootPath, string candidatePath)
    {
        var relative = Path.GetRelativePath(rootPath, candidatePath);
        return string.Equals(relative, ".", StringComparison.Ordinal) ||
               (!Path.IsPathFullyQualified(relative) &&
                !string.Equals(relative, "..", StringComparison.Ordinal) &&
                !relative.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal) &&
                !relative.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal));
    }

    private static bool PathsEqual(string first, string second) =>
        string.Equals(first, second, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    private static string Sha256(ReadOnlySpan<byte> bytes) => Convert.ToHexString(SHA256.HashData(bytes));
}
