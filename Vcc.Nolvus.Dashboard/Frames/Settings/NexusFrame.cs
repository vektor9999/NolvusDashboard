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
using Vcc.Nolvus.Browser.Forms;
using Vcc.Nolvus.NexusApi;
using Vcc.Nolvus.NexusApi.SSO;
using Vcc.Nolvus.NexusApi.SSO.Events;
using Vcc.Nolvus.NexusApi.SSO.Responses;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;

namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    public partial class NexusFrame : DashboardFrame
    {
        private NexusSSOManager NexusSSOManager;

        public NexusFrame()
        {
            InitializeComponent();
        }

        public NexusFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();
        }        

        public void ChangeButtonText(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)ChangeButtonText, Value);
                return;
            }

            BtnAuthenticate.Text = Value;            
        }

        private void ToggleMessage(bool Visible)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<bool>)ToggleMessage, Visible);
                return;
            }

            PnlMessage.Visible = Visible;
            PicBox.Visible = Visible;
            LblMessage.Visible = Visible;
        }

        private void ToggleAuhtenticateButton(bool Active)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<bool>)ToggleAuhtenticateButton, Active);
                return;
            }

            BtnAuthenticate.Enabled = Active;
        }

        public void SetReturnMessage(string Message, bool Error)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string, bool>)SetReturnMessage, Message, Error);
                return;
            }

            ToggleMessage(true);

            if (!Error)
            {
                PnlMessage.BackColor = Color.FromArgb(92, 184, 92);
                PicBox.Image = Properties.Resources.Info;
                LblMessage.Text = Message;
            }
            else
            {
                PnlMessage.BackColor = Color.FromArgb(217, 83, 79);
                PicBox.Image = Properties.Resources.Warning_Message;
                LblMessage.Text = Message;
            }
        }

        protected override void OnLoaded()
        {
            ToggleMessage(false);      

            NexusSSOManager = new NexusSSOManager(new NexusSSOSettings()
            {
                Browser = () => { return Invoke((Func<IBrowserInstance>)(() => { return new BrowserWindow(); })) as IBrowserInstance; }
            });

            NexusSSOManager.OnAuthenticating += NexusSSOManager_OnAuthenticating;
            NexusSSOManager.OnAuthenticated += NexusSSOManager_OnAuthenticated;
            NexusSSOManager.OnRequestError += NexusSSOManager_OnRequestError;
            NexusSSOManager.OnBrowserClosed += NexusSSOManager_OnBrowserClosed;            
        }

        private void NexusSSOManager_OnBrowserClosed(object sender, EventArgs EventArgs)
        {
            ChangeButtonText("Nexus SSO Authentication");
            ToggleAuhtenticateButton(true);
        }

        private void NexusSSOManager_OnAuthenticating(object sender, AuthenticatingEventArgs EventArgs)
        {            
            ChangeButtonText("Authenticating...");
        }

        private void NexusSSOManager_OnRequestError(object sender, RequestErrorEventArgs EventArgs)
        {            
            SetReturnMessage(EventArgs.Message, true);
            ToggleAuhtenticateButton(true);
        }

        private void NexusSSOManager_OnAuthenticated(object sender, AuthenticationEventArgs EventArgs)
        {
            ChangeButtonText("Nexus SSO Authentication");
            SettingsCache.NexusApiKey = EventArgs.ApiKey;            
            SetReturnMessage("Authentication successfull! Click on the \"Next\" button", false);
            ToggleAuhtenticateButton(true);
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
                    ApiManager.Init(SettingsCache.NexusApiKey, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36", Path.GetTempPath());                    

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
            if (SettingsCache.NexusApiKey == string.Empty)
            {                
                SetReturnMessage("You must authenticate using the Nexus SSO!", true);                
            }
            else
            {
                ShowLoading();

                ServiceSingleton.Files.RemoveDirectory(ServiceSingleton.Folders.NexusCacheDirectory, false);                

                if (!await NexusAuthenticate())
                {                    
                    SetReturnMessage("Error connecting to Nexus Web site, please check your api key or browse the Nexus web site to check its availability.", true);                    
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

        private async void BtnAuthenticate_Click(object sender, EventArgs e)
        {
            if (!NexusSSOManager.Authenticated)
            {
                ToggleMessage(false);
                ToggleAuhtenticateButton(false);
                await NexusSSOManager.Connect();            
                await NexusSSOManager.Authenticate();
            }
            else
            {
                NolvusMessageBox.ShowMessage("Info", "You are already authenticated", Nolvus.Core.Enums.MessageBoxType.Info);
            }
        }
    }
}
