using System.Text;

namespace Vcc.Nolvus.Recovery;

public static class RecoveryReportFormatter
{
    public static string Format(RecoveryInspectionReport report)
    {
        ArgumentNullException.ThrowIfNull(report);
        var builder = new StringBuilder();

        builder.AppendLine("Nolvus Recovery Inspection");
        builder.AppendLine("==========================");
        builder.AppendLine($"Root: {report.RootPath}");
        builder.AppendLine($"Root exists: {YesNo(report.RootExists)}");
        builder.AppendLine("Mode: read-only");
        builder.AppendLine();

        builder.AppendLine("Instance data candidates");
        builder.AppendLine("------------------------");
        foreach (var candidate in report.Candidates)
        {
            builder.AppendLine($"- {candidate.Kind}: {candidate.Path}");
            builder.AppendLine($"  status={StatusText(candidate.Status)}; bytes={candidate.Size}; instances={candidate.InstanceCount}");
            if (candidate.Sha256 is not null)
            {
                builder.AppendLine($"  sha256={candidate.Sha256}");
            }

            foreach (var evidence in candidate.Evidence)
            {
                builder.AppendLine($"  evidence: {evidence}");
            }
        }

        builder.AppendLine();
        builder.AppendLine("Log evidence");
        builder.AppendLine("------------");
        builder.AppendLine($"Path: {report.Log.Path}");
        builder.AppendLine($"Exists: {YesNo(report.Log.Exists)}; lines={report.Log.LinesRead}");
        builder.AppendLine($"Unique queued: {report.Log.Queued.Count}");
        builder.AppendLine($"Unique completed: {report.Log.Completed.Count}");
        builder.AppendLine($"In-flight (queued minus completed): {report.Log.InFlight.Count}");
        foreach (var name in report.Log.InFlight)
        {
            builder.AppendLine($"- in-flight: {name}");
        }

        foreach (var evidence in report.Log.Evidence)
        {
            builder.AppendLine($"evidence: {evidence}");
        }

        builder.AppendLine();
        builder.AppendLine("MODS folder sampling");
        builder.AppendLine("--------------------");
        builder.AppendLine($"Requested: {YesNo(report.Mods.Requested)}");
        if (report.Mods.Requested)
        {
            builder.AppendLine($"Roots found: {report.Mods.SearchRoots.Count}");
            foreach (var root in report.Mods.SearchRoots)
            {
                builder.AppendLine($"- root: {root}");
            }

            builder.AppendLine($"Folders: {report.Mods.Folders.Count}; present-only={report.Mods.PresentCount}; suspicious={report.Mods.SuspiciousCount}");
            foreach (var folder in report.Mods.Folders.Where(x => x.Status != ModFolderStatus.Present))
            {
                builder.AppendLine($"- {FolderStatusText(folder.Status)}: {folder.Name}");
                builder.AppendLine($"  path={folder.Path}");
                builder.AppendLine($"  files={folder.FileCount}; sampled-files={folder.SampledFileCount}; sampled-bytes={folder.SampledBytes}");
                builder.AppendLine($"  evidence: {folder.Evidence}");
            }
        }

        foreach (var evidence in report.Mods.Evidence)
        {
            builder.AppendLine($"evidence: {evidence}");
        }

        builder.AppendLine();
        builder.AppendLine("Safety and scope");
        builder.AppendLine("----------------");
        foreach (var evidence in report.Evidence)
        {
            builder.AppendLine($"- {evidence}");
        }

        builder.AppendLine("- This inspect command does not generate or activate InstancesData.xml.");
        return builder.ToString();
    }

    private static string StatusText(XmlCandidateStatus status) => status switch
    {
        XmlCandidateStatus.AllNull => "all-NUL",
        XmlCandidateStatus.DtdProhibited => "DTD/external-entity input prohibited",
        XmlCandidateStatus.MalformedXml => "malformed XML",
        XmlCandidateStatus.WrongRoot => "wrong root",
        XmlCandidateStatus.InvalidStructure => "invalid XML structure",
        XmlCandidateStatus.MissingRequiredNodes => "missing required instance nodes",
        XmlCandidateStatus.InvalidRequiredValues => "invalid required instance values",
        XmlCandidateStatus.IoError => "I/O error",
        _ => status.ToString().ToLowerInvariant()
    };

    private static string FolderStatusText(ModFolderStatus status) => status switch
    {
        ModFolderStatus.CorruptSampledAllNull => "corrupt-suspected/all-NUL sample",
        ModFolderStatus.Inaccessible => "inaccessible",
        ModFolderStatus.Empty => "corrupt-suspected/empty",
        _ => "present-only"
    };

    private static string YesNo(bool value) => value ? "yes" : "no";
}
