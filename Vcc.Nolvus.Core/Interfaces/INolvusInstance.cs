using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface INolvusInstance
    {             
        string Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get;}
        string InstallDir { get; set; }
        string ArchiveDir { get; set; }
        string StockGame { get; set; }        
        DateTime LastUpdate { get; set; }
        IInstanceSettings Settings { get; }
        IInstancePerformance Performance { get; }
        IInstanceOptions Options { get; }
        IInstanceStatus Status { get; }
        Task<bool> IsBeta();
        Task<string> GetState();
        Task<string> GetLatestVersion();
        Task<IInstallPackageDTO> GetLatestPackage();
        Task<bool> LatestPackageRequireNewGame();
    }
}
