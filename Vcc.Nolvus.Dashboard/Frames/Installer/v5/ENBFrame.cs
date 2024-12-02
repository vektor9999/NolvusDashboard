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

namespace Vcc.Nolvus.Dashboard.Frames.Installer.v5
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
            DrpDwnLstENB.DataSource = ENBs.GetAvailableENBsForV5();

            DrpDwnLstENB.SelectedIndex = 0;

            ServiceSingleton.Dashboard.Info("ENB Selection");
        }               
               
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<v5.OptionsFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {                       
            ServiceSingleton.Dashboard.LoadFrame<PageFileFrame>();            
        }

        private void DrpDwnLstENB_SelectedIndexChanged(object sender, EventArgs e)
        {        
            ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = DrpDwnLstENB.SelectedValue.ToString();
        }

        private void LnkLblENB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.nolvus.net/guide/asc/enb");
        }
    }
}
