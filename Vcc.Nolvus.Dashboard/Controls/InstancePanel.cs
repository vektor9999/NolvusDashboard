using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;

namespace Vcc.Nolvus.Dashboard.Controls
{
    public partial class InstancePanel : UserControl
    {
        INolvusInstance _Instance;               

        public InstancePanel()
        {
            InitializeComponent();            
        }

        private void InstancePane_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
             Color.Gray, 1, ButtonBorderStyle.Solid, // left
             Color.Gray, 1, ButtonBorderStyle.Solid, // top
             Color.Gray, 1, ButtonBorderStyle.Solid, // right
             Color.Gray, 1, ButtonBorderStyle.Solid);// bottom
        }

        private void SetPlayText(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetPlayText, Text);
                return;
            }

            this.BtnPlay.Text = Text;
            this.BtnPlay.Enabled = true;
        }

        public async void LoadInstance(INolvusInstance Instance)
        {
            _Instance = Instance;            

            LblInstanceName.Text = _Instance.Name;
            LblVersion.Text = _Instance.Version;
            LblDesc.Text = _Instance.Description;

            LblBeta.Text = string.Empty;

            if (await _Instance.IsBeta())
            {
                LblBeta.Text = "(Beta)";
            }

            LblStatus.Text = await _Instance.GetState();

            if (_Instance.Name == "Nolvus Ascension")
            {
                PicInstanceImage.Image = Properties.Resources.Nolvus_V5;
            }            

            if (LblStatus.Text == "Installed")
            {
                LblStatus.ForeColor = Color.Orange;
            }
            else if (LblStatus.Text.Contains("New version available"))
            {
                LblStatus.ForeColor = Color.Orange;
                BtnUpdate.Visible = true;
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            Process[] ModOrganizerProcesses = Process.GetProcessesByName("ModOrganizer");

            if (ModOrganizerProcesses.Length == 0)
            {
                Process ModOrganizer = Process.Start(Path.Combine(_Instance.InstallDir, "MO2", "ModOrganizer.exe"));

                this.BtnPlay.Text = "Running...";
                this.BtnPlay.Enabled = false;

                Task.Run(() =>
                {
                    ModOrganizer.WaitForExit();

                    if (ModOrganizer.ExitCode == 0)
                    {
                        this.SetPlayText("Play");
                    }
                });
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is already running!", MessageBoxType.Error);
            }                       
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            Process[] ModOrganizerProcesses = Process.GetProcessesByName("ModOrganizer");

            if (ModOrganizerProcesses.Length == 0)
            {                
                ServiceSingleton.Dashboard.LoadFrame<ChangeLogFrame>(new FrameParameters(new FrameParameter() {Key = "Instance", Value =_Instance }));
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is running! Please close it before updating.", MessageBoxType.Error);
            }
        }

        private void BtnView_Click(object sender, EventArgs e)
        {            
            popupMenu1.Show(BtnView, new Point(0, BtnView.Height));
        }

        private async void BrItmMods_Click(object sender, EventArgs e)
        {            
            ServiceSingleton.Instances.WorkingInstance = _Instance;
            await ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>();
        }

        private void BrItmDelete_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<DeleteFrame>(new FrameParameters(new FrameParameter() { Key = "Instance", Value = _Instance as INolvusInstance }, new FrameParameter() { Key = "Action", Value = InstanceAction.Delete }));
        }       
    }
}
