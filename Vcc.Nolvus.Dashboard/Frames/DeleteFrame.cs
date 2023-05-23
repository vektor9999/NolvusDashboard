using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;

namespace Vcc.Nolvus.Dashboard.Frames
{
    public partial class DeleteFrame : DashboardFrame
    {
        public DeleteFrame()
        {
            InitializeComponent();
        }

        public DeleteFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        public INolvusInstance Instance
        {
            get { return Parameters["Instance"] as INolvusInstance; }
        }

        public InstanceAction Action
        {
            get { return (InstanceAction)Parameters["Action"]; }
        }       

        protected override void OnLoad()
        {            
            if (Action == InstanceAction.Delete)
            {
                LblStepText.Text = "Delete Nolvus Instance";
                BtnAction.Text = "Delete";
                LblInfo.Text = "Click on the Delete button to delete your instance. WARNING, this will not be reversible.";

                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Delete Instance]");
                ServiceSingleton.Dashboard.Info("Delete your Nolvus instance");                

                LblInstanceInfo.Text = "Instance to delete";
            }            
            else if (Action == InstanceAction.Cancel)
            {
                LblStepText.Text = "Cancel Nolvus Instance installation";
                BtnAction.Text = "Cancel";
                LblInfo.Text = "Click on the Cancel button to cancel your current installation. WARNING, this will not be reversible.";

                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Cancel Installation]");
                ServiceSingleton.Dashboard.Info("Cancel your current Nolvus installation");                

                LblInstanceInfo.Text = "Instance to cancel";
            }

            string Version = Instance.Version;

            if (Version == string.Empty)
            {
                Version = Instance.Version;
            }

            LblInstance.Text = this.Instance.Name + " v" + Version;
        }

        public void UpdateProgress(int Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<int>)UpdateProgress, Value);
                return;
            }

            LblDeleteInfo.Text = "Deleting instance (" + Value.ToString() + "%)";
        }

        private async Task DeleteInstance(string[] Files)
        {
            LblDeleteInfo.Visible = true;
            BtnAction.Enabled = false;
            BtnBack.Enabled = false;

            var Tsk = Task.Run(() =>
            {
                int Counter = 0;

                foreach (var _File in Files)
                {
                    File.Delete(_File);

                    int PercentDone = System.Convert.ToInt16(((double)++Counter / Files.Length) * 100);

                    UpdateProgress(PercentDone);

                    ServiceSingleton.Dashboard.Progress(PercentDone);
                }

                ServiceSingleton.Files.RemoveDirectory(Instance.InstallDir, false);
            });

            await Tsk;
        }

        private async void DoAction()
        {
            string Message = string.Empty;

            if (Action == InstanceAction.Delete)
            {
                Message = string.Format("Are you sure you want to delete {0} and everything inside {1}?", Instance.Name, Instance.InstallDir);
            }            
            else
            {
                Message = string.Format("Are you sure you want to cancel {0} installation?", Instance.Name);
            }

            if (NolvusMessageBox.ShowConfirmation("Confirmation", Message) == DialogResult.Yes)
            {
                try
                {
                    try
                    {                        
                        await DeleteInstance(Directory.GetFiles(Instance.InstallDir, "*.*", SearchOption.AllDirectories));

                        ServiceSingleton.Instances.RemoveInstance(Instance);

                        await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
                    }
                    finally
                    {
                        ServiceSingleton.Dashboard.NoStatus();
                        ServiceSingleton.Dashboard.ProgressCompleted();
                    }
                }
                catch(Exception ex)
                {
                    await ServiceSingleton.Dashboard.Error("Error during deleting instance", ex.Message);
                }
            }
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] ModOrganizerProcesses = System.Diagnostics.Process.GetProcessesByName("ModOrganizer");

            if (ModOrganizerProcesses.Length == 0)
            {
                DoAction();
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is running! Please close it first.", MessageBoxType.Error);
            }
        }

        private async void BtnBack_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
        }
    }
}
