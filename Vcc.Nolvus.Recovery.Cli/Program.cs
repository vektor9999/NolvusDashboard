using System.Text;
using System.Text.Json;
using Vcc.Nolvus.Recovery;

return Run(args);

static int Run(string[] args)
{
    Console.OutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    if (args.Length == 0)
    {
        PrintUsage();
        return 2;
    }

    try
    {
        return args[0].ToLowerInvariant() switch
        {
            "inspect" => RunInspect(args[1..]),
            "reconstruct" => RunReconstruct(args[1..]),
            "--help" or "-h" or "help" => ShowHelp(),
            _ => FailWithUsage($"Unknown command: {args[0]}")
        };
    }
    catch (Exception exception) when (IsExpectedFailure(exception))
    {
        Console.Error.WriteLine($"Recovery command refused: {exception.Message}");
        return 1;
    }
}

static int RunInspect(string[] args)
{
    if (!TryParseInspect(args, out var root, out var reportPath, out var auditMods, out var error))
    {
        return FailWithUsage(error);
    }

    var report = new RecoveryInspector().Inspect(
        root,
        new RecoveryInspectionOptions { AuditMods = auditMods });
    var formatted = RecoveryReportFormatter.Format(report);
    Console.Write(formatted);

    if (reportPath is not null)
    {
        var fullReportPath = Path.GetFullPath(reportPath);
        WriteReportCreateNew(report.RootPath, fullReportPath, formatted);
        Console.WriteLine($"Report written: {fullReportPath}");
    }

    return 0;
}

static int RunReconstruct(string[] args)
{
    if (!TryParseReconstruct(
            args,
            out var root,
            out var profile,
            out var output,
            out var audit,
            out var error))
    {
        return FailWithUsage(error);
    }

    var result = InstancesDataReconstructor.Reconstruct(
        new ReconstructionRequest(root, profile, output, audit));

    Console.WriteLine("Nolvus recovery candidate reconstructed.");
    Console.WriteLine($"Candidate: {result.OutputXmlPath}");
    Console.WriteLine($"Candidate status: {result.Candidate.Status}");
    Console.WriteLine($"Candidate SHA-256: {result.Candidate.Sha256}");
    Console.WriteLine($"Audit: {result.AuditJsonPath}");
    Console.WriteLine($"Profile SHA-256: {result.ProfileSha256}");
    Console.WriteLine($"Log SHA-256: {result.LogSha256}");
    Console.WriteLine($"Exact completed checkpoints adopted: {result.CompletedMods.Count}");
    Console.WriteLine($"Queued entries retained for retry: {result.RetryMods.Count}");
    Console.WriteLine($"Legacy TotalMods preserved: {result.TotalMods}");
    Console.WriteLine("Safety: the candidate was not activated and the inspected Nolvus root was not modified.");
    return 0;
}

static bool TryParseInspect(
    string[] args,
    out string root,
    out string? reportPath,
    out bool auditMods,
    out string error)
{
    root = string.Empty;
    reportPath = null;
    auditMods = false;
    error = string.Empty;
    var seenOptions = new HashSet<string>(StringComparer.Ordinal);

    for (var index = 0; index < args.Length; index++)
    {
        if (!seenOptions.Add(args[index]))
        {
            error = $"Duplicate option: {args[index]}";
            return false;
        }

        switch (args[index])
        {
            case "--root":
                if (!TryReadValue(args, ref index, "--root", out root, out error))
                {
                    return false;
                }

                break;

            case "--report":
                if (!TryReadValue(args, ref index, "--report", out var report, out error))
                {
                    return false;
                }

                reportPath = report;
                break;

            case "--audit-mods":
                auditMods = true;
                break;

            default:
                error = $"Unknown inspect option: {args[index]}";
                return false;
        }
    }

    if (string.IsNullOrWhiteSpace(root))
    {
        error = "inspect requires --root <path>.";
        return false;
    }

    return true;
}

static bool TryParseReconstruct(
    string[] args,
    out string root,
    out string profile,
    out string output,
    out string audit,
    out string error)
{
    root = string.Empty;
    profile = string.Empty;
    output = string.Empty;
    audit = string.Empty;
    error = string.Empty;
    var seenOptions = new HashSet<string>(StringComparer.Ordinal);

    for (var index = 0; index < args.Length; index++)
    {
        if (!seenOptions.Add(args[index]))
        {
            error = $"Duplicate option: {args[index]}";
            return false;
        }

        switch (args[index])
        {
            case "--root":
                if (!TryReadValue(args, ref index, "--root", out root, out error))
                {
                    return false;
                }

                break;

            case "--profile":
                if (!TryReadValue(args, ref index, "--profile", out profile, out error))
                {
                    return false;
                }

                break;

            case "--output":
                if (!TryReadValue(args, ref index, "--output", out output, out error))
                {
                    return false;
                }

                break;

            case "--audit":
                if (!TryReadValue(args, ref index, "--audit", out audit, out error))
                {
                    return false;
                }

                break;

            default:
                error = $"Unknown reconstruct option: {args[index]}";
                return false;
        }
    }

    if (string.IsNullOrWhiteSpace(root)
        || string.IsNullOrWhiteSpace(profile)
        || string.IsNullOrWhiteSpace(output)
        || string.IsNullOrWhiteSpace(audit))
    {
        error = "reconstruct requires --root, --profile, --output, and --audit.";
        return false;
    }

    return true;
}

static bool TryReadValue(
    IReadOnlyList<string> args,
    ref int index,
    string option,
    out string value,
    out string error)
{
    value = string.Empty;
    error = string.Empty;

    if (index + 1 >= args.Count
        || string.IsNullOrWhiteSpace(args[index + 1])
        || args[index + 1].StartsWith("--", StringComparison.Ordinal))
    {
        error = $"{option} requires a value.";
        return false;
    }

    value = args[++index];
    return true;
}

static void WriteReportCreateNew(string inspectedRoot, string reportPath, string contents)
{
    if (IsWithin(inspectedRoot, reportPath))
    {
        throw new InvalidDataException(
            "The report path must be outside the inspected Nolvus root.");
    }

    var parent = Path.GetDirectoryName(reportPath)
        ?? throw new InvalidDataException("The report path has no parent directory.");

    if (!Directory.Exists(parent))
    {
        throw new DirectoryNotFoundException(
            $"The report parent directory does not exist: {parent}");
    }

    RejectReparsePointAncestors(inspectedRoot, "The inspected root");
    RejectReparsePointAncestors(parent, "The report parent path");

    using var stream = new FileStream(
        reportPath,
        FileMode.CreateNew,
        FileAccess.Write,
        FileShare.None);
    using var writer = new StreamWriter(stream, new UTF8Encoding(false));
    writer.Write(contents);
    writer.Flush();
    stream.Flush(flushToDisk: true);
}

static void RejectReparsePointAncestors(string directoryPath, string label)
{
    for (var current = new DirectoryInfo(Path.GetFullPath(directoryPath));
         current is not null;
         current = current.Parent)
    {
        if ((current.Attributes & FileAttributes.ReparsePoint) != 0)
        {
            throw new InvalidOperationException(
                $"{label} traverses the reparse point '{current.FullName}'.");
        }
    }
}

static bool IsWithin(string root, string candidate)
{
    var relative = Path.GetRelativePath(Path.GetFullPath(root), Path.GetFullPath(candidate));
    return relative == "."
        || (!Path.IsPathRooted(relative)
            && relative != ".."
            && !relative.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            && !relative.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal));
}

static bool IsExpectedFailure(Exception exception)
{
    return exception is ArgumentException
        or DirectoryNotFoundException
        or InvalidDataException
        or InvalidOperationException
        or IOException
        or JsonException
        or UnauthorizedAccessException;
}

static int ShowHelp()
{
    PrintUsage();
    return 0;
}

static int FailWithUsage(string error)
{
    Console.Error.WriteLine(error);
    PrintUsage();
    return 2;
}

static void PrintUsage()
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine(
        "  Vcc.Nolvus.Recovery.Cli inspect --root <NolvusRoot> [--audit-mods] [--report <path>]");
    Console.Error.WriteLine(
        "  Vcc.Nolvus.Recovery.Cli reconstruct --root <NolvusRoot> --profile <confirmed.json> --output <candidate.xml> --audit <audit.json>");
    Console.Error.WriteLine();
    Console.Error.WriteLine(
        "reconstruct requires new output paths outside the inspected root and never activates the candidate.");
}
