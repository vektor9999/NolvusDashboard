using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface IInstalledInstanceDTO : IInstallerApiDTO
    {
        string Name { get; }
        DateTime InstallDate { get; }     
    }

    public class InstalledInstanceObject : InstallerApiObject
    {
        public string Name { get; set; }
        public DateTime InstallDate { get; set; }
    }

    public class InstalledInstanceDTO : InstalledInstanceObject, IInstalledInstanceDTO
    {
        public string Id { get; set; }    
    }
}
