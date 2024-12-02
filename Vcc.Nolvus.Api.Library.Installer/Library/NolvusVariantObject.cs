using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusVariantDTO : IInstallerApiDTO
    {        
        string Name { get; }

        string Textures { get; }
        string Trees { get; }

        string Cities { get; }        
    }

    public class NolvusVariantObject : InstallerApiObject
    {        
        public string Name { get; set; }

        public string Textures { get; set; }
        public string Trees { get; set; }

        public string Cities { get; set; }       
    }

    public class NolvusVariantDTO : NolvusVariantObject, INolvusVariantDTO
    {        
        public string Id { get; set; }        
    }
}
