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
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Installer.v5
{
    public partial class PerformanceFrame : DashboardFrame
    {                        
        private List<string> IniSettings = new List<string>();

        public PerformanceFrame()
        {
            InitializeComponent();                      
        }
        public PerformanceFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();                                    

            this.TglBtnPhysics.ActiveState.Text = "ON";
            this.TglBtnPhysics.ActiveState.BackColor = Color.Orange;
            this.TglBtnPhysics.ActiveState.BorderColor = Color.Orange;
            this.TglBtnPhysics.ActiveState.ForeColor = Color.White;
            this.TglBtnPhysics.ActiveState.HoverColor = Color.Orange;

            this.TglBtnPhysics.InactiveState.Text = "OFF";
            this.TglBtnPhysics.InactiveState.BackColor = Color.White;
            this.TglBtnPhysics.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnPhysics.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnPhysics.InactiveState.HoverColor = Color.White;

            this.TglBtnDownScale.ActiveState.Text = "ON";
            this.TglBtnDownScale.ActiveState.BackColor = Color.Orange;
            this.TglBtnDownScale.ActiveState.BorderColor = Color.Orange;
            this.TglBtnDownScale.ActiveState.ForeColor = Color.White;
            this.TglBtnDownScale.ActiveState.HoverColor = Color.Orange;

            this.TglBtnDownScale.InactiveState.Text = "OFF";
            this.TglBtnDownScale.InactiveState.BackColor = Color.White;
            this.TglBtnDownScale.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnDownScale.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnDownScale.InactiveState.HoverColor = Color.White;

            this.TglBtnRayTracing.ActiveState.Text = "ON";
            this.TglBtnRayTracing.ActiveState.BackColor = Color.Orange;
            this.TglBtnRayTracing.ActiveState.BorderColor = Color.Orange;
            this.TglBtnRayTracing.ActiveState.ForeColor = Color.White;
            this.TglBtnRayTracing.ActiveState.HoverColor = Color.Orange;

            this.TglBtnRayTracing.InactiveState.Text = "OFF";
            this.TglBtnRayTracing.InactiveState.BackColor = Color.White;
            this.TglBtnRayTracing.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnRayTracing.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnRayTracing.InactiveState.HoverColor = Color.White;

            this.TglBtnFPSStabilizer.ActiveState.Text = "ON";
            this.TglBtnFPSStabilizer.ActiveState.BackColor = Color.Orange;
            this.TglBtnFPSStabilizer.ActiveState.BorderColor = Color.Orange;
            this.TglBtnFPSStabilizer.ActiveState.ForeColor = Color.White;
            this.TglBtnFPSStabilizer.ActiveState.HoverColor = Color.Orange;

            this.TglBtnFPSStabilizer.InactiveState.Text = "OFF";
            this.TglBtnFPSStabilizer.InactiveState.BackColor = Color.White;
            this.TglBtnFPSStabilizer.InactiveState.BorderColor = Color.FromArgb(150, 150, 150);
            this.TglBtnFPSStabilizer.InactiveState.ForeColor = Color.FromArgb(80, 80, 80);
            this.TglBtnFPSStabilizer.InactiveState.HoverColor = Color.White;
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

        private int DownResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.SettingsManager.DownWidth + "x" + WorkingInstance.SettingsManager.DownHeight);

            return Index == -1 ? 0 : Index;           
        }

        private int AntiAliasingIndex(IEnumerable<INolvusOptionChoiceDTO> AntiAliasing)
        {
            var Index = AntiAliasing.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing"));
            return Index == -1 ? 0 : Index;
        }

        private int VariantIndex(IEnumerable<INolvusVariantDTO> Variants)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Variants.ToList().FindIndex(x => x.Name == WorkingInstance.SettingsManager.Variant);

            return Index == -1 ? 0 : Index;
        }

        private int LODsIndex(IEnumerable<INolvusOptionChoiceDTO> LODs)
        {
            var Index = LODs.ToList().FindIndex(x => x.Value == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("LODs"));
            return Index == -1 ? 0 : Index;
        }

        private bool IsNvidiaRTX()
        {
            return ServiceSingleton.Globals.GetVideoAdapters().Where(x => x.Contains("NVIDIA") && x.Contains("RTX")).FirstOrDefault() != null;
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

                DrpDwnLstResolution.DataSource = ServiceSingleton.Globals.WindowsResolutions;

                DrpDwnLstResolution.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);

                DrpDwnLstScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;
                DrpDwnLstScreenRes.SelectedIndex = DownResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
                DrpDwnLstScreenRes.Enabled = false;

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

                if (!ServiceSingleton.Settings.ForceAA)
                {
                    LblGPUs.Text = GPU;

                    if (!IsNvidiaRTX())
                    {
                        ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AntiAliasing").Value = "TAA";
                        DrpDwnLstAntiAliasing.Enabled = false;
                    }
                }
                else
                {
                    LblGPUs.Text = GPU + " (CHECK BYPASSED)";
                }

                #endregion

                #region Variant

                var Variants = await ApiManager.Service.Installer.GetNolvusVariants(Instance.Id);

                DrpDwnLstVariant.DataSource = Variants;
                DrpDwnLstVariant.DisplayMember = "FullName";
                DrpDwnLstVariant.ValueMember = "Id";

                DrpDwnLstVariant.SelectedIndex = VariantIndex(Variants);

                this.TglBtnPhysics.ToggleState = ToggleButtonState.Inactive;

                if (Instance.OptionsManager.GetOptionValue("AdvancedPhysics") == "TRUE")
                {
                    this.TglBtnPhysics.ToggleState = ToggleButtonState.Active;
                }

                this.TglBtnRayTracing.ToggleState = ToggleButtonState.Inactive;

                if (Instance.OptionsManager.GetOptionValue("RayTracing") == "TRUE")
                {
                    this.TglBtnRayTracing.ToggleState = ToggleButtonState.Active;
                }

                this.TglBtnFPSStabilizer.ToggleState = ToggleButtonState.Inactive;

                if (Instance.OptionsManager.GetOptionValue("FPSStabilizer") == "TRUE")
                {
                    this.TglBtnFPSStabilizer.ToggleState = ToggleButtonState.Active;
                }

                #endregion

                ServiceSingleton.Dashboard.Info("Graphics and Performance Settings");
            }
            catch(Exception ex)
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
            var OptionsManager = Instance.OptionsManager;          

            if (SettingsManager.DownScaling == "TRUE" && ((SettingsManager.Height == SettingsManager.DownHeight || System.Convert.ToInt32(SettingsManager.DownHeight) > System.Convert.ToInt32(SettingsManager.Height)) && (SettingsManager.Width == SettingsManager.DownWidth || System.Convert.ToInt32(SettingsManager.DownWidth) > System.Convert.ToInt32(SettingsManager.Width))))
            {
                NolvusMessageBox.ShowMessage("Invalid Downscaling setting", "If downscaling is enabled, the downscaled resolution must be less than the monitor resolution!", MessageBoxType.Error);
            }
            else
            {
                if (NolvusMessageBox.ShowConfirmation("Confirmation", "Some of the options you selected (like the variant, LODs quality, Advanced physics, Global Illumination or FPS stabilizer) can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
                {                    
                    if ((SettingsManager.Variant == "Redux") && (OptionsManager.GetOptionValue("AdvancedPhysics") == "TRUE" || OptionsManager.GetOptionValue("RayTracing") == "TRUE" || OptionsManager.GetOptionValue("AntiAliasing") == "DLAA"))
                    {
                        if (NolvusMessageBox.ShowConfirmation("Confirmation", "You selected the Redux variant with other effects that are normally disabled by default with this variant. Be sure you have more than the minimum requirement. Are you sure you want to continue?") == DialogResult.Yes)
                        {
                            ServiceSingleton.Dashboard.LoadFrameAsync<v5.OptionsFrame>();
                        }
                    }
                    else
                    {                        
                        ServiceSingleton.Dashboard.LoadFrameAsync<v5.OptionsFrame>();
                    }                                      
                }                
            }
        }       

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing") != "DLAA")
            {
                if (e.ToggleState == ToggleButtonState.Active)
                {
                    DrpDwnLstScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "TRUE";
                    DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
                }
                else
                {
                    DrpDwnLstScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "FALSE";
                    DrpDwnLstAntiAliasing.Enabled = true;
                }
            }
            else
            {
                DrpDwnLstAntiAliasing.SelectedIndex = 0;
                DrpDwnLstAntiAliasing.Enabled = false;                
                NolvusMessageBox.ShowMessage("Antialiasing", "This option is not compatible with DLAA. Antialiasing has been forced to TAA.", MessageBoxType.Info);
                DrpDwnLstScreenRes.Enabled = true;
                ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownScaling = "TRUE";
                DrpDwnLstScreenRes.SelectedIndex = DownResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
            }
                                                     
        }

        private void TglBtnPhysics_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AdvancedPhysics").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("AdvancedPhysics").Value = "FALSE";
            }
        }

        private void TglBtnRayTracing_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("RayTracing").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("RayTracing").Value = "FALSE";
            }
        }

        private void TglBtnFPSStabilizer_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("FPSStabilizer").Value = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("FPSStabilizer").Value = "FALSE";
            }
        }

        private void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.SettingsManager.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();            
        }       

        private async void DrpDwnLstVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant = (DrpDwnLstVariant.SelectedItem as INolvusVariantDTO).Name;

            await LoadLODs();

            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant == "Ultra")
            {                
                LblLods.ForeColor = Color.White;
                DrpDwnLstLODs.Enabled = true;
            }
            else
            {                
                LblLods.ForeColor = Color.Gray;
                DrpDwnLstLODs.Enabled = false;
                DrpDwnLstLODs.SelectedIndex = 0;
            }

            this.DisplayHardwareRequirement();
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
            }

            DisplayHardwareRequirement();
        }

        private void DrpDwnLstLODs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("LODs").Value = (DrpDwnLstLODs.SelectedItem as INolvusOptionChoiceDTO).Value;

            DisplayHardwareRequirement();
        }

        private void DrpDwnLstRatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOption("Ratio").Value = (DrpDwnLstRatios.SelectedItem as INolvusOptionChoiceDTO).Value;
        }

        private void DrpDwnLstResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            string Resolution = DrpDwnLstResolution.SelectedValue.ToString();

            string[] Reso = Resolution.Split(new char[] { 'x' });

            WorkingInstance.SettingsManager.Width = Reso[0];
            WorkingInstance.SettingsManager.Height = Reso[1];
        }

        private void DisplayHardwareRequirement()
        {
            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant == "Ultra")
            {
                if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("LODs") == "Ultra")
                {
                    this.LblGPU.Text = "Recommended : RTX 3080 Ti";
                    LblVRAM.Text = "Recommended : 12Gb @1440p (HIGHER GPU with HIGHER VRAM needed beyond 1440p)";
                    LblSTORAGE.Text = "Mods: 277 Gb, Archives: 115Gb, Total: 392Gb";
                }
                else if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("LODs") == "Performance")
                {
                    this.LblGPU.Text = "Recommended : RTX 3080";
                    LblVRAM.Text = "Recommended : 10Gb @1440p (HIGHER GPU with HIGHER VRAM needed beyond 1440p)";
                    LblSTORAGE.Text = "Mods: 254 Gb, Archives: 115Gb, Total: 369Gb";
                }
                else if (ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("LODs") == "Ultra Performance")
                {
                    this.LblGPU.Text = "Recommended : RTX 2080 Ti";
                    LblVRAM.Text = "Recommended : 10Gb @1440p (HIGHER GPU with HIGHER VRAM needed beyond 1440p)";
                    LblSTORAGE.Text = "Mods: 251 Gb, Archives: 115Gb, Total: 366Gb";
                }
            }
            else
            {
                this.LblGPU.Text = "Minimum : GTX 1080, Recommended : RTX 2070";
                LblVRAM.Text = "Recommended : 8Gb @1080p (HIGHER GPU with HIGHER VRAM needed beyond 1080p)";
                LblSTORAGE.Text = "Mods: 240 Gb, Archives: 105Gb, Total: 345Gb";
            }
        }

        private void LnkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/appendix/installer/tech");
        }
    }
}
