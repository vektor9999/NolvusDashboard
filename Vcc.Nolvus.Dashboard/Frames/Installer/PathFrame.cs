using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class PathFrame : DashboardFrame
    {
        public PathFrame()
        {
            InitializeComponent();
        }

        public PathFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();

            this.TglBtnEnableArchive.ActiveState.Text = "ON";
            this.TglBtnEnableArchive.ActiveState.BackColor = Color.Orange;
            this.TglBtnEnableArchive.ActiveState.BorderColor = Color.Orange;
            this.TglBtnEnableArchive.ActiveState.ForeColor = Color.White;
            this.TglBtnEnableArchive.ActiveState.HoverColor = Color.Orange;

            this.TglBtnEnableArchive.InactiveState.Text = "OFF";
            this.TglBtnEnableArchive.InactiveState.BackColor = Color.White;
            this.TglBtnEnableArchive.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnEnableArchive.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnEnableArchive.InactiveState.HoverColor = Color.White;
        }

        protected override void OnLoad()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            this.TxtBxInstancePath.Text = Instance.InstallDir;
            this.TxtBxArchivePath.Text = Instance.ArchiveDir;

            this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Settings.EnableArchiving)
            {
                this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Active;
            }

            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;

            ServiceSingleton.Dashboard.Info("Instance Paths");
        }

        private void BtnBrowseInstancePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (ServiceSingleton.Files.IsDirectoryEmpty(folderBrowserDialog1.SelectedPath))
                {
                    TxtBxInstancePath.Text = folderBrowserDialog1.SelectedPath;

                    var Instance = ServiceSingleton.Instances.WorkingInstance;

                    Instance.InstallDir = this.TxtBxInstancePath.Text;
                    Instance.StockGame = Path.Combine(Instance.InstallDir, "STOCK GAME");
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Invalid Installation Directory.", "The specified directory is not empty. Please select an other directory.", MessageBoxType.Error);                    
                }
                
            }
        }

        private void BtnBrowseArchivePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtBxArchivePath.Text = folderBrowserDialog1.SelectedPath;

                ServiceSingleton.Instances.WorkingInstance.ArchiveDir = this.TxtBxArchivePath.Text;
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {            

            switch (ServiceSingleton.Instances.WorkingInstance.Name)
            {
                case Strings.NolvusAscension:
                    ServiceSingleton.Dashboard.LoadFrameAsync<v5.PerformanceFrame>();
                    break;
                case Strings.NolvusAwakening:
                    ServiceSingleton.Dashboard.LoadFrameAsync<v6.PerformanceFrame>();
                    break;
            }                        
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {            
            ServiceSingleton.Dashboard.LoadFrameAsync<SelectInstanceFrame>();
        }

        private void TglBtnEnableArchive_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.EnableArchiving = this.TglBtnEnableArchive.ToggleState == ToggleButtonState.Active;
        }              
    }
}
