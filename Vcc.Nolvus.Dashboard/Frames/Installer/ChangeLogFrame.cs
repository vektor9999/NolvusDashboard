using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using CefSharp;
using CefSharp.WinForms;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{ 
    public partial class ChangeLogFrame : DashboardFrame
    {
        private ChromiumWebBrowser Browser;
        private string _FromVersion;
        private string _ToVersion;

        public ChangeLogFrame()
        {
            InitializeComponent();
        }      

        public ChangeLogFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override async void OnLoad()
        {
            ServiceSingleton.Dashboard.Info("Change Log");

            INolvusInstance Instance = Parameters["Instance"] as INolvusInstance;

            _FromVersion = Instance.Version;
            _ToVersion = await Instance.GetLatestVersion();
            
            ServiceSingleton.Dashboard.Status("Loading...");

            Browser = new ChromiumWebBrowser("https://www.nolvus.net/appendix/changelog?from=" + _FromVersion + "&to=" + _ToVersion)
            {
                Dock = DockStyle.Fill,
            };

            Browser.Name = "Chromium";
            
            Browser.FrameLoadEnd += Browser_FrameLoadEnd;

            Content.Controls.Add(Browser);

        }

        private void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            LoadingBox.Show();
        }

        private void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)HideLoading);
                return;
            }


            LoadingBox.Hide();
        }

        private void ShowBrowser()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowBrowser);
                return;
            }


            Browser.Show();
        }

        private void HideBrowser()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)HideBrowser);
                return;
            }


            Browser.Hide();
        }


        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {           
            if (e.Frame.IsMain)
            {
                HideLoading();
                ShowBrowser();
                
                ServiceSingleton.Dashboard.Status("Change Log for " + (Parameters["Instance"] as INolvusInstance).Name + " v" + _ToVersion);
            }
        }

        private async void BtnContinue_Click(object sender, EventArgs e)
        {
            var LatestPackage = await (Parameters["Instance"] as INolvusInstance).GetLatestPackage();

            if (LatestPackage.NewGame)
            {
                if (NolvusMessageBox.ShowConfirmation("Warning", "This new Nolvus version requires a new game. Your current saves will not work with it. Are you really sure you want to proceed with the installation?") == DialogResult.Yes)
                {
                    StartUpdate();
                }
            }
            else
            {
                StartUpdate();
            }

        }
        private async void StartUpdate()
        {
            ServiceSingleton.Instances.WorkingInstance = (Parameters["Instance"] as INolvusInstance);
            ServiceSingleton.Instances.PrepareInstanceForUpdate();
            await ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }        
    }
}
