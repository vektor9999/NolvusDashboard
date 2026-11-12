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
        string FullName { get; }
        string Tag { get; set; }
        string Version { get; set; }
        string Description { get;}
        DateTime LastUpdate { get; set; }
        string Overwrite { get; }        
        string OverwriteSl { get; }
        string InstanceVersion { get; }
        INolvusSettingsManager SettingsManager { get; }
        INolvusOptionsManager OptionsManager { get; }
        INolvusStatusManager StatusManager { get; }
        Task<bool> IsBeta();
        Task<string> GetState();
        Task<string> GetLatestVersion();
        Task<IInstallPackageDTO> GetLatestPackage();
        Task<bool> LatestPackageRequireNewGame();
        Task<bool> LatestPackageRequireReInstall();
        Task BackupGameSaveFiles(Action<int> Progress);
    }
}
