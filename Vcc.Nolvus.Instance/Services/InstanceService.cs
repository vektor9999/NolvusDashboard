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
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Instance.Status;

namespace Vcc.Nolvus.Instance.Services
{
    public class InstanceService : IInstanceService
    {
        #region Fields

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

        private void MoveBuffer()
        {
            File.Move(BufferInstancesDataFile.FullName, InstancesDataFile.FullName);
        }

        private XmlDocument InitializeStorage()
        {
            XmlDocument Storage = new XmlDocument();

            if (NoInstanceFile())
            {
                if (!Directory.Exists(InstancesDirectory))
                {
                    Directory.CreateDirectory(InstancesDirectory);
                }
                
                Save();                
            }
            else if (OnlyBufferExists())
            {
                MoveBuffer();
            }

            Storage.Load(InstancesDataFile.FullName);

            return Storage;
        }

        private bool IsFileFromV3()
        {
            //Check if the InstancesData.xml file is still from Dashboard v3
            lock (SyncRoot)
            {
                XmlDocument Storage = this.InitializeStorage();

                XmlNodeList InstanceElements = Storage.SelectNodes("Instances/Instance");

                if (InstanceElements.Count > 0)
                {
                    return InstanceElements[0]["InstallPath"] != null;
                }
                else
                {
                    return false;
                }
            }

        }

        private void Loadv3()
        {   //Temporary method to migrate the InstancesData.xml from v3 to v4 new structure
            //Not the smoothest but will work until everyone has migrated
            lock (SyncRoot)
            {
                this.Instances.Clear();

                XmlDocument Storage = InitializeStorage();                

                XmlNodeList InstanceElements = Storage.SelectNodes("Instances/Instance");
                                
                foreach (XmlNode InstanceNode in InstanceElements)
                {
                    NolvusInstance Instance = new NolvusInstance();

                    XmlNode SettingsNode = InstanceNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Settings").FirstOrDefault();
                    XmlNode PerformanceNode = InstanceNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Performance").FirstOrDefault();
                    XmlNode OptionsNode = InstanceNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Options").FirstOrDefault();
                    XmlNode StatusNode = InstanceNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Status").FirstOrDefault();

                    Instance.Id = InstanceNode["Id"].InnerText.Trim();
                    Instance.Name = InstanceNode["Name"].InnerText.Trim();

                    if (InstanceNode["Tag"] != null)
                    {
                        Instance.Tag = InstanceNode["Tag"].InnerText.Trim();
                    }
                    
                    Instance.Version = InstanceNode["Version"].InnerText.Trim();
                    Instance.Description = InstanceNode["Description"].InnerText.Trim();

                    Instance.SettingsManager.InstallDir = InstanceNode["InstallPath"].InnerText.Trim();
                    Instance.SettingsManager.ArchiveDir = InstanceNode["ArchivePath"].InnerText.Trim();
                    Instance.SettingsManager.StockGame = InstanceNode["StockGame"].InnerText.Trim();                    
                    Instance.SettingsManager.Variant = PerformanceNode["Variant"].InnerText.Trim();
                    Instance.SettingsManager.Height = SettingsNode["Height"].InnerText.Trim();
                    Instance.SettingsManager.Width = SettingsNode["Width"].InnerText.Trim();
                    Instance.SettingsManager.DownScaling = PerformanceNode["DownScaling"].InnerText.Trim();
                    Instance.SettingsManager.DownHeight = PerformanceNode["DownHeight"].InnerText.Trim();
                    Instance.SettingsManager.DownWidth = PerformanceNode["DownWidth"].InnerText.Trim();
                    Instance.SettingsManager.IniSettings = PerformanceNode["IniSettings"].InnerText.Trim();
                    Instance.SettingsManager.EnableArchiving = System.Convert.ToBoolean(SettingsNode["EnableArchiving"].InnerText.Trim());
                    Instance.SettingsManager.CDN = SettingsNode["CDN"].InnerText.Trim();
                    Instance.SettingsManager.LgCode = SettingsNode["LgCode"].InnerText.Trim();
                    Instance.SettingsManager.LgName = SettingsNode["LgName"].InnerText.Trim();

                    Instance.OptionsManager.AddOption("AlternateENB", "ENB", "Visual", "string", OptionsNode["AlternateENB"].InnerText.Trim());
                    Instance.OptionsManager.AddOption("AlternateLeveling", "Alternate Leveling", "Gameplay", "bool", OptionsNode["AlternateLeveling"].InnerText.Trim());
                    Instance.OptionsManager.AddOption("AntiAliasing", "Anti Aliasing", "Performance", "list", PerformanceNode["AntiAliasing"].InnerText.Trim());
                    Instance.OptionsManager.AddOption("LODs", "LODs", "Performance", "list", PerformanceNode["LODs"].InnerText.Trim());
                    Instance.OptionsManager.AddOption("Nudity", "Nudity", "Visual", "bool", OptionsNode["Nudity"].InnerText.Trim());
                    Instance.OptionsManager.AddOption("Ratio", "Screen Ratio", "User Interface", "list", SettingsNode["Ratio"].InnerText.Trim());

                    switch (Instance.Name)
                    {
                        case Strings.NolvusAscension:
                            Instance.OptionsManager.AddOption("AlternateStart", "Alternate Start", "Gameplay", "bool", OptionsNode["AlternateStart"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("HardcoreMode", "Hardcore Mode", "Difficulty", "bool", OptionsNode["HardcoreMode"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("FantasyMode", "Fantasy Mode", "Gameplay", "bool", OptionsNode["FantasyMode"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("AdvancedPhysics", "Advanced Physics", "Performance", "bool", PerformanceNode["AdvancedPhysics"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("FPSStabilizer", "FPS Stabilizer", "Performance", "bool", PerformanceNode["FPSStabilizer"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("RayTracing", "Ray Tracing", "Performance", "bool", PerformanceNode["RayTracing"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("SkinType", "Skin Type", "Visual", "list", OptionsNode["SkinType"].InnerText.Trim());

                            break;

                        case Strings.NolvusAwakening:
                            Instance.OptionsManager.AddOption("Boss", "Boss Encounter", "Difficulty", "bool", OptionsNode["Boss"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("CombatAnimation", "Combat Animation", "Gameplay", "list", OptionsNode["CombatAnimation"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("CombatScaling", "Diffulty Preset", "Difficulty", "list", OptionsNode["CombatScaling"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("Controller", "Controller Support", "Gameplay", "bool", OptionsNode["Controller"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("Exhaustion", "Exhaustion", "Difficulty", "bool", OptionsNode["Exhaustion"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("FrameGeneration", "Frame Generation", "Performance", "bool", PerformanceNode["FrameGeneration"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("Gore", "Gore", "Gameplay", "bool", OptionsNode["Gore"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("NerfPA", "Nerf Power Attacks", "Difficulty", "list", OptionsNode["NerfPA"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("SREX", "SR Exterior Cities", "Performance", "bool", PerformanceNode["SREX"].InnerText.Trim());
                            Instance.OptionsManager.AddOption("UI", "User Interface", "User Interface", "list", OptionsNode["UI"].InnerText.Trim());

                            if (OptionsNode["ZoneLevelIndicator"] != null)
                            {
                                Instance.OptionsManager.AddOption("ZoneLevelIndicator", "Zone Level Indicator", "Difficulty", "bool", OptionsNode["ZoneLevelIndicator"].InnerText.Trim());
                            }
                            
                            break;
                    }

                    (Instance.StatusManager as NolvusStatusManager).Load(StatusNode);

                    Instances.Add(Instance);
                }
            }
        }

        public async Task Load()
        {
            Instances.Clear();

            var Storage = InitializeStorage();

            if (!IsFileFromV3())
            {
                foreach (XmlNode InstanceNode in Storage.SelectNodes("Instances/Instance"))
                {
                    NolvusInstance Instance = new NolvusInstance();

                    await Instance.Load(InstanceNode);

                    Instances.Add(Instance);
                }
            }
            else
            {
                Loadv3();
                Save();
            }
        }

        public void Save()
        {
            lock (SyncRoot)
            {                
                Encoding Utf8noBOM = new UTF8Encoding(false);
                XmlWriterSettings Settings = new XmlWriterSettings();
                Settings.Indent = true;
                Settings.Encoding = Utf8noBOM;

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


                    File.WriteAllBytes(BufferInstancesDataFile.FullName, Output.ToArray());                    
                }                
                
                if (InstancesDataFile.Exists)
                {
                    InstancesDataFile.Delete();                    
                }

                MoveBuffer();
            }
        }

        public List<INolvusInstance> InstancesToResume
        {
            get
            {
                return Instances.Where(x => x.StatusManager.InstallStatus != InstanceInstallStatus.Installed).Cast<INolvusInstance>().ToList();
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

                WorkingInstance.StatusManager.InstallStatus = InstanceInstallStatus.Installing;

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
                WorkingInstance.StatusManager.InstallStatus = InstanceInstallStatus.Updating;

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
                WorkingInstance.StatusManager.InstallStatus = InstanceInstallStatus.ChangingEnb;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void PrepareInstanceForOption()
        {
            if (WorkingInstance != null)
            {
                WorkingInstance.StatusManager.InstallStatus = InstanceInstallStatus.ChangingOption;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void FinalizeInstance()
        {
            WorkingInstance.StatusManager.InstallStatus = InstanceInstallStatus.Installed;
            WorkingInstance.StatusManager.InstalledMods.Clear();
            WorkingInstance.StatusManager.Fields.Clear();
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

        public bool PropertyExists(string Name)
        {
            var PropInInstance = typeof(INolvusInstance).GetProperty(Name) != null;
            var PropInSettings = typeof(INolvusSettingsManager).GetProperty(Name) != null;
            var PropInOptions = ServiceSingleton.Instances.WorkingInstance.OptionsManager.OptionExists(Name);

            return PropInInstance || PropInSettings || PropInOptions;
        }

        public string GetValueFromKey(string Key)
        {
            string EnvValue = Key;

            try
            {                     
                System.Reflection.PropertyInfo PropToCompare = typeof(INolvusInstance).GetProperty(Key);

                if (PropToCompare == null)
                {
                    PropToCompare = typeof(INolvusSettingsManager).GetProperty(Key);

                    if (PropToCompare == null)
                    {
                        EnvValue = ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue(Key);
                    }
                    else
                    {
                        EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.SettingsManager);
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

