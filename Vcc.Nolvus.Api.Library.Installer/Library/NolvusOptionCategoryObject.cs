using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusOptionCategoryDTO : IInstallerApiDTO
    {
        string Name { get; }      
    }

    public class NolvusOptionCategoryObject : InstallerApiObject
    {
        public string Name { get; set; }       
    }

    public class NolvusOptionCategoryDTO : NolvusOptionCategoryObject, INolvusOptionCategoryDTO
    {
        public string Id { get; set; }
    }
}
