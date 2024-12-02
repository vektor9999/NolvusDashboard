using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusVersionDTO : IInstallerApiDTO
    {
        string Code { get; }
        string Name { get; }

        string MinCPU { get; }
        string MaxCPU { get; }

        string MinGPU { get; }
        string MaxGPU { get; }

        string MinRAM { get; }
        string MaxRAM { get; }

        string MinVRAM { get; }
        string MaxVRAM { get; }

        string ModsStorageSpace { get; }
        string ArchiveStorageSpace { get; }
        string Description { get;}
        string Version { get; }
        bool IsBeta { get;}
        string Image { get; }
        Image ImageObject { get; set; }

        bool Maintenance { get; set; }
    }

    public class NolvusVersionObject : InstallerApiObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string MinCPU { get; set; }
        public string MaxCPU { get; set; }

        public string MinGPU { get; set; }
        public string MaxGPU { get; set; }

        public string MinRAM { get; set; }
        public string MaxRAM { get; set; }

        public string MinVRAM { get; set; }
        public string MaxVRAM { get; set; }

        public string ModsStorageSpace { get; set; }
        public string ArchiveStorageSpace { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public bool IsBeta { get; set; }
        public string Image { get; set; }

        public bool Maintenance { get; set; }
    }

    public class NolvusVersionDTO : NolvusVersionObject, INolvusVersionDTO
    {        
        public string Id { get; set; }
        public Image ImageObject { get; set; }
    }
}
