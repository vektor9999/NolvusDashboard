using System.Security.Cryptography;
using System.Security;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Vcc.Nolvus.Recovery;

internal static class XmlCandidateInspector
{
    private const int MaximumCandidateBytes = 64 * 1024 * 1024;

    private static readonly string[] InstanceRequired =
    {
        "Id", "Name", "Description", "Version", "InstallPath", "ArchivePath", "StockGame",
        "Settings", "Performance", "Options", "Status"
    };

    private static readonly IReadOnlyDictionary<string, string[]> NestedRequired =
        new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["Settings"] = new[]
            {
                "Ratio", "Height", "Width", "EnableArchiving", "CDN", "LgCode", "LgName"
            },
            ["Performance"] = new[]
            {
                "AdvancedPhysics", "DownScaling", "DownHeight", "DownWidth", "IniSettings",
                "AntiAliasing", "Variant", "LODs", "RayTracing"
            },
            ["Options"] = new[]
            {
                "Nudity", "AlternateENB", "FantasyMode", "HardcoreMode", "AlternateLeveling", "SkinType"
            },
            ["Status"] = new[]
            {
                "TotalMods", "InstallStatus", "InstalledMods"
            }
        };

    public static XmlCandidateReport Inspect(CandidateKind kind, string path)
    {
        try
        {
            byte[] bytes;
            using (var stream = OpenRead(path))
            {
                if (stream.Length > MaximumCandidateBytes)
                {
                    return Report(
                        kind,
                        path,
                        XmlCandidateStatus.IoError,
                        stream.Length,
                        null,
                        0,
                        $"Candidate exceeds the {MaximumCandidateBytes}-byte safety limit.");
                }

                bytes = new byte[(int)stream.Length];
                stream.ReadExactly(bytes);
            }

            if (bytes.Length == 0)
            {
                return Report(kind, path, XmlCandidateStatus.Empty, 0, EmptySha256(), 0, "File contains zero bytes.");
            }

            var hash = Convert.ToHexString(SHA256.HashData(bytes));
            var allNull = bytes.All(value => value == 0);

            if (allNull)
            {
                return Report(
                    kind,
                    path,
                    XmlCandidateStatus.AllNull,
                    bytes.LongLength,
                    hash,
                    0,
                    $"Every one of the {bytes.Length} bytes is NUL (0x00)." );
            }

            XDocument document;
            try
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit,
                    XmlResolver = null,
                    MaxCharactersInDocument = 64L * 1024L * 1024L,
                    IgnoreComments = false,
                    IgnoreWhitespace = false
                };

                using var stream = new MemoryStream(bytes, writable: false);
                using var reader = XmlReader.Create(stream, settings);
                document = XDocument.Load(reader, LoadOptions.SetLineInfo);
            }
            catch (XmlException exception)
            {
                var status = ClassifyXmlException(exception);
                return Report(kind, path, status, bytes.LongLength, hash, 0, OneLine(exception.Message));
            }

            if (document.Root is null)
            {
                return Report(kind, path, XmlCandidateStatus.Empty, bytes.LongLength, hash, 0, "Document has no root element.");
            }

            if (document.Root.Name != "Instances")
            {
                return Report(
                    kind,
                    path,
                    XmlCandidateStatus.WrongRoot,
                    bytes.LongLength,
                    hash,
                    0,
                    $"Expected root element 'Instances'; found '{document.Root.Name}'.");
            }

            var unexpectedRootChildren = document.Root.Elements()
                .Where(element => element.Name != "Instance")
                .Select(element => element.Name.ToString())
                .ToArray();
            if (unexpectedRootChildren.Length > 0)
            {
                return new XmlCandidateReport(
                    kind,
                    path,
                    XmlCandidateStatus.InvalidStructure,
                    bytes.LongLength,
                    hash,
                    0,
                    unexpectedRootChildren
                        .Select(name => $"Instances contains unexpected element '{name}'.")
                        .ToArray());
            }

            var instances = document.Root.Elements("Instance").ToList();
            var missing = ValidateRequiredNodes(instances);
            if (missing.Count > 0)
            {
                return new XmlCandidateReport(
                    kind,
                    path,
                    XmlCandidateStatus.MissingRequiredNodes,
                    bytes.LongLength,
                    hash,
                    instances.Count,
                    missing);
            }

            var invalid = ValidateRequiredValues(instances);
            if (invalid.Count > 0)
            {
                return new XmlCandidateReport(
                    kind,
                    path,
                    XmlCandidateStatus.InvalidRequiredValues,
                    bytes.LongLength,
                    hash,
                    instances.Count,
                    invalid);
            }

            return Report(
                kind,
                path,
                XmlCandidateStatus.Valid,
                bytes.LongLength,
                hash,
                instances.Count,
                $"Strict XML parse succeeded with {instances.Count} Instance element(s)." );
        }
        catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
        {
            return Report(kind, path, XmlCandidateStatus.Missing, 0, null, 0, "File does not exist.");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
        {
            return Report(kind, path, XmlCandidateStatus.IoError, 0, null, 0, OneLine(exception.Message));
        }
    }

    private static FileStream OpenRead(string path) => new(
        path,
        FileMode.Open,
        FileAccess.Read,
        FileShare.Read,
        81920,
        FileOptions.SequentialScan);

    private static List<string> ValidateRequiredNodes(IReadOnlyList<XElement> instances)
    {
        var missing = new List<string>();

        for (var index = 0; index < instances.Count; index++)
        {
            var instance = instances[index];
            foreach (var name in InstanceRequired)
            {
                var count = instance.Elements(name).Count();
                if (count == 0)
                {
                    missing.Add($"Instance[{index + 1}] is missing required element '{name}'.");
                }
                else if (count > 1)
                {
                    missing.Add($"Instance[{index + 1}] contains {count} copies of required element '{name}'; expected exactly one.");
                }
            }

            foreach (var (parentName, childNames) in NestedRequired)
            {
                var parent = instance.Element(parentName);
                if (parent is null)
                {
                    continue;
                }

                foreach (var childName in childNames)
                {
                    var count = parent.Elements(childName).Count();
                    if (count == 0)
                    {
                        missing.Add($"Instance[{index + 1}]/{parentName} is missing required element '{childName}'.");
                    }
                    else if (count > 1)
                    {
                        missing.Add($"Instance[{index + 1}]/{parentName} contains {count} copies of required element '{childName}'; expected exactly one.");
                    }
                }
            }

            var fields = instance.Element("Status")?.Element("Fields");
            if (fields is not null)
            {
                var fieldIndex = 0;
                foreach (var field in fields.Elements("Field"))
                {
                    fieldIndex++;
                    foreach (var childName in new[] { "Key", "Value" })
                    {
                        var count = field.Elements(childName).Count();
                        if (count == 0)
                        {
                            missing.Add($"Instance[{index + 1}]/Status/Fields/Field[{fieldIndex}] is missing required element '{childName}'.");
                        }
                        else if (count > 1)
                        {
                            missing.Add($"Instance[{index + 1}]/Status/Fields/Field[{fieldIndex}] contains {count} copies of required element '{childName}'; expected exactly one.");
                        }
                    }
                }
            }

            RequireNonBlank(instance, "Id", index, missing);
            RequireNonBlank(instance, "Version", index, missing);
        }

        return missing;
    }

    private static List<string> ValidateRequiredValues(IReadOnlyList<XElement> instances)
    {
        var invalid = new List<string>();
        var installStatuses = new HashSet<string>(
            new[] { "None", "Installing", "Installed", "Updating", "ChangingEnb" },
            StringComparer.Ordinal);

        for (var index = 0; index < instances.Count; index++)
        {
            var instance = instances[index];
            var enableArchiving = instance.Element("Settings")!.Element("EnableArchiving")!.Value.Trim();
            if (!bool.TryParse(enableArchiving, out _))
            {
                invalid.Add($"Instance[{index + 1}]/Settings/EnableArchiving value '{enableArchiving}' is not Boolean.");
            }

            var totalMods = instance.Element("Status")!.Element("TotalMods")!.Value.Trim();
            if (!short.TryParse(totalMods, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedTotalMods))
            {
                invalid.Add($"Instance[{index + 1}]/Status/TotalMods value '{totalMods}' is not a 16-bit integer.");
            }
            else if (parsedTotalMods < 0)
            {
                invalid.Add($"Instance[{index + 1}]/Status/TotalMods value '{totalMods}' must not be negative.");
            }

            var installStatus = instance.Element("Status")!.Element("InstallStatus")!.Value.Trim();
            if (!installStatuses.Contains(installStatus))
            {
                invalid.Add($"Instance[{index + 1}]/Status/InstallStatus value '{installStatus}' is not recognized.");
            }
        }

        return invalid;
    }

    private static void RequireNonBlank(XElement instance, string name, int index, ICollection<string> evidence)
    {
        var element = instance.Element(name);
        if (element is not null && string.IsNullOrWhiteSpace(element.Value))
        {
            evidence.Add($"Instance[{index + 1}] required element '{name}' is blank.");
        }
    }

    private static XmlCandidateStatus ClassifyXmlException(XmlException exception)
    {
        if (exception.Message.Contains("DTD", StringComparison.OrdinalIgnoreCase) ||
            exception.Message.Contains("DOCTYPE", StringComparison.OrdinalIgnoreCase))
        {
            return XmlCandidateStatus.DtdProhibited;
        }

        if (exception.Message.Contains("unexpected end", StringComparison.OrdinalIgnoreCase) ||
            exception.Message.Contains("unclosed", StringComparison.OrdinalIgnoreCase))
        {
            return XmlCandidateStatus.Truncated;
        }

        if (exception.Message.Contains("root element is missing", StringComparison.OrdinalIgnoreCase))
        {
            return XmlCandidateStatus.Empty;
        }

        return XmlCandidateStatus.MalformedXml;
    }

    private static XmlCandidateReport Report(
        CandidateKind kind,
        string path,
        XmlCandidateStatus status,
        long size,
        string? sha256,
        int instanceCount,
        string evidence) => new(kind, path, status, size, sha256, instanceCount, new[] { evidence });

    private static string EmptySha256() => Convert.ToHexString(SHA256.HashData(Array.Empty<byte>()));

    private static string OneLine(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Trim();
}
