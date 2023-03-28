using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.NexusApi;

namespace Vcc.Nolvus.Dashboard.Frames
{
    public partial class ResumeFrame : DashboardFrame
    {
        public ResumeFrame()
        {
            InitializeComponent();
        }

        public ResumeFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        private int DownloadLocationIndex(List<string> Locations)
        {
            int Index = 0;

            if (ServiceSingleton.Instances.WorkingInstance.Settings.CDN != string.Empty)
            {
                foreach (var Location in Locations)
                {
                    if (Location == ServiceSingleton.Instances.WorkingInstance.Settings.CDN)
                    {
                        break;
                    }

                    Index++;
                }
            }

            return Index;
        }

        private List<string> GetDownloadLocations()
        {
            List<string> Result = new List<string>();

            Result.Add("Paris");
            Result.Add("Nexus CDN");
            Result.Add("Amsterdam");
            Result.Add("Prague");
            Result.Add("Chicago");
            Result.Add("Los Angeles");
            Result.Add("Miami");
            Result.Add("Singapore");

            return Result;
        }

        protected override void OnLoad()
        {
            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Instance Auto Installer]");

            DrpDwnLstInstances.DataSource = ServiceSingleton.Instances.InstancesToResume;
            DrpDwnLstInstances.DisplayMember = "Name";
            DrpDwnLstInstances.SelectedIndex = 0;

            if (!ApiManager.AccountInfo.IsPremium)
            {
                LblDownLoc.Visible = false;
                DrpDwnLstDownLoc.Visible = false;
            }                           
        }       

        private void DrpDwnLstInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Instance = DrpDwnLstInstances.SelectedItem as INolvusInstance;

            ServiceSingleton.Instances.WorkingInstance = Instance;

            if (ApiManager.AccountInfo.IsPremium)
            {
                DrpDwnLstDownLoc.DataSource = GetDownloadLocations();
                DrpDwnLstDownLoc.SelectedIndex = DownloadLocationIndex(GetDownloadLocations());
            }
        }

        private void Resume_Click(object sender, EventArgs e)
        {            
            ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>(); 
        }

        private void DrpDwnLstDownLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Instance = DrpDwnLstInstances.SelectedItem as INolvusInstance;

            Instance.Settings.CDN = DrpDwnLstDownLoc.SelectedItem.ToString();

            ServiceSingleton.Instances.Save();            
        }        

        private void BtnMegaLogin_Click(object sender, EventArgs e)
        {
            //InstallerEnvironment.MegaLogin = string.Empty;
            //InstallerEnvironment.MegaPassword = string.Empty;

            //FrmMegaAccount MegaLogin = new FrmMegaAccount();

            //if (MegaLogin.ShowDialog() == DialogResult.OK)
            //{
            //    InstallerEnvironment.MegaLogin = MegaLogin.UserName;
            //    InstallerEnvironment.MegaPassword = MegaLogin.Password;

            //    NolvusMessageBox.ShowMessage("Mega API", "Connection successfull", MessageBoxType.Info);
            //}


            //var Window = new BrowserWindow();
            //Window.Show();

            //var link = await Window.AwaitUserManualDownload("https://www.afkmods.com/index.php?/files/file/2377-expanded-towns-and-cities-sse/");


            //string test = "test";
            //string test2 = test;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (NolvusMessageBox.ShowConfirmation("Cancel install?", "Are you sure you want to cancel installation? Your current installation will be deleted.") == DialogResult.Yes)
            {                
                ServiceSingleton.Dashboard.LoadFrame<DeleteFrame>(new FrameParameters(new FrameParameter() {Key="Instance", Value= DrpDwnLstInstances.SelectedItem as INolvusInstance }, new FrameParameter() {Key="Action", Value=InstanceAction.Cancel }));               
            }
        }
    }
}
