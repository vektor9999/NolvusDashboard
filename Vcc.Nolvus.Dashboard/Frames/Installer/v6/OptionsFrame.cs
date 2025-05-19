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

            TglBtnAltStart.ActiveState.Text = "ON";
            TglBtnAltStart.ActiveState.BackColor = Color.Orange;
            TglBtnAltStart.ActiveState.BorderColor = Color.Orange;
            TglBtnAltStart.ActiveState.ForeColor = Color.White;
            TglBtnAltStart.ActiveState.HoverColor = Color.Orange;

            TglBtnAltStart.InactiveState.Text = "OFF";
            TglBtnAltStart.InactiveState.BackColor = Color.White;
            TglBtnAltStart.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnAltStart.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnAltStart.InactiveState.HoverColor = Color.White;

            TglBtnStancesPerksTree.ActiveState.Text = "ON";
            TglBtnStancesPerksTree.ActiveState.BackColor = Color.Orange;
            TglBtnStancesPerksTree.ActiveState.BorderColor = Color.Orange;
            TglBtnStancesPerksTree.ActiveState.ForeColor = Color.White;
            TglBtnStancesPerksTree.ActiveState.HoverColor = Color.Orange;

            TglBtnStancesPerksTree.InactiveState.Text = "OFF";
            TglBtnStancesPerksTree.InactiveState.BackColor = Color.White;
            TglBtnStancesPerksTree.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnStancesPerksTree.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnStancesPerksTree.InactiveState.HoverColor = Color.White;

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

        private int AnimsIndex(List<string> Anims)
        {            
            var Index = Anims.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Options.CombatAnimation);

            return Index == -1 ? 0 : Index;                            
        }

        protected override void OnLoad()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            TglBtnNudity.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Nudity == "TRUE")
            {
                TglBtnNudity.ToggleState = ToggleButtonState.Active;
            }           

            TglBtnLeveling.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.AlternateLeveling == "TRUE")
            {
                TglBtnLeveling.ToggleState = ToggleButtonState.Active;
            }

            TglBtnAltStart.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.AlternateStart == "TRUE")
            {
                TglBtnAltStart.ToggleState = ToggleButtonState.Active;
            }

            TglBtnStancesPerksTree.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.StancesPerksTree == "TRUE")
            {
                TglBtnStancesPerksTree.ToggleState = ToggleButtonState.Active;
            }

            TglBtnGore.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Gore == "TRUE")
            {
                TglBtnGore.ToggleState = ToggleButtonState.Active;
            }

            if (Instance.Options.Controller == "TRUE")
            {
                TglBtnController.ToggleState = ToggleButtonState.Active;
            }

            List<string> CombatAnims = new List<string>();

            CombatAnims.Add("Conventional");
            CombatAnims.Add("Fantasy");

            DrpDwnLstCombatAnims.DataSource = CombatAnims;

            DrpDwnLstCombatAnims.SelectedIndex = AnimsIndex(CombatAnims);

            ServiceSingleton.Dashboard.Info("Options");
        }
        
        private async void BtnPrevious_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<v6.PerformanceFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (NolvusMessageBox.ShowConfirmation("Confirmation", "The options you selected can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
            {                
                ServiceSingleton.Dashboard.LoadFrame<v6.DifficultyFrame>();
            }
        }

        private void TglBtnNudity_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Nudity = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Nudity = "FALSE";
            }
        }       


        private void TglBtnLeveling_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateLeveling = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateLeveling = "FALSE";
            }
        }

        private void TglBtnAltStart_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateStart = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.AlternateStart = "FALSE";
            }
        }

        private void TglBtnStancesPerksTree_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.StancesPerksTree = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.StancesPerksTree = "FALSE";
            }
        }

        private void TglBtnGore_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Gore = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Gore = "FALSE";
            }
        }

        private void DrpDwnLstCombatAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Options.CombatAnimation = DrpDwnLstCombatAnims.SelectedValue.ToString();
        }

        private void TglBtnController_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Controller = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.Controller = "FALSE";
            }
        }
    }
}
