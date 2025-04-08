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
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
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

            TglBtnResistance.ActiveState.Text = "ON";
            TglBtnResistance.ActiveState.BackColor = Color.Orange;
            TglBtnResistance.ActiveState.BorderColor = Color.Orange;
            TglBtnResistance.ActiveState.ForeColor = Color.White;
            TglBtnResistance.ActiveState.HoverColor = Color.Orange;

            TglBtnResistance.InactiveState.Text = "OFF";
            TglBtnResistance.InactiveState.BackColor = Color.White;
            TglBtnResistance.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnResistance.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnResistance.InactiveState.HoverColor = Color.White;

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

            TglBtnPoise.ActiveState.Text = "ON";
            TglBtnPoise.ActiveState.BackColor = Color.Orange;
            TglBtnPoise.ActiveState.BorderColor = Color.Orange;
            TglBtnPoise.ActiveState.ForeColor = Color.White;
            TglBtnPoise.ActiveState.HoverColor = Color.Orange;

            TglBtnPoise.InactiveState.Text = "OFF";
            TglBtnPoise.InactiveState.BackColor = Color.White;
            TglBtnPoise.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnPoise.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnPoise.InactiveState.HoverColor = Color.White;

            #endregion
        }

        private int ScalingsIndex(List<string> Scalings)
        {            
            var Index = Scalings.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Options.CombatScaling);

            return Index == -1 ? 0 : Index;                            
        }

        private int NerfPAIndex(List<string> NerfPAs)
        {
            var Index = NerfPAs.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Options.NerfPA);

            return Index == -1 ? 0 : Index;
        }


        private bool CheckIfPrepareToDie()
        {            
            if (ServiceSingleton.Instances.WorkingInstance.Options.CombatScaling == "Hard" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Exhaustion == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.NerfPA == "Player Only" &&
                ServiceSingleton.Instances.WorkingInstance.Options.EnemiesResistance == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Boss == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Poise == "TRUE")
            {

                return true;
            }

            return false;
        }

        private bool CheckIfHackAndSash()
        {
            if (ServiceSingleton.Instances.WorkingInstance.Options.CombatScaling == "Easy" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Exhaustion == "FALSE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.NerfPA == "NPCs Only" &&
                ServiceSingleton.Instances.WorkingInstance.Options.EnemiesResistance == "FALSE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Boss == "FALSE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Poise == "FALSE")
            {

                return true;
            }

            return false;
        }

        private bool CheckIfModerate()
        {

            if (ServiceSingleton.Instances.WorkingInstance.Options.CombatScaling == "Medium" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Exhaustion == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.NerfPA == "Both" &&
                ServiceSingleton.Instances.WorkingInstance.Options.EnemiesResistance == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Boss == "TRUE" &&
                ServiceSingleton.Instances.WorkingInstance.Options.Poise == "FALSE")                
            {

                return true;
            }

            return false;
        }

        protected override void OnLoad()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            List<string> Presets = new List<string>();

            Presets.Add("Hack and Slash");
            Presets.Add("Moderate");
            Presets.Add("Prepare to Die");            

            DrpDwnLstPreset.SelectedIndexChanged -= DrpDwnLstPreset_SelectedIndexChanged;
            
            if (CheckIfPrepareToDie())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 2;                
            }
            else if (CheckIfModerate())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 1;                
            }
            else if (CheckIfHackAndSash())
            {
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 0;                
            }
            else
            {
                Presets.Add("Customized");
                DrpDwnLstPreset.DataSource = Presets;
                DrpDwnLstPreset.SelectedIndex = 3;
            }
            
            DrpDwnLstPreset.SelectedIndexChanged += DrpDwnLstPreset_SelectedIndexChanged;


            List<string> CombatScalings = new List<string>();

            CombatScalings.Add("Easy");
            CombatScalings.Add("Medium");
            CombatScalings.Add("Hard");

            DrpDwnLstCombatScaling.DataSource = CombatScalings;

            DrpDwnLstCombatScaling.SelectedIndex = ScalingsIndex(CombatScalings);

            TglBtnExhaustion.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Exhaustion == "TRUE")
            {
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;
            }      

            List<string> NerfPAs = new List<string>();

            NerfPAs.Add("None");
            NerfPAs.Add("Player Only");
            NerfPAs.Add("NPCs Only");
            NerfPAs.Add("Both");

            DrpDwnLstNerfPA.DataSource = NerfPAs;

            DrpDwnLstNerfPA.SelectedIndex = NerfPAIndex(NerfPAs);

            TglBtnResistance.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.EnemiesResistance == "TRUE")
            {
                TglBtnResistance.ToggleState = ToggleButtonState.Active;
            }

            TglBtnBoss.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Boss == "TRUE")
            {
                TglBtnBoss.ToggleState = ToggleButtonState.Active;
            }

            TglBtnPoise.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Poise == "TRUE")
            {
                TglBtnPoise.ToggleState = ToggleButtonState.Active;
            }

            ServiceSingleton.Dashboard.Info("Difficulty options");
        }
        
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<v6.OptionsFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (NolvusMessageBox.ShowConfirmation("Confirmation", "The options you selected can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
            {
                ServiceSingleton.Dashboard.LoadFrame<v6.ENBFrame>();
            }
        }       

        private void DrpDwnLstPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstPreset.SelectedIndex == 2)
            {                
                DrpDwnLstCombatScaling.SelectedIndex = 2;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;
                //TglBtnNerf.ToggleState = ToggleButtonState.Active;
                DrpDwnLstNerfPA.SelectedIndex = 1;
                TglBtnResistance.ToggleState = ToggleButtonState.Active;
                TglBtnBoss.ToggleState = ToggleButtonState.Active;
                TglBtnPoise.ToggleState = ToggleButtonState.Active;                
            }
            else if (DrpDwnLstPreset.SelectedIndex == 1)
            {
                DrpDwnLstCombatScaling.SelectedIndex = 1;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Active;
                //TglBtnNerf.ToggleState = ToggleButtonState.Inactive;
                DrpDwnLstNerfPA.SelectedIndex = 3;
                TglBtnResistance.ToggleState = ToggleButtonState.Active;
                TglBtnBoss.ToggleState = ToggleButtonState.Active;
                TglBtnPoise.ToggleState = ToggleButtonState.Inactive;
            }
            else
            {
                DrpDwnLstCombatScaling.SelectedIndex = 0;
                TglBtnExhaustion.ToggleState = ToggleButtonState.Inactive;
                //TglBtnNerf.ToggleState = ToggleButtonState.Inactive;
                DrpDwnLstNerfPA.SelectedIndex = 2;
                TglBtnResistance.ToggleState = ToggleButtonState.Inactive;
                TglBtnBoss.ToggleState = ToggleButtonState.Inactive;
                TglBtnPoise.ToggleState = ToggleButtonState.Inactive;
            }
            
        }        

        private void TglBtnExhaustion_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Exhaustion = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Exhaustion = "FALSE";
            }
        }

        //private void TglBtnNerf_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        //{
        //    if (e.ToggleState == ToggleButtonState.Active)
        //    {
        //        ServiceSingleton.Instances.WorkingInstance.Options.NerfPA = "TRUE";
        //    }
        //    else
        //    {
        //        ServiceSingleton.Instances.WorkingInstance.Options.NerfPA = "FALSE";
        //    }
        //}

        private void TglBtnResistance_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.EnemiesResistance = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.EnemiesResistance = "FALSE";
            }
        }

        private void TglBtnBoss_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Boss = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Boss = "FALSE";
            }
        }

        private void TglBtnPoise_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Poise = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Poise = "FALSE";
            }
        }

        private void DrpDwnLstPreset_DropDownOpening(object sender, Syncfusion.WinForms.ListView.Events.DropDownOpeningEventArgs e)
        {
            List<string> Presets = new List<string>();

            Presets.Add("Hack and Slash");
            Presets.Add("Moderate");
            Presets.Add("Prepare to Die");

            DrpDwnLstPreset.DataSource = Presets;
        }

        private void DrpDwnLstCombatScaling_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Options.CombatScaling = DrpDwnLstCombatScaling.SelectedValue.ToString();
        }

        private void DrpDwnLstNerfPA_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Options.NerfPA = DrpDwnLstNerfPA.SelectedValue.ToString();
        }
    }
}
