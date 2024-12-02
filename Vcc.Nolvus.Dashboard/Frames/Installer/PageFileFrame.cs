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
using Vcc.Nolvus.Core.Misc;
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
        
               
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            switch (ServiceSingleton.Instances.WorkingInstance.Name)
            {
                case Strings.NolvusAscension:
                    ServiceSingleton.Dashboard.LoadFrame<v5.ENBFrame>();
                    break;
                case Strings.NolvusAwakening:
                    ServiceSingleton.Dashboard.LoadFrame<v6.ENBFrame>();
                    break;
            }            
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
                    switch (ServiceSingleton.Instances.WorkingInstance.Name)
                    {
                        case Strings.NolvusAscension:
                            ServiceSingleton.Dashboard.LoadFrame<v5.SummaryFrame>();
                            break;
                        case Strings.NolvusAwakening:
                            ServiceSingleton.Dashboard.LoadFrame<v6.SummaryFrame>();
                            break;
                    }
                }
            }
        }
        

        private void LnkLblPageFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.nolvus.net/appendix/pagefile");
        }
    }
}
