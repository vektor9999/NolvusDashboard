namespace Vcc.Nolvus.Recovery;

public sealed class RecoveryInspector
{
    public RecoveryInspectionReport Inspect(string nolvusRoot, RecoveryInspectionOptions? options = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nolvusRoot);
        options ??= new RecoveryInspectionOptions();

        var root = Path.GetFullPath(nolvusRoot);
        var rootExists = Directory.Exists(root);
        var instancesDirectory = Path.Combine(root, "Instances");
        var candidates = InspectCandidates(instancesDirectory);
        var log = LogInspector.Inspect(Path.Combine(root, "Log.txt"));
        var mods = ModsAuditor.Inspect(instancesDirectory, options);
        var evidence = new List<string>
        {
            "Inspection is read-only. No candidate was generated, promoted, renamed, deleted, or modified.",
            "A valid XML candidate is evidence about the ledger only; it does not verify installed mod payloads."
        };

        if (!rootExists)
        {
            evidence.Insert(0, "The requested Nolvus root directory does not exist.");
        }

        return new RecoveryInspectionReport(root, rootExists, candidates, log, mods, evidence);
    }

    private static IReadOnlyList<XmlCandidateReport> InspectCandidates(string instancesDirectory)
    {
        var livePath = Path.Combine(instancesDirectory, "InstancesData.xml");
        var tempPath = Path.Combine(instancesDirectory, "InstancesData.temp.xml");
        var candidates = new List<XmlCandidateReport>
        {
            XmlCandidateInspector.Inspect(CandidateKind.Live, livePath),
            XmlCandidateInspector.Inspect(CandidateKind.Temporary, tempPath)
        };

        var known = new HashSet<string>(new[] { livePath, tempPath }, StringComparer.OrdinalIgnoreCase);
        if (Directory.Exists(instancesDirectory))
        {
            try
            {
                var additionalCandidates = Directory.GetFiles(instancesDirectory)
                    .Where(IsBackupCandidate)
                    .OrderBy(path => path, StablePathComparer.Instance);

                foreach (var candidate in additionalCandidates)
                {
                    if (known.Add(candidate))
                    {
                        var kind = IsGeneratedTemporaryCandidate(candidate)
                            ? CandidateKind.Temporary
                            : CandidateKind.Backup;
                        candidates.Add(XmlCandidateInspector.Inspect(kind, candidate));
                    }
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                candidates.Add(new XmlCandidateReport(
                    CandidateKind.Backup,
                    instancesDirectory,
                    XmlCandidateStatus.IoError,
                    0,
                    null,
                    0,
                    new[] { $"Backup enumeration failed: {OneLine(exception.Message)}" }));
            }
        }

        return candidates;
    }

    private static bool IsBackupCandidate(string path)
    {
        var name = Path.GetFileName(path);
        if (!name.StartsWith("InstancesData", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return name.Contains("backup", StringComparison.OrdinalIgnoreCase) ||
               name.EndsWith(".bak", StringComparison.OrdinalIgnoreCase) ||
               name.EndsWith(".old", StringComparison.OrdinalIgnoreCase) ||
               name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsGeneratedTemporaryCandidate(string path)
    {
        const string prefix = "InstancesData.temp.";
        const string suffix = ".xml";
        const int guidLength = 32;

        var name = Path.GetFileName(path);
        if (name.Length != prefix.Length + guidLength + suffix.Length ||
            !name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
            !name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return Guid.TryParseExact(name.Substring(prefix.Length, guidLength), "N", out _);
    }

    private static string OneLine(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Trim();

    private sealed class StablePathComparer : IComparer<string>
    {
        public static StablePathComparer Instance { get; } = new();

        public int Compare(string? left, string? right)
        {
            var primary = StringComparer.OrdinalIgnoreCase.Compare(left, right);
            return primary != 0 ? primary : StringComparer.Ordinal.Compare(left, right);
        }
    }
}
