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
    public partial class DifficultyFrame : DashboardFrame
    {
        public DifficultyFrame()
        {
            InitializeComponent();
        }
        public DifficultyFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)
        {
            InitializeComponent();

            #region Toggle Buttons            

            TglBtnExhaustion.ActiveState.Text = "ON";
            TglBtnExhaustion.ActiveState.BackColor = Color.Orange;
            TglBtnExhaustion.ActiveState.BorderColor = Color.Orange;
            TglBtnExhaustion.ActiveState.ForeColor = Color.White;
            TglBtnExhaustion.ActiveState.HoverColor = Color.Orange;

            TglBtnExhaustion.InactiveState.Text = "OFF";
            TglBtnExhaustion.InactiveState.BackColor = Color.White;
            TglBtnExhaustion.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnExhaustion.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnExhaustion.InactiveState.HoverColor = Color.White;                        

            TglBtnBoss.ActiveState.Text = "ON";
            TglBtnBoss.ActiveState.BackColor = Color.Orange;
            TglBtnBoss.ActiveState.BorderColor = Color.Orange;
            TglBtnBoss.ActiveState.ForeColor = Color.White;
            TglBtnBoss.ActiveState.HoverColor = Color.Orange;

            TglBtnBoss.InactiveState.Text = "OFF";
            TglBtnBoss.InactiveState.BackColor = Color.White;
            TglBtnBoss.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnBoss.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnBoss.InactiveState.HoverColor = Color.White;

            TglBtnZoneLevel.ActiveState.Text = "ON";
            TglBtnZoneLevel.ActiveState.BackColor = Color.Orange;
            TglBtnZoneLevel.ActiveState.BorderColor = Color.Orange;
            TglBtnZoneLevel.ActiveState.ForeColor = Color.White;
            TglBtnZoneLevel.ActiveState.HoverColor = Color.Orange;

            TglBtnZoneLevel.InactiveState.Text = "OFF";
            TglBtnZoneLevel.InactiveState.BackColor = Color.White;
            TglBtnZoneLevel.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnZoneLevel.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnZoneLevel.InactiveState.HoverColor = Color.White;

            #endregion
        }

        private int ScalingsIndex(IEnumerable<INolvusOptionChoiceDTO> Scalings)
        {            
            var Index = Scalings.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatScaling"));

            return Index == -1 ? 0 : Index;                            
        }

        private int NerfPAIndex(IEnumerable<INolvusOptionChoiceDTO> NerfPAs)
        {
            var Index = NerfPAs.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("NerfPA"));

            return Index == -1 ? 0 : Index;
        }


        private bool CheckIfPrepareToDie()
        {            
            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatScaling") == "Hard" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Exhaustion") == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("NerfPA") == "Player Only" &&                
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Boss") == "TRUE")                
            {

                return true;
            }

            return false;
        }

        private bool CheckIfHackAndSash()
        {
            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatScaling") == "Easy" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Exhaustion") == "FALSE" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("NerfPA") == "NPCs Only" &&                
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Boss") == "FALSE")                
            {

                return true;
            }

            return false;
        }

        private bool CheckIfHMilkDrinker()
        {
            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatScaling") == "Very Easy" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Exhaustion") == "FALSE" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("NerfPA") == "NPCs Only" &&                
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Boss") == "FALSE")                
            {

                return true;
            }

            return false;
        }

        private bool CheckIfTrueNord()
        {

            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("CombatScaling") == "Medium" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Exhaustion") == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("NerfPA") == "Both" &&                
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Boss") == "TRUE")                              
            {

                return true;
            }

            return false;
        }

        protected override async Task OnLoadAsync()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;
                        
            TglBtnBoss.ToggleState = ToggleButtonState.Inactive;
            TglBtnExhaustion.ToggleState = ToggleButtonState.Inactive;
            TglBtnZoneLevel.ToggleState = ToggleButtonState.Inactive;
                               
            List<string> Presets = new List<string>();

            Presets.Add("Milk Drinker");
            Presets.Add("Hack and Slash");
            Presets.Add("True Nord");
            Presets.Add("Prepare to Die");

            DrpDwnLstPreset.SelectedIndexChanged -= DrpDwnLstPreset_SelectedIndexChanged;

            if (CheckIfPrepareToDie())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 3;
            }
            else if (CheckIfTrueNord())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 2;
            }
            else if (CheckIfHackAndSash())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 1;
            }
            else if (CheckIfHMilkDrinker())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 0;
            }
            else
            {
                Presets.Add("Customized");
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 4;
            }

            DrpDwnLstPreset.SelectedIndexChanged += DrpDwnLstPreset_SelectedIndexChanged;

            var CombatScalings = await ApiManager.Service.Installer.GetOptionChoices("CombatScaling", Instance.SettingsManager.Variant, Instance.Id);

            DrpDwnLstCombatScaling.DataSource = CombatScalings;
            DrpDwnLstCombatScaling.DisplayMember = "Value";
            DrpDwnLstCombatScaling.ValueMember = "Id";

            DrpDwnLstCombatScaling.SelectedIndex = ScalingsIndex(CombatScalings);

            var NerfPAs = await ApiManager.Service.Installer.GetOptionChoices("NerfPA", Instance.SettingsManager.Variant, Instance.Id);

            DrpDwnLstNerfPA.DataSource = NerfPAs;
            DrpDwnLstNerfPA.DisplayMember = "Value";
            DrpDwnLstNerfPA.ValueMember = "Id";

            DrpDwnLstNerfPA.SelectedIndex = NerfPAIndex(NerfPAs);

            if (Instance.OptionsManager.GetOptionValue("Boss") == "TRUE")
            {
                TglBtnBoss.ToggleState = ToggleButtonState.Active;
            }

            if (Instance.OptionsManager.GetOptionValue("Exhaustion") == "TRUE")
            {
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;
            }

            if (Instance.OptionsManager.GetOptionValue("ZoneLevelIndicator") == "TRUE")
            {
                TglBtnZoneLevel.ToggleState = ToggleButtonState.Active;
            }            

            ServiceSingleton.Dashboard.Info("Difficulty options");
        }
        
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrameAsync<v6.OptionsFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<v6.ENBFrame>();            
        }       

        private void DrpDwnLstPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            TglBtnBoss.Enabled = true;

            if (DrpDwnLstPreset.SelectedIndex == 3)
            {                
                //P2D
                DrpDwnLstCombatScaling.SelectedIndex = 3;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;                
                DrpDwnLstNerfPA.SelectedIndex = 1;                
                TglBtnBoss.ToggleState = ToggleButtonState.Active;                
            }
            else if (DrpDwnLstPreset.SelectedIndex == 2)
            {
                //True Nord
                DrpDwnLstCombatScaling.SelectedIndex = 2;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;                
                DrpDwnLstNerfPA.SelectedIndex = 3;                
                TglBtnBoss.ToggleState = ToggleButtonState.Active;                
            }
            else if (DrpDwnLstPreset.SelectedIndex == 1)
            {
                //Hack & Slash
                DrpDwnLstCombatScaling.SelectedIndex = 1;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Inactive;                
                DrpDwnLstNerfPA.SelectedIndex = 2;                
                TglBtnBoss.ToggleState = ToggleButtonState.Inactive;                
            }
            else
            {
                //Milk Drinker
                DrpDwnLstCombatScaling.SelectedIndex = 0;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Inactive;
                DrpDwnLstNerfPA.SelectedIndex = 2;                
                TglBtnBoss.ToggleState = ToggleButtonState.Inactive;
                TglBtnBoss.Enabled = false;              
            }

        }

        private void TglBtnBoss_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Boss").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Boss").Value = "FALSE";
            }
        }

        private void TglBtnExhaustion_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Exhaustion").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Exhaustion").Value = "FALSE";
            }
        }

        private void TglBtnZoneLevel_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("ZoneLevelIndicator").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("ZoneLevelIndicator").Value = "FALSE";
            }
        }

        private void DrpDwnLstPreset_DropDownOpening(object sender, Syncfusion.WinForms.ListView.Events.DropDownOpeningEventArgs e)
        {
            List<string> Presets = new List<string>();

            Presets.Add("Milk Drinker");
            Presets.Add("Hack and Slash");
            Presets.Add("True Nord");
            Presets.Add("Prepare to Die");

            DrpDwnLstPreset.DataSource = Presets;
        }

        private void DrpDwnLstCombatScaling_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("CombatScaling").Value = (DrpDwnLstCombatScaling.SelectedItem as INolvusOptionChoiceDTO).Value;
        }

        private void DrpDwnLstNerfPA_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("NerfPA").Value = (DrpDwnLstNerfPA.SelectedItem as INolvusOptionChoiceDTO).Value;
        }

        private void BtnPresetsExplained_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/combat/presets");
        }

        private void BtnScalingExplained_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/combat/scaling");
        }

        private void BtnZoneLevelExplained_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/guide/awake/appendix/player-guide/combat/scaling");
        }
        
    }
}
