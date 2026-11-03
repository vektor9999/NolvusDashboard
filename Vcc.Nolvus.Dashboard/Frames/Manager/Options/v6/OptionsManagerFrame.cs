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

namespace Vcc.Nolvus.Dashboard.Frames.Manager.Options.v6
{
    public partial class OptionsManagerFrame : DashboardFrame
    {
        public OptionsManagerFrame()
        {
            InitializeComponent();
        }

        public OptionsManagerFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            TglBtnNewValue.ActiveState.Text = "ON";
            TglBtnNewValue.ActiveState.BackColor = Color.Orange;
            TglBtnNewValue.ActiveState.BorderColor = Color.Orange;
            TglBtnNewValue.ActiveState.ForeColor = Color.White;
            TglBtnNewValue.ActiveState.HoverColor = Color.Orange;

            TglBtnNewValue.InactiveState.Text = "OFF";
            TglBtnNewValue.InactiveState.BackColor = Color.White;
            TglBtnNewValue.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnNewValue.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnNewValue.InactiveState.HoverColor = Color.White;

            TglBtnNewValue.Visible = false;
            DrpDwnLstNewValue.Visible = false;

            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Options Manager]");
            ServiceSingleton.Dashboard.Info("Select an option to change");
        }

        private int ValueIndex(IEnumerable<INolvusOptionChoiceDTO> Values, string Option)
        {
            var Index = Values.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue(Option));

            return Index == -1 ? 0 : Index;
        }

        private string GetNewValue()
        {            
            var SelectedOption = (DrpDwnLstOption.SelectedItem as INolvusOptionDTO);            

            if (SelectedOption.DataType == "bool")
            {                
                return TglBtnNewValue.ToggleState == ToggleButtonState.Active ? "TRUE" : "FALSE";
            }
            else if (SelectedOption.DataType == "list")
            {
                return (DrpDwnLstNewValue.SelectedItem as INolvusOptionChoiceDTO).Value;
            }

            return string.Empty;
        }

        private string GetFormatedValue(bool Current)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            var SelectedOption = (DrpDwnLstOption.SelectedItem as INolvusOptionDTO);
            var CurrentOption = Instance.OptionsManager.GetOption(SelectedOption.Name);

            if (SelectedOption.DataType == "bool")
            {
                if (Current)
                {
                    return CurrentOption.Value == "TRUE" ? "Yes" : "No";
                }
                else
                {
                    return TglBtnNewValue.ToggleState == ToggleButtonState.Active ? "Yes" : "No";
                }                
            }
            else if (SelectedOption.DataType == "list")
            {
                if(Current)
                {
                    return CurrentOption.Value;
                }
                else
                {
                    return (DrpDwnLstNewValue.SelectedItem as INolvusOptionChoiceDTO).Value;
                }
            }

            return string.Empty;
        }

        protected override async Task OnLoadedAsync()
        {
            var Categories = await ApiManager.Service.Installer.GetOptionCategories();

            DrpDwnLstCategory.DataSource = Categories;
            DrpDwnLstCategory.DisplayMember = "Name";
            DrpDwnLstCategory.ValueMember = "Id";

            DrpDwnLstCategory.SelectedIndex = 0;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.UnloadWorkingIntance();
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }        

        private async void DrpDwnLstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;
            var Options = await ApiManager.Service.Installer.GetOptionsByCategory(Instance.SettingsManager.Variant, (DrpDwnLstCategory.SelectedItem as INolvusOptionCategoryDTO).Name, Instance.Id);            

            DrpDwnLstOption.DataSource = null;

            DrpDwnLstOption.DataSource = Options;
            DrpDwnLstOption.DisplayMember = "FriendlyName";
            DrpDwnLstOption.ValueMember = "Name";

            DrpDwnLstOption.SelectedIndex = 0;
        }

        private async void DrpDwnLstOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            var SelectedOption = (DrpDwnLstOption.SelectedItem as INolvusOptionDTO);
            var CurrentOption = Instance.OptionsManager.GetOption(SelectedOption.Name);

            string CurrentValue = string.Empty;

            TglBtnNewValue.Visible = false;
            DrpDwnLstNewValue.Visible = false;

            if (SelectedOption.DataType == "bool")
            {
                TglBtnNewValue.Visible = true;

                TglBtnNewValue.ToggleState = ToggleButtonState.Inactive;

                if (CurrentOption.Value == "TRUE")
                {
                    TglBtnNewValue.ToggleState = ToggleButtonState.Active;
                }

                CurrentValue = CurrentOption.Value == "TRUE" ? "Yes" : "No";
            }
            else if (SelectedOption.DataType == "list")
            {
                DrpDwnLstNewValue.Visible = true;

                var Values = await ApiManager.Service.Installer.GetOptionChoices(SelectedOption.Name, Instance.SettingsManager.Variant, Instance.Id);

                DrpDwnLstNewValue.DataSource = null;
                DrpDwnLstNewValue.DataSource = Values;
                DrpDwnLstNewValue.DisplayMember = "Value";
                DrpDwnLstNewValue.ValueMember = "Value";

                DrpDwnLstNewValue.SelectedIndex = ValueIndex(Values, SelectedOption.Name);

                CurrentValue = CurrentOption.Value;
            }
            else
            {
                CurrentValue = CurrentOption.Value;
            }

            LblCurrentValue.Text = CurrentValue;
        }

        private async void DoChangeOption(INolvusOption CurrentOption, string NewValue, bool BackupSaveFiles)
        {            
            try
            {
                var Instance = ServiceSingleton.Instances.WorkingInstance;                        

                var ModsToUpdate = await ServiceSingleton.OptionManager.PrepareModsToUpdate(CurrentOption.Name, CurrentOption.Value, GetNewValue());

                if (ModsToUpdate.Count > 0)
                {
                    Instance.StatusManager.AddField("Option", CurrentOption.Name);
                    Instance.StatusManager.AddField("OldOption", CurrentOption.Value);
                    Instance.StatusManager.AddField("NewOption", NewValue);

                    Instance.OptionsManager.SetOptionValue(CurrentOption.Name, NewValue);

                    if (BackupSaveFiles)
                    {
                        await Instance.BackupGameSaveFiles();
                    }

                    ServiceSingleton.Instances.PrepareInstanceForOption();
                    
                    await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>(new FrameParameters(new FrameParameter() { Key = "ModsToInstall", Value = ModsToUpdate.Cast<IInstallableElement>().ToList() }));
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Info", string.Format("{0} doesn't have any mod associated with it for {1}!\n\nThis Option Could be linked to an older or new Nolvus version.\n\nCheck for Nolvus list update.", CurrentOption.Name, Instance.FullName), MessageBoxType.Error);
                }
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during option change initialization", ex.Message, ex.StackTrace);
            }                      
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            if (!ModOrganizer.IsRunning)
            {
                var Instance = ServiceSingleton.Instances.WorkingInstance;

                var SelectedOption = (DrpDwnLstOption.SelectedItem as INolvusOptionDTO);
                var CurrentOption = Instance.OptionsManager.GetOption(SelectedOption.Name);

                if (GetNewValue() == CurrentOption.Value)
                {
                    NolvusMessageBox.ShowMessage("Error", string.Format("New value ({0}) for {1} didn't change!", CurrentOption.Value, CurrentOption.Name), MessageBoxType.Error);
                }
                else
                {
                    try
                    {
                        if (NolvusMessageBox.ShowConfirmation("Confirmation", string.Format("You are about to change {0} from {1} to {2}.\n\nWarning!!!\n\nIf you customized the list, all added mods will be placed at the end of the loadorder and left unchecked.\n\nAre you sure you want to continue?", CurrentOption.FriendlyName, GetFormatedValue(true), GetFormatedValue(false))) == DialogResult.Yes)
                        {
                            if (!SelectedOption.NewGameRequired)
                            {
                                DisableButtons();
                                ServiceSingleton.Dashboard.DisableSettings();

                                DoChangeOption(CurrentOption, GetNewValue(), false);
                            }
                            else if (NolvusMessageBox.ShowConfirmation("Confirmation", string.Format("Changing {0} will require a new game!\n\nTo prevent issues, your save files will be removed from the saves directory and will be backed up!\n\nAre you sure you want to continue?", CurrentOption.FriendlyName)) == DialogResult.Yes)
                            {
                                DisableButtons();
                                ServiceSingleton.Dashboard.DisableSettings();

                                DoChangeOption(CurrentOption, GetNewValue(), true);
                            }
                        }
                    }
                    finally
                    {
                        ServiceSingleton.Dashboard.EnableSettings();
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
