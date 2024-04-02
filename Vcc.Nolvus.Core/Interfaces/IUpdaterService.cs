using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IUpdaterService : INolvusService
    {
        string Version { get; }
        string UpdaterExe { get; }
        bool IsOlder(string LatestVersion);
        bool Installed { get; }
        Task<bool> IsValid(string UpdaterCRC);
        Task Launch();
    }
}
