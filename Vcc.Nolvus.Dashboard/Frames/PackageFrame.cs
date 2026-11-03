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
using Vcc.Nolvus.Dashboard.Frames.Manager.ENB.v6;
using Vcc.Nolvus.Dashboard.Frames.Manager.Options.v6;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Dashboard.Forms;


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

        public InstanceMode Mode
        {
            get { return (InstanceMode)Parameters["Mode"]; }
        }

        private bool CheckDashboardVersionForPackages(INolvusInstance Instance, IEnumerable<IInstallPackageDTO> Packages)
        {
            foreach (var Package in Packages)
            {
                if (ServiceSingleton.Dashboard.IsOlder(Package.InstallerVersion))
                {
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();

                    var Message = string.Format("To install {0} - v {1}, you need the latest Dashboard version {2}.\n\nClose the Dashboard and restart it.\n\nIf the dashboard didn't auto update, please ask for support in the Nolvus discord.", Instance.Name, Packages.Last().Version, Package.InstallerVersion);

                    ServiceSingleton.Logger.Log(string.Format("Dashboard Version Error ==> {0}", Message));

                    NolvusMessageBox.ShowMessage("Dashboard Version Error", Message, MessageBoxType.Error);

                    return false;
                }
            }

            return true;
        }        

        protected async Task Install(INolvusInstance Instance)
        {
            IInstallPackageDTO Package = await ApiManager.Service.Installer.GetLatestPackage(Instance.Id);

            Instance.Version = Package.Version;

            await ServiceSingleton.Packages.Load(Package, (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Start installing {0} - v {1}...", Instance.Name, Instance.Version));            

            if (ServiceSingleton.Dashboard.IsOlder(Package.InstallerVersion))
            {
                ServiceSingleton.Dashboard.NoStatus();
                ServiceSingleton.Dashboard.ProgressCompleted();

                var Message = string.Format("To install {0} - v {1}, you need the latest Dashboard version {2}.\n\nClose the Dashboard and restart it.\n\nIf the dashboard didn't auto update, please ask for support in the Nolvus discord.", Instance.Name, Instance.Version, Package.InstallerVersion);

                ServiceSingleton.Logger.Log(string.Format("Dashboard Version Error ==> {0}", Message));

                NolvusMessageBox.ShowMessage("Dashboard Version Error", Message, MessageBoxType.Error);

                await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
            }
            else
            {
                await ServiceSingleton.Dashboard.LoadFrameAsync<StockGameFrame>();
            }
            
        }

        protected async Task Resume(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Resume installing {0} - v {1}...", Instance.Name, Instance.Version));

            await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();
        }

        protected async Task Update(INolvusInstance Instance)
        {
            var Packages = await ApiManager.Service.Installer.GetLatestPackages(Instance.Id, Instance.Version);

            await ServiceSingleton.Packages.Merge(Packages, (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Dashboard.NoStatus();
            ServiceSingleton.Dashboard.ProgressCompleted();

            if (!CheckDashboardVersionForPackages(Instance, Packages))
            {                
                await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
            }
            else
            {
                if (ServiceSingleton.EnbManager.EnbPresetsNeedUpdate)
                {
                    IInstanceStatusField EnbDeleted = ServiceSingleton.Instances.WorkingInstance.StatusManager.GetFieldByKey("EnbDeleted");

                    if (EnbDeleted == null)
                    {
                        await ServiceSingleton.EnbManager.DeleteENB((s, p) =>
                        {
                            ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                            ServiceSingleton.Dashboard.Progress(p);
                        });

                        ServiceSingleton.Instances.WorkingInstance.StatusManager.AddField("EnbDeleted", "TRUE");
                        ServiceSingleton.Instances.Save();
                    }
                }

                ServiceSingleton.Logger.Log(string.Format("Updating {0} - v {1} to v {2}...", Instance.Name, Instance.Version, Packages.Last().Version));

                await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();
            }
            
        }

        protected async Task ChangeEnb(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Changing ENB for {0} from {1} to {2}...", Instance.Name, Instance.StatusManager.GetFieldByKey("OldENB").Value, Instance.StatusManager.GetFieldByKey("NewENB").Value));

            var Mods = await ServiceSingleton.EnbManager.PrepareModsToUpdate(Instance.StatusManager.GetFieldByKey("OldENB").Value, Instance.StatusManager.GetFieldByKey("NewENB").Value);

            await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>(new FrameParameters(new FrameParameter() { Key = "ModsToInstall", Value = Mods.Cast<IInstallableElement>().ToList() }));
        }

        protected async Task ChangeOption(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Changing option {0} for {1} from {2} to {3}...", Instance.StatusManager.GetFieldByKey("Option").Value, Instance.Name, Instance.StatusManager.GetFieldByKey("OldOption").Value, Instance.StatusManager.GetFieldByKey("NewOption").Value));

            var Mods = await ServiceSingleton.OptionManager.PrepareModsToUpdate(Instance.StatusManager.GetFieldByKey("Option").Value, Instance.StatusManager.GetFieldByKey("OldOption").Value, Instance.StatusManager.GetFieldByKey("NewOption").Value);

            Instance.OptionsManager.SetOptionValue(Instance.StatusManager.GetFieldByKey("Option").Value, Instance.StatusManager.GetFieldByKey("NewOption").Value);

            await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>(new FrameParameters(new FrameParameter() { Key = "ModsToInstall", Value = Mods.Cast<IInstallableElement>().ToList() }));
        }

        protected async Task View(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Viewing {0} - v {1}...", Instance.Name, Instance.Version));

            await ServiceSingleton.Dashboard.LoadFrameAsync<InstanceDetailFrame>();
        }

        protected async Task LoadEnbManager(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Loading ENB Manager for {0} - v {1}...", Instance.Name, Instance.Version));

            await ServiceSingleton.Dashboard.LoadFrameAsync<ENBManagerFrame>();
        }

        protected async Task LoadOptionsManager(INolvusInstance Instance)
        {
            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(Instance.Id, Instance.Version), (s, p) =>
            {
                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                ServiceSingleton.Dashboard.Progress(p);
            });

            ServiceSingleton.Logger.Log(string.Format("Loading Options Manager for {0} - v {1}...", Instance.Name, Instance.Version));

            await ServiceSingleton.Dashboard.LoadFrameAsync<OptionsManagerFrame>();
        }

        protected override async Task OnLoadedAsync()        
        {
            ServiceSingleton.Dashboard.Info("Package loading...");

            try
            {
                try
                {
                    INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                    ServiceSingleton.Dashboard.DisableSettings();

                    switch (Instance.StatusManager.InstallStatus)
                    {
                        case InstanceInstallStatus.None:                            
                            await Install(Instance);
                            break;

                        case InstanceInstallStatus.Installing:                            
                            await Resume(Instance);
                            break;

                        case InstanceInstallStatus.Updating:                            
                            await Update(Instance);
                            break;
                        case InstanceInstallStatus.ChangingEnb:
                            await ChangeEnb(Instance);
                            break;
                        case InstanceInstallStatus.ChangingOption:
                            await ChangeOption(Instance);
                            break;

                        default:
                            if (Mode == InstanceMode.View)
                            {
                                await View(Instance);
                            }
                            else if (Mode == InstanceMode.ENB)
                            {
                                await LoadEnbManager(Instance);
                            }
                            else if (Mode == InstanceMode.Options)
                            {
                                await LoadOptionsManager(Instance);
                            }

                            break;

                    }                  
                }
                catch (Exception ex)
                {
                    await ServiceSingleton.Dashboard.Error("Error during package loading", ex.Message, ex.StackTrace);
                }
            }
            finally
            {
                ServiceSingleton.Dashboard.EnableSettings();
                ServiceSingleton.Dashboard.NoStatus();
                ServiceSingleton.Dashboard.ProgressCompleted();
            }
        }
    }
}
