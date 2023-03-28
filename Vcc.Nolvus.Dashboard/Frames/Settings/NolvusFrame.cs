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
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Dashboard.Core;

namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    public partial class NolvusFrame : DashboardFrame
    {
        private const string IniFile = @"[Path]
GamePath={0}

[Nexus]
ApiKey={1}
UserAgent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36

[Nolvus]
Version=v1
UserName={2}
Password={3}

[Process]
Count = {4}
Retry = 10
";

        public NolvusFrame()
        {
            InitializeComponent();
        }

        public NolvusFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {            
            LblCheck.Visible = false;

            TxtBxUserName.Text = SettingsCache.NolvusUser;
            TxtBxPassword.Text = SettingsCache.NolvusPassword;
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

        private Task<bool> NolvusAuthenticate()
        {
            return Task.Run(async () =>
            {              
                ApiManager.Init(ServiceSingleton.Globals.ApiUrl, "v1", TxtBxUserName.Text, TxtBxPassword.Text);

                return await ApiManager.Service.Installer.Authenticate(TxtBxUserName.Text, TxtBxPassword.Text);                                                    
            });
        }

        private async void BtnContinue_Click(object sender, EventArgs e)
        {
            if (TxtBxUserName.Text.Trim() == string.Empty || TxtBxPassword.Text.Trim() == string.Empty)
            {
                this.SetCheck("You must enter your user name/password!", true);
            }
            else
            {
                ShowLoading();
                SetCheck("Connecting to Nolvus...", false);
                                
                if (!await NolvusAuthenticate())
                {
                    HideLoading();
                    SetCheck("Invalid user name / password or your account has not been activated!", true);
                }
                else
                {
                    SettingsCache.NolvusUser = TxtBxUserName.Text;
                    SettingsCache.NolvusPassword = TxtBxPassword.Text;

                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashboard.ini"), string.Format(IniFile, SettingsCache.GameDirectory, SettingsCache.NexusApiKey, SettingsCache.NolvusUser, ServiceSingleton.Lib.EncryptString(SettingsCache.NolvusPassword), Environment.ProcessorCount));

                    await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
                }                          
            }
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<NexusFrame>();
        }
    }
}
