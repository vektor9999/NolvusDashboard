using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusOptionDTO : IInstallerApiDTO
    {
        string Name { get; }
        string FriendlyName { get; }
        bool Beta { get; }
        bool NewGameRequired { get; }
        string Category { get; }
        bool Manageable { get; }
    }

    public class NolvusOptionObject : InstallerApiObject
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public bool Beta { get; set; }
        public bool NewGameRequired { get; set; }
        public string Category { get; set; }
        public bool Manageable { get; set; }
    }

    public class NolvusOptionDTO : NolvusOptionObject, INolvusOptionDTO
    {
        public string Id { get; set; }
    }
}
