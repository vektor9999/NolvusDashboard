namespace Vcc.Nolvus.Recovery;

internal static class ModsAuditor
{
    private static readonly EnumerationOptions FileEnumeration = new()
    {
        RecurseSubdirectories = true,
        IgnoreInaccessible = false,
        ReturnSpecialDirectories = false,
        AttributesToSkip = FileAttributes.ReparsePoint
    };

    public static ModsAuditReport Inspect(string instancesDirectory, RecoveryInspectionOptions options)
    {
        if (!options.AuditMods)
        {
            return new ModsAuditReport(
                false,
                Array.Empty<string>(),
                Array.Empty<ModFolderReport>(),
                new[] { "Folder sampling was not requested. Use --audit-mods to enable it." });
        }

        if (options.MaxSampleFilesPerMod < 1 || options.SampleBytesPerRegion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "Sampling limits must be greater than zero.");
        }

        var folders = new List<ModFolderReport>();
        var evidence = new List<string>();
        var roots = FindModRoots(instancesDirectory, evidence);

        foreach (var modsRoot in roots)
        {
            string[] modDirectories;
            try
            {
                var allDirectories = Directory.GetDirectories(modsRoot);
                var physicalDirectories = allDirectories
                    .Where(path => IsPhysicalDirectoryTraversal(
                        path,
                        evidence,
                        $"mod directory '{path}'"))
                    .ToArray();
                var separators = physicalDirectories.Count(path =>
                    Path.GetFileName(path).EndsWith("_separator", StringComparison.OrdinalIgnoreCase));
                if (separators > 0)
                {
                    evidence.Add($"Excluded {separators} Mod Organizer separator folder(s) beneath '{modsRoot}'.");
                }

                modDirectories = physicalDirectories
                    .Where(path => !Path.GetFileName(path).EndsWith("_separator", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                Array.Sort(modDirectories, StablePathComparer.Instance);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                evidence.Add($"Could not enumerate '{modsRoot}': {OneLine(exception.Message)}");
                continue;
            }

            foreach (var modDirectory in modDirectories)
            {
                folders.Add(InspectFolder(modDirectory, options));
            }
        }

        if (roots.Count == 0)
        {
            evidence.Add("No '<instance>/MODS/mods' directory was found beneath the Instances directory.");
        }

        evidence.Add("A nonzero sample proves only that a folder is present; it does not verify the package or every file.");
        evidence.Add("An all-NUL result is based on sampled regions and is conservatively flagged as corrupt.");
        evidence.Add("Directory entries marked as reparse points at inspection time were rejected or skipped; filesystem races remain possible.");

        return new ModsAuditReport(true, roots, folders, evidence);
    }

    private static IReadOnlyList<string> FindModRoots(string instancesDirectory, ICollection<string> evidence)
    {
        if (!Directory.Exists(instancesDirectory))
        {
            return Array.Empty<string>();
        }

        if (!IsPhysicalDirectoryTraversal(instancesDirectory, evidence, "Instances directory"))
        {
            return Array.Empty<string>();
        }

        try
        {
            var roots = new List<string>();
            foreach (var instanceDirectory in Directory.GetDirectories(instancesDirectory))
            {
                if (string.Equals(Path.GetFileName(instanceDirectory), "ARCHIVE", StringComparison.OrdinalIgnoreCase) ||
                    !IsPhysicalDirectoryTraversal(
                        instanceDirectory,
                        evidence,
                        $"instance directory '{instanceDirectory}'"))
                {
                    continue;
                }

                var modsDirectory = Path.Combine(instanceDirectory, "MODS");
                var modsRoot = Path.Combine(modsDirectory, "mods");
                if (!Directory.Exists(modsDirectory) ||
                    !IsPhysicalDirectoryTraversal(modsDirectory, evidence, $"MODS directory '{modsDirectory}'") ||
                    !Directory.Exists(modsRoot) ||
                    !IsPhysicalDirectoryTraversal(modsRoot, evidence, $"mods root '{modsRoot}'"))
                {
                    continue;
                }

                roots.Add(modsRoot);
            }

            roots.Sort(StablePathComparer.Instance);
            return roots;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            evidence.Add($"Could not enumerate '{instancesDirectory}': {OneLine(exception.Message)}");
            return Array.Empty<string>();
        }
    }

    private static ModFolderReport InspectFolder(string modDirectory, RecoveryInspectionOptions options)
    {
        try
        {
            var files = Directory.EnumerateFiles(modDirectory, "*", FileEnumeration)
                .OrderBy(path => path, StablePathComparer.Instance)
                .ToArray();

            if (files.Length == 0)
            {
                return new ModFolderReport(
                    Path.GetFileName(modDirectory),
                    modDirectory,
                    ModFolderStatus.Empty,
                    0,
                    0,
                    0,
                    "Folder contains no files and is corrupt/incomplete evidence.");
            }

            var sampledFiles = files.Take(options.MaxSampleFilesPerMod).ToArray();
            long sampledBytes = 0;
            var sawNonzero = false;
            foreach (var file in sampledFiles)
            {
                var result = SampleFile(file, options.SampleBytesPerRegion);
                sampledBytes += result.BytesRead;
                sawNonzero |= result.SawNonzero;
            }

            if (!sawNonzero)
            {
                return new ModFolderReport(
                    Path.GetFileName(modDirectory),
                    modDirectory,
                    ModFolderStatus.CorruptSampledAllNull,
                    files.LongLength,
                    sampledFiles.Length,
                    sampledBytes,
                    $"All {sampledBytes} sampled bytes across {sampledFiles.Length} file(s) were NUL, or the sampled files were zero-length.");
            }

            return new ModFolderReport(
                Path.GetFileName(modDirectory),
                modDirectory,
                ModFolderStatus.Present,
                files.LongLength,
                sampledFiles.Length,
                sampledBytes,
                "At least one sampled byte was nonzero; present only, not cryptographically verified.");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return new ModFolderReport(
                Path.GetFileName(modDirectory),
                modDirectory,
                ModFolderStatus.Inaccessible,
                0,
                0,
                0,
                OneLine(exception.Message));
        }
    }

    private static SampleResult SampleFile(string path, int regionSize)
    {
        EnsurePhysicalFileTraversal(path);

        using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite | FileShare.Delete,
            regionSize,
            FileOptions.RandomAccess);

        if (stream.Length == 0)
        {
            return new SampleResult(false, 0);
        }

        var buffer = new byte[regionSize];
        var offsets = new[]
        {
            0L,
            Math.Max(0L, (stream.Length / 2L) - (regionSize / 2L)),
            Math.Max(0L, stream.Length - regionSize)
        }.Distinct().ToArray();

        long bytesRead = 0;
        var sawNonzero = false;
        foreach (var offset in offsets)
        {
            stream.Position = offset;
            var count = stream.Read(buffer, 0, (int)Math.Min(regionSize, stream.Length - offset));
            bytesRead += count;
            for (var index = 0; index < count; index++)
            {
                if (buffer[index] != 0)
                {
                    sawNonzero = true;
                    break;
                }
            }
        }

        return new SampleResult(sawNonzero, bytesRead);
    }

    private static bool IsPhysicalDirectoryTraversal(
        string path,
        ICollection<string> evidence,
        string description)
    {
        try
        {
            DirectoryInfo? current = new(Path.GetFullPath(path));
            while (current is not null)
            {
                current.Refresh();
                if (!current.Exists)
                {
                    evidence.Add($"Excluded {description} because traversal component '{current.FullName}' does not exist.");
                    return false;
                }

                if ((current.Attributes & FileAttributes.ReparsePoint) != 0 || current.LinkTarget is not null)
                {
                    evidence.Add($"Excluded {description} because its path traverses reparse point '{current.FullName}'.");
                    return false;
                }

                current = current.Parent;
            }

            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            evidence.Add($"Excluded {description} because its path could not be verified: {OneLine(exception.Message)}");
            return false;
        }
    }

    private static void EnsurePhysicalFileTraversal(string path)
    {
        var file = new FileInfo(Path.GetFullPath(path));
        file.Refresh();
        if (!file.Exists)
        {
            throw new FileNotFoundException("Sample file no longer exists.", file.FullName);
        }

        if ((file.Attributes & FileAttributes.ReparsePoint) != 0 || file.LinkTarget is not null)
        {
            throw new IOException($"Sample file is a reparse point: '{file.FullName}'.");
        }

        DirectoryInfo? current = file.Directory;
        while (current is not null)
        {
            current.Refresh();
            if (!current.Exists)
            {
                throw new DirectoryNotFoundException($"Sample path component no longer exists: '{current.FullName}'.");
            }

            if ((current.Attributes & FileAttributes.ReparsePoint) != 0 || current.LinkTarget is not null)
            {
                throw new IOException($"Sample path traverses reparse point '{current.FullName}'.");
            }

            current = current.Parent;
        }
    }

    private static string OneLine(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Trim();

    private readonly record struct SampleResult(bool SawNonzero, long BytesRead);

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
