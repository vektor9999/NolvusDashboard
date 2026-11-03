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
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Syncfusion.Windows.Forms.Tools;


namespace Vcc.Nolvus.Dashboard.Frames.Installer.v6
{
    public partial class OptionsFrame : DashboardFrame
    {
        public OptionsFrame()
        {
            InitializeComponent();
        }
        public OptionsFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)
        {
            InitializeComponent();

            #region Toggle Buttons

            TglBtnNudity.ActiveState.Text = "ON";
            TglBtnNudity.ActiveState.BackColor = Color.Orange;
            TglBtnNudity.ActiveState.BorderColor = Color.Orange;
            TglBtnNudity.ActiveState.ForeColor = Color.White;
            TglBtnNudity.ActiveState.HoverColor = Color.Orange;

            TglBtnNudity.InactiveState.Text = "OFF";
            TglBtnNudity.InactiveState.BackColor = Color.White;
            TglBtnNudity.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnNudity.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnNudity.InactiveState.HoverColor = Color.White;

            TglBtnLeveling.ActiveState.Text = "ON";
            TglBtnLeveling.ActiveState.BackColor = Color.Orange;
            TglBtnLeveling.ActiveState.BorderColor = Color.Orange;
            TglBtnLeveling.ActiveState.ForeColor = Color.White;
            TglBtnLeveling.ActiveState.HoverColor = Color.Orange;

            TglBtnLeveling.InactiveState.Text = "OFF";
            TglBtnLeveling.InactiveState.BackColor = Color.White;
            TglBtnLeveling.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnLeveling.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnLeveling.InactiveState.HoverColor = Color.White;          

            TglBtnGore.ActiveState.Text = "ON";
            TglBtnGore.ActiveState.BackColor = Color.Orange;
            TglBtnGore.ActiveState.BorderColor = Color.Orange;
            TglBtnGore.ActiveState.ForeColor = Color.White;
            TglBtnGore.ActiveState.HoverColor = Color.Orange;

            TglBtnGore.InactiveState.Text = "OFF";
            TglBtnGore.InactiveState.BackColor = Color.White;
            TglBtnGore.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnGore.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnGore.InactiveState.HoverColor = Color.White;

            TglBtnController.ActiveState.Text = "ON";
            TglBtnController.ActiveState.BackColor = Color.Orange;
            TglBtnController.ActiveState.BorderColor = Color.Orange;
            TglBtnController.ActiveState.ForeColor = Color.White;
            TglBtnController.ActiveState.HoverColor = Color.Orange;

            TglBtnController.InactiveState.Text = "OFF";
            TglBtnController.InactiveState.BackColor = Color.White;
            TglBtnController.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnController.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnController.InactiveState.HoverColor = Color.White;

            #endregion
        }

        private int AnimsIndex(IEnumerable<INolvusOptionChoiceDTO> Anims)
        {            
            var Index = Anims.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatAnimation"));

            return Index == -1 ? 0 : Index;                            
        }

        private int UIsIndex(IEnumerable<INolvusOptionChoiceDTO> UIs)
        {
            var Index = UIs.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("UI"));

            return Index == -1 ? 0 : Index;
        }

        protected override async Task OnLoadAsync()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            LblGOWarning.Visible = Instance.SettingsManager.Variant == Strings.GO;

            TglBtnNudity.ToggleState = ToggleButtonState.Inactive;

            TglBtnLeveling.ToggleState = ToggleButtonState.Inactive;
            TglBtnGore.ToggleState = ToggleButtonState.Inactive;
            TglBtnController.ToggleState = ToggleButtonState.Inactive;

            TglBtnLeveling.Enabled = Instance.SettingsManager.Variant != Strings.GO;
            TglBtnGore.Enabled = Instance.SettingsManager.Variant != Strings.GO;
            TglBtnController.Enabled = Instance.SettingsManager.Variant != Strings.GO;

            LblALNotUsed.Visible = Instance.SettingsManager.Variant == Strings.GO;
            LblCANotUsed.Visible = Instance.SettingsManager.Variant == Strings.GO;
            LblGoreNotUsed.Visible = Instance.SettingsManager.Variant == Strings.GO;
            LblCSNotUsed.Visible = Instance.SettingsManager.Variant == Strings.GO;

            DrpDwnLstCombatAnims.Enabled = Instance.SettingsManager.Variant != Strings.GO;

            if (Instance.OptionsManager.GetOptionValue("Nudity") == "TRUE")
            {
                TglBtnNudity.ToggleState = ToggleButtonState.Active;                
            }            

            if (Instance.SettingsManager.Variant != Strings.GO)
            {
                if (Instance.OptionsManager.GetOptionValue("AlternateLeveling") == "TRUE")
                {
                    TglBtnLeveling.ToggleState = ToggleButtonState.Active;
                }

                if (Instance.OptionsManager.GetOptionValue("Gore") == "TRUE")
                {
                    TglBtnGore.ToggleState = ToggleButtonState.Active;
                }

                if (Instance.OptionsManager.GetOptionValue("Controller") == "TRUE")
                {
                    TglBtnController.ToggleState = ToggleButtonState.Active;
                }

                var CombatAnims = await ApiManager.Service.Installer.GetOptionChoices("CombatAnimation", Instance.SettingsManager.Variant, Instance.Id);

                DrpDwnLstCombatAnims.DataSource = CombatAnims;
                DrpDwnLstCombatAnims.DisplayMember = "Value";
                DrpDwnLstCombatAnims.ValueMember = "Id";

                DrpDwnLstCombatAnims.SelectedIndex = AnimsIndex(CombatAnims);                
            }
            else
            {
                var GOCombatAnims = new List<string>();
                GOCombatAnims.Add("Not used");
                DrpDwnLstCombatAnims.DataSource = GOCombatAnims;
                DrpDwnLstCombatAnims.SelectedIndex = 0;
            }

            var UIs = await ApiManager.Service.Installer.GetOptionChoices("UI", Instance.SettingsManager.Variant, Instance.Id);

            DrpDwnLstUI.DataSource = UIs;
            DrpDwnLstUI.DisplayMember = "Value";
            DrpDwnLstUI.ValueMember = "Id";

            DrpDwnLstUI.SelectedIndex = UIsIndex(UIs);

            ServiceSingleton.Dashboard.Info("Options");
        }
        
        private async void BtnPrevious_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<v6.PerformanceFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant != Strings.GO)
            {
                ServiceSingleton.Dashboard.LoadFrameAsync<v6.DifficultyFrame>();
            }
            else
            {
                ServiceSingleton.Dashboard.LoadFrame<v6.ENBFrame>();
            }                      
        }

        private void TglBtnNudity_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Nudity").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Nudity").Value = "FALSE";
            }
        }       


        private void TglBtnLeveling_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AlternateLeveling").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AlternateLeveling").Value = "FALSE";
            }
        }       

        private void TglBtnGore_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Gore").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Gore").Value = "FALSE";
            }
        }

        private void DrpDwnLstCombatAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant != Strings.GO)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("CombatAnimation").Value = (DrpDwnLstCombatAnims.SelectedItem as INolvusOptionChoiceDTO).Value;
            }                    
        }

        private void TglBtnController_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Controller").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Controller").Value = "FALSE";
            }
        }

        private void DrpDwnLstUI_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("UI").Value = (DrpDwnLstUI.SelectedItem as INolvusOptionChoiceDTO).Value;
        }

        private void BtnShowUI_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/ui/choices");
        }

        private void BtnShowAlternateLeveling_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/game/leveling");
        }

        private void BtnShowGore_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/combat/gore");
        }

        private void BtnShowController_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/keybinds/controller");
        }
    }
}
