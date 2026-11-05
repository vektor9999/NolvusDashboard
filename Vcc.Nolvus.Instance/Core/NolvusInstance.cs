using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Instance.Settings;
using Vcc.Nolvus.Instance.Options;
using Vcc.Nolvus.Instance.Status;


namespace Vcc.Nolvus.Instance.Core
{    
    public class NolvusInstance : INolvusInstance
    {
        #region Fields

        NolvusOptionsManager _OptionsManager;
        NolvusSettingsManager _SettingsManager;
        NolvusStatusManager _StatusManager;

        #endregion

        #region Properties  
              
        public string Id { get; set; } = string.Empty;        
        public string Name { get; set; } = string.Empty;
        private string _Tag = string.Empty;
        public string Tag
        {
            get
            {
                return _Tag;
            }

            set
            {
                _Tag = value;

                SettingsManager.InstallDir = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, string.Format("{0} - {1}", Name, _Tag));
                SettingsManager.StockGame = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, string.Format("{0} - {1}", Name, _Tag), "STOCK GAME");
            }
        }
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; }                                                              
        public DateTime LastUpdate { get; set; }                                   
        public string Overwrite
        {
            get
            {
                return Path.Combine(SettingsManager.InstallDir, "MODS", "overwrite");
            }
        }
        public string OverwriteSl
        {
            get
            {
                return Overwrite + "\\";
            }
        }
        public string InstanceVersion
        {
            get
            {
                return string.Format("{0} v{1}", SettingsManager.Variant, ServiceSingleton.Packages.LoadedVersion);
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} - {1} v{2}{3}", Name, SettingsManager.Variant, Version, Tag != string.Empty ? string.Format(" - ({0})", Tag) : string.Empty);
            }
        }

        public INolvusSettingsManager SettingsManager
        {
            get { return _SettingsManager; }
        }
        public INolvusOptionsManager OptionsManager
        {
            get { return _OptionsManager; }
        }
        public INolvusStatusManager StatusManager
        {
            get { return _StatusManager; }
        }

        #endregion

        public NolvusInstance()
        {
            _SettingsManager = new NolvusSettingsManager();                        
            _OptionsManager = new NolvusOptionsManager();
            _StatusManager = new NolvusStatusManager();
        }

        #region Methods

        public static async Task<NolvusInstance> CreateFromDTO(INolvusVersionDTO DTO)
        {
            var Instance = new NolvusInstance
            {
                Id = DTO.Id,                
                Name = DTO.Name,
                Description = DTO.Description                
            };

            Instance.SettingsManager.InstallDir = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, DTO.Name);
            Instance.SettingsManager.ArchiveDir = ServiceSingleton.Instances.ArchivesDirectory;
            Instance.SettingsManager.StockGame = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, DTO.Name, "STOCK GAME");

            await Instance.OptionsManager.LoadFromServer(Instance);            

            return Instance;
        }

        public async Task<bool> IsBeta()
        {
            var Package = await ApiManager.Service.Installer.GetPackage(Id, Version);

            return Package.IsBeta;
        }

        public async Task<string> GetState()
        {           
            var Package = await ApiManager.Service.Installer.GetLatestPackage(Id);            

            if (this.Version == Package.Version)
            {               
                return "Installed";                             
            }
            else
            {
                if (Package.IsBeta)
                {
                    return "New version available (" + Package.Version + " - Beta)";
                }

                return "New version available (" + Package.Version + ")";
            }
        }
        
        public async Task<string> GetLatestVersion()
        {
            var Package = await ApiManager.Service.Installer.GetLatestPackage(Id);

            return Package.Version;
        }
        
        public async Task<IInstallPackageDTO> GetLatestPackage()
        {
            return await ApiManager.Service.Installer.GetLatestPackage(Id);            
        }

        public async Task<bool> LatestPackageRequireNewGame()
        {
            return await ApiManager.Service.Installer.LatestPackageRequireNewGame(Id, Version);
        }

        public async Task<bool> LatestPackageRequireReInstall()
        {
            return await ApiManager.Service.Installer.LatestPackageRequireReInstall(Id, Version);
        }

        public InstanceCheckStatus Check()
        {
            if (Version == string.Empty)
            {
                return InstanceCheckStatus.VersionError;
            }
            else if (Id == string.Empty)
            {
                return InstanceCheckStatus.NoId;
            }

            return InstanceCheckStatus.OK;
        }

        public async Task Load(XmlNode Node)
        {
            Id = Node["Id"].InnerText.Trim();
            Name = Node["Name"].InnerText.Trim();
            Tag = Node["Tag"] != null ? Node["Tag"].InnerText.Trim() : string.Empty;
            Version = Node["Version"].InnerText.Trim();
            Description = Node["Description"].InnerText.Trim();


            ServiceSingleton.Logger.Log(string.Format("Instance Name : {0}", Name));
            ServiceSingleton.Logger.Log(string.Format("Instance Tag : {0}", Tag));
            ServiceSingleton.Logger.Log(string.Format("Instance Version : {0}", Version));


            _SettingsManager.Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Settings").FirstOrDefault());
            await _OptionsManager.Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Options").FirstOrDefault(), this);
            _StatusManager.Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Status").FirstOrDefault());
        }

        public void Save(XmlWriter XMLWriter)
        {            
            XMLWriter.WriteStartElement("Instance");

            XMLWriter.WriteStartElement("Id");
            XMLWriter.WriteString(Id.Trim());
            XMLWriter.WriteEndElement();           

            XMLWriter.WriteStartElement("Name");
            XMLWriter.WriteString(Name.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Tag");
            XMLWriter.WriteString(Tag.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Version");
            XMLWriter.WriteString(Version.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Description");
            XMLWriter.WriteString(Description.Trim());
            XMLWriter.WriteEndElement();            

            _SettingsManager.Save(XMLWriter);
            _OptionsManager.Save(XMLWriter);
            _StatusManager.Save(XMLWriter);                                  

            XMLWriter.WriteEndElement();
        }

        public async Task BackupGameSaveFiles()
        {
            await Task.Run(() =>
            {
                var BackupDirName = string.Format("backup-saves-{0}", DateTime.Now.ToString("yyyyddMHHmmss"));

                var SaveDir = Path.Combine(SettingsManager.InstallDir, "MODS", "profiles", Name, "saves");
                var BackupDir = Path.Combine(SettingsManager.InstallDir, "MODS", "profiles", Name, "saves", BackupDirName);                

                if (!ServiceSingleton.Files.IsDirectoryEmpty(SaveDir, SearchOption.TopDirectoryOnly))
                {
                    ServiceSingleton.Logger.Log(string.Format("Backuping game save files in {0}", BackupDir));

                    ServiceSingleton.Files.MoveFiles(SaveDir, BackupDir, SearchOption.TopDirectoryOnly);

                    ServiceSingleton.Logger.Log("Backup done");
                }
                else
                {
                    ServiceSingleton.Logger.Log(string.Format("No game save files to backup in {0}", SaveDir));
                }                
            });
        }

        #endregion
    }
}

