using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface IInstallPackageDTO : IInstallerApiDTO
    {
        string InstallLink { get; }
        string UpdateLink { get; }
        string Version { get; }
        string Name { get; }
        DateTime LastUpdate { get; }
        string InstallerVersion { get; }
        bool Active { get; }
        bool IsBeta { get; }
        bool NewGame { get; }
        string DevLink { get; }
    }

    public class InstallPackageObject : InstallerApiObject
    {
        public string InstallLink { get; set; }
        public string UpdateLink { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public string InstallerVersion { get; set; }
        public bool Active { get; set; }
        public bool IsBeta { get; set; }
        public bool NewGame { get; set; }
        public string DevLink { get; set; }
    }

    public class InstallPackageDTO : InstallPackageObject, IInstallPackageDTO
    {
        public string Id { get; set; }    
    }
}
