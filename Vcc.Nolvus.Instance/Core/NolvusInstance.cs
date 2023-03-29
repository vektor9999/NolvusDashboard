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
        public IInstanceStatus Status { get;}

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
            var Package = await ApiManager.Service.Installer.GetLatestPackage(this.Id);            

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
            var Package = await ApiManager.Service.Installer.GetLatestPackage(this.Id);

            return Package.IsBeta;
        }
        public async Task<string> GetLatestVersion()
        {
            var Package = await ApiManager.Service.Installer.GetLatestPackage(this.Id);

            return Package.Version;
        }
        
        public async Task<IInstallPackageDTO> GetLatestPackage()
        {
            return await ApiManager.Service.Installer.GetLatestPackage(this.Id);            
        }
              
        public InstanceCheckStatus Check()
        {
            if (this.Version == string.Empty)
            {
                return InstanceCheckStatus.VersionError;
            }
            else if ( this.Id == string.Empty)
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

            (Settings as InstanceSettings).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Settings").FirstOrDefault());
            (Performance as InstancePerformance).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Performance").FirstOrDefault());
            (Options as InstanceOptions).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Options").FirstOrDefault());                                                                                   
            (Status as InstanceStatus).Load(Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Status").FirstOrDefault());
            
        }
        public XmlNode Save(XmlDocument Storage)
        {
            var InstanceNode = Storage.CreateNode("element", "Instance", "");

            XmlNode IdNode = Storage.CreateNode("element", "Id", "");
            IdNode.InnerText = Id.Trim();
            InstanceNode.AppendChild(IdNode);

            XmlNode NameNode = Storage.CreateNode("element", "Name", "");
            NameNode.InnerText = Name.Trim();
            InstanceNode.AppendChild(NameNode);

            XmlNode DescNode = Storage.CreateNode("element", "Description", "");
            DescNode.InnerText = Description.Trim();
            InstanceNode.AppendChild(DescNode);

            XmlNode VersionNode = Storage.CreateNode("element", "Version", "");
            VersionNode.InnerText = Version.Trim();
            InstanceNode.AppendChild(VersionNode);          

            XmlNode InstallPathNode = Storage.CreateNode("element", "InstallPath", "");
            InstallPathNode.InnerText = InstallDir.Trim();
            InstanceNode.AppendChild(InstallPathNode);

            XmlNode ArchivePathNode = Storage.CreateNode("element", "ArchivePath", "");
            ArchivePathNode.InnerText = ArchiveDir.Trim();
            InstanceNode.AppendChild(ArchivePathNode);

            XmlNode StockGameNode = Storage.CreateNode("element", "StockGame", "");
            StockGameNode.InnerText = StockGame.Trim();
            InstanceNode.AppendChild(StockGameNode);

            InstanceNode.AppendChild((Settings as InstanceSettings).Save(Storage));            
            InstanceNode.AppendChild((Performance as InstancePerformance).Save(Storage));
            InstanceNode.AppendChild((Options as InstanceOptions).Save(Storage));
            InstanceNode.AppendChild((Status as InstanceStatus).Save(Storage));

            return InstanceNode;
        }
    }
}

