using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IPackageService : INolvusService, ISoftwareProvider, IModsContainer
    {        
        Task Load(IInstallPackageDTO Package, Action<string, int> Progress = null);
        Task Merge(IEnumerable<IInstallPackageDTO> Packages, Action<string, int> Progress = null);        
        Task InstallModList(List<IInstallableElement> Mods = null, ModInstallSettings Settings = null);
        int ModsCount { get; }
        List<IMOElement> MO2List { get; }
        List<IMod> AllMods { get; }
        List<string> OptionalEsps { get; }
        List<string> LoadOrder { get; }
        List<IInstallableElement> InstallingModsQueue { get; }
        IProgressQueue ProgressQueue { get; }
        string LoadedVersion { get; }        
        double InstallProgression { get; }
        IErrorHandler ErrorHandler { get; }        
        bool Processing { get; }        
        IMod GetModByName(string Name);
    }
}
