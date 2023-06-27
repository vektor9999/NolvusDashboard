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
            DrpDwnLstENB.ValueMember = "Code";
            DrpDwnLstENB.DisplayMember = "Name";
            DrpDwnLstENB.DataSource = ENBs.GetAvailableENBs();

            DrpDwnLstENB.SelectedIndex = 0;

            ServiceSingleton.Dashboard.Info("ENB Selection");
        }               
               
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<OptionsFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {                       
            ServiceSingleton.Dashboard.LoadFrame<PageFileFrame>();            
        }

        private void DrpDwnLstENB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DrpDwnLstENB.SelectedIndex == 0)
            //{
            //    ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = "FALSE";
            //}
            //else
            //{
            //    ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = "TRUE";
            //}

            ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = DrpDwnLstENB.SelectedValue.ToString();
        }
    }
}
