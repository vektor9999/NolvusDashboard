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

namespace Vcc.Nolvus.Dashboard.Frames.Instance.v6
{
    public partial class ControllerBindingFrame : DashboardFrame
    {        
        public ControllerBindingFrame()
        {
            InitializeComponent();
        }

        public ControllerBindingFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();            
        }

        protected override void OnLoad()
        {   
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private void BtnKeyboard_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<v6.KeysBindingFrame>();
        }
    }
}
