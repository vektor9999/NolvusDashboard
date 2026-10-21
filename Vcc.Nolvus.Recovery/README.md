# Nolvus instance recovery

This project adds a fail-closed recovery path for a damaged
`Instances\InstancesData.xml` and hardens normal dashboard persistence against
the same class of failure.

## Why this exists

The legacy save path wrote a shared temporary file, deleted the live ledger,
and then moved the temporary file into place. A process termination, power
loss, or storage failure during that sequence could leave the live ledger
missing or unreadable. The dashboard then failed during startup with an XML
error such as `Root element is missing`, even when downloaded archives and
installed mod payloads were still present.

The installer log contains a deterministic install-session marker and two
useful, independently reviewable checkpoints:

- `Start installing <instance name> - v <package version>...`
- `Mod : <name> added to queue.`
- `Mod : <name> installed.`

Recovery deliberately treats only an exact, case-sensitive `installed.` line
inside a hash-pinned, explicitly bounded package session as evidence that a
mod completed. Every checkpoint must also match the reviewed package manifest,
and a completion must have a queue entry in the same session. Folder presence
is never promoted to proof.

## What changed

### Crash-safe dashboard persistence

`InstanceService` now:

1. serializes and validates the complete document in memory;
2. writes a unique same-directory transaction file with `CreateNew`,
   `WriteThrough`, and `Flush(true)`;
3. parses and validates the transaction file;
4. atomically replaces the live ledger while retaining its previous generation;
5. parses the committed live ledger before reporting success; and
6. retains the three newest valid generated backups without pruning manual or
   invalid evidence files.

If a replacement fails after the transaction file is valid, the file is kept
for recovery. Startup promotes a temporary candidate only when the live ledger
is absent and exactly one candidate passes strict validation. Invalid loads do
not clear the in-memory instance list.

Installed-mod recording is also serialized through `RecordInstalledMod`, which
de-duplicates the checkpoint and persists it under the same service lock.

### Evidence-only recovery CLI

The .NET 8 recovery CLI provides two commands:

- `inspect` reads ledger candidates and installer checkpoints without changing
  the inspected installation.
- `reconstruct` creates a separate candidate XML and provenance audit from a
  reviewed profile plus exact log checkpoints.

The CLI refuses output inside the inspected Nolvus root, rejects reparse-point
paths, never overwrites output, and never activates a candidate. If a valid
live ledger contains other instances, reconstruction verifies its exact
instance inventory and preserves every non-target record. A missing or damaged
ledger fails closed unless the reviewed profile explicitly confirms that the
root contained only the target instance.

Exit code `0` means success (or help), `1` means a safety/runtime refusal, and
`2` means invalid command-line usage.

## Build

The recovery projects require the .NET 8 SDK.

```powershell
dotnet build '.\Nolvus Recovery.sln' -c Release
```

If the host has an MSBuild named-pipe restriction, add `--maxcpucount:1`.

## Recovery workflow

### 1. Quiesce and inspect

Close the dashboard, updater, launcher, and Mod Organizer before recovery.
Run inspection first:

```powershell
.\Vcc.Nolvus.Recovery.Cli\bin\Release\net8.0\Vcc.Nolvus.Recovery.Cli.exe `
  inspect --root 'C:\Nolvus'
```

Add `--audit-mods` for deterministic folder sampling. Sampling can identify an
empty or all-NUL payload, but a nonzero sample proves only that a folder exists.

Inspection writes nothing by default. `--report <path>` is the only write
option; the report must be a new path outside the inspected root.

### 2. Prepare and review a profile

Copy [`reconstruction-profile.example.json`](reconstruction-profile.example.json)
to a working directory outside `C:\Nolvus`. Fill every blank blocker from
authoritative package metadata, persisted settings, or explicit user choices.

The profile-v2 evidence block is deliberately strict:

- `logSha256` is the uppercase SHA-256 of the exact `Log.txt` being reviewed;
- `sessionStartLine` is the one-based line containing exactly
  `Start installing <instance.name> - v <packageVersion>...`;
- `sessionEndLine` is the last line admitted as evidence and must not cross a
  later install-session marker; and
- `packageModNames` is the complete, unique set of exact mod names from the
  authoritative package manifest for that version and selected configuration.

For example, capture the log hash without modifying the installation:

```powershell
(Get-FileHash -LiteralPath 'C:\Nolvus\Log.txt' -Algorithm SHA256).Hash
Select-String -LiteralPath 'C:\Nolvus\Log.txt' -Pattern '^Start installing '
```

Do not manufacture the package manifest from installed folders or from all
historical lines in the append-only log.

The `ledgerSafety` block prevents one recovered instance from erasing another:

- when the live ledger is valid, `expectedInstanceIds` must list every ID in
  its exact order and `singleInstanceOnlyConfirmed` remains `false`;
- when the live ledger is missing or invalid, reconstruction is allowed only
  when independent evidence confirms that the root contained one instance,
  `singleInstanceOnlyConfirmed` is `true`, and `expectedInstanceIds` contains
  only the recovered `instance.id`.

Keep `confirmed` set to `false` while reviewing. Set it to `true` only after:

- the log hash, session bounds, deterministic marker, and exact package
  manifest have been independently checked;
- the ledger inventory or single-instance assertion has been independently
  checked;
- `packageVersion` exactly matches `instance.version`;
- the instance ID, description, paths, language, performance, and option values
  are confirmed;
- `settings.enableArchiving`, `status.totalMods`, and every `status.fields`
  entry have been explicitly reviewed instead of inherited from the example;
- `status.totalMods` exactly equals the number of `packageModNames` entries;
- `stockGame` is `<installPath>\STOCK GAME`; and
- uncertain values have not been inferred from folder presence.

`status.totalMods` remains legacy metadata rather than the runtime resume
denominator. Profile v2 nevertheless requires it to equal the reviewed manifest
count so the reconstructed ledger is internally consistent. The installer
recomputes its progress total from the package plan when it resumes.

### 3. Reconstruct outside the installation

```powershell
.\Vcc.Nolvus.Recovery.Cli\bin\Release\net8.0\Vcc.Nolvus.Recovery.Cli.exe `
  reconstruct `
  --root 'C:\Nolvus' `
  --profile 'C:\Recovery\confirmed-profile.json' `
  --output 'C:\Recovery\InstancesData.candidate.xml' `
  --audit 'C:\Recovery\InstancesData.candidate.audit.json'
```

Both output paths must be new, distinct files with existing parent directories.
The candidate is parsed after it is written, and its on-disk length and SHA-256
must match the generated bytes. The audit records the profile and log hashes,
session boundary, package-manifest hash, ledger inventory decision, candidate
hash, ordered queue, exact completed checkpoints, and queued-minus-completed
retry entries.

### 4. Review before activation

At minimum, independently verify:

- candidate, profile, and log hashes match the audit;
- the candidate strictly parses and contains the expected instance/version;
- every non-target instance from a valid live ledger is unchanged;
- completed checkpoints and retry entries match a fresh log parse;
- every checkpoint is present in the reviewed package manifest;
- user-selected settings match the reviewed profile; and
- the live ledger and other evidence have not changed during reconstruction.

### 5. Activate as a separate operation

Activation is intentionally not a recovery CLI command. It changes the live
installation and must have separate operator approval. A safe deployment
procedure must preserve the current live file, durably stage the validated
candidate in the same directory, atomically replace the live path, and then
re-parse and hash both the new live file and preserved preimage. Never delete
the live file first, and never discard invalid preimages automatically.

After activation, run `inspect` again before launching the dashboard. On a
successful resume, the dashboard keeps the recovered completed checkpoints and
queues only unfinished work; it may also expand dependencies from the package
plan as installation continues.

## Tests

With the .NET 8 SDK installed, build and run the recovery suite explicitly (it
is a console harness, not a `dotnet test` project):

```powershell
dotnet build '.\Nolvus Recovery.sln' -c Release --maxcpucount:1
dotnet run --project '.\Vcc.Nolvus.Recovery.Tests\Vcc.Nolvus.Recovery.Tests.csproj' -c Release --no-build
```

The .NET Framework persistence harness is in
`Vcc.Nolvus.Instance.PersistenceTests`. It requires Windows, Visual Studio
MSBuild, and the .NET Framework 4.6.1, 4.6.2, and 4.7.2 targeting packs. On a
clean checkout, restore the legacy `packages.config` dependencies first. Build
the main solution so its ignored `Assemblies` outputs are current, then build
and run the harness:

```powershell
nuget restore '.\Nolvus Dashboard.sln'
msbuild '.\Nolvus Dashboard.sln' /p:Configuration=Release /m:1
msbuild '.\Vcc.Nolvus.Instance.PersistenceTests\Vcc.Nolvus.Instance.PersistenceTests.csproj' /p:Configuration=Release /m:1
.\Vcc.Nolvus.Instance.PersistenceTests\bin\Release\Vcc.Nolvus.Instance.PersistenceTests.exe
```

The harness covers atomic replacement, backup retention, corrupt-preimage
preservation, load rollback, temporary-file recovery, failed replacement, and
concurrent completion recording. It writes only beneath its build output.

## Safety boundary

Recovery artifacts can contain local paths and installation metadata. Keep
filled or incident-specific profiles, candidates, audits, reports, and
activation receipts outside source control. The repository ignores the local
`recovery-output` directory for that reason.
