using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Api.Installer.Controllers
{
    public interface IInstallerController    
    {        
        string LoggedUser { get; }
        Task<IEnumerable<INolvusVersionDTO>> GetNolvusVersions();
        Task<IEnumerable<INolvusVariantDTO>> GetNolvusVariants();
        Task<IEnumerable<INolvusGPUDTO>> GetGPUs();
        Task<IEnumerable<INolvusVariantRequirementDTO>> GetNolvusVariantMinimumRequirements(string VariantId);
        Task<IEnumerable<INolvusVariantRequirementDTO>> GetNolvusVariantRecommendedRequirements(string VariantId);
        Task<IEnumerable<INolvusVersionDTO>> GetDebugNolvusVersions();
        Task<IInstallPackageDTO> GetLatestPackage(string GuideId);
        Task<string> GetLatestPackageVersion(string GuideId);
        Task<IEnumerable<IInstallPackageDTO>> GetLatestPackages(string GuideId, string From);
        Task<IEnumerable<IInstallPackageDTO>> GetLatestPackagesTo(string GuideId, string From, string To);
        Task<IInstallPackageDTO> GetPackage(string GuideId, string Version);
        Task<bool> Authenticate(string UserName, string Password);        
        Task<IDashBoardDTO> GetInstallerByVersion(string Version);
        Task<IDashBoardDTO> GetLatestInstaller();
        Task<string> GetLatestInstallerVersion();
        Task<string> GetLatestInstallerLink();
        Task<string> GetLatestUpdaterVersion();
        Task<string> GetLatestUpdaterLink();
        Task<IInstalledInstanceDTO> GetInstalledInstance(string UserName);
        Task<IEnumerable<IInstalledInstanceDTO>> GetInstalledInstances(string UserName);
        Task<object> SetInstalledInstance(string Name, DateTime InstallDate, string UserName);
        Task<IGamePackageDTO> GetGamePackage(string Version);
        Task<IGamePackageDTO> GetLatestGamePackage();
        Task<bool> LatestPackageRequireNewGame(string GuideId, string CurrentVersion);        
    }
}
