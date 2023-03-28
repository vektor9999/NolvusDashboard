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
    public partial class CDNFrame : DashboardFrame
    {
        public CDNFrame()
        {
            InitializeComponent();
        }

        public CDNFrame(IDashboard Dashboard, FrameParameters Params) 
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
            DrpDwnLstDownLoc.DataSource = this.GetDownloadLocations();
            DrpDwnLstDownLoc.SelectedIndex = this.DownloadLocationIndex(this.GetDownloadLocations());

            ServiceSingleton.Dashboard.Info("CDN Location");
        }        
                
        private void DrpDwnLstDownLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.CDN = DrpDwnLstDownLoc.SelectedItem.ToString();                                   
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            //ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>(new FrameParameters(FrameParameter.Create("InstallMode", InstallMode.Install)));            
            ServiceSingleton.Dashboard.LoadFrame<SummaryFrame>();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<ENBFrame>();
            
        }
    }
}
