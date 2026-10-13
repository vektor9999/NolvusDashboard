using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusOptionChoiceDTO : IInstallerApiDTO
    {        
        bool Beta { get; }
        string Value { get; }
    }

    public class NolvusOptionChoiceObject : InstallerApiObject
    {        
        public bool Beta { get; set; }
        public string Value { get; set; }
    }

    public class NolvusOptionChoiceDTO : NolvusOptionChoiceObject, INolvusOptionChoiceDTO
    {
        public string Id { get; set; }
    }
}
