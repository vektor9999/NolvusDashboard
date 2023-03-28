using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Dashboard.Core;

namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    public partial class GameFrame : DashboardFrame
    {
        public GameFrame()
        {
            InitializeComponent();
        }

        public GameFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            LblError.Visible = false;
            TxtBxGamePath.Text = SettingsCache.GameDirectory;
        }


        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (this.TxtBxGamePath.Text.Trim() != string.Empty)
            {
                SettingsCache.GameDirectory = TxtBxGamePath.Text;                
                ServiceSingleton.Dashboard.LoadFrame<NexusFrame>();
            }
            else
            {
                LblError.Visible = true;
                LblError.ForeColor = Color.FromArgb(217, 83, 79);
                LblError.Text = "You must select a directory";
            }

        }

        private void BtnDetect_Click(object sender, EventArgs e)
        {
            LblError.Visible = false;

            if (ServiceSingleton.Game.IsGameInstalled())
            {
                TxtBxGamePath.Text = ServiceSingleton.Game.GetSkyrimSEDirectory();
            }
            else
            {
                LblError.Visible = true;
                LblError.ForeColor = Color.FromArgb(217, 83, 79);
                LblError.Text = "Skyrim Anniversary Edition not found! Check if your game is installed. If it is installed, browse the installation directory manually.";
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtBxGamePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
