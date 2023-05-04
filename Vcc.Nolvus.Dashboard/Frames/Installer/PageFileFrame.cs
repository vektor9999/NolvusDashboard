using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class PageFileFrame : DashboardFrame
    {
        public PageFileFrame()
        {
            InitializeComponent();
        }

        public PageFileFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {                       
            ServiceSingleton.Dashboard.Info("Page file size configuration");
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
        }
               
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<ENBFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (NolvusMessageBox.ShowConfirmation("Confirmation", "Page file size configuration is REALLY IMPORTANT to avoid crashes, be sure you set it up correctly. Do you want to continue?") == DialogResult.Yes)
            {
                if (NexusApi.ApiManager.AccountInfo.IsPremium)
                {
                    ServiceSingleton.Dashboard.LoadFrame<CDNFrame>();
                }
                else
                {
                    ServiceSingleton.Dashboard.LoadFrame<SummaryFrame>();
                }
            }
        }
        

        private void LnkLblPageFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.nolvus.net/appendix/pagefile");
        }
    }
}
