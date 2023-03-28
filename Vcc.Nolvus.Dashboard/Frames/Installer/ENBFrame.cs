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
    public partial class ENBFrame : DashboardFrame
    {
        public ENBFrame()
        {
            InitializeComponent();
        }

        public ENBFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {            
            List<string> ENBs = new List<string>();

            ENBs.Add("Standard ENB (PI-CHO ENB)");

            DrpDwnLstENB.DataSource = ENBs;

            DrpDwnLstENB.SelectedIndex = 0;

            ServiceSingleton.Dashboard.Info("ENB Selection");
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
            ServiceSingleton.Dashboard.LoadFrame<OptionsFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {           
            if (NexusApi.ApiManager.AccountInfo.IsPremium)
            {
                ServiceSingleton.Dashboard.LoadFrame<CDNFrame>();
            }
            else
            {
                //ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>(new FrameParameters(FrameParameter.Create("InstallMode", InstallMode.Install)));
                ServiceSingleton.Dashboard.LoadFrame<SummaryFrame>();
            }
        }

        private void DrpDwnLstENB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstENB.SelectedIndex == 0)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = "FALSE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = "TRUE";
            }
        }
    }
}
