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
using Vcc.Nolvus.Dashboard.Controls;
using Vcc.Nolvus.Dashboard.Frames.Installer;

namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    public partial class InstancesFrame : DashboardFrame
    {        
        public InstancesFrame()
        {
            InitializeComponent();
        }

        public InstancesFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();            
        }

        public void LockButtons()
        {
            BtnDiscord.Enabled = false;
            BtnDonate.Enabled = false;
            BtnKeyBind.Enabled = false;
            BtnNewInstance.Enabled = false;
            BtnPatreon.Enabled = false;            
        }

        public void UnLockButtons()
        {
            BtnDiscord.Enabled = true;
            BtnDonate.Enabled = true;
            BtnKeyBind.Enabled = true;
            BtnNewInstance.Enabled = true;
            BtnPatreon.Enabled = true;
        }

        protected override void OnLoad()
        {            
            ServiceSingleton.Dashboard.Title("Nolvus Dashboard");
            ServiceSingleton.Dashboard.Info("Manage your Nolvus instances");

            InstancesPanel.ContainerFrame = this;
            InstancesPanel.LoadInstances(ServiceSingleton.Instances.InstanceList);
        }

        private async void BtnNewInstance_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<SelectInstanceFrame>(new FrameParameters(new FrameParameter() { Key = "Cancel", Value = true }));
        }

        private void BtnDiscord_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/Zkh5PwD");
        }

        private void BtnDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/nolvus");
        }

        private void BtnPatreon_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.patreon.com/nolvus");
        }


        private void BtnKeyBind_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<KeysBindingFrame>();
        }
    }
}
