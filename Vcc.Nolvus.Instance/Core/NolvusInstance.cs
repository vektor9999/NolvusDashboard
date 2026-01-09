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

namespace Vcc.Nolvus.Instance.Core
{    
    public class NolvusInstance : INolvusInstance
    {
        #region Properties

        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }        
        public string Version { get; set; } = string.Empty;        
        public string InstallDir { get; set; } = string.Empty;
        public string ArchiveDir { get; set; } = string.Empty;        
        public string StockGame { get; set; } = string.Empty;                                  
        public DateTime LastUpdate { get; set; }                                      
        public IInstanceSettings Settings { get; }
        public IInstancePerformance Performance { get; }
        public IInstanceOptions Options { get; }
        public IInstanceStatus Status { get; }        

        public string Overwrite
        {
            get
            {
                return Path.Combine(InstallDir, "MODS", "overwrite");
            }
        }

        public string OverwriteSl
        {
            get
            {
                return Overwrite + "\\";
            }
        }      

        #endregion

        public NolvusInstance()
        {
            Settings = new InstanceSettings();            
            Performance = new InstancePerformance();
            Options = new InstanceOptions();
            Status = new InstanceStatus();            
        }
        public NolvusInstance(INolvusVersionDTO DTO)
        {
            Id = DTO.Id;
            Code = DTO.Code;                   
            Name = DTO.Name;
            Description = DTO.Description;

            Settings = new InstanceSettings();
            Performance = new InstancePerformance();
            Options = new InstanceOptions();
            Status = new InstanceStatus();

            InstallDir = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, Name);
            ArchiveDir = ServiceSingleton.Instances.ArchivesDirectory;
            StockGame = Path.Combine(ServiceSingleton.Instances.InstancesDirectory, Name, "STOCK GAME");            
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
                return "New version available (" + Package.Version + ")";
            }
        }
        public async Task<bool> IsBeta()
        {
            var Package = await ApiManager.Service.Installer.GetLatestPackage(Id);

            return Package.IsBeta;
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

        public string GetSelectedResolution()
        {
            return string.Format("{0}x{1}", GetSelectedWidth(), GetSelectedHeight());
        }

        public string GetSelectedHeight()
        {
            if (Performance.DownScaling == "TRUE")
            {
                return Performance.DownHeight;
            }
            else
            {
                return Settings.Height;
            }
        }

        public string GetSelectedWidth()
        {
            if (Performance.DownScaling == "TRUE")
            {
                return Performance.DownWidth;
            }
            else
            {
                return Settings.Width;
            }
        }

        public InstanceCheckStatus Check()
        {
            if (Version == string.Empty)
            {
                return InstanceCheckStatus.VersionError;
            }
            else if ( Id == string.Empty)
            {
                return InstanceCheckStatus.NoId;
            }

            return InstanceCheckStatus.OK;
        }
        public void Load(XmlNode Node)
        {
            Id = Node["Id"].InnerText.Trim();            
            Name = Node["Name"].InnerText.Trim();
            Description = Node["Description"].InnerText.Trim();
            Version = Node["Version"].InnerText.Trim();            
            InstallDir = Node["InstallPath"].InnerText.Trim();
            ArchiveDir = Node["ArchivePath"].InnerText.Trim();
            StockGame = Node["StockGame"].InnerText.Trim();

            ServiceSingleton.Logger.Log(string.Format("Instance name : {0}", Name));
            ServiceSingleton.Logger.Log(string.Format("Instance version : {0}", Version));
            ServiceSingleton.Logger.Log(string.Format("Instance install directory : {0}", InstallDir));
            ServiceSingleton.Logger.Log(string.Format("Instance archive directory : {0}", ArchiveDir));

            (Settings as InstanceSettings).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Settings").FirstOrDefault());
            (Performance as InstancePerformance).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Performance").FirstOrDefault());
            (Options as InstanceOptions).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Options").FirstOrDefault());                                                                                   
            (Status as InstanceStatus).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Status").FirstOrDefault());                        
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

            XMLWriter.WriteStartElement("Description");
            XMLWriter.WriteString(Description.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Version");
            XMLWriter.WriteString(Version.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("InstallPath");
            XMLWriter.WriteString(InstallDir.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("ArchivePath");
            XMLWriter.WriteString(ArchiveDir.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("StockGame");
            XMLWriter.WriteString(StockGame.Trim());
            XMLWriter.WriteEndElement();

            (Settings as InstanceSettings).Save(XMLWriter);
            (Performance as InstancePerformance).Save(XMLWriter);
            (Options as InstanceOptions).Save(XMLWriter);
            (Status as InstanceStatus).Save(XMLWriter);            


            XMLWriter.WriteEndElement();
        }        
    }
}

