namespace Vcc.Nolvus.Recovery;

public enum CandidateKind
{
    Live,
    Temporary,
    Backup
}

public enum XmlCandidateStatus
{
    Missing,
    Empty,
    AllNull,
    DtdProhibited,
    Truncated,
    MalformedXml,
    WrongRoot,
    InvalidStructure,
    MissingRequiredNodes,
    InvalidRequiredValues,
    Valid,
    IoError
}

public sealed record XmlCandidateReport(
    CandidateKind Kind,
    string Path,
    XmlCandidateStatus Status,
    long Size,
    string? Sha256,
    int InstanceCount,
    IReadOnlyList<string> Evidence)
{
    public bool IsValid => Status == XmlCandidateStatus.Valid;
}

public sealed record LogInspectionReport(
    string Path,
    bool Exists,
    long LinesRead,
    IReadOnlyList<string> Queued,
    IReadOnlyList<string> Completed,
    IReadOnlyList<string> InFlight,
    IReadOnlyList<string> Evidence);

public enum ModFolderStatus
{
    Present,
    Empty,
    CorruptSampledAllNull,
    Inaccessible
}

public sealed record ModFolderReport(
    string Name,
    string Path,
    ModFolderStatus Status,
    long FileCount,
    int SampledFileCount,
    long SampledBytes,
    string Evidence);

public sealed record ModsAuditReport(
    bool Requested,
    IReadOnlyList<string> SearchRoots,
    IReadOnlyList<ModFolderReport> Folders,
    IReadOnlyList<string> Evidence)
{
    public int PresentCount => Folders.Count(x => x.Status == ModFolderStatus.Present);
    public int SuspiciousCount => Folders.Count(x =>
        x.Status is ModFolderStatus.Empty or ModFolderStatus.CorruptSampledAllNull or ModFolderStatus.Inaccessible);
}

public sealed record RecoveryInspectionReport(
    string RootPath,
    bool RootExists,
    IReadOnlyList<XmlCandidateReport> Candidates,
    LogInspectionReport Log,
    ModsAuditReport Mods,
    IReadOnlyList<string> Evidence);

public sealed class RecoveryInspectionOptions
{
    public bool AuditMods { get; init; }

    public int MaxSampleFilesPerMod { get; init; } = 4;

    public int SampleBytesPerRegion { get; init; } = 4096;
}
