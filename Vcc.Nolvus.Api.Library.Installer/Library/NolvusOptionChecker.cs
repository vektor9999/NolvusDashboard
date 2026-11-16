using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusOptionCheckerDTO : IInstallerApiDTO
    {
        string OptionName { get; }
        string OptionFriendlyName { get; }
        string Value { get; }
        string FormattedValue { get; }
        bool Accept { get; }      
    }

    public class NolvusOptionCheckerObject : InstallerApiObject
    {
        public string OptionName { get; set; }
        public string OptionFriendlyName { get; set; }
        public string Value { get; set; }
        public string FormattedValue { get; set; }
        public bool Accept { get; set; }       
    }

    public class NolvusOptionCheckerDTO : NolvusOptionCheckerObject, INolvusOptionCheckerDTO
    {
        public string Id { get; set; }
    }
}
