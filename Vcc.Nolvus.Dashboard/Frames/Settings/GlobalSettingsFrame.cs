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
using Vcc.Nolvus.Dashboard.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    public partial class GlobalSettingsFrame : DashboardFrame
    {
        public GlobalSettingsFrame()
        {
            InitializeComponent();
        }

        public GlobalSettingsFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();

            TglBtnAnonymous.ActiveState.Text = "ON";
            TglBtnAnonymous.ActiveState.BackColor = Color.Orange;
            TglBtnAnonymous.ActiveState.BorderColor = Color.Orange;
            TglBtnAnonymous.ActiveState.ForeColor = Color.White;
            TglBtnAnonymous.ActiveState.HoverColor = Color.Orange;

            TglBtnAnonymous.InactiveState.Text = "OFF";
            TglBtnAnonymous.InactiveState.BackColor = Color.White;
            TglBtnAnonymous.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnAnonymous.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnAnonymous.InactiveState.HoverColor = Color.White;
        }

        protected override void OnLoad()
        {
            try

            {
                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Settings]");
                ServiceSingleton.Dashboard.Info("Global Settings");

                TxtBxUserName.Text = ServiceSingleton.Globals.MegaEmail;
                TxtBxPassword.Text = ServiceSingleton.Globals.MegaPassword;

                PnlMessage.BackColor = Color.FromArgb(92, 184, 92);
                PicBox.Image = Properties.Resources.Info;

                TglBtnAnonymous.ToggleState = ToggleButtonState.Active;

                if (!ServiceSingleton.Globals.MegaAnonymousConnection)
                {
                    TglBtnAnonymous.ToggleState = ToggleButtonState.Inactive;
                }                
            }
            catch(Exception ex)
            {
                ServiceSingleton.Dashboard.Error("Error during global settings loading", ex.Message, ex.StackTrace);
            }
        }

        private void UpdateButton(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((Action<string>)UpdateButton, Text);
                return;
            }

            BtnSaveMegaInfo.Text = Text;
        }

        private async void BtnBack_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
        }

        private void LnkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://mega.nz/login");
        }

        private async void BtnSaveMegaInfo_Click(object sender, EventArgs e)
        {
            if (TglBtnAnonymous.ToggleState == ToggleButtonState.Inactive && TxtBxUserName.Text.Trim() == string.Empty && TxtBxPassword.Text.Trim() == string.Empty)
            {
                NolvusMessageBox.ShowMessage("Error", "Please enter your credentials!", MessageBoxType.Error);
            }
            else
            {                
                try
                {
                    if (TglBtnAnonymous.ToggleState == ToggleButtonState.Inactive)
                    {
                        UpdateButton("Validating...");
                        await ServiceSingleton.Files.AuthenticateToMegaApi(TxtBxUserName.Text, TxtBxPassword.Text);                                              
                    }                    
                }
                catch
                {
                    UpdateButton("Save");
                    NolvusMessageBox.ShowMessage("Error", "Unable to connect to mega.nz! Please review your credentials", MessageBoxType.Error);
                    return;
                }

                try
                {
                    ServiceSingleton.Globals.MegaAnonymousConnection = TglBtnAnonymous.ToggleState == ToggleButtonState.Active;

                    if (TxtBxUserName.Text != string.Empty)
                    {
                        ServiceSingleton.Globals.MegaEmail = TxtBxUserName.Text;
                    }
                    
                    if ( TxtBxPassword.Text != string.Empty)
                    {
                        ServiceSingleton.Globals.MegaPassword = TxtBxPassword.Text;
                    }                    

                    NolvusMessageBox.ShowMessage("Information", "Your mega.nz configuration has been valiated and saved", MessageBoxType.Info);

                    UpdateButton("Save");
                }
                catch(Exception ex)
                {
                    UpdateButton("Save");
                    NolvusMessageBox.ShowMessage("Error", string.Format("Unexpected error ocurred with message : {0}", ex.Message), MessageBoxType.Error);                                            
                }                    
            }
        }

        private void TglBtnAnonymous_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {            
            TxtBxUserName.Enabled = e.ToggleState == ToggleButtonState.Inactive;
            TxtBxPassword.Enabled = e.ToggleState == ToggleButtonState.Inactive;
            label3.Enabled = e.ToggleState == ToggleButtonState.Inactive;
            label4.Enabled = e.ToggleState == ToggleButtonState.Inactive;            
            label5.Enabled = e.ToggleState == ToggleButtonState.Inactive;
            PnlMessage.Enabled = e.ToggleState == ToggleButtonState.Inactive;                                                
        }
    }
}
