using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer.Controllers;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;


namespace Vcc.Nolvus.Api.Installer.Controllers
{
    public class InstallerController : BaseController, IInstallerController
    {
        private string _UserName;

        public string LoggedUser
        {
            get
            {
                return _UserName;
            }
        }

        public InstallerController(IApiService Service, string Api)
            : base(Service, Api)
        {

        }

        public async Task<IEnumerable<INolvusVersionDTO>> GetNolvusVersions()
        {
            return await this._Service.GetPolyMorphic<List<NolvusVersionDTO>>($"{_Api}/getnolvusversions");
        }

        public async Task<IEnumerable<INolvusVariantDTO>> GetNolvusVariants()
        {
            return await this._Service.GetPolyMorphic<List<NolvusVariantDTO>>($"{_Api}/getnolvusvariants");
        }

        public async Task<IEnumerable<INolvusGPUDTO>> GetGPUs()
        {
            return await this._Service.GetPolyMorphic<List<NolvusGPUDTO>>($"{_Api}/getgpus");
        }

        public async Task<IEnumerable<INolvusVariantRequirementDTO>> GetNolvusVariantMinimumRequirements(string VariantId)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("VariantId", VariantId);

            return await this._Service.GetPolyMorphic<List<NolvusVariantRequirementDTO>>($"{_Api}/getnolvusvariantminrequirement", Params);
        }

        public async Task<IEnumerable<INolvusVariantRequirementDTO>> GetNolvusVariantRecommendedRequirements(string VariantId)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("VariantId", VariantId);

            return await this._Service.GetPolyMorphic<List<NolvusVariantRequirementDTO>>($"{_Api}/getnolvusvarianttecrequirement", Params);
        }

        public async Task<IEnumerable<INolvusVersionDTO>> GetDebugNolvusVersions()
        {
            return await this._Service.GetPolyMorphic<List<NolvusVersionDTO>>($"{_Api}/getdebugnolvusversions");
        }

        public async Task<IInstallPackageDTO> GetLatestPackage(string GuideId)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();            

            Params.Add("GuideId", GuideId);

            return await this._Service.GetPolyMorphic<InstallPackageDTO>($"{_Api}/getlatestpackage", Params);
        }

        public async Task<string> GetLatestPackageVersion(string GuideId)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("GuideId", GuideId);

            return await this._Service.GetUnRestricted<string>($"{_Api}/getlatestpackageversion", Params);
        }

        public async Task<IEnumerable<IInstallPackageDTO>> GetLatestPackages(string GuideId, string From)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("GuideId", GuideId);
            Params.Add("From", From);

            return await this._Service.GetPolyMorphic<List<InstallPackageDTO>>($"{_Api}/getlatestpackages", Params);
        }

        public async Task<IEnumerable<IInstallPackageDTO>> GetLatestPackagesTo(string GuideId, string From, string To)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("GuideId", GuideId);
            Params.Add("From", From);
            Params.Add("To", To);

            return await this._Service.GetPolyMorphic<List<InstallPackageDTO>>($"{_Api}/getlatestpackagesto", Params);
        }

        public async Task<IInstallPackageDTO> GetPackage(string GuideId, string Version)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("GuideId", GuideId);
            Params.Add("Version", Version);

            return await this._Service.GetPolyMorphic<InstallPackageDTO>($"{_Api}/getpackage", Params);
        }

        public async Task<bool> Authenticate(string UserName, string Password)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("UserName", UserName);
            Params.Add("Password", Password);

            var Result = await this._Service.GetUnRestricted<bool>($"{_Api}/validatelogin", Params);

            if (Result)
            {
                _UserName = UserName;
            }

            return Result;
        }
       
        public async Task<IDashBoardDTO> GetInstallerByVersion(string Version)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("Version", Version);

            return await this._Service.GetPolyMorphic<DashBoardDTO>($"{_Api}/getinstaller", Params);
        }

        public async Task<IDashBoardDTO> GetLatestInstaller()
        {
            return await this._Service.GetPolyMorphic<DashBoardDTO>($"{_Api}/getlatestinstaller");
        }

        public async Task<string> GetLatestInstallerVersion()
        {
            return await this._Service.GetUnRestricted<string>($"{_Api}/getlatestinstallerversion");
        }

        public async Task<string> GetLatestInstallerLink()
        {
            return await this._Service.GetUnRestricted<string>($"{_Api}/getlatestinstallerlink");
        }

        public async Task<string> GetLatestUpdaterVersion()
        {
            return await this._Service.GetUnRestricted<string>($"{_Api}/getlatestupdaterversion");
        }

        public async Task<string> GetLatestUpdaterLink()
        {
            return await this._Service.GetPolyMorphic<string>($"{_Api}/getlatestupdaterlink");
        }

        public async Task<IInstalledInstanceDTO> GetInstalledInstance(string UserName)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("UserName", UserName);

            return await this._Service.GetPolyMorphic<InstalledInstanceDTO>($"{_Api}/getinstalledinstance", Params);
        }

        public async Task<IEnumerable<IInstalledInstanceDTO>> GetInstalledInstances(string UserName)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("UserName", UserName);

            return await this._Service.GetPolyMorphic<List<InstalledInstanceDTO>>($"{_Api}/getinstalledinstances", Params);
        }

        public async Task<object> SetInstalledInstance(string Name, DateTime InstallDate, string UserName)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("Name", Name);
            Params.Add("InstallDate", InstallDate);
            Params.Add("UserName", UserName);

            return await this._Service.PostPolyMorphic($"{_Api}/setinstalledinstance", Params);
        }       

        public async Task<IGamePackageDTO> GetGamePackage(string Version)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("Version", Version);

            return await this._Service.GetPolyMorphic<GamePackageDTO>($"{_Api}/getgamepackage", Params);
        }

        public async Task<IGamePackageDTO> GetLatestGamePackage()
        {            
            return await this._Service.GetUnRestricted<GamePackageDTO>($"{_Api}/getlatestgamepackage");
        }

        public async Task<bool> LatestPackageRequireNewGame(string GuideId, string CurrentVersion)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("GuideId", GuideId);
            Params.Add("CurrentVersion", CurrentVersion);

            return await this._Service.GetPolyMorphic<bool>($"{_Api}/latestpackagenewgame", Params);
        }

        public async Task<bool> LatestPackageRequireReInstall(string GuideId, string CurrentVersion)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();

            Params.Add("GuideId", GuideId);
            Params.Add("CurrentVersion", CurrentVersion);

            return await this._Service.GetPolyMorphic<bool>($"{_Api}/latestpackagenewinstall", Params);
        }
    }
}
