using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstanceStatus
    {
        string CurrentMod { get; set; }
        InstanceInstallStatus InstallStatus { get; set; }
        int TotalMods { get; set; }
        List<string> InstalledMods { get;}
    }
}
