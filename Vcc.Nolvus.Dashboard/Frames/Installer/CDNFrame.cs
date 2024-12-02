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
using Vcc.Nolvus.Core.Misc;
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
            if (ServiceSingleton.Instances.WorkingInstance.Settings.CDN != string.Empty)
            {
                var Index = Locations.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Settings.CDN);

                return Index == -1 ? 0 : Index;
            }

            return 0;
        }      

        protected override void OnLoad()
        {
            DrpDwnLstDownLoc.DataSource = CDN.Get();
            DrpDwnLstDownLoc.SelectedIndex = DownloadLocationIndex(CDN.Get());

            ServiceSingleton.Dashboard.Info("CDN Location");
        }        
                
        private void DrpDwnLstDownLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.CDN = DrpDwnLstDownLoc.SelectedItem.ToString();                                   
        }

        private void BtnContinue_Click(object sender, EventArgs e)
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

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<PageFileFrame>();
            
        }
    }
}
