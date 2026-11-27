using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Manager.Variant.v6
{
    public partial class VariantManagerFrame : DashboardFrame
    {     
        private INolvusOptionDTO SelectedOption
        {
            get
            {
                return (DrpDwnLstVariant.SelectedItem as INolvusOptionDTO);
            }
        }
        
        private INolvusOption CurrentOption
        {
            get
            {
                return ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption(SelectedOption.Name);
            }
        }   
        public VariantManagerFrame()
        {
            InitializeComponent();
        }

        public VariantManagerFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();
           

            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Variant Manager]");
            ServiceSingleton.Dashboard.Info("Select the variant to change");
        }

                              
        protected override async Task OnLoadedAsync()
        {            
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            LblCurrentVariant.Text = Instance.SettingsManager.Variant;

            var Variants = await ApiManager.Service.Installer.GetNolvusVariants(Instance.Id);

            DrpDwnLstVariant.DataSource = Variants;
            DrpDwnLstVariant.DisplayMember = "FullName";
            DrpDwnLstVariant.ValueMember = "Id";

            DrpDwnLstVariant.SelectedIndex = 0;
        }


        private async void DoChangeVariant(string CurrentVariant, string NewVariant)
        {
            try
            {
                var Instance = ServiceSingleton.Instances.WorkingInstance;

                var ModsToUpdate = await ServiceSingleton.VariantManager.PrepareModsToUpdate("Variant", CurrentVariant, NewVariant);

                if (ModsToUpdate.Count > 0)
                {                    
                    Instance.StatusManager.AddField("OldVariant", CurrentVariant);
                    Instance.StatusManager.AddField("NewVariant", NewVariant);

                    Instance.SettingsManager.Variant = NewVariant;
                    
                    ServiceSingleton.Logger.Log("New game required, about to backup game save files");

                    await Instance.BackupGameSaveFiles((p) =>
                    {
                        ServiceSingleton.Dashboard.Status(string.Format("Backuping game save files ({0}%)...", p));
                        ServiceSingleton.Dashboard.Progress(p);
                    });

                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();

                    await Instance.OptionsManager.SynchronizeOptions(Instance);

                    ServiceSingleton.Instances.PrepareInstanceForVariant();

                    await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>(new FrameParameters(new FrameParameter() { Key = "ModsToInstall", Value = ModsToUpdate.Cast<IInstallableElement>().ToList() }));
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Error", "Unable to process, 0 mods returned!", MessageBoxType.Error);
                }
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during variant change initialization", ex.Message, ex.StackTrace);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.UnloadWorkingIntance();
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            if (!ModOrganizer.IsRunning)
            {
                BtnChange.Text = "Running...";

                var CurrentVariant = ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant;
                var NewVariant = (DrpDwnLstVariant.SelectedItem as INolvusVariantDTO).Name;

                if (CurrentVariant == NewVariant)
                {
                    NolvusMessageBox.ShowMessage("Error", string.Format("You selected the same variant ({0}). Unable to process!", NewVariant), MessageBoxType.Error);
                }
                else
                {
                    try
                    {
                        if (NolvusMessageBox.ShowConfirmation("Confirmation", string.Format("You are about to change the variant from {0} to {1}.\n\nWarning!!!\n\nThis will require a new game\n\nIf you customized the list, all added mods will be placed at the end of the loadorder and left unchecked.\n\nAre you sure you want to continue?", CurrentVariant, NewVariant)) == DialogResult.Yes)
                        {
                            DisableButtons();
                            ServiceSingleton.Dashboard.DisableSettings();

                            DoChangeVariant(CurrentVariant, NewVariant);
                        }                        
                    }
                    finally
                    {
                        EnableButtons();
                        BtnChange.Text = "Change";
                        ServiceSingleton.Dashboard.EnableSettings();
                        ServiceSingleton.Dashboard.NoStatus();
                        ServiceSingleton.Dashboard.ProgressCompleted();
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
