using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface IDashBoardDTO : IInstallerApiDTO
    {
        string Version { get; }
        string DownloadLink { get; }            
        string UpdaterVersion { get;}
        string UpdaterLink { get; }
        string UpdaterHash { get; }
    }

    public class DashBoardObject : InstallerApiObject
    {
        public string Version { get; set; }
        public string DownloadLink { get; set; }
        public string UpdaterVersion { get; set; }
        public string UpdaterLink { get; set; }
        public string UpdaterHash { get; set; }
    }

    public class DashBoardDTO : DashBoardObject, IDashBoardDTO
    {
        public string Id { get; set; }    
    }
}
