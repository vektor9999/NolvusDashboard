using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Instance.Services;

namespace Vcc.Nolvus.Instance.PersistenceTests
{
    internal static class Program
    {
        private static readonly string TestBaseDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string TestInstancesDirectory = Path.GetFullPath(
            Path.Combine(TestBaseDirectory, "Instances"));

        private static readonly Regex GeneratedBackupName = new Regex(
            @"^InstancesData\.backup\.\d{17}\.[0-9a-f]{32}\.xml$",
            RegexOptions.CultureInvariant);

        private static readonly Regex GeneratedTemporaryName = new Regex(
            @"^InstancesData\.temp\.[0-9a-f]{32}\.xml$",
            RegexOptions.CultureInvariant);

        private static int Passed;
        private static int Skipped;
        private static int Failed;
        private static bool ReplaceSupported;
        private static string ReplaceUnavailableReason;

        private static int Main()
        {
            ServiceSingleton.RegisterService<ILogService>(new StubLogService());

            try
            {
                ProbeReplaceSupport();
                Run("initial save produces valid XML", TestInitialSave);
                Run("second save creates a valid backup", TestSecondSaveCreatesBackup);
                Run("corrupt preimage is preserved", TestCorruptPreimagePreserved);
                Run("retention keeps three generated backups and preserves manual backup", TestRetentionAndManualBackup);
                Run("invalid live load leaves memory untouched", TestInvalidLoadPreservesMemory);
                Run("undefined numeric install status is rejected", TestUndefinedNumericInstallStatusRejected);
                Run("negative total mod count is rejected", TestNegativeTotalModsRejected);
                Run("unique retained temp promotes only when live is absent", TestUniqueTemporaryPromotion);
                Run("multiple valid temps fail without creating live", TestMultipleTemporaryCandidates);
                Run("parallel installed-mod recording retains every unique name", TestParallelInstalledMods);
                Run("replace failure preserves a validated temp", TestReplaceFailurePreservesTemporaryFile);
            }
            finally
            {
                try
                {
                    DeleteScopedInstancesDirectory();
                    Assert(!Directory.Exists(TestInstancesDirectory), "Final scoped test directory cleanup failed.");
                }
                catch (Exception Exception)
                {
                    Failed++;
                    Console.Error.WriteLine("FAIL final cleanup: {0}", Exception);
                }
            }

            Console.WriteLine("RESULT: {0} passed, {1} skipped, {2} failed", Passed, Skipped, Failed);
            return Failed == 0 ? 0 : 1;
        }

        private static void Run(string Name, Action Test)
        {
            try
            {
                PrepareScopedInstancesDirectory();
                Test();
                Passed++;
                Console.WriteLine("PASS {0}", Name);
            }
            catch (SkipTestException Exception)
            {
                Skipped++;
                Console.WriteLine("SKIP {0}: {1}", Name, Exception.Message);
            }
            catch (Exception Exception)
            {
                Failed++;
                Console.Error.WriteLine("FAIL {0}: {1}", Name, Exception);
            }
        }

        private static void TestInitialSave()
        {
            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("initial", out Instance);

            Service.Save();

            string LivePath = GetLivePath();
            Assert(File.Exists(LivePath), "Live data file was not created.");
            XmlDocument Document = LoadXml(LivePath);
            Assert(Document.SelectNodes("/Instances/Instance").Count == 1, "Expected one persisted instance.");
            Assert(GetGeneratedBackups().Count == 0, "Initial save must not create a backup.");
        }

        private static void TestSecondSaveCreatesBackup()
        {
            RequireReplaceSupport();

            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("second", out Instance);
            Service.Save();

            Instance.Description = "updated description";
            Service.Save();

            List<string> Backups = GetGeneratedBackups();
            Assert(Backups.Count == 1, "Second save should create exactly one generated backup.");
            Assert(
                LoadXml(Backups[0]).SelectSingleNode("/Instances/Instance/Description").InnerText == "description second",
                "Backup did not preserve the previous document generation.");
            Assert(
                LoadXml(GetLivePath()).SelectSingleNode("/Instances/Instance/Description").InnerText == "updated description",
                "Live document did not contain the new generation.");
        }

        private static void TestCorruptPreimagePreserved()
        {
            RequireReplaceSupport();

            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("corrupt", out Instance);
            byte[] CorruptBytes = Enumerable.Repeat((byte)0, 257).ToArray();
            File.WriteAllBytes(GetLivePath(), CorruptBytes);

            Service.Save();

            List<string> Backups = GetGeneratedBackups();
            Assert(Backups.Count == 1, "Corrupt preimage backup was not retained.");
            Assert(File.ReadAllBytes(Backups[0]).SequenceEqual(CorruptBytes), "Corrupt preimage bytes changed.");
            LoadXml(GetLivePath());
        }

        private static void TestRetentionAndManualBackup()
        {
            RequireReplaceSupport();

            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("retention", out Instance);
            Service.Save();

            string ManualBackup = Path.Combine(TestInstancesDirectory, "InstancesData.backup.manual-recovery.xml");
            File.Copy(GetLivePath(), ManualBackup);

            for (int Index = 0; Index < 7; Index++)
            {
                Instance.Description = string.Format("generation {0}", Index);
                Service.Save();
            }

            Assert(GetGeneratedBackups().Count == 3, "Generated valid backup retention must be exactly three.");
            Assert(File.Exists(ManualBackup), "Manual recovery backup was pruned.");
            LoadXml(ManualBackup);
        }

        private static void TestInvalidLoadPreservesMemory()
        {
            NolvusInstance Sentinel;
            InstanceService Service = CreateServiceWithInstance("sentinel", out Sentinel);
            File.WriteAllBytes(GetLivePath(), Enumerable.Repeat((byte)0, 64).ToArray());

            ExpectException<InvalidDataException>(() => Service.Load());

            Assert(Service.Instances.Count == 1, "Invalid load changed the in-memory instance count.");
            Assert(object.ReferenceEquals(Service.Instances[0], Sentinel), "Invalid load replaced the in-memory instance.");
        }

        private static void TestUndefinedNumericInstallStatusRejected()
        {
            CreateValidDocumentBytes("undefined-status");
            XmlDocument Document = LoadXml(GetLivePath());
            XmlNode InstallStatus = Document.SelectSingleNode("/Instances/Instance/Status/InstallStatus");
            Assert(InstallStatus != null, "Valid test document has no InstallStatus element.");
            InstallStatus.InnerText = "32767";
            Document.Save(GetLivePath());

            InstanceService Service = CreateRegisteredService();
            ExpectException<InvalidDataException>(() => Service.Load());
            Assert(Service.Instances.Count == 0, "Rejected install status changed the in-memory instance collection.");
        }

        private static void TestNegativeTotalModsRejected()
        {
            CreateValidDocumentBytes("negative-total");
            XmlDocument Document = LoadXml(GetLivePath());
            XmlNode TotalMods = Document.SelectSingleNode("/Instances/Instance/Status/TotalMods");
            Assert(TotalMods != null, "Valid test document has no TotalMods element.");
            TotalMods.InnerText = "-1";
            Document.Save(GetLivePath());

            InstanceService Service = CreateRegisteredService();
            ExpectException<InvalidDataException>(() => Service.Load());
            Assert(Service.Instances.Count == 0, "Rejected total mod count changed the in-memory instance collection.");
        }

        private static void TestUniqueTemporaryPromotion()
        {
            byte[] ValidDocument = CreateValidDocumentBytes("unique-temp");
            File.Delete(GetLivePath());

            string TemporaryPath = GetUniqueTemporaryPath();
            File.WriteAllBytes(TemporaryPath, ValidDocument);

            InstanceService Loader = CreateRegisteredService();
            Loader.Load();

            Assert(File.Exists(GetLivePath()), "Unique valid temporary file was not promoted.");
            Assert(!File.Exists(TemporaryPath), "Promoted temporary file still exists.");
            Assert(Loader.Instances.Count == 1 && Loader.Instances[0].Id == "unique-temp", "Promoted data was not loaded.");

            PrepareScopedInstancesDirectory();
            string CandidateBesideCorruptLive = GetUniqueTemporaryPath();
            File.WriteAllBytes(CandidateBesideCorruptLive, ValidDocument);
            byte[] CorruptLive = Enumerable.Repeat((byte)0, 91).ToArray();
            File.WriteAllBytes(GetLivePath(), CorruptLive);

            InstanceService CorruptLoader = CreateRegisteredService();
            InvalidDataException Error = ExpectException<InvalidDataException>(() => CorruptLoader.Load());

            Assert(File.ReadAllBytes(GetLivePath()).SequenceEqual(CorruptLive), "Corrupt live file was overwritten.");
            Assert(File.Exists(CandidateBesideCorruptLive), "Recovery candidate beside corrupt live was moved.");
            Assert(Error.Message.Contains(CandidateBesideCorruptLive), "Corrupt-live error did not surface the candidate.");
        }

        private static void TestMultipleTemporaryCandidates()
        {
            byte[] ValidDocument = CreateValidDocumentBytes("multiple-temp");
            File.Delete(GetLivePath());

            string First = GetUniqueTemporaryPath();
            string Second = GetUniqueTemporaryPath();
            File.WriteAllBytes(First, ValidDocument);
            File.WriteAllBytes(Second, ValidDocument);

            InstanceService Loader = CreateRegisteredService();
            InvalidDataException Error = ExpectException<InvalidDataException>(() => Loader.Load());

            Assert(!File.Exists(GetLivePath()), "Ambiguous temporary candidates created a live file.");
            Assert(File.Exists(First) && File.Exists(Second), "Ambiguous temporary candidates were modified.");
            Assert(Error.Message.Contains(First) && Error.Message.Contains(Second), "Ambiguity error did not name both candidates.");
        }

        private static void TestParallelInstalledMods()
        {
            RequireReplaceSupport();

            const int UniqueNames = 40;
            const int Records = 160;

            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("parallel", out Instance);
            Instance.Status.TotalMods = UniqueNames;

            Parallel.For(
                0,
                Records,
                Index => Service.RecordInstalledMod(string.Format("Mod-{0:D3}", Index % UniqueNames)));

            Assert(Instance.Status.InstalledMods.Count == UniqueNames, "Concurrent recording lost or duplicated names in memory.");
            Assert(Instance.Status.InstalledMods.Distinct(StringComparer.Ordinal).Count() == UniqueNames, "In-memory names are not unique.");

            XmlDocument Document = LoadXml(GetLivePath());
            List<string> PersistedNames = Document
                .SelectNodes("/Instances/Instance/Status/InstalledMods/InstalledMod")
                .Cast<XmlNode>()
                .Select(x => x.InnerText)
                .ToList();

            Assert(PersistedNames.Count == UniqueNames, "Concurrent recording lost or duplicated persisted names.");
            Assert(PersistedNames.Distinct(StringComparer.Ordinal).Count() == UniqueNames, "Persisted names are not unique.");
            Assert(GetGeneratedBackups().Count <= 3, "Concurrent saves exceeded generated backup retention.");
        }

        private static void TestReplaceFailurePreservesTemporaryFile()
        {
            RequireReplaceSupport();

            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance("locked", out Instance);
            Service.Save();

            string LivePath = GetLivePath();
            byte[] OriginalLive = File.ReadAllBytes(LivePath);
            Instance.Description = "validated pending generation";

            Exception SaveError = null;
            using (FileStream Lock = new FileStream(LivePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                try
                {
                    Service.Save();
                }
                catch (IOException Exception)
                {
                    SaveError = Exception;
                }
                catch (UnauthorizedAccessException Exception)
                {
                    SaveError = Exception;
                }
            }

            Assert(SaveError != null, "Exclusive live-file lock did not force Replace failure.");
            Assert(File.ReadAllBytes(LivePath).SequenceEqual(OriginalLive), "Failed replace changed the live document.");

            List<string> TemporaryFiles = GetGeneratedTemporaryFiles();
            Assert(TemporaryFiles.Count == 1, "Validated failed-commit temporary file was not preserved.");
            Assert(
                LoadXml(TemporaryFiles[0]).SelectSingleNode("/Instances/Instance/Description").InnerText == "validated pending generation",
                "Preserved temporary file does not contain the pending generation.");
        }

        private static InstanceService CreateServiceWithInstance(string Id, out NolvusInstance Instance)
        {
            InstanceService Service = CreateRegisteredService();
            Instance = new NolvusInstance
            {
                Id = Id,
                Name = string.Format("Instance {0}", Id),
                Description = string.Format("description {0}", Id),
                Version = "1.0.0",
                InstallDir = Path.Combine(TestInstancesDirectory, string.Format("Install-{0}", Id)),
                ArchiveDir = Path.Combine(TestInstancesDirectory, "ARCHIVE"),
                StockGame = Path.Combine(TestInstancesDirectory, string.Format("Stock-{0}", Id))
            };

            Service.Instances.Add(Instance);
            Service.WorkingInstance = Instance;
            return Service;
        }

        private static InstanceService CreateRegisteredService()
        {
            InstanceService Service = new InstanceService();
            ServiceSingleton.RegisterService<IInstanceService>(Service);
            return Service;
        }

        private static byte[] CreateValidDocumentBytes(string Id)
        {
            NolvusInstance Instance;
            InstanceService Service = CreateServiceWithInstance(Id, out Instance);
            Service.Save();
            return File.ReadAllBytes(GetLivePath());
        }

        private static XmlDocument LoadXml(string PathValue)
        {
            XmlReaderSettings Settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            XmlDocument Document = new XmlDocument
            {
                XmlResolver = null
            };

            using (XmlReader Reader = XmlReader.Create(PathValue, Settings))
            {
                Document.Load(Reader);
            }

            Assert(Document.DocumentElement != null && Document.DocumentElement.Name == "Instances", "XML root is not Instances.");
            return Document;
        }

        private static List<string> GetGeneratedBackups()
        {
            return Directory
                .GetFiles(TestInstancesDirectory, "InstancesData.backup.*.xml")
                .Where(x => GeneratedBackupName.IsMatch(Path.GetFileName(x)))
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<string> GetGeneratedTemporaryFiles()
        {
            return Directory
                .GetFiles(TestInstancesDirectory, "InstancesData.temp.*.xml")
                .Where(x => GeneratedTemporaryName.IsMatch(Path.GetFileName(x)))
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static string GetUniqueTemporaryPath()
        {
            return Path.Combine(
                TestInstancesDirectory,
                string.Format("InstancesData.temp.{0}.xml", Guid.NewGuid().ToString("N")));
        }

        private static string GetLivePath()
        {
            return Path.Combine(TestInstancesDirectory, "InstancesData.xml");
        }

        private static void ProbeReplaceSupport()
        {
            PrepareScopedInstancesDirectory();

            string Source = Path.Combine(TestInstancesDirectory, "replace-probe-source.txt");
            string Destination = Path.Combine(TestInstancesDirectory, "replace-probe-destination.txt");
            string Backup = Path.Combine(TestInstancesDirectory, "replace-probe-backup.txt");

            try
            {
                File.WriteAllText(Source, "source");
                File.WriteAllText(Destination, "destination");
                File.Replace(Source, Destination, Backup);
                ReplaceSupported = true;
            }
            catch (IOException Exception)
            {
                ReplaceUnavailableReason = string.Format("{0}: {1}", Exception.GetType().Name, Exception.Message);
            }
            catch (UnauthorizedAccessException Exception)
            {
                ReplaceUnavailableReason = string.Format("{0}: {1}", Exception.GetType().Name, Exception.Message);
            }
            catch (PlatformNotSupportedException Exception)
            {
                ReplaceUnavailableReason = string.Format("{0}: {1}", Exception.GetType().Name, Exception.Message);
            }
            finally
            {
                DeleteScopedInstancesDirectory();
            }
        }

        private static void RequireReplaceSupport()
        {
            if (!ReplaceSupported)
            {
                throw new SkipTestException(string.Format(
                    "System.IO.File.Replace is unavailable in the scoped test directory ({0}).",
                    ReplaceUnavailableReason ?? "unknown reason"));
            }
        }

        private static void PrepareScopedInstancesDirectory()
        {
            DeleteScopedInstancesDirectory();
            Directory.CreateDirectory(TestInstancesDirectory);
        }

        private static void DeleteScopedInstancesDirectory()
        {
            string ResolvedBase = TestBaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string ResolvedInstances = TestInstancesDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string Parent = Path.GetDirectoryName(ResolvedInstances);

            if (!string.Equals(Parent, ResolvedBase, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(Path.GetFileName(ResolvedInstances), "Instances", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Refusing to delete a path outside the harness AppDomain base Instances directory.");
            }

            if (Directory.Exists(ResolvedInstances))
            {
                Directory.Delete(ResolvedInstances, true);
            }
        }

        private static TException ExpectException<TException>(Action Action)
            where TException : Exception
        {
            try
            {
                Action();
            }
            catch (TException Exception)
            {
                return Exception;
            }

            throw new InvalidOperationException(string.Format("Expected {0}.", typeof(TException).FullName));
        }

        private static void Assert(bool Condition, string Message)
        {
            if (!Condition)
            {
                throw new InvalidOperationException(Message);
            }
        }

        private sealed class SkipTestException : Exception
        {
            public SkipTestException(string Message)
                : base(Message)
            {
            }
        }

        private sealed class StubLogService : ILogService
        {
            public void Log(string Message)
            {
            }

            public void LineBreak()
            {
            }

            public void ClearLog()
            {
            }
        }
    }
}
