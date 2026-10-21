using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;

namespace Vcc.Nolvus.Instance.Services
{
    public class InstanceService : IInstanceService
    {
        #region Fields

        private const int BackupRetentionCount = 3;
        private static readonly string[] RequiredInstanceElements =
        {
            "Id",
            "Name",
            "Description",
            "Version",
            "InstallPath",
            "ArchivePath",
            "StockGame",
            "Settings",
            "Performance",
            "Options",
            "Status"
        };

        private static readonly string[] RequiredSettingsElements =
        {
            "Ratio",
            "Height",
            "Width",
            "EnableArchiving",
            "CDN",
            "LgCode",
            "LgName"
        };

        private static readonly string[] RequiredPerformanceElements =
        {
            "AdvancedPhysics",
            "DownScaling",
            "DownHeight",
            "DownWidth",
            "IniSettings",
            "AntiAliasing",
            "Variant",
            "LODs",
            "RayTracing"
        };

        private static readonly string[] RequiredOptionsElements =
        {
            "Nudity",
            "AlternateENB",
            "FantasyMode",
            "HardcoreMode",
            "AlternateLeveling",
            "SkinType"
        };

        private static readonly string[] RequiredStatusElements =
        {
            "TotalMods",
            "InstallStatus",
            "InstalledMods"
        };

        private readonly object SyncRoot = new object();
        string _DataFilePath = string.Empty;
        public List<NolvusInstance> Instances = new List<NolvusInstance>();

        #endregion

        #region Properties

        public string InstancesDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Instances");
            }
        }

        public string ArchivesDirectory
        {
            get
            {
                return Path.Combine(InstancesDirectory, "ARCHIVE");
            }
        }

        protected FileInfo InstancesDataFile
        {
            get
            {
                return new FileInfo(Path.Combine(InstancesDirectory, "InstancesData.xml"));
            }
        }

        protected FileInfo BufferInstancesDataFile
        {
            get
            {
                return new FileInfo(Path.Combine(InstancesDirectory, "InstancesData.temp.xml"));
            }
        }

        protected bool NoInstanceFile()
        {
            return !InstancesDataFile.Exists && !BufferInstancesDataFile.Exists;
        }

        protected bool OnlyBufferExists()
        {
            return !InstancesDataFile.Exists && BufferInstancesDataFile.Exists;
        }

        public INolvusInstance WorkingInstance { get; set; }
        public IInstanceOptions Options { get; set; }

        public List<INolvusInstance> InstanceList { get { return Instances.Cast<INolvusInstance>().ToList(); } }

        #endregion

        #region Methods

        public InstanceService()
        {
            Directory.CreateDirectory(InstancesDirectory);
            Directory.CreateDirectory(ArchivesDirectory);
        }

        private byte[] SerializeInstances()
        {
            Encoding Utf8noBOM = new UTF8Encoding(false);
            XmlWriterSettings Settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Utf8noBOM,
                CloseOutput = false
            };

            using (MemoryStream Output = new MemoryStream())
            {
                using (XmlWriter XMLWriter = XmlWriter.Create(Output, Settings))
                {
                    XMLWriter.WriteStartDocument();
                    XMLWriter.WriteComment("Warning DO NOT MODIFY this file yourself, you will break your installation!!!");
                    XMLWriter.WriteStartElement("Instances");

                    foreach (NolvusInstance Instance in Instances)
                    {
                        Instance.Save(XMLWriter);
                    }

                    XMLWriter.WriteEndElement();
                    XMLWriter.WriteEndDocument();
                }

                return Output.ToArray();
            }
        }

        private static XmlDocument LoadValidatedDocument(byte[] Data)
        {
            using (MemoryStream Input = new MemoryStream(Data, false))
            {
                return LoadValidatedDocument(Input);
            }
        }

        private static XmlDocument LoadValidatedDocument(string Path)
        {
            using (FileStream Input = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return LoadValidatedDocument(Input);
            }
        }

        private static XmlDocument LoadValidatedDocument(Stream Input)
        {
            XmlReaderSettings Settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                CloseInput = false
            };

            XmlDocument Document = new XmlDocument
            {
                XmlResolver = null
            };

            using (XmlReader Reader = XmlReader.Create(Input, Settings))
            {
                Document.Load(Reader);
            }

            ValidateDocument(Document);
            return Document;
        }

        private static void ValidateDocument(XmlDocument Document)
        {
            if (Document.DocumentType != null)
            {
                throw new XmlException("Instances data must not contain a document type declaration.");
            }

            XmlElement Root = Document.DocumentElement;
            if (Root == null || Root.Name != "Instances" || Root.NamespaceURI != string.Empty)
            {
                throw new XmlException("Instances data must have an unqualified Instances root element.");
            }

            foreach (XmlNode Node in Root.ChildNodes)
            {
                if (Node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                XmlElement InstanceElement = (XmlElement)Node;
                if (InstanceElement.Name != "Instance" || InstanceElement.NamespaceURI != string.Empty)
                {
                    throw new XmlException("Instances data contains an unexpected element under the Instances root.");
                }

                foreach (string ElementName in RequiredInstanceElements)
                {
                    RequireSingleChildElement(InstanceElement, ElementName);
                }

                ValidateRequiredChildren(
                    RequireSingleChildElement(InstanceElement, "Settings"),
                    RequiredSettingsElements);
                ValidateRequiredChildren(
                    RequireSingleChildElement(InstanceElement, "Performance"),
                    RequiredPerformanceElements);
                ValidateRequiredChildren(
                    RequireSingleChildElement(InstanceElement, "Options"),
                    RequiredOptionsElements);
                ValidateRequiredChildren(
                    RequireSingleChildElement(InstanceElement, "Status"),
                    RequiredStatusElements);
                ValidateInstanceSemanticValues(InstanceElement);
            }
        }

        private static XmlElement RequireSingleChildElement(XmlElement Parent, string Name)
        {
            XmlElement Match = null;

            foreach (XmlNode Node in Parent.ChildNodes)
            {
                XmlElement Element = Node as XmlElement;
                if (Element == null || Element.Name != Name || Element.NamespaceURI != string.Empty)
                {
                    continue;
                }

                if (Match != null)
                {
                    throw new XmlException(string.Format("{0} contains more than one {1} element.", Parent.Name, Name));
                }

                Match = Element;
            }

            if (Match == null)
            {
                throw new XmlException(string.Format("{0} is missing the required {1} element.", Parent.Name, Name));
            }

            return Match;
        }

        private static void ValidateRequiredChildren(XmlElement Parent, string[] RequiredChildren)
        {
            foreach (string ChildName in RequiredChildren)
            {
                RequireSingleChildElement(Parent, ChildName);
            }
        }

        private static XmlElement GetOptionalSingleChildElement(XmlElement Parent, string Name)
        {
            XmlElement Match = null;

            foreach (XmlNode Node in Parent.ChildNodes)
            {
                XmlElement Element = Node as XmlElement;
                if (Element == null || Element.Name != Name || Element.NamespaceURI != string.Empty)
                {
                    continue;
                }

                if (Match != null)
                {
                    throw new XmlException(string.Format("{0} contains more than one {1} element.", Parent.Name, Name));
                }

                Match = Element;
            }

            return Match;
        }

        private static void ValidateInstanceSemanticValues(XmlElement InstanceElement)
        {
            XmlElement SettingsElement = RequireSingleChildElement(InstanceElement, "Settings");
            bool EnableArchiving;

            if (!bool.TryParse(
                RequireSingleChildElement(SettingsElement, "EnableArchiving").InnerText.Trim(),
                out EnableArchiving))
            {
                throw new XmlException("Settings/EnableArchiving must contain a Boolean value.");
            }

            XmlElement StatusElement = RequireSingleChildElement(InstanceElement, "Status");
            short TotalMods;

            if (!short.TryParse(
                RequireSingleChildElement(StatusElement, "TotalMods").InnerText.Trim(),
                out TotalMods))
            {
                throw new XmlException("Status/TotalMods must contain a valid Int16 value.");
            }

            if (TotalMods < 0)
            {
                throw new XmlException("Status/TotalMods must not be negative.");
            }

            string InstallStatus = RequireSingleChildElement(StatusElement, "InstallStatus").InnerText;
            object ParsedInstallStatus;

            try
            {
                ParsedInstallStatus = Enum.Parse(typeof(InstanceInstallStatus), InstallStatus);
            }
            catch (ArgumentException Exception)
            {
                throw new XmlException("Status/InstallStatus contains an invalid value.", Exception);
            }
            catch (OverflowException Exception)
            {
                throw new XmlException("Status/InstallStatus contains an invalid numeric value.", Exception);
            }

            if (!Enum.IsDefined(typeof(InstanceInstallStatus), ParsedInstallStatus))
            {
                throw new XmlException("Status/InstallStatus contains an undefined numeric value.");
            }

            XmlElement FieldsElement = GetOptionalSingleChildElement(StatusElement, "Fields");

            if (FieldsElement == null)
            {
                return;
            }

            foreach (XmlNode Node in FieldsElement.ChildNodes)
            {
                XmlElement FieldElement = Node as XmlElement;
                if (FieldElement == null || FieldElement.Name != "Field" || FieldElement.NamespaceURI != string.Empty)
                {
                    continue;
                }

                RequireSingleChildElement(FieldElement, "Key");
                RequireSingleChildElement(FieldElement, "Value");
            }
        }

        private static List<NolvusInstance> DeserializeInstances(XmlDocument Storage)
        {
            List<NolvusInstance> LoadedInstances = new List<NolvusInstance>();

            foreach (XmlNode InstanceNode in Storage.DocumentElement.ChildNodes)
            {
                if (InstanceNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                NolvusInstance Instance = new NolvusInstance();
                Instance.Load(InstanceNode);
                LoadedInstances.Add(Instance);
            }

            return LoadedInstances;
        }

        private static void WriteFileDurably(string Path, byte[] Data)
        {
            using (FileStream Output = new FileStream(
                Path,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                4096,
                FileOptions.WriteThrough))
            {
                Output.Write(Data, 0, Data.Length);
                Output.Flush(true);
            }
        }

        private string CreateUniqueTemporaryPath()
        {
            string Path;

            do
            {
                Path = System.IO.Path.Combine(
                    InstancesDirectory,
                    string.Format("InstancesData.temp.{0}.xml", Guid.NewGuid().ToString("N")));
            }
            while (File.Exists(Path));

            return Path;
        }

        private string CreateUniqueBackupPath()
        {
            string Path;

            do
            {
                Path = System.IO.Path.Combine(
                    InstancesDirectory,
                    string.Format(
                        "InstancesData.backup.{0}.{1}.xml",
                        DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                        Guid.NewGuid().ToString("N")));
            }
            while (File.Exists(Path));

            return Path;
        }

        private static bool IsGeneratedBackupPath(string Path)
        {
            const string Prefix = "InstancesData.backup.";
            const string Suffix = ".xml";
            const string TimestampFormat = "yyyyMMddHHmmssfff";
            const int TimestampLength = 17;
            const int GuidLength = 32;

            string FileName = System.IO.Path.GetFileName(Path);
            int ExpectedLength = Prefix.Length + TimestampLength + 1 + GuidLength + Suffix.Length;

            if (FileName == null ||
                FileName.Length != ExpectedLength ||
                !FileName.StartsWith(Prefix, StringComparison.Ordinal) ||
                !FileName.EndsWith(Suffix, StringComparison.Ordinal) ||
                FileName[Prefix.Length + TimestampLength] != '.')
            {
                return false;
            }

            string TimestampText = FileName.Substring(Prefix.Length, TimestampLength);
            DateTime Timestamp;

            if (!DateTime.TryParseExact(
                TimestampText,
                TimestampFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out Timestamp) ||
                !string.Equals(
                    Timestamp.ToString(TimestampFormat, System.Globalization.CultureInfo.InvariantCulture),
                    TimestampText,
                    StringComparison.Ordinal))
            {
                return false;
            }

            string GuidText = FileName.Substring(Prefix.Length + TimestampLength + 1, GuidLength);
            Guid ParsedGuid;

            return Guid.TryParseExact(GuidText, "N", out ParsedGuid) &&
                string.Equals(ParsedGuid.ToString("N"), GuidText, StringComparison.Ordinal);
        }

        private static bool IsGeneratedTemporaryPath(string Path)
        {
            const string Prefix = "InstancesData.temp.";
            const string Suffix = ".xml";
            const int GuidLength = 32;

            string FileName = System.IO.Path.GetFileName(Path);
            int ExpectedLength = Prefix.Length + GuidLength + Suffix.Length;

            if (FileName == null ||
                FileName.Length != ExpectedLength ||
                !FileName.StartsWith(Prefix, StringComparison.Ordinal) ||
                !FileName.EndsWith(Suffix, StringComparison.Ordinal))
            {
                return false;
            }

            string GuidText = FileName.Substring(Prefix.Length, GuidLength);
            Guid ParsedGuid;

            return Guid.TryParseExact(GuidText, "N", out ParsedGuid) &&
                string.Equals(ParsedGuid.ToString("N"), GuidText, StringComparison.Ordinal);
        }

        private List<string> GetTemporaryRecoveryCandidates()
        {
            List<string> Candidates = new List<string>();
            string LegacyTemporaryPath = BufferInstancesDataFile.FullName;

            if (File.Exists(LegacyTemporaryPath))
            {
                Candidates.Add(LegacyTemporaryPath);
            }

            foreach (string CandidatePath in Directory.GetFiles(InstancesDirectory, "InstancesData.temp.*.xml"))
            {
                if (IsGeneratedTemporaryPath(CandidatePath))
                {
                    Candidates.Add(CandidatePath);
                }
            }

            return Candidates
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static bool TryValidateRecoveryCandidate(string Path, out string Error)
        {
            try
            {
                LoadValidatedDocument(Path);
                Error = null;
                return true;
            }
            catch (XmlException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (InvalidDataException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (IOException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (UnauthorizedAccessException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (FormatException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (ArgumentException Exception)
            {
                Error = Exception.Message;
                return false;
            }
            catch (OverflowException Exception)
            {
                Error = Exception.Message;
                return false;
            }
        }

        private string SelectTemporaryRecoveryCandidate(List<string> Candidates)
        {
            List<string> ValidCandidates = new List<string>();
            List<string> CandidateDescriptions = new List<string>();

            foreach (string Candidate in Candidates)
            {
                string Error;

                if (TryValidateRecoveryCandidate(Candidate, out Error))
                {
                    ValidCandidates.Add(Candidate);
                    CandidateDescriptions.Add(string.Format("[valid] {0}", Candidate));
                }
                else
                {
                    CandidateDescriptions.Add(string.Format("[invalid: {0}] {1}", Error, Candidate));
                }
            }

            if (ValidCandidates.Count == 1)
            {
                return ValidCandidates[0];
            }

            throw new InvalidDataException(string.Format(
                "The live instances data file is missing, and a unique valid recovery candidate could not be selected. Candidates:{0}{1}",
                Environment.NewLine,
                string.Join(Environment.NewLine, CandidateDescriptions.ToArray())));
        }

        private string DescribeDiscoverableRecoveryCandidates()
        {
            try
            {
                string DataPath = InstancesDataFile.FullName;
                List<string> Candidates = Directory
                    .GetFiles(InstancesDirectory, "InstancesData*.xml")
                    .Where(x => !string.Equals(x, DataPath, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                return Candidates.Count == 0
                    ? "No recovery candidates were found."
                    : string.Format(
                        "Discoverable recovery candidates:{0}{1}",
                        Environment.NewLine,
                        string.Join(Environment.NewLine, Candidates.ToArray()));
            }
            catch (IOException Exception)
            {
                return string.Format("Recovery candidate discovery failed: {0}", Exception.Message);
            }
            catch (UnauthorizedAccessException Exception)
            {
                return string.Format("Recovery candidate discovery failed: {0}", Exception.Message);
            }
        }

        private void PruneValidatedBackups()
        {
            string[] BackupPaths;

            try
            {
                BackupPaths = Directory.GetFiles(InstancesDirectory, "InstancesData.backup.*.xml");
            }
            catch (IOException)
            {
                return;
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            List<FileInfo> ValidBackups = new List<FileInfo>();

            foreach (string BackupPath in BackupPaths)
            {
                if (!IsGeneratedBackupPath(BackupPath))
                {
                    continue;
                }

                try
                {
                    LoadValidatedDocument(BackupPath);
                    ValidBackups.Add(new FileInfo(BackupPath));
                }
                catch (XmlException)
                {
                    // Preserve unvalidated files for manual recovery; they do not count toward retention.
                }
                catch (InvalidDataException)
                {
                    // Preserve unvalidated files for manual recovery; they do not count toward retention.
                }
                catch (IOException)
                {
                    // A file that cannot be read safely must not be pruned.
                }
                catch (UnauthorizedAccessException)
                {
                    // A file that cannot be read safely must not be pruned.
                }
            }

            List<FileInfo> BackupsToDelete;

            try
            {
                BackupsToDelete = ValidBackups
                    .OrderByDescending(x => x.LastWriteTimeUtc)
                    .ThenByDescending(x => x.Name, StringComparer.Ordinal)
                    .Skip(BackupRetentionCount)
                    .ToList();
            }
            catch (IOException)
            {
                return;
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            foreach (FileInfo Backup in BackupsToDelete)
            {
                try
                {
                    Backup.Delete();
                }
                catch (IOException)
                {
                    // Retention is best effort after the new live document has committed successfully.
                }
                catch (UnauthorizedAccessException)
                {
                    // Retention is best effort after the new live document has committed successfully.
                }
            }
        }

        private void MoveBuffer()
        {
            string BufferPath = BufferInstancesDataFile.FullName;
            string DataPath = InstancesDataFile.FullName;

            LoadValidatedDocument(BufferPath);
            File.Move(BufferPath, DataPath);
            LoadValidatedDocument(DataPath);
        }

        private XmlDocument InitializeStorage()
        {
            if (!Directory.Exists(InstancesDirectory))
            {
                Directory.CreateDirectory(InstancesDirectory);
            }

            string DataPath = InstancesDataFile.FullName;

            if (!File.Exists(DataPath))
            {
                List<string> RecoveryCandidates = GetTemporaryRecoveryCandidates();

                if (RecoveryCandidates.Count == 0)
                {
                    Save();
                }
                else
                {
                    string RecoveryCandidate = SelectTemporaryRecoveryCandidate(RecoveryCandidates);
                    File.Move(RecoveryCandidate, DataPath);
                }
            }

            try
            {
                return LoadValidatedDocument(DataPath);
            }
            catch (XmlException Exception)
            {
                throw new InvalidDataException(string.Format(
                    "The live instances data file is invalid. {0}",
                    DescribeDiscoverableRecoveryCandidates()),
                    Exception);
            }
        }

        public void Load()
        {
            lock (SyncRoot)
            {
                XmlDocument Storage = InitializeStorage();
                List<NolvusInstance> LoadedInstances = DeserializeInstances(Storage);

                Instances.Clear();
                Instances.AddRange(LoadedInstances);
            }
        }

        public void Save()
        {
            lock (SyncRoot)
            {
                Directory.CreateDirectory(InstancesDirectory);

                byte[] Data = SerializeInstances();
                LoadValidatedDocument(Data);

                string TemporaryPath = CreateUniqueTemporaryPath();
                string DataPath = InstancesDataFile.FullName;
                Exception OriginalException = null;
                bool TemporaryFileValidated = false;
                bool CommitSucceeded = false;

                try
                {
                    WriteFileDurably(TemporaryPath, Data);
                    LoadValidatedDocument(TemporaryPath);
                    TemporaryFileValidated = true;

                    if (File.Exists(DataPath))
                    {
                        string BackupPath = CreateUniqueBackupPath();
                        File.Replace(TemporaryPath, DataPath, BackupPath);
                    }
                    else
                    {
                        File.Move(TemporaryPath, DataPath);
                    }

                    LoadValidatedDocument(DataPath);
                    CommitSucceeded = true;
                    PruneValidatedBackups();
                }
                catch (Exception Exception)
                {
                    OriginalException = Exception;
                    throw;
                }
                finally
                {
                    bool DeleteTemporaryFile = !TemporaryFileValidated || CommitSucceeded;

                    if (DeleteTemporaryFile && File.Exists(TemporaryPath))
                    {
                        try
                        {
                            File.Delete(TemporaryPath);
                        }
                        catch
                        {
                            if (OriginalException == null)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        public void RecordInstalledMod(string Name)
        {
            if (Name == null)
            {
                throw new ArgumentNullException("Name");
            }

            lock (SyncRoot)
            {
                if (WorkingInstance == null || WorkingInstance.Status == null || WorkingInstance.Status.InstalledMods == null)
                {
                    throw new InvalidOperationException("No working instance is available to record an installed mod.");
                }

                if (!WorkingInstance.Status.InstalledMods.Any(x => string.Equals(x, Name, StringComparison.Ordinal)))
                {
                    WorkingInstance.Status.InstalledMods.Add(Name);
                }

                Save();
            }
        }

        public List<INolvusInstance> InstancesToResume
        {
            get
            {
                return Instances.Where(x => x.Status.InstallStatus != InstanceInstallStatus.Installed).Cast<INolvusInstance>().ToList();
            }            
        }

        public bool CheckInstances(out string Status)
        {
            foreach (var Instance in Instances)
            {
                var CheckStatus = Instance.Check();

                switch (CheckStatus)
                {
                    case InstanceCheckStatus.NoId:
                        Status = "Instance " + Instance.Name + " has no valid Id!";
                        return false;

                    case InstanceCheckStatus.VersionError:
                        Status = "Instance " + Instance.Name + " has no valid version!";
                        return false;

                }
            }

            Status = string.Empty;

            return true;
        }

        public bool InstanceExists(string Name)
        {            
            return Instances.Where(x => x.Name == Name).FirstOrDefault() != null;
        }

        public bool InstanceExists(string Name, string Tag)
        {
            return Instances.Where(x => x.Name == Name && x.Tag == Tag).FirstOrDefault() != null;
        }

        public void RemoveInstance(INolvusInstance Instance)
        {
            Instances.Remove(Instance as NolvusInstance);

            Save();

            UnloadWorkingIntance();
        }

        public void PrepareInstanceForInstall()
        {
            if (WorkingInstance != null)
            {
                if(!InstanceExists(WorkingInstance.Name, WorkingInstance.Tag))
                {
                    Instances.Add(WorkingInstance as NolvusInstance);
                }

                WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Installing;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void PrepareInstanceForUpdate()
        {
            if (WorkingInstance != null)
            {                
                WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Updating;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void PrepareInstanceForEnb()
        {
            if (WorkingInstance != null)
            {
                WorkingInstance.Status.InstallStatus = InstanceInstallStatus.ChangingEnb;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public bool WorkingOnInstance
        {
            get
            {
                return WorkingInstance != null && (WorkingInstance.Status.InstallStatus == InstanceInstallStatus.Installing || WorkingInstance.Status.InstallStatus == InstanceInstallStatus.Updating);
            }            
        }

        public void FinalizeInstance()
        {
            WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Installed;
            WorkingInstance.Status.InstalledMods.Clear();
            WorkingInstance.Status.Fields.Clear();
            WorkingInstance.Version = ServiceSingleton.Packages.LoadedVersion;            

            Save();

            WorkingInstance = null;
        }

        public void UnloadWorkingIntance()
        {
            WorkingInstance = null;
        }

        public bool Empty
        {
            get { return Instances.Count == 0; }
        }             

        public string GetValueFromKey(string Key)
        {
            string EnvValue = Key;

            try
            {                     
                System.Reflection.PropertyInfo PropToCompare = typeof(INolvusInstance).GetProperty(Key);

                if (PropToCompare == null)
                {
                    PropToCompare = typeof(IInstanceSettings).GetProperty(Key);

                    if (PropToCompare == null)
                    {
                        PropToCompare = typeof(IInstancePerformance).GetProperty(Key);

                        if (PropToCompare == null)
                        {
                            PropToCompare = typeof(IInstanceOptions).GetProperty(Key);

                            EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Options);
                        }
                        else
                        {
                            EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Performance);
                        }
                    }
                    else
                    {
                        EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Settings);
                    }
                }
                else
                {
                    EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance);
                }
            }
            catch(Exception ex)
            {
                ServiceSingleton.Logger.Log(string.Format("Error during environment value checking (Key : {0}) with message {1}", Key, ex.Message));
                throw ex;
            }

            return EnvValue;   
        }

        public void SetCurrenTask(string Key, string Value)
        {
            
        }        

        #endregion
    }
}

