using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vcc.Nolvus.Recovery;

public sealed class ReconstructionProfile
{
    public required int ProfileVersion { get; init; }

    public required bool Confirmed { get; init; }

    public required string PackageVersion { get; init; }

    public required RecoveryEvidenceProfile Evidence { get; init; }

    public required RecoveryLedgerSafetyProfile LedgerSafety { get; init; }

    public required RecoveryInstanceProfile Instance { get; init; }

    public required RecoverySettingsProfile Settings { get; init; }

    public required RecoveryPerformanceProfile Performance { get; init; }

    public required RecoveryOptionsProfile Options { get; init; }

    public required RecoveryStatusProfile Status { get; init; }
}

public sealed class RecoveryEvidenceProfile
{
    public required string LogSha256 { get; init; }

    public required long SessionStartLine { get; init; }

    public required long SessionEndLine { get; init; }

    public required IReadOnlyList<string> PackageModNames { get; init; }
}

public sealed class RecoveryLedgerSafetyProfile
{
    public required bool SingleInstanceOnlyConfirmed { get; init; }

    public required IReadOnlyList<string> ExpectedInstanceIds { get; init; }
}

public sealed class RecoveryInstanceProfile
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Tag { get; init; }

    public required string Description { get; init; }

    public required string Version { get; init; }

    public required string InstallPath { get; init; }

    public required string ArchivePath { get; init; }

    public required string StockGame { get; init; }
}

public sealed class RecoverySettingsProfile
{
    public required string Ratio { get; init; }

    public required string Height { get; init; }

    public required string Width { get; init; }

    public required bool EnableArchiving { get; init; }

    public required string Cdn { get; init; }

    public required string LgCode { get; init; }

    public required string LgName { get; init; }
}

public sealed class RecoveryPerformanceProfile
{
    public required string AdvancedPhysics { get; init; }

    public required string DownScaling { get; init; }

    public required string DownHeight { get; init; }

    public required string DownWidth { get; init; }

    public required string IniSettings { get; init; }

    public required string AntiAliasing { get; init; }

    public required string Variant { get; init; }

    public required string Lods { get; init; }

    public required string RayTracing { get; init; }

    public required string FpsStabilizer { get; init; }

    public required string Srex { get; init; }

    public required string FrameGeneration { get; init; }
}

public sealed class RecoveryOptionsProfile
{
    public required string Nudity { get; init; }

    public required string AlternateEnb { get; init; }

    public required string FantasyMode { get; init; }

    public required string HardcoreMode { get; init; }

    public required string AlternateLeveling { get; init; }

    public required string SkinType { get; init; }

    public required string AlternateStart { get; init; }

    public required string CombatAnimation { get; init; }

    public required string StancesPerksTree { get; init; }

    public required string DeleveledEnemies { get; init; }

    public required string Exhaustion { get; init; }

    public required string NerfPa { get; init; }

    public required string EnemiesResistance { get; init; }

    public required string Boss { get; init; }

    public required string Poise { get; init; }

    public required string Gore { get; init; }

    public required string CombatScaling { get; init; }

    public required string Controller { get; init; }

    public required string Ui { get; init; }
}

public sealed class RecoveryStatusProfile
{
    public required int TotalMods { get; init; }

    public required IReadOnlyList<RecoveryStatusFieldProfile> Fields { get; init; }
}

public sealed class RecoveryStatusFieldProfile
{
    public required string Key { get; init; }

    public required string Value { get; init; }
}

public static class ReconstructionProfileLoader
{
    private const int MaximumProfileBytes = 4 * 1024 * 1024;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        ReadCommentHandling = JsonCommentHandling.Disallow,
        AllowTrailingCommas = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        NumberHandling = JsonNumberHandling.Strict,
        MaxDepth = 16
    };

    public static ReconstructionProfile Load(string profilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(profilePath);
        var fullPath = Path.GetFullPath(profilePath);

        using var stream = new FileStream(
            fullPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            81920,
            FileOptions.SequentialScan);

        if (stream.Length > MaximumProfileBytes)
        {
            throw new InvalidDataException($"Recovery profile exceeds the {MaximumProfileBytes}-byte safety limit.");
        }

        var bytes = new byte[(int)stream.Length];
        stream.ReadExactly(bytes);
        return Parse(bytes);
    }

    internal static ReconstructionProfile Parse(byte[] json)
    {
        try
        {
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 16
            });

            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidDataException("Recovery profile root must be a JSON object.");
            }

            RejectDuplicateProperties(document.RootElement, "$" );

            var profile = JsonSerializer.Deserialize<ReconstructionProfile>(json, SerializerOptions)
                ?? throw new InvalidDataException("Recovery profile deserialized to null.");

            Validate(profile);
            return profile;
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Recovery profile JSON is invalid: {OneLine(exception.Message)}", exception);
        }
    }

    private static void Validate(ReconstructionProfile profile)
    {
        if (profile.ProfileVersion != 2)
        {
            throw new InvalidDataException($"Unsupported recovery profile version '{profile.ProfileVersion}'; expected 2.");
        }

        if (!profile.Confirmed)
        {
            throw new InvalidDataException("Recovery profile must contain confirmed: true before a candidate can be generated.");
        }

        RequireCanonicalNonBlank(profile.PackageVersion, "packageVersion", 64);
        RequireSafeCode(profile.PackageVersion, "packageVersion", character =>
            char.IsAsciiDigit(character) || character is '.' or '-' or '+');

        ArgumentNullException.ThrowIfNull(profile.Evidence);
        RequireUpperHexSha256(profile.Evidence.LogSha256, "evidence.logSha256");
        if (profile.Evidence.SessionStartLine < 1)
        {
            throw new InvalidDataException("evidence.sessionStartLine must be at least 1.");
        }

        if (profile.Evidence.SessionEndLine < profile.Evidence.SessionStartLine)
        {
            throw new InvalidDataException("evidence.sessionEndLine must be greater than or equal to evidence.sessionStartLine.");
        }

        if (profile.Evidence.SessionEndLine - profile.Evidence.SessionStartLine > 10_000_000)
        {
            throw new InvalidDataException("The declared evidence line range is unreasonably large.");
        }

        ArgumentNullException.ThrowIfNull(profile.Evidence.PackageModNames);
        if (profile.Evidence.PackageModNames.Count == 0)
        {
            throw new InvalidDataException("evidence.packageModNames must contain the reviewed exact-package mod manifest.");
        }

        if (profile.Evidence.PackageModNames.Count > 50_000)
        {
            throw new InvalidDataException("evidence.packageModNames exceeds the 50,000-entry safety limit.");
        }

        var packageModNames = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < profile.Evidence.PackageModNames.Count; index++)
        {
            var name = profile.Evidence.PackageModNames[index];
            RequireCanonicalNonBlank(name, $"evidence.packageModNames[{index}]", 512);
            if (!packageModNames.Add(name))
            {
                throw new InvalidDataException($"evidence.packageModNames contains duplicate exact name '{name}'.");
            }
        }

        ArgumentNullException.ThrowIfNull(profile.LedgerSafety);
        ArgumentNullException.ThrowIfNull(profile.LedgerSafety.ExpectedInstanceIds);
        if (profile.LedgerSafety.ExpectedInstanceIds.Count == 0)
        {
            throw new InvalidDataException("ledgerSafety.expectedInstanceIds must contain the reviewed instance inventory.");
        }

        if (profile.LedgerSafety.ExpectedInstanceIds.Count > 1_000)
        {
            throw new InvalidDataException("ledgerSafety.expectedInstanceIds exceeds the 1,000-entry safety limit.");
        }

        ArgumentNullException.ThrowIfNull(profile.Instance);
        ArgumentNullException.ThrowIfNull(profile.Settings);
        ArgumentNullException.ThrowIfNull(profile.Performance);
        ArgumentNullException.ThrowIfNull(profile.Options);
        ArgumentNullException.ThrowIfNull(profile.Status);

        RequireCanonicalNonBlank(profile.Instance.Id, "instance.id", 128);
        RequireSafeCode(profile.Instance.Id, "instance.id", character =>
            char.IsAsciiLetterOrDigit(character) || character is '!' or '.' or '_' or '-');
        RequireCanonicalNonBlank(profile.Instance.Name, "instance.name", 200);
        RequirePathSegment(profile.Instance.Name, "instance.name");
        RequireCanonicalPresent(profile.Instance.Tag, "instance.tag", 100);
        if (profile.Instance.Tag.Length > 0)
        {
            RequirePathSegment(profile.Instance.Tag, "instance.tag");
        }

        RequireCanonicalPresent(profile.Instance.Description, "instance.description", 8_192);
        RequireCanonicalNonBlank(profile.Instance.Version, "instance.version", 64);
        if (!string.Equals(profile.Instance.Version, profile.PackageVersion, StringComparison.Ordinal))
        {
            throw new InvalidDataException("instance.version must exactly match packageVersion.");
        }

        RequireCanonicalAbsolutePath(profile.Instance.InstallPath, "instance.installPath");
        RequireCanonicalAbsolutePath(profile.Instance.ArchivePath, "instance.archivePath");
        RequireCanonicalAbsolutePath(profile.Instance.StockGame, "instance.stockGame");

        var instanceIds = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < profile.LedgerSafety.ExpectedInstanceIds.Count; index++)
        {
            var instanceId = profile.LedgerSafety.ExpectedInstanceIds[index];
            RequireCanonicalNonBlank(instanceId, $"ledgerSafety.expectedInstanceIds[{index}]", 128);
            RequireSafeCode(instanceId, $"ledgerSafety.expectedInstanceIds[{index}]", character =>
                char.IsAsciiLetterOrDigit(character) || character is '!' or '.' or '_' or '-');
            if (!instanceIds.Add(instanceId))
            {
                throw new InvalidDataException($"ledgerSafety.expectedInstanceIds contains duplicate id '{instanceId}'.");
            }
        }

        if (!instanceIds.Contains(profile.Instance.Id))
        {
            throw new InvalidDataException("ledgerSafety.expectedInstanceIds must include instance.id.");
        }

        if (profile.LedgerSafety.SingleInstanceOnlyConfirmed &&
            (profile.LedgerSafety.ExpectedInstanceIds.Count != 1 ||
             !string.Equals(profile.LedgerSafety.ExpectedInstanceIds[0], profile.Instance.Id, StringComparison.Ordinal)))
        {
            throw new InvalidDataException(
                "When singleInstanceOnlyConfirmed is true, expectedInstanceIds must contain only instance.id.");
        }

        RequireOneOf(profile.Settings.Ratio, "settings.ratio", "16:9", "21:9", "32:9");
        var height = RequireDimension(profile.Settings.Height, "settings.height");
        var width = RequireDimension(profile.Settings.Width, "settings.width");
        RequireCanonicalNonBlank(profile.Settings.Cdn, "settings.cdn", 128);
        RequireCanonicalNonBlank(profile.Settings.LgCode, "settings.lgCode", 8);
        RequireSafeCode(profile.Settings.LgCode, "settings.lgCode", char.IsAsciiLetterUpper);
        RequireCanonicalNonBlank(profile.Settings.LgName, "settings.lgName", 128);

        RequireTrueFalse(profile.Performance.AdvancedPhysics, "performance.advancedPhysics");
        RequireTrueFalse(profile.Performance.DownScaling, "performance.downScaling");
        RequireOneOf(profile.Performance.IniSettings, "performance.iniSettings", "0", "1", "2");
        RequireOneOf(profile.Performance.AntiAliasing, "performance.antiAliasing", "TAA", "DLAA", "FSR");
        RequireCanonicalNonBlank(profile.Performance.Variant, "performance.variant", 128);
        RequireOneOf(profile.Performance.Lods, "performance.lods", "Ultra", "Performance", "Ultra Performance");
        RequireTrueFalse(profile.Performance.RayTracing, "performance.rayTracing");
        RequireTrueFalse(profile.Performance.FpsStabilizer, "performance.fpsStabilizer");
        RequireTrueFalse(profile.Performance.Srex, "performance.srex");
        RequireTrueFalse(profile.Performance.FrameGeneration, "performance.frameGeneration");

        if (string.Equals(profile.Performance.DownScaling, "TRUE", StringComparison.Ordinal))
        {
            var downHeight = RequireDimension(profile.Performance.DownHeight, "performance.downHeight");
            var downWidth = RequireDimension(profile.Performance.DownWidth, "performance.downWidth");
            if (downHeight >= height || downWidth >= width)
            {
                throw new InvalidDataException(
                    "Enabled downscaling requires both downHeight and downWidth to be smaller than the selected resolution.");
            }
        }
        else
        {
            RequireOptionalDimension(profile.Performance.DownHeight, "performance.downHeight");
            RequireOptionalDimension(profile.Performance.DownWidth, "performance.downWidth");
        }

        RequireTrueFalse(profile.Options.Nudity, "options.nudity");
        RequireCanonicalNonBlank(profile.Options.AlternateEnb, "options.alternateEnb", 64);
        RequireSafeCode(profile.Options.AlternateEnb, "options.alternateEnb", character =>
            char.IsAsciiLetterUpper(character) || char.IsAsciiDigit(character) || character is '_' or '-');
        RequireTrueFalse(profile.Options.FantasyMode, "options.fantasyMode");
        RequireTrueFalse(profile.Options.HardcoreMode, "options.hardcoreMode");
        RequireTrueFalse(profile.Options.AlternateLeveling, "options.alternateLeveling");
        RequireOneOf(profile.Options.SkinType, "options.skinType", "Smooth", "Muscular");
        RequireTrueFalse(profile.Options.AlternateStart, "options.alternateStart");
        RequireOneOf(profile.Options.CombatAnimation, "options.combatAnimation", "Conventional", "Fantasy");
        RequireTrueFalse(profile.Options.StancesPerksTree, "options.stancesPerksTree");
        RequireTrueFalse(profile.Options.DeleveledEnemies, "options.deleveledEnemies");
        RequireTrueFalse(profile.Options.Exhaustion, "options.exhaustion");
        RequireOneOf(profile.Options.NerfPa, "options.nerfPa", "None", "Player Only", "NPCs Only", "Both");
        RequireTrueFalse(profile.Options.EnemiesResistance, "options.enemiesResistance");
        RequireTrueFalse(profile.Options.Boss, "options.boss");
        RequireTrueFalse(profile.Options.Poise, "options.poise");
        RequireTrueFalse(profile.Options.Gore, "options.gore");
        RequireOneOf(profile.Options.CombatScaling, "options.combatScaling", "Very Easy", "Easy", "Medium", "Hard");
        RequireTrueFalse(profile.Options.Controller, "options.controller");
        RequireOneOf(profile.Options.Ui, "options.ui", "Untarnished UI", "Edge UI", "Oathvein UI");

        if (profile.Status.TotalMods is < 1 or > short.MaxValue)
        {
            throw new InvalidDataException($"status.totalMods must be between 1 and {short.MaxValue}.");
        }

        if (profile.Status.TotalMods != profile.Evidence.PackageModNames.Count)
        {
            throw new InvalidDataException(
                "status.totalMods must exactly match the reviewed evidence.packageModNames count.");
        }

        ArgumentNullException.ThrowIfNull(profile.Status.Fields);
        var fieldKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < profile.Status.Fields.Count; index++)
        {
            var field = profile.Status.Fields[index]
                ?? throw new InvalidDataException($"status.fields[{index}] must not be null.");
            RequireCanonicalNonBlank(field.Key, $"status.fields[{index}].key", 256);
            RequireCanonicalPresent(field.Value, $"status.fields[{index}].value", 4_096);
            if (!fieldKeys.Add(field.Key))
            {
                throw new InvalidDataException($"status.fields contains duplicate key '{field.Key}'.");
            }
        }
    }

    private static void RequireUpperHexSha256(string? value, string property)
    {
        RequireCanonicalNonBlank(value, property, 64);
        if (value!.Length != 64 || value.Any(character =>
                !(char.IsAsciiDigit(character) || character is >= 'A' and <= 'F')))
        {
            throw new InvalidDataException($"{property} must be exactly 64 uppercase hexadecimal characters.");
        }
    }

    private static void RequireSafeCode(string value, string property, Func<char, bool> allowed)
    {
        if (value.Any(character => !allowed(character)))
        {
            throw new InvalidDataException($"{property} contains a character outside its canonical code domain.");
        }
    }

    private static void RequirePathSegment(string value, string property)
    {
        if (value is "." or ".." ||
            value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            value.Contains(Path.DirectorySeparatorChar) ||
            value.Contains(Path.AltDirectorySeparatorChar))
        {
            throw new InvalidDataException($"{property} must be a single safe path segment.");
        }
    }

    private static int RequireDimension(string? value, string property)
    {
        RequireCanonicalNonBlank(value, property, 5);
        if (!int.TryParse(
                value,
                System.Globalization.NumberStyles.None,
                System.Globalization.CultureInfo.InvariantCulture,
                out var dimension) ||
            dimension is < 1 or > 32_768 ||
            !string.Equals(
                value,
                dimension.ToString(System.Globalization.CultureInfo.InvariantCulture),
                StringComparison.Ordinal))
        {
            throw new InvalidDataException($"{property} must be a canonical integer between 1 and 32768.");
        }

        return dimension;
    }

    private static void RequireOptionalDimension(string? value, string property)
    {
        RequireCanonicalPresent(value, property, 5);
        if (value!.Length > 0)
        {
            _ = RequireDimension(value, property);
        }
    }

    private static void RequireTrueFalse(string? value, string property) =>
        RequireOneOf(value, property, "TRUE", "FALSE");

    private static void RequireOneOf(string? value, string property, params string[] allowed)
    {
        RequireCanonicalNonBlank(value, property, allowed.Max(item => item.Length));
        if (!allowed.Contains(value!, StringComparer.Ordinal))
        {
            throw new InvalidDataException(
                $"{property} must be one of [{string.Join(", ", allowed)}].");
        }
    }

    private static void RequireCanonicalAbsolutePath(string? value, string property)
    {
        RequireCanonicalNonBlank(value, property, 32_767);
        if (!Path.IsPathFullyQualified(value!))
        {
            throw new InvalidDataException($"{property} must be a fully qualified path.");
        }

        string normalized;
        try
        {
            normalized = Path.TrimEndingDirectorySeparator(Path.GetFullPath(value!));
        }
        catch (Exception exception) when (
            exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            throw new InvalidDataException($"{property} is not a valid fully qualified path.", exception);
        }

        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!string.Equals(value, normalized, comparison))
        {
            throw new InvalidDataException($"{property} must be canonical and must not contain aliases or a trailing separator.");
        }
    }

    private static void RequireCanonicalNonBlank(string? value, string property, int maximumLength)
    {
        RequireCanonicalPresent(value, property, maximumLength);
        if (value!.Length == 0)
        {
            throw new InvalidDataException($"{property} must not be blank.");
        }
    }

    private static void RequireCanonicalPresent(string? value, string property, int maximumLength)
    {
        RequirePresent(value, property);
        if (!string.Equals(value, value!.Trim(), StringComparison.Ordinal))
        {
            throw new InvalidDataException($"{property} must not contain boundary whitespace.");
        }

        if (value.Length > maximumLength)
        {
            throw new InvalidDataException($"{property} exceeds the {maximumLength}-character safety limit.");
        }

        if (value.Contains('\r') || value.Contains('\n') ||
            value.Any(character => char.IsControl(character)))
        {
            throw new InvalidDataException($"{property} must not contain line breaks or control characters.");
        }

        try
        {
            System.Xml.XmlConvert.VerifyXmlChars(value);
        }
        catch (System.Xml.XmlException exception)
        {
            throw new InvalidDataException($"{property} contains characters that cannot be represented in XML.", exception);
        }
    }

    private static void RejectDuplicateProperties(JsonElement element, string path)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in element.EnumerateObject())
            {
                if (!names.Add(property.Name))
                {
                    throw new InvalidDataException($"Recovery profile contains duplicate property '{path}.{property.Name}'.");
                }

                RejectDuplicateProperties(property.Value, $"{path}.{property.Name}");
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            var index = 0;
            foreach (var item in element.EnumerateArray())
            {
                RejectDuplicateProperties(item, $"{path}[{index}]");
                index++;
            }
        }
    }

    private static void RequirePresent(string? value, string property)
    {
        if (value is null)
        {
            throw new InvalidDataException($"{property} must be present.");
        }
    }

    private static string OneLine(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Trim();
}
