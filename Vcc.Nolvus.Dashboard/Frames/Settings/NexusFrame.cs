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
using Vcc.Nolvus.NexusApi;
using Vcc.Nolvus.Dashboard.Core;

namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    public partial class NexusFrame : DashboardFrame
    {
        public NexusFrame()
        {
            InitializeComponent();
        }

        public NexusFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            LblCheck.Visible = false;
            TxtBxNexusApiKey.Text = SettingsCache.NexusApiKey;
        }

        public void SetCheck(string Value, bool Error)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string, bool>)SetCheck, Value, Error);
                return;
            }

            if (Error)
            {
                LblCheck.ForeColor = Color.FromArgb(217, 83, 79);
            }
            else
            {
                LblCheck.ForeColor = Color.White;
            }

            LblCheck.Text = Value;

            LblCheck.Visible = true;
        }

        public void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            BtnContinue.Text = "Validating...";
            BtnContinue.Enabled = false;
        }

        public void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            BtnContinue.Text = "Next";
            BtnContinue.Enabled = true;
        }

        private Task<bool> NexusAuthenticate()
        {
            return Task.Run(() =>
            {                
                try
                {
                    ApiManager.Init(TxtBxNexusApiKey.Text, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36", Path.GetTempPath());

                    SettingsCache.NexusApiKey = TxtBxNexusApiKey.Text;

                    return true;
                }
                catch
                {
                    return false;
                }

            });
        }

        private async void BtnContinue_Click(object sender, EventArgs e)
        {
            if (TxtBxNexusApiKey.Text.Trim() == string.Empty)
            {
                SetCheck("You must enter your Nexus Api Key!", true);
            }
            else
            {
                ShowLoading();
                SetCheck("Connecting to Nexus...", false);

                if (!await NexusAuthenticate())
                {
                    SetCheck("Error connecting to Nexus Web site, please check your api key or browse Nexus Web to check its availability.", true);
                }
                else
                {
                    ServiceSingleton.Dashboard.LoadFrame<NolvusFrame>();
                }               
            }                       
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<GameFrame>();
        }

        private void LnkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.nolvus.net/appendix/installer/requirements");
        }
    }
}
