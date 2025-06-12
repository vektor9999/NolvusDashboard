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
        private List<string> AntiAliasing = new List<string>();
        private List<string> LODs = new List<string>();
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

            #endregion

        }

        private int RatioIndex(List<string> Ratios)
        {
            var Index = Ratios.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Settings.Ratio);

            return Index == -1 ? 0 : Index;
        }

        private int ResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.Settings.Width + "x" + WorkingInstance.Settings.Height);

            return Index == -1 ? 0 : Index;
        }

        private int DownscalingResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.Performance.DownWidth + "x" + WorkingInstance.Performance.DownHeight);

            return Index == -1 ? 0 : Index;
        }

        private int VariantIndex(IEnumerable<INolvusVariantDTO> Variants)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Variants.ToList().FindIndex(x => x.Name == WorkingInstance.Performance.Variant);

            return Index == -1 ? 0 : Index;
        }

        private int AntiAliasingIndex(List<string> AntiAliasing)
        {
            var Index = AntiAliasing.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing);
            return Index == -1 ? 0 : Index;
        }

        private int LODsIndex(List<string> LODs)
        {
            var Index = LODs.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Performance.LODs);
            return Index == -1 ? 0 : Index;
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

                List<string> Ratios = new List<string>();

                Ratios.Add("16:9");
                Ratios.Add("21:9");
                Ratios.Add("32:9");

                DrpDwnLstRatios.DataSource = Ratios;

                DrpDwnLstRatios.SelectedIndex = RatioIndex(Ratios);

                Instance.Settings.Height = Screen.PrimaryScreen.Bounds.Height.ToString();
                Instance.Settings.Width = Screen.PrimaryScreen.Bounds.Width.ToString();

                DrpDwnLstScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;                

                DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);

                DrpDwnLstDownscalingScreenRes.Enabled = false;

                DrpDwnLstDownscalingScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;

                DrpDwnLstDownscalingScreenRes.SelectedIndex = DownscalingResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);

                TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;

                if (Instance.Performance.DownScaling == "TRUE")
                {
                    TglBtnDownScale.ToggleState = ToggleButtonState.Active;
                }

                #endregion                

                #region Graphics

                AntiAliasing.Add("TAA");
                AntiAliasing.Add("DLAA");
                AntiAliasing.Add("FSR");

                LODs.Add("Ultra");
                LODs.Add("Performance");
                LODs.Add("Ultra Performance");

                IniSettings.Add("Low");
                IniSettings.Add("Medium");
                IniSettings.Add("High");

                DrpDwnLstAntiAliasing.DataSource = AntiAliasing;
                DrpDwnLstAntiAliasing.SelectedIndex = AntiAliasingIndex(AntiAliasing);

                DrpDwnLstIni.DataSource = IniSettings;
                DrpDwnLstIni.SelectedIndex = System.Convert.ToInt16(Instance.Performance.IniSettings);                

                DrpDwnLstLODs.DataSource = LODs;
                DrpDwnLstLODs.SelectedIndex = LODsIndex(LODs);

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

                var Variants = await ApiManager.Service.Installer.GetNolvusVariants();

                DrpDwnLstVariant.DataSource = Variants;
                DrpDwnLstVariant.DisplayMember = "Name";
                DrpDwnLstVariant.ValueMember = "Id";

                DrpDwnLstVariant.SelectedIndex = VariantIndex(Variants);

                TglBtnSREX.ToggleStateChanging -= TglBtnSREX_ToggleStateChanging;

                TglBtnSREX.ToggleState = ToggleButtonState.Inactive;

                if (Instance.Performance.SREX == "TRUE")
                {
                    TglBtnSREX.ToggleState = ToggleButtonState.Active;
                }

                TglBtnSREX.ToggleStateChanging += TglBtnSREX_ToggleStateChanging;

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
            var Performance = Instance.Performance;            

            if (Performance.DownScaling == "TRUE" && (Performance.DownScaling == "TRUE" && (Instance.Settings.Height == Performance.DownHeight || System.Convert.ToInt32(Performance.DownHeight) > System.Convert.ToInt32(Instance.Settings.Height)) && (Instance.Settings.Width == Performance.DownWidth || System.Convert.ToInt32(Performance.DownWidth) > System.Convert.ToInt32(Instance.Settings.Width))))
            {
                NolvusMessageBox.ShowMessage("Invalid Downscaling setting", "If downscaling is enabled, the downscaled resolution must be less than the monitor resolution!", MessageBoxType.Error);
            }
            else
            {
                if (NolvusMessageBox.ShowConfirmation("Confirmation", "Remember, running the list without the right hardware requirement for the variant you choose can make the game instable. The variant can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
                {                    
                    ServiceSingleton.Dashboard.LoadFrame<v6.OptionsFrame>();                    
                }                
            }
        }

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing != "DLAA" && ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing != "FSR")
            {
                if (e.ToggleState == ToggleButtonState.Active)
                {
                    DrpDwnLstDownscalingScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.Performance.DownScaling = "TRUE";
                    DrpDwnLstDownscalingScreenRes.SelectedIndex = DownscalingResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
                }
                else
                {
                    DrpDwnLstDownscalingScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.Performance.DownScaling = "FALSE";
                    DrpDwnLstAntiAliasing.Enabled = true;
                }
            }
            else
            {                
                DrpDwnLstAntiAliasing.SelectedIndex = 0;
                DrpDwnLstAntiAliasing.Enabled = false;
                NolvusMessageBox.ShowMessage("Antialiasing", "This option is not compatible with DLAA/FSR. Antialiasing has been forced to TAA.", MessageBoxType.Info);
                DrpDwnLstDownscalingScreenRes.Enabled = true;
                ServiceSingleton.Instances.WorkingInstance.Performance.DownScaling = "TRUE";
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
                ServiceSingleton.Instances.WorkingInstance.Performance.SREX = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.SREX = "FALSE";
            }

            UpdateHardwareConfiguration();
        }

        private async void DrpDwnLstVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.Variant = (DrpDwnLstVariant.SelectedItem as INolvusVariantDTO).Name;

            INolvusVariantDTO Variant = DrpDwnLstVariant.SelectedItem as INolvusVariantDTO;

            LblTextures.Text = Variant.Textures;
            LblTrees.Text = Variant.Trees;
            LblCities.Text = Variant.Cities;
            
            if (ServiceSingleton.Instances.WorkingInstance.Performance.Variant == "Ultra")
            {
                LblLods.ForeColor = Color.White;
                DrpDwnLstLODs.Enabled = true;
            }
            else
            {
                LblLods.ForeColor = Color.Gray;
                DrpDwnLstLODs.SelectedIndex = 0;
                DrpDwnLstLODs.Enabled = false;
            }

            MinRequirements = await ApiManager.Service.Installer.GetNolvusVariantMinimumRequirements(DrpDwnLstVariant.SelectedValue.ToString());
            RecRequirements = await ApiManager.Service.Installer.GetNolvusVariantRecommendedRequirements(DrpDwnLstVariant.SelectedValue.ToString());

            UpdateHardwareConfiguration();
        }

        private void DrpDwnLstAntiAliasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstAntiAliasing.SelectedValue != null)
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing = DrpDwnLstAntiAliasing.SelectedValue.ToString();

                if (DrpDwnLstAntiAliasing.SelectedValue.ToString() == "DLAA")
                {
                    if (!IsNvidiaRTX() && !ServiceSingleton.Settings.ForceAA)
                    {
                        NolvusMessageBox.ShowMessage("Anti Alisasing", "DLAA is only compatible with NVIDIA graphics cards!", MessageBoxType.Error);
                        DrpDwnLstAntiAliasing.SelectedIndexChanged -= DrpDwnLstAntiAliasing_SelectedIndexChanged;
                        DrpDwnLstAntiAliasing.SelectedIndex = 0;
                        ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing = "TAA";
                        DrpDwnLstAntiAliasing.SelectedIndexChanged += DrpDwnLstAntiAliasing_SelectedIndexChanged;
                    }
                }                                
            }
        }        

        private void DrpDwnLstScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;            

            string Resolution = DrpDwnLstScreenRes.SelectedValue.ToString();

            string[] Reso = Resolution.Split(new char[] { 'x' });

            WorkingInstance.Settings.Width = Reso[0];
            WorkingInstance.Settings.Height = Reso[1];            

            UpdateHardwareConfiguration();
        }

        private void DrpDwnLstDownscalingScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstDownscalingScreenRes.SelectedValue != null)
            {
                string Resolution = DrpDwnLstDownscalingScreenRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                ServiceSingleton.Instances.WorkingInstance.Performance.DownWidth = Reso[0];
                ServiceSingleton.Instances.WorkingInstance.Performance.DownHeight = Reso[1];                

                UpdateHardwareConfiguration();
            }
        }

        private void DrpDwnLstRatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.Ratio = DrpDwnLstRatios.SelectedValue.ToString();
        }        

        private void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();
        }

        private void DrpDwnLstLODs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.LODs = DrpDwnLstLODs.SelectedValue.ToString();

            UpdateHardwareConfiguration();
        }

        private void UpdateHardwareConfiguration()
        {            
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            LblSelRes.Text = WorkingInstance.GetSelectedResolution();

            if (MinRequirements != null)
            {

                INolvusVariantRequirementDTO MinRequirement = MinRequirements.Where(x => x.Height.ToString() == WorkingInstance.GetSelectedHeight() &&
                                                                                         x.Width.ToString() == WorkingInstance.GetSelectedWidth() &&
                                                                                         x.SREX.ToString().ToUpper() == WorkingInstance.Performance.SREX &&
                                                                                         x.Lods == WorkingInstance.Performance.LODs).FirstOrDefault();                

                if (MinRequirement != null)
                {
                    LblMinCPU.Text = MinRequirement.CPU;
                    LblMinGPU.Text = string.Format("{0} {1}", MinRequirement.GPUVendor, MinRequirement.GPUName);
                    LblMinVRAM.Text = string.Format("{0} GB", MinRequirement.VRAM.ToString());
                    LblMinInstallSize.Text = string.Format("{0} GB", MinRequirement.InstallationSize.ToString());
                    LblMinDownloadSize.Text = string.Format("{0} GB (Optional)", MinRequirement.DownloadSize.ToString());
                }
                else
                {

                    MinRequirement = MinRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.Performance.SREX && x.Lods == WorkingInstance.Performance.LODs).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.GetSelectedWidth()) - x.Width)).FirstOrDefault();

                    if ( MinRequirement != null)
                    {
                        LblMinCPU.Text = MinRequirement.CPU;
                        LblMinGPU.Text = string.Format("{0} {1}", MinRequirement.GPUVendor, MinRequirement.GPUName);
                        LblMinVRAM.Text = string.Format("{0} GB", MinRequirement.VRAM.ToString());
                        LblMinInstallSize.Text = string.Format("{0} GB", MinRequirement.InstallationSize.ToString());
                        LblMinDownloadSize.Text = string.Format("{0} GB (Optional)", MinRequirement.DownloadSize.ToString());
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
                INolvusVariantRequirementDTO RecRequirement = RecRequirements.Where(x => x.Height.ToString() == WorkingInstance.GetSelectedHeight() &&
                                                                                         x.Width.ToString() == WorkingInstance.GetSelectedWidth() &&
                                                                                         x.SREX.ToString().ToUpper() == WorkingInstance.Performance.SREX &&
                                                                                         x.Lods == WorkingInstance.Performance.LODs).FirstOrDefault();

                if (RecRequirement != null)
                {
                    LblMaxCPU.Text = RecRequirement.CPU;
                    LblMaxGPU.Text = string.Format("{0} {1}", RecRequirement.GPUVendor, RecRequirement.GPUName);
                    LblMaxVRAM.Text = string.Format("{0} GB", RecRequirement.VRAM.ToString());
                    LblMinInstallSize.Text = string.Format("{0} GB", RecRequirement.InstallationSize.ToString());
                    LblMinDownloadSize.Text = string.Format("{0} GB (Optional)", RecRequirement.DownloadSize.ToString());
                }
                else
                {
                    RecRequirement = RecRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.Performance.SREX && x.Lods == WorkingInstance.Performance.LODs).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.GetSelectedWidth()) - x.Width)).FirstOrDefault();

                    if (RecRequirement != null)
                    {
                        LblMaxCPU.Text = RecRequirement.CPU;
                        LblMaxGPU.Text = string.Format("{0} {1}", RecRequirement.GPUVendor, RecRequirement.GPUName);
                        LblMaxVRAM.Text = string.Format("{0} GB", RecRequirement.VRAM.ToString());
                        LblMinInstallSize.Text = string.Format("{0} GB", RecRequirement.InstallationSize.ToString());
                        LblMinDownloadSize.Text = string.Format("{0} GB (Optional)", RecRequirement.DownloadSize.ToString());
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
                    Value = MinRequirements.Where(x => x.SREX.ToString().ToUpper() == WorkingInstance.Performance.SREX &&
                                                  x.Lods == WorkingInstance.Performance.LODs
                                                 ).OrderBy(x => Math.Abs(System.Convert.ToInt32(WorkingInstance.GetSelectedWidth()) - x.Width)).FirstOrDefault()
                })
            );
        }

        private void BtnVariantPreviex_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=y-Xis9XuETk");
        }
    }
}
