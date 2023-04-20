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
        private CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        private TaskCompletionSource<object> CancelTasks = new TaskCompletionSource<object>();
        private Random Rnd = new Random();
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
            ServiceSingleton.Dashboard.Info("Installing mods (" + Math.Floor(((double)ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Count / ServiceSingleton.Packages.ModsCount) * 100).ToString() + "%)");
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
            ModsBox.Refresh();                        
        }        

        private void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((Action)ShowLoading);
                return;
            }

            LoadingBox.Show();
        }

        private void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((Action)HideLoading);
                return;
            }

            LoadingBox.Hide();
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
                ServiceSingleton.Dashboard.Status("Installing " + Instance.Name + " (v " + ServiceSingleton.Packages.LoadedVersion + ")");
                ServiceSingleton.Dashboard.Info("Installing mods...");

                ServiceSingleton.Files.RemoveDirectory(ServiceSingleton.Folders.NexusCacheDirectory, false);

                HideLoading();

                Refresh(10);                 

                await ServiceSingleton.Packages.InstallModList(new ModInstallSettings()
                {                    
                    OnModInstalled = (Mod) =>
                    {
                        GlobalProgress();
                        ServiceSingleton.Logger.Log("Mod : " + Mod.Name + " installed.");
                    },                    
                    Browser = () =>
                    {
                        return Invoke((Func<IBrowserInstance>)(() => { return new BrowserWindow(); })) as IBrowserInstance;
                    }
                });                

                await ServiceSingleton.Dashboard.LoadFrameAsync<LoadOrderFrame>(new FrameParameters(new FrameParameter() { Key = "Mode", Value = "Install" }));
            }
            catch (Exception ex)
            {
                ServiceSingleton.Logger.Log(string.Format("Error during mod list installation with message {0}", ex.Message));
                await ServiceSingleton.Dashboard.Error("Error during mod installation", ex.Message, ex.StackTrace);
            }

        }            
    }
}
