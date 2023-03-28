using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;


namespace Vcc.Nolvus.Dashboard.Frames
{
    public partial class PackageFrame : DashboardFrame
    {
        public PackageFrame()
        {
            InitializeComponent(); 
        }

        public PackageFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override async Task OnLoadedAsync()        
        {
            ServiceSingleton.Dashboard.Info("Package loading...");

            try
            {
                try
                {
                    INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;
                    
                    if (Instance.Status.InstallStatus == InstanceInstallStatus.None)
                    {
                        #region New Installation

                        IInstallPackageDTO Package = await ApiManager.Service.Installer.GetLatestPackage(Instance.Id);

                        Instance.Version = Package.Version;

                        await ServiceSingleton.Packages.Load(Package, (s, p) =>
                        {
                            ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                            ServiceSingleton.Dashboard.Progress(p);
                        });

                        await ServiceSingleton.Dashboard.LoadFrameAsync<StockGameFrame>();

                        #endregion
                    }
                    else if (Instance.Status.InstallStatus == InstanceInstallStatus.Installing)
                    {
                        #region Resume Installation                        

                        await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
                        {
                            ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                            ServiceSingleton.Dashboard.Progress(p);
                        });

                        await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();

                        #endregion
                    }
                    else if (Instance.Status.InstallStatus == InstanceInstallStatus.Updating)
                    {
                        #region Update

                        await ServiceSingleton.Packages.Merge(await ApiManager.Service.Installer.GetLatestPackages(Instance.Id, Instance.Version), (s, p) =>
                        {
                            ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                            ServiceSingleton.Dashboard.Progress(p);
                        });

                        await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();

                        #endregion
                    }
                    else
                    {
                        #region Installed Instance

                        await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
                        {
                            ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                            ServiceSingleton.Dashboard.Progress(p);
                        });

                        await ServiceSingleton.Dashboard.LoadFrameAsync<InstanceDetailFrame>();

                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    await ServiceSingleton.Dashboard.Error("Error during package loading", ex.Message);
                }
            }
            finally
            {
                ServiceSingleton.Dashboard.NoStatus();
                ServiceSingleton.Dashboard.ProgressCompleted();
            }
        }
    }
}
