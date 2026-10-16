namespace Vcc.Nolvus.Recovery;

internal static class LogInspector
{
    private const string Prefix = "Mod : ";
    private const string QueuedSuffix = " added to queue.";
    private const string CompletedSuffix = " installed.";
    private const string SessionPrefix = "Start installing ";

    public static LogInspectionReport Inspect(string path) =>
        InspectCore(path, scope: null);

    public static LogInspectionReport Inspect(
        string path,
        long sessionStartLine,
        long sessionEndLine,
        string expectedSessionMarker)
    {
        if (sessionStartLine < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sessionStartLine));
        }

        if (sessionEndLine < sessionStartLine)
        {
            throw new ArgumentOutOfRangeException(nameof(sessionEndLine));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(expectedSessionMarker);
        return InspectCore(
            path,
            new EvidenceScope(sessionStartLine, sessionEndLine, expectedSessionMarker));
    }

    private static LogInspectionReport InspectCore(string path, EvidenceScope? scope)
    {
        if (!File.Exists(path))
        {
            return new LogInspectionReport(
                path,
                false,
                0,
                Array.Empty<string>(),
                Array.Empty<string>(),
                Array.Empty<string>(),
                new[] { "Log.txt does not exist." });
        }

        var queued = new OrderedSet();
        var completed = new OrderedSet();
        long lineCount = 0;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                lineCount++;

                if (scope is not null)
                {
                    if (lineCount < scope.StartLine || lineCount > scope.EndLine)
                    {
                        continue;
                    }

                    if (lineCount == scope.StartLine &&
                        !string.Equals(line, scope.ExpectedMarker, StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(
                            $"The declared session start line {scope.StartLine} does not exactly match the expected package marker.");
                    }

                    if (lineCount > scope.StartLine &&
                        line.StartsWith(SessionPrefix, StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(
                            $"The evidence range crosses another install session marker at line {lineCount}.");
                    }
                }

                if (TryExtract(line, QueuedSuffix, out var queuedName))
                {
                    queued.Add(queuedName);
                }
                else if (TryExtract(line, CompletedSuffix, out var completedName))
                {
                    completed.Add(completedName);
                }
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return new LogInspectionReport(
                path,
                true,
                lineCount,
                queued.Items,
                completed.Items,
                Array.Empty<string>(),
                new[] { $"Log read stopped: {OneLine(exception.Message)}" });
        }

        if (scope is not null && lineCount < scope.EndLine)
        {
            throw new InvalidDataException(
                $"The declared evidence range ends at line {scope.EndLine}, but Log.txt has only {lineCount} lines.");
        }

        var completedNames = new HashSet<string>(completed.Items, StringComparer.Ordinal);
        var inFlight = queued.Items.Where(name => !completedNames.Contains(name)).ToArray();
        var evidence = new List<string>
        {
            "Only exact, case-sensitive 'Mod : <name> added to queue.' and 'Mod : <name> installed.' lines were counted.",
            "In-flight is the ordered set difference: queued minus completed; it is never promoted to completed."
        };

        if (scope is not null)
        {
            evidence.Add(
                $"Checkpoints were restricted to reviewed session lines {scope.StartLine}-{scope.EndLine}; " +
                "the first line matched the deterministic package marker and no later session marker occurred.");
        }

        return new LogInspectionReport(
            path,
            true,
            lineCount,
            queued.Items,
            completed.Items,
            inFlight,
            evidence);
    }

    private static bool TryExtract(string line, string suffix, out string name)
    {
        name = string.Empty;
        if (!line.StartsWith(Prefix, StringComparison.Ordinal) ||
            !line.EndsWith(suffix, StringComparison.Ordinal) ||
            line.Length <= Prefix.Length + suffix.Length)
        {
            return false;
        }

        name = line.Substring(Prefix.Length, line.Length - Prefix.Length - suffix.Length);
        return name.Length > 0;
    }

    private static string OneLine(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Trim();

    private sealed record EvidenceScope(long StartLine, long EndLine, string ExpectedMarker);

    private sealed class OrderedSet
    {
        private readonly HashSet<string> _seen = new(StringComparer.Ordinal);
        private readonly List<string> _items = new();

        public IReadOnlyList<string> Items => _items;

        public void Add(string value)
        {
            if (_seen.Add(value))
            {
                _items.Add(value);
            }
        }
    }
}
