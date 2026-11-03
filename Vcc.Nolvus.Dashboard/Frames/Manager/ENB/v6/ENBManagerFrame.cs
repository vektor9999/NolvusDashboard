using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Manager.ENB.v6
{
    public partial class ENBManagerFrame : DashboardFrame
    {
        public ENBManagerFrame()
        {
            InitializeComponent();
        }

        public ENBManagerFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [ENB Manager]");
            ServiceSingleton.Dashboard.Info("Select an ENB to install");
        }

        private void SetDataSource(IEnumerable<IENBPreset> Source)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<IEnumerable<IENBPreset>>)SetDataSource, Source);
                return;
            }

            ENBListBox.DataSource = Source;
            ENBListBox.SelectedIndex =0;
            PicLoading.Hide();
            ENBListBox.Show();
        }                

        protected override async Task OnLoadedAsync()
        {                     
            SetDataSource(await ServiceSingleton.EnbManager.GetEnbPresets());
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.UnloadWorkingIntance();
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private async void BtnInstall_Click(object sender, EventArgs e)
        {
            if (!ModOrganizer.IsRunning)
            {
                var Preset = ENBListBox.SelectedItem as IENBPreset;

                if (Preset.GetFieldValueByKey("EnbCode") == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AlternateENB"))
                {
                    NolvusMessageBox.ShowMessage("Error", "This ENB is already installed!", MessageBoxType.Error);
                }
                else
                {
                    if (NolvusMessageBox.ShowConfirmation("Confirmation", string.Format("You are about to change ENB from {0} to {1}.\n\nWarning!!!\n\nIf you customized the list, all added mods will be placed at the end of the loadorder and left unchecked.\n\nAre you sure you want to continue?", ServiceSingleton.EnbManager.CurrentPreset(ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AlternateENB")), Preset)) == DialogResult.Yes)
                    {
                        try
                        {
                            try
                            {
                                DisableButtons();

                                ServiceSingleton.Dashboard.DisableSettings();

                                var ModsToUpdate = await ServiceSingleton.EnbManager.PrepareModsToUpdate(ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AlternateENB"), Preset.GetFieldValueByKey("EnbCode"));

                                ServiceSingleton.Instances.WorkingInstance.StatusManager.AddField("OldENB", ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AlternateENB"));
                                ServiceSingleton.Instances.WorkingInstance.StatusManager.AddField("NewENB", Preset.GetFieldValueByKey("EnbCode"));

                                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AlternateENB").Value = Preset.GetFieldValueByKey("EnbCode");

                                await ServiceSingleton.EnbManager.DeleteENB((s, p) =>
                                {
                                    ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                    ServiceSingleton.Dashboard.Progress(p);
                                });

                                ServiceSingleton.Instances.PrepareInstanceForEnb();

                                await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>(new FrameParameters(new FrameParameter() { Key = "ModsToInstall", Value = ModsToUpdate.Cast<IInstallableElement>().ToList() }));
                            }
                            catch (Exception ex)
                            {
                                await ServiceSingleton.Dashboard.Error("Error during ENB update initialization", ex.Message, ex.StackTrace);
                            }
                        }
                        finally
                        {
                            ServiceSingleton.Dashboard.EnableSettings();
                            ServiceSingleton.Dashboard.NoStatus();
                            ServiceSingleton.Dashboard.ProgressCompleted();
                        }
                    }
                }
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is running! Please close it first.", MessageBoxType.Error);
            }
        }
    }
}
