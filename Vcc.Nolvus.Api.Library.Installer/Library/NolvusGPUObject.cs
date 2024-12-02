using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusGPUDTO : IInstallerApiDTO
    {
        string Vendor { get; }
        string Name { get; }

        int VRAM { get; }
        int Index { get; }       
    }

    public class NolvusGPUObject : InstallerApiObject
    {
        public string Vendor { get; set; }
        public string Name { get; set; }

        public int VRAM { get; set; }
        public int Index { get; set; }
    }

    public class NolvusGPUDTO : NolvusGPUObject, INolvusGPUDTO
    {        
        public string Id { get; set; }        
    }
}
