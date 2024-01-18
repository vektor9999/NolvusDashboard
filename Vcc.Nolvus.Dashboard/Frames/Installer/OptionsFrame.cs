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


namespace Vcc.Nolvus.Dashboard.Frames.Installer
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

            this.TglBtnNudity.ActiveState.Text = "ON";
            this.TglBtnNudity.ActiveState.BackColor = Color.Orange;
            this.TglBtnNudity.ActiveState.BorderColor = Color.Orange;
            this.TglBtnNudity.ActiveState.ForeColor = Color.White;
            this.TglBtnNudity.ActiveState.HoverColor = Color.Orange;

            this.TglBtnNudity.InactiveState.Text = "OFF";
            this.TglBtnNudity.InactiveState.BackColor = Color.White;
            this.TglBtnNudity.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnNudity.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnNudity.InactiveState.HoverColor = Color.White;            

            this.TglBtnHardcore.ActiveState.Text = "ON";
            this.TglBtnHardcore.ActiveState.BackColor = Color.Orange;
            this.TglBtnHardcore.ActiveState.BorderColor = Color.Orange;
            this.TglBtnHardcore.ActiveState.ForeColor = Color.White;
            this.TglBtnHardcore.ActiveState.HoverColor = Color.Orange;

            this.TglBtnHardcore.InactiveState.Text = "OFF";
            this.TglBtnHardcore.InactiveState.BackColor = Color.White;
            this.TglBtnHardcore.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnHardcore.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnHardcore.InactiveState.HoverColor = Color.White;

            this.TglBtnLeveling.ActiveState.Text = "ON";
            this.TglBtnLeveling.ActiveState.BackColor = Color.Orange;
            this.TglBtnLeveling.ActiveState.BorderColor = Color.Orange;
            this.TglBtnLeveling.ActiveState.ForeColor = Color.White;
            this.TglBtnLeveling.ActiveState.HoverColor = Color.Orange;

            this.TglBtnLeveling.InactiveState.Text = "OFF";
            this.TglBtnLeveling.InactiveState.BackColor = Color.White;
            this.TglBtnLeveling.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnLeveling.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnLeveling.InactiveState.HoverColor = Color.White;

            this.TglBtnAltStart.ActiveState.Text = "ON";
            this.TglBtnAltStart.ActiveState.BackColor = Color.Orange;
            this.TglBtnAltStart.ActiveState.BorderColor = Color.Orange;
            this.TglBtnAltStart.ActiveState.ForeColor = Color.White;
            this.TglBtnAltStart.ActiveState.HoverColor = Color.Orange;

            this.TglBtnAltStart.InactiveState.Text = "OFF";
            this.TglBtnAltStart.InactiveState.BackColor = Color.White;
            this.TglBtnAltStart.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnAltStart.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnAltStart.InactiveState.HoverColor = Color.White;

            this.TglBtnFantasyMode.ActiveState.Text = "ON";
            this.TglBtnFantasyMode.ActiveState.BackColor = Color.Orange;
            this.TglBtnFantasyMode.ActiveState.BorderColor = Color.Orange;
            this.TglBtnFantasyMode.ActiveState.ForeColor = Color.White;
            this.TglBtnFantasyMode.ActiveState.HoverColor = Color.Orange;

            this.TglBtnFantasyMode.InactiveState.Text = "OFF";
            this.TglBtnFantasyMode.InactiveState.BackColor = Color.White;
            this.TglBtnFantasyMode.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnFantasyMode.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnFantasyMode.InactiveState.HoverColor = Color.White;
        }

        private int SkinTypeIndex(List<string> SkinTypes)
        {            
            var Index = SkinTypes.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Options.SkinType);

            return Index == -1 ? 0 : Index;                            
        }

        protected override void OnLoad()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            this.TglBtnNudity.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.Nudity == "TRUE")
            {
                this.TglBtnNudity.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnHardcore.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.HardcoreMode == "TRUE")
            {
                this.TglBtnHardcore.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnLeveling.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.AlternateLeveling == "TRUE")
            {
                this.TglBtnLeveling.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnAltStart.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.AlternateStart == "TRUE")
            {
                this.TglBtnAltStart.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnFantasyMode.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Options.FantasyMode == "TRUE")
            {
                this.TglBtnFantasyMode.ToggleState = ToggleButtonState.Active;
            }

            List<string> SkinTypes = new List<string>();

            SkinTypes.Add("Smooth");
            SkinTypes.Add("Muscular");

            DrpDwnLstSkin.DataSource = SkinTypes;

            DrpDwnLstSkin.SelectedIndex = SkinTypeIndex(SkinTypes);
            
            ServiceSingleton.Dashboard.Info("Additional Options");
        }
        
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<PerformanceFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (NolvusMessageBox.ShowConfirmation("Confirmation", "The options you selected can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
            {
                ServiceSingleton.Dashboard.LoadFrame<ENBFrame>();
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

        private void TglBtnFantasy_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.FantasyMode = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.FantasyMode = "FALSE";
            }
        }

        private void TglBtnHardcore_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.HardcoreMode = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.HardcoreMode = "FALSE";
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

        private void DrpDwnLstSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Options.SkinType = DrpDwnLstSkin.SelectedValue.ToString();
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

        private void TglBtnFantasyMode_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Options.FantasyMode = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Options.FantasyMode = "FALSE";
            }
        }
    }
}
