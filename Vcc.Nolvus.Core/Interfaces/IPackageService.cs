using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IPackageService : INolvusService
    {        
        Task Load(IInstallPackageDTO Package, Action<string, int> Progress = null);
        Task Merge(IEnumerable<IInstallPackageDTO> Packages, Action<string, int> Progress = null);
        Task InstallModList(ModInstallSettings Settings = null);
        int ModsCount { get; }
        List<IMOElement> GetInstallList();        
        List<string> GetOptionalEsps();
        List<string> LoadOrder { get; }
        List<IInstallableElement> InstallingModsQueue { get; }
        List<ModProgress> ProgressQueue { get; }
        string LoadedVersion { get; }
        List<string> GameBaseModsList { get; }
        List<string> GameBasePluginsList { get; }
    }
}
