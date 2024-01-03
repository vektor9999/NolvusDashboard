using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Styles;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Browser.Forms;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Controls;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class InstallFrame : DashboardFrame
    {                                
        public InstallFrame()
        {
            InitializeComponent();
        }

        public InstallFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)            
        {
            InitializeComponent();
            ModsBox.ScalingFactor = ServiceSingleton.Dashboard.ScalingFactor;                 
        }        

        private void GlobalProgress()
        {
            ServiceSingleton.Dashboard.Info(string.Format("Installing mods ({0}%)",ServiceSingleton.Packages.InstallProgression));
            ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Mods {0}/{1}", ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Count, ServiceSingleton.Packages.ModsCount));            
        }

        private void RefreshBox()
        {
            if (InvokeRequired)
            {
                Invoke((Action)RefreshBox);
                return;
            }
            
            ModsBox.DataSource = ServiceSingleton.Packages.ProgressQueue.ToList();
            
            ServiceSingleton.Dashboard.AdditionalTertiaryInfo(string.Format("Download : {0}MB/s", ServiceSingleton.Packages.DownloadSpeed.ToString("0.0")));

            ModsBox.Refresh();                                    
        }        
      
        private void Refresh(int Ms)
        {
            Task.Run(async () => 
            {                
                while (true)
                {
                    await Task.Delay(Ms);
                    RefreshBox();
                }                                
            });
        }

        protected override async Task OnLoadedAsync()
        {

            try
            {
                var Instance = ServiceSingleton.Instances.WorkingInstance;
                
                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Instance Auto Installer]");
                ServiceSingleton.Dashboard.Status(string.Format("Installing {0} (v{1})", Instance.Name, ServiceSingleton.Packages.LoadedVersion));
                ServiceSingleton.Dashboard.AdditionalSecondaryInfo("Error(s) : 0");

                GlobalProgress();

                ServiceSingleton.Files.RemoveDirectory(ServiceSingleton.Folders.NexusCacheDirectory, false);                                             

                await ServiceSingleton.Packages.InstallModList(new ModInstallSettings()
                {
                    OnStartInstalling = () =>
                    {
                        Refresh(ServiceSingleton.Settings.RefreshInterval);
                    },
                    OnModInstalled = (Mod) =>
                    {
                        GlobalProgress();
                        ServiceSingleton.Logger.Log(string.Format("Mod : {0} installed.", Mod.Name));
                    }, 
                    OnModError = (ErrorCount) => 
                    {
                        if (ServiceSingleton.Packages.ErrorHandler.ThresholdEnabled)
                        {
                            ServiceSingleton.Dashboard.AdditionalSecondaryInfo(string.Format("Error(s) : {0} Threshold : {1} {2}", ServiceSingleton.Packages.ErrorHandler.ErrorsCount, ServiceSingleton.Settings.ErrorsThreshold, "(Errors will be displayed at the end of the installation)"));
                        }
                        else
                        {
                            ServiceSingleton.Dashboard.AdditionalSecondaryInfo(string.Format("Error(s) : {0} {1}", ServiceSingleton.Packages.ErrorHandler.ErrorsCount, "(Errors will be displayed at the end of the installation)"));
                        }
                    },
                    OnMaxErrors = () =>
                    {
                        ServiceSingleton.Dashboard.AdditionalSecondaryInfo(string.Format("Error(s) : {0} {1}", ServiceSingleton.Packages.ErrorHandler.ErrorsCount, "(Maximum errors threshold reached, waiting for current queue to finish...)"));
                    },               
                    Browser = () =>
                    {
                        return Invoke((Func<IBrowserInstance>)(() => { return new BrowserWindow(); })) as IBrowserInstance;
                    }
                });                

                await ServiceSingleton.Dashboard.LoadFrameAsync<LoadOrderFrame>(new FrameParameters(new FrameParameter(){Key = "Mode", Value = "Install"}));
            }
            catch
            {
                ServiceSingleton.Dashboard.ClearInfo();                
                ServiceSingleton.Dashboard.LoadFrame<ErrorSummaryFrame>();                
            }

        }            
    }
}
