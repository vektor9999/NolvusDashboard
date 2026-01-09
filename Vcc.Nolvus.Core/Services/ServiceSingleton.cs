using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Core.Services
{
    public static class ServiceSingleton
    {
        private static Dictionary<Type, object> _ServiceSingleton = new Dictionary<Type, object>();

        public static T RegisterService<T>(T Service) where T : INolvusService
        {
            _ServiceSingleton[typeof(T)] = Service;

            return GetService<T>();
        }

        public static T GetService<T>() where T : INolvusService
        {
            object Result = null;
            _ServiceSingleton.TryGetValue(typeof(T), out Result);
            return (T)Result;
        }

        public static IFolderService Folders
        {
            get
            {
                return ServiceSingleton.GetService<IFolderService>();
            }
        }

        public static IInstanceService Instances
        {
            get
            {
                return ServiceSingleton.GetService<IInstanceService>();
            }
        }

        public static IPackageService Packages
        {
            get
            {
                return ServiceSingleton.GetService<IPackageService>();
            }
        }

        public static IGlobalsService Globals
        {
            get
            {
                return ServiceSingleton.GetService<IGlobalsService>();
            }
        }

        public static ISettingsService Settings
        {
            get
            {
                return ServiceSingleton.GetService<ISettingsService>();
            }
        }

        public static IDashboard Dashboard
        {
            get
            {
                return ServiceSingleton.GetService<IDashboard>();
            }
        }
        public static IUpdaterService Updater
        {
            get
            {
                return ServiceSingleton.GetService<IUpdaterService>();
            }
        }

        public static ILibService Lib
        {
            get
            {
                return ServiceSingleton.GetService<ILibService>();
            }
        }

        public static ILogService Logger
        {
            get
            {
                return ServiceSingleton.GetService<ILogService>();
            }
        }

        public static IGameService Game
        {
            get
            {
                return ServiceSingleton.GetService<IGameService>();
            }
        }

        public static IFileService Files
        {
            get
            {
                return ServiceSingleton.GetService<IFileService>();
            }
        }

        public static ISoftwareProvider SoftwareProvider
        {
            get
            {
                return ServiceSingleton.GetService<ISoftwareProvider>();
            }
        }

        public static IReportService Report
        {
            get
            {
                return ServiceSingleton.GetService<IReportService>();
            }
        }

        public static ICheckerService CheckerService
        {
            get
            {
                return ServiceSingleton.GetService<ICheckerService>();
            }
        }

        public static IENBService EnbManager
        {
            get
            {
                return ServiceSingleton.GetService<IENBService>();
            }
        }
    }
}
