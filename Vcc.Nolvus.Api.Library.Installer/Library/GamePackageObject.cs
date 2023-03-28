using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Library
{    
    public interface IGamePackageDTO : IInstallerApiDTO
    {
        string Name { get; }
        string DownloadLink { get; }
        string Game { get; }
    }

    public class GamePackageObject : InstallerApiObject
    {
        public string Name { get; set; }
        public string DownloadLink { get; set; }
        public string Game { get; set; }
    }

    public class GamePackageDTO : GamePackageObject, IGamePackageDTO
    {
        public string Id { get; set; }
    }   
}
