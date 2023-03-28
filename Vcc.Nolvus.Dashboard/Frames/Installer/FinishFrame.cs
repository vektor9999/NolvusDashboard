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
using System.Runtime.InteropServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class FinishFrame : DashboardFrame
    {
        public FinishFrame()
        {
            InitializeComponent();
        }

        public FinishFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            ServiceSingleton.Dashboard.Info("Installation completed");
        }        

        private void BtnDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/nolvus");
        }

        private void BtnPatreon_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.patreon.com/nolvus");
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }               
    }
}
