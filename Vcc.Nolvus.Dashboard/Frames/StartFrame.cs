using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Dashboard.Frames.Settings;

namespace Vcc.Nolvus.Dashboard.Frames
{
    public partial class StartFrame : DashboardFrame
    {       
        public StartFrame()
        {
            InitializeComponent();            
        }       
        public StartFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {            
            InitializeComponent();                  
        }               

        private async Task CheckNolvus()
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {
                    ServiceSingleton.Dashboard.Progress(0);
                    ServiceSingleton.Dashboard.Status("Connecting to Nolvus...");

                    var ApiUrl = ServiceSingleton.Globals.ApiUrl;
                    var ApiVersion = ServiceSingleton.Globals.ApiVersion;                    
                    var UserName = ServiceSingleton.Globals.NolvusUserName;
                    var Password = ServiceSingleton.Globals.NolvusPassword;

                    if (ApiVersion == string.Empty || UserName == string.Empty || Password == string.Empty)
                    {                        
                        throw new Exception("Nolvus settings missing, please check your Nolvus settings!");                        
                    }
                    else
                    {
                        ApiManager.Init(ApiUrl, ApiVersion, UserName, ServiceSingleton.Lib.DecryptString(Password));

                        if (!await ApiManager.Service.Installer.Authenticate(UserName, ServiceSingleton.Lib.DecryptString(Password)))
                        {
                            throw new Exception("Invalid Nolvus user name / password or your account has not been activated!");
                        }                        
                    }

                    ServiceSingleton.Dashboard.Progress(25);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during Nolvus connection. The Nolvus web site may have issues currently. Original message : " + ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            });

            await Tsk;            
        }
        private async Task CheckNexus()
        {
            var Tsk = Task.Run(() => 
            {
                try
                {
                    ServiceSingleton.Dashboard.Status("Connecting to Nexus...");

                    var NexusApiKey = ServiceSingleton.Globals.NexusApiKey;
                    var UserAgent = ServiceSingleton.Globals.NexusUserAgent;

                    if (NexusApiKey == string.Empty || UserAgent == string.Empty)
                    {
                        throw new Exception("Nexus Api key or User Agent setting missing!");
                    }
                    else
                    {
                        NexusApi.ApiManager.Init(NexusApiKey, UserAgent, ServiceSingleton.Folders.NexusCacheDirectory);

                        try
                        {
                            if (NexusApi.ApiManager.AccountInfo.ProfileUrl != null)
                            {
                                ServiceSingleton.Dashboard.LoadAccountImage(NexusApi.ApiManager.AccountInfo.ProfileUrl);
                            }
                        }
                        catch
                        {

                        }

                        ServiceSingleton.Dashboard.TitleInfo(NexusApi.ApiManager.AccountInfo.Name);
                        ServiceSingleton.Dashboard.NexusAccount(ApiManager.Service.Installer.LoggedUser + "@" + NexusApi.ApiManager.AccountInfo.Name);

                        if (NexusApi.ApiManager.AccountInfo.IsPremium)
                        {
                            ServiceSingleton.Dashboard.AccountType("(Premium)");
                        }
                        else if (Vcc.Nolvus.NexusApi.ApiManager.AccountInfo.IsSupporter)
                        {
                            ServiceSingleton.Dashboard.AccountType("(Supporter)");
                        }
                        else
                        {
                            ServiceSingleton.Dashboard.AccountType("(Default)");
                        }

                        ServiceSingleton.Dashboard.Progress(75);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during Nexus connection. The Nexus web site may have issues currently. Original message : " + ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            });

            await Tsk;           
        }
        private async Task<InstanceCheck> CheckInstances()
        {
            var Tsk = Task.Run(() => 
            {
                try
                {
                    ServiceSingleton.Dashboard.Status("Checking instances...");

                    ServiceSingleton.Instances.Load();

                    if (ServiceSingleton.Instances.InstanceFileExists && !ServiceSingleton.Instances.Empty)
                    {                        
                        var InstanceMessage = string.Empty;

                        if (ServiceSingleton.Instances.CheckInstances(out InstanceMessage))
                        {
                            if (ServiceSingleton.Instances.InstancesToResume.Count > 0)
                            {
                                return InstanceCheck.InstancesToResume;
                            }
                            else
                            {
                                return InstanceCheck.InstalledInstances;
                            }
                        }
                        else
                        {
                            throw new Exception(InstanceMessage + ".This can happen if you modify the file InstancesData.xml manually!");
                        }
                    }

                    return InstanceCheck.NoInstance;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return await Tsk;            
        }
        private async Task CheckForUpdates()
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    ServiceSingleton.Dashboard.Status("Checking for updates...");

                    var LatestDashboard = await Api.Installer.Services.ApiManager.Service.Installer.GetLatestInstaller();
                    ServiceSingleton.Dashboard.Progress(50);                    

                    if (ServiceSingleton.Dashboard.IsOlder(LatestDashboard.Version))
                    {
                        if (!ServiceSingleton.Updater.Installed || ServiceSingleton.Updater.IsOlder(LatestDashboard.UpdaterVersion))
                        {
                            await ServiceSingleton.Files.DownloadFile(LatestDashboard.UpdaterLink, ServiceSingleton.Updater.UpdaterExe, (s, e) =>
                            {
                                ServiceSingleton.Dashboard.Status("Downloading Updater (" + e.ProgressPercentage + "%)");
                                ServiceSingleton.Dashboard.Progress(e.ProgressPercentage);
                            });

                            await ServiceSingleton.Updater.Launch();
                        }
                        else
                        {
                            await ServiceSingleton.Updater.Launch();
                        }

                        ServiceSingleton.Dashboard.ShutDown();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during Nolvus updates checking. The Nolvus web site may have issues currently. Original message : " + ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            });

            await Tsk;
        }        
        protected override async Task OnLoadedAsync()
        {
            try
            {
                try
                {

                    if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashboard.ini")))
                    {
                        ServiceSingleton.Dashboard.LoadFrame<GameFrame>();
                    }
                    else
                    {
                        await CheckNolvus();
                        await CheckForUpdates();
                        await CheckNexus();

                        var InstancesCheck = await CheckInstances();

                        switch (InstancesCheck)
                        {
                            case InstanceCheck.NoInstance:
                                await ServiceSingleton.Dashboard.LoadFrameAsync<SelectInstanceFrame>();
                                break;
                            case InstanceCheck.InstancesToResume:
                                ServiceSingleton.Dashboard.LoadFrame<ResumeFrame>();
                                break;
                            case InstanceCheck.InstalledInstances:
                                ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
                                break;
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    await ServiceSingleton.Dashboard.Error("Error during initialization", ex.Message, ex.StackTrace);
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
