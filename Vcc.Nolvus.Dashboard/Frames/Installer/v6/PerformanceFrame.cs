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
    public partial class PerformanceFrame : DashboardFrame
    {        
        private List<string> IniSettings = new List<string>();

        private IEnumerable<INolvusVariantRequirementDTO> MinRequirements;
        private IEnumerable<INolvusVariantRequirementDTO> RecRequirements;

        public PerformanceFrame()
        {
            InitializeComponent();                      
        }
        public PerformanceFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();

            #region Toggle Buttons

            TglBtnDownScale.ActiveState.Text = "ON";
            TglBtnDownScale.ActiveState.BackColor = Color.Orange;
            TglBtnDownScale.ActiveState.BorderColor = Color.Orange;
            TglBtnDownScale.ActiveState.ForeColor = Color.White;
            TglBtnDownScale.ActiveState.HoverColor = Color.Orange;

            TglBtnDownScale.InactiveState.Text = "OFF";
            TglBtnDownScale.InactiveState.BackColor = Color.White;
            TglBtnDownScale.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnDownScale.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnDownScale.InactiveState.HoverColor = Color.White;

            TglBtnSREX.ActiveState.Text = "ON";
            TglBtnSREX.ActiveState.BackColor = Color.Orange;
            TglBtnSREX.ActiveState.BorderColor = Color.Orange;
            TglBtnSREX.ActiveState.ForeColor = Color.White;
            TglBtnSREX.ActiveState.HoverColor = Color.Orange;

            TglBtnSREX.InactiveState.Text = "OFF";
            TglBtnSREX.InactiveState.BackColor = Color.White;
            TglBtnSREX.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnSREX.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnSREX.InactiveState.HoverColor = Color.White;

            TglBtnFrameGen.ActiveState.Text = "ON";
            TglBtnFrameGen.ActiveState.BackColor = Color.Orange;
            TglBtnFrameGen.ActiveState.BorderColor = Color.Orange;
            TglBtnFrameGen.ActiveState.ForeColor = Color.White;
            TglBtnFrameGen.ActiveState.HoverColor = Color.Orange;

            TglBtnFrameGen.InactiveState.Text = "OFF";
            TglBtnFrameGen.InactiveState.BackColor = Color.White;
            TglBtnFrameGen.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            TglBtnFrameGen.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            TglBtnFrameGen.InactiveState.HoverColor = Color.White;

            #endregion

        }

        private int RatioIndex(IEnumerable<INolvusOptionChoiceDTO> Ratios)
        {
            var Index = Ratios.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("Ratio"));

            return Index == -1 ? 0 : Index;
        }

        private int ResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.SettingsManager.Width + "x" + WorkingInstance.SettingsManager.Height);

            return Index == -1 ? 0 : Index;
        }

        private int DownscalingResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.SettingsManager.DownWidth + "x" + WorkingInstance.SettingsManager.DownHeight);

            return Index == -1 ? 0 : Index;
        }

        private int VariantIndex(IEnumerable<INolvusVariantDTO> Variants)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Variants.ToList().FindIndex(x => x.Name == WorkingInstance.SettingsManager.Variant);

            return Index == -1 ? 0 : Index;
        }

        private int AntiAliasingIndex(IEnumerable<INolvusOptionChoiceDTO> AntiAliasing)
        {
            var Index = AntiAliasing.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing"));
            return Index == -1 ? 0 : Index;
        }

        private int LODsIndex(IEnumerable<INolvusOptionChoiceDTO> LODs)
        {
            var Index = LODs.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("LODs"));
            return Index == -1 ? 0 : Index;
        }

        private async Task LoadLODs()
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            var LODs = await ApiManager.Service.Installer.GetOptionChoices("LODs", Instance.SettingsManager.Variant, Instance.Id);

            DrpDwnLstLODs.DataSource = null;
            DrpDwnLstLODs.DataSource = LODs;
            DrpDwnLstLODs.DisplayMember = "Value";
            DrpDwnLstLODs.ValueMember = "Id";

            DrpDwnLstLODs.SelectedIndex = LODsIndex(LODs);
        }

        private bool IsNvidiaRTX()
        {
            return ServiceSingleton.Globals.GetVideoAdapters().Where(x => x.Contains("NVIDIA") && x.Contains("RTX")).FirstOrDefault() != null;
        }

        protected override async Task OnLoadAsync()
        {
            try
            {
                var Instance = ServiceSingleton.Instances.WorkingInstance;

                #region Resolution                

                var Ratios = await ApiManager.Service.Installer.GetOptionChoices("Ratio", Instance.SettingsManager.Variant, Instance.Id);

                DrpDwnLstRatios.DataSource = Ratios;
                DrpDwnLstRatios.DisplayMember = "Value";
                DrpDwnLstRatios.ValueMember = "Id";

                DrpDwnLstRatios.SelectedIndex = RatioIndex(Ratios);

                Instance.SettingsManager.Height = Screen.PrimaryScreen.Bounds.Height.ToString();
                Instance.SettingsManager.Width = Screen.PrimaryScreen.Bounds.Width.ToString();

                DrpDwnLstScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;                

                DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);

                DrpDwnLstDownscalingScreenRes.Enabled = false;

                DrpDwnLstDownscalingScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;

                DrpDwnLstDownscalingScreenRes.SelectedIndex = DownscalingResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);

                TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;

                if (Instance.SettingsManager.DownScaling == "TRUE")
                {
                    TglBtnDownScale.ToggleState = ToggleButtonState.Active;
                }

                #endregion                

                #region Graphics                               

                IniSettings.Add("Low");
                IniSettings.Add("Medium");
                IniSettings.Add("High");

                DrpDwnLstIni.DataSource = IniSettings;
                DrpDwnLstIni.SelectedIndex = System.Convert.ToInt16(Instance.SettingsManager.IniSettings);

                var AAs = await ApiManager.Service.Installer.GetOptionChoices("Antialiasing", Instance.SettingsManager.Variant, Instance.Id);

                DrpDwnLstAntiAliasing.DataSource = AAs;
                DrpDwnLstAntiAliasing.DisplayMember = "Value";
                DrpDwnLstAntiAliasing.ValueMember = "Id";
                
                DrpDwnLstAntiAliasing.SelectedIndex = AntiAliasingIndex(AAs);                

                await LoadLODs();

                LblCPU.Text = await ServiceSingleton.Globals.GetCPUInfo();

                var Ram = await ServiceSingleton.Globals.GetRamCount();

                if (Ram != "RAM count not found")
                {
                    LblRAM.Text = Ram + " GB";
                }
                else
                {
                    LblRAM.Text = Ram;
                }

                var GPU = string.Join(Environment.NewLine, ServiceSingleton.Globals.GetVideoAdapters().ToArray());

                LblGPUs.Text = GPU;

                if (ServiceSingleton.Settings.ForceAA)
                {
                    LblGPUs.Text = GPU + " (CHECK BYPASSED)";
                }

                #endregion

                #region Variants

                var Variants = await ApiManager.Service.Installer.GetNolvusVariants(Instance.Id);                

                DrpDwnLstVariant.DataSource = Variants;
                DrpDwnLstVariant.DisplayMember = "FullName";
                DrpDwnLstVariant.ValueMember = "Id";

                DrpDwnLstVariant.SelectedIndex = VariantIndex(Variants);

                TglBtnSREX.ToggleStateChanging -= TglBtnSREX_ToggleStateChanging;

                TglBtnSREX.ToggleState = ToggleButtonState.Inactive;

                if (Instance.OptionsManager.GetOptionValue("SREX") == "TRUE")
                {
                    TglBtnSREX.ToggleState = ToggleButtonState.Active;
                }

                TglBtnSREX.ToggleStateChanging += TglBtnSREX_ToggleStateChanging;

                TglBtnFrameGen.ToggleState = ToggleButtonState.Inactive;

                if (Instance.OptionsManager.GetOptionValue("FrameGeneration") == "TRUE")
                {
                    TglBtnFrameGen.ToggleState = ToggleButtonState.Active;
                }                

                #endregion

                ServiceSingleton.Dashboard.Info("Graphics and Performance Settings");
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during performance options loading", ex.Message, ex.StackTrace);
            }
        }        
        
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<PathFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;
            var SettingsManager = Instance.SettingsManager;            

            if (SettingsManager.DownScaling == "TRUE" && ((SettingsManager.Height == SettingsManager.DownHeight || System.Convert.ToInt32(SettingsManager.DownHeight) > System.Convert.ToInt32(SettingsManager.Height)) && (SettingsManager.Width == SettingsManager.DownWidth || System.Convert.ToInt32(SettingsManager.DownWidth) > System.Convert.ToInt32(SettingsManager.Width))))
            {
                NolvusMessageBox.ShowMessage("Invalid Downscaling setting", "If downscaling is enabled, the downscaled resolution must be less than the monitor resolution!", MessageBoxType.Error);
            }
            else
            {
                if (NolvusMessageBox.ShowConfirmation("Confirmation", "Remember, running the list without the right hardware requirement for the variant you choose can make the game unstable. The variant can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
                {                    
                    ServiceSingleton.Dashboard.LoadFrameAsync<v6.OptionsFrame>();                    
                }                
            }
        }

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing") != "DLAA" && ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing") != "FSR" && ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("FrameGeneration") != "TRUE")
            {
                if (e.ToggleState == ToggleButtonState.Active)
                {
                    DrpDwnLstDownscalingScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "TRUE";
                    DrpDwnLstDownscalingScreenRes.SelectedIndex = DownscalingResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
                }
                else
                {
                    DrpDwnLstDownscalingScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "FALSE";
                    DrpDwnLstAntiAliasing.Enabled = true;
                    TglBtnFrameGen.Enabled = true;
                }
            }
            else
            {                
                DrpDwnLstAntiAliasing.SelectedIndex = 0;
                DrpDwnLstAntiAliasing.Enabled = false;
                TglBtnFrameGen.ToggleState = ToggleButtonState.Inactive;
                TglBtnFrameGen.Enabled = false;
                NolvusMessageBox.ShowMessage("Antialiasing", "This option is not compatible with DLAA/FSR/Frame Generation. Antialiasing has been forced to TAA and Frame Generation has been disabled.", MessageBoxType.Info);
                DrpDwnLstDownscalingScreenRes.Enabled = true;
                ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "TRUE";
                DrpDwnLstDownscalingScreenRes.SelectedIndex = DownscalingResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
            }

            UpdateHardwareConfiguration();
        }

        private void TglBtnSREX_ToggleStateChanging(object sender, CancelEventArgs e)
        {
            if (TglBtnSREX.ToggleState == ToggleButtonState.Inactive)
            {
                e.Cancel = NolvusMessageBox.ShowConfirmation("Warning", "The SR Exterior Cities option is very performance heavy Don't try to run it at 4K without any tools like Framegen or Loseless Scaling. Are you sure?") == DialogResult.No;
            }            
        }

        private void TglBtnSREX_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("SREX").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("SREX").Value = "FALSE";
            }

            UpdateHardwareConfiguration();
        }

        private void TglBtnFrameGen_ToggleStateChanging(object sender, CancelEventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling == "TRUE")
            {
                NolvusMessageBox.ShowMessage("Frame Generation", "Frame Generation is not compatible with downscaling, Frame Generation has been disabled.", MessageBoxType.Info);
                e.Cancel = true;
            }
        }

        private void TglBtnFrameGen_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                NolvusMessageBox.ShowMessage("Frame Generation", "Warning, the ENB FPS counter will still show the normal FPS, to see the interpolated FPS, please use the NVIDIA or AMD integrated overlay.\n\nFramegen will not work out of the box if your monitor doesn't handle 120hz minimum.\n\nTo bypass that in game, open the ENB ui (shift+enter), on the left panel go under performance and check Force Framegen. Hit apply settings then save settings and restart the game.", MessageBoxType.Info);
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("FrameGeneration").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("FrameGeneration").Value = "FALSE";
            }
        }

        private async void DrpDwnLstVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            Instance.SettingsManager.Variant = (DrpDwnLstVariant.SelectedItem as INolvusVariantDTO).Name;

            await Instance.OptionsManager.SynchronizeOptions(Instance);

            await LoadLODs();

            INolvusVariantDTO Variant = DrpDwnLstVariant.SelectedItem as INolvusVariantDTO;

            LblTextures.Text = Variant.Textures;
            LblTrees.Text = Variant.Trees;
            LblCities.Text = Variant.Cities;
            
            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant == "Ultra")
            {
                LblLods.ForeColor = Color.White;
                DrpDwnLstLODs.Enabled = true;
            }
            else if(ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant == "Ultimate" || ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant == Strings.GO)
            {
                LblLods.ForeColor = Color.Gray;
                DrpDwnLstLODs.SelectedIndex = 0;
                DrpDwnLstLODs.Enabled = false;
            }
            else
            {
                LblLods.ForeColor = Color.Gray;                
                DrpDwnLstLODs.SelectedIndex = 0;
                DrpDwnLstLODs.Enabled = false;                
            }

            MinRequirements = await ApiManager.Service.Installer.GetNolvusVariantMinimumRequirements(DrpDwnLstVariant.SelectedValue.ToString());
            RecRequirements = await ApiManager.Service.Installer.GetNolvusVariantRecommendedRequirements(DrpDwnLstVariant.SelectedValue.ToString());

            LblNote.Text = Variant.Note;

            UpdateHardwareConfiguration();
        }

        private void ForceTAA(string Message)
        {
            NolvusMessageBox.ShowMessage("Anti Alisasing", Message, MessageBoxType.Error);
            DrpDwnLstAntiAliasing.SelectedIndexChanged -= DrpDwnLstAntiAliasing_SelectedIndexChanged;
            DrpDwnLstAntiAliasing.SelectedIndex = 0;
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AntiAliasing").Value = "TAA";
            DrpDwnLstAntiAliasing.SelectedIndexChanged += DrpDwnLstAntiAliasing_SelectedIndexChanged;
        }

        private void DrpDwnLstAntiAliasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstAntiAliasing.SelectedValue != null)
            {
                var AAValue = (DrpDwnLstAntiAliasing.SelectedItem as INolvusOptionChoiceDTO).Value;

                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AntiAliasing").Value = AAValue;

                if (AAValue == "DLAA")
                {
                    if (!IsNvidiaRTX() && !ServiceSingleton.Settings.ForceAA)
                    {
                        ForceTAA("DLAA is only compatible with NVIDIA graphics cards!");                        
                    }
                    else if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling == "TRUE")
                    {
                        ForceTAA("DLAA is not compatible with downscaling");
                    }                  
                }
                else if (AAValue == "FSR")
                {
                    if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling == "TRUE")
                    {
                        ForceTAA("FSR is not compatible with downscaling");
                    }                    
                }                                                
            }
        }        

        private void DrpDwnLstScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;            

            string Resolution = DrpDwnLstScreenRes.SelectedValue.ToString();

            string[] Reso = Resolution.Split(new char[] { 'x' });

            WorkingInstance.SettingsManager.Width = Reso[0];
            WorkingInstance.SettingsManager.Height = Reso[1];

            if (
                    (WorkingInstance.SettingsManager.Width == "2560" && WorkingInstance.SettingsManager.Height == "1080") ||
                    (WorkingInstance.SettingsManager.Width == "3440" && WorkingInstance.SettingsManager.Height == "1440") ||
                    (WorkingInstance.SettingsManager.Width == "5120" && WorkingInstance.SettingsManager.Height == "2160")
                )
            {
                DrpDwnLstRatios.SelectedIndex = 1;
            }
            else if (
                        (WorkingInstance.SettingsManager.Width == "3840" && WorkingInstance.SettingsManager.Height == "1080") ||
                        (WorkingInstance.SettingsManager.Width == "5120" && WorkingInstance.SettingsManager.Height == "1440") ||
                        (WorkingInstance.SettingsManager.Width == "7680" && WorkingInstance.SettingsManager.Height == "2160")
                    )
            {
                DrpDwnLstRatios.SelectedIndex = 2;
            }

            UpdateHardwareConfiguration();
        }

        private void DrpDwnLstDownscalingScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstDownscalingScreenRes.SelectedValue != null)
            {
                string Resolution = DrpDwnLstDownscalingScreenRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownWidth = Reso[0];
                ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownHeight = Reso[1];                

                UpdateHardwareConfiguration();
            }
        }

        private void DrpDwnLstRatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Ratio").Value = (DrpDwnLstRatios.SelectedItem as INolvusOptionChoiceDTO).Value;
        }        

        private void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.SettingsManager.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();
        }

        private void DrpDwnLstLODs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("LODs").Value = (DrpDwnLstLODs.SelectedItem as INolvusOptionChoiceDTO).Value;

            UpdateHardwareConfiguration();
        }

        private void UpdateHardwareConfiguration()
        {            
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;            

            LblSelRes.Text = WorkingInstance.SettingsManager.GetSelectedResolution();

            if (MinRequirements != null)
            {

                INolvusVariantRequirementDTO MinRequirement = MinRequirements.Where(x => x.Height.ToString() == WorkingInstance.SettingsManager.GetSelectedHeight() &&
                                                                                         x.Width.ToString() == WorkingInstance.SettingsManager.GetSelectedWidth() &&
                                                                                         x.SREX.ToString().ToUpper() == WorkingInstance.OptionsManager.GetOptionValue("SREX") &&
                                                                                         x.Lods == WorkingInstance.OptionsManager.GetOptionValue("LODs")).FirstOrDefault();                

                if (MinRequirement != null)
                {
                    LblMinCPU.Text = MinRequirement.CPU;
                    LblMinGPU.Text = string.Format("{0} {1}", MinRequirement.GPUVendor, MinRequirement.GPUName);
                    LblMinVRAM.Text = string.Format("{0} GB", MinRequirement.VRAM.ToString());
                    LblMinInstallSize.Text = string.Format("{0} GB", MinRequirement.InstallationSize.ToString());
                    LblMinDownloadSize.Text = string.Format("{0} GB (Optional, if you don't want to keep archives, disable them in the previous screen)", MinRequirement.DownloadSize.ToString());
                }
                else
                {

                    MinRequirement = MinRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.OptionsManager.GetOptionValue("SREX") && x.Lods == WorkingInstance.OptionsManager.GetOptionValue("LODs")).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.SettingsManager.GetSelectedWidth()) - x.Width)).FirstOrDefault();

                    if ( MinRequirement != null)
                    {
                        LblMinCPU.Text = MinRequirement.CPU;
                        LblMinGPU.Text = string.Format("{0} {1}", MinRequirement.GPUVendor, MinRequirement.GPUName);
                        LblMinVRAM.Text = string.Format("{0} GB", MinRequirement.VRAM.ToString());
                        LblMinInstallSize.Text = string.Format("{0} GB", MinRequirement.InstallationSize.ToString());
                        LblMinDownloadSize.Text = string.Format("{0} GB (Optional, if you don't want to keep archives, disable them in the previous screen)", MinRequirement.DownloadSize.ToString());
                    }
                    else
                    {
                        LblMinCPU.Text = "NA";
                        LblMinGPU.Text = "NA";
                        LblMinVRAM.Text = "NA";
                        LblMinInstallSize.Text = "NA";
                        LblMinDownloadSize.Text = "NA";
                    }                    
                }
            }

            if (RecRequirements != null)
            {
                INolvusVariantRequirementDTO RecRequirement = RecRequirements.Where(x => x.Height.ToString() == WorkingInstance.SettingsManager.GetSelectedHeight() &&
                                                                                         x.Width.ToString() == WorkingInstance.SettingsManager.GetSelectedWidth() &&
                                                                                         x.SREX.ToString().ToUpper() == WorkingInstance.OptionsManager.GetOptionValue("SREX") &&
                                                                                         x.Lods == WorkingInstance.OptionsManager.GetOptionValue("LODs")).FirstOrDefault();

                if (RecRequirement != null)
                {
                    LblMaxCPU.Text = RecRequirement.CPU;
                    LblMaxGPU.Text = string.Format("{0} {1}", RecRequirement.GPUVendor, RecRequirement.GPUName);
                    LblMaxVRAM.Text = string.Format("{0} GB", RecRequirement.VRAM.ToString());
                    LblMinInstallSize.Text = string.Format("{0} GB", RecRequirement.InstallationSize.ToString());
                    LblMinDownloadSize.Text = string.Format("{0} GB (Optional, if you don't want to keep archives, disable them in the previous screen)", RecRequirement.DownloadSize.ToString());
                }
                else
                {
                    RecRequirement = RecRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.OptionsManager.GetOptionValue("SREX") && x.Lods == WorkingInstance.OptionsManager.GetOptionValue("LODs")).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.SettingsManager.GetSelectedWidth()) - x.Width)).FirstOrDefault();

                    if (RecRequirement != null)
                    {
                        LblMaxCPU.Text = RecRequirement.CPU;
                        LblMaxGPU.Text = string.Format("{0} {1}", RecRequirement.GPUVendor, RecRequirement.GPUName);
                        LblMaxVRAM.Text = string.Format("{0} GB", RecRequirement.VRAM.ToString());
                        LblMinInstallSize.Text = string.Format("{0} GB", RecRequirement.InstallationSize.ToString());
                        LblMinDownloadSize.Text = string.Format("{0} GB (Optional, if you don't want to keep archives, disable them in the previous screen)", RecRequirement.DownloadSize.ToString());
                    }
                    else
                    {
                        LblMaxCPU.Text = "NA";
                        LblMaxGPU.Text = "NA";
                        LblMaxVRAM.Text = "NA";
                        LblMinInstallSize.Text = "NA";
                        LblMinDownloadSize.Text = "NA";
                    }                    
                }
            }
        }

        private void BtnVerifyGPU_Click(object sender, EventArgs e)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            ServiceSingleton.Dashboard.LoadFrameAsync<v6.GPUFrame>(
                new FrameParameters(new FrameParameter()
                {
                    Key = "VariantRequirement",
                    Value = MinRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.OptionsManager.GetOptionValue("SREX") &&
                                                  x.Lods == WorkingInstance.OptionsManager.GetOptionValue("LODs")
                                                 ).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.SettingsManager.GetSelectedWidth()) - x.Width)).FirstOrDefault()
                })
            );
        }

        private void LnkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/appendix/installer/tech");
        }

        private void BtnVariantPreview_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=y-Xis9XuETk");
        }
    }
}
