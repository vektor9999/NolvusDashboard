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
    public partial class PerformanceFrame : DashboardFrame
    {
        
        private List<string> Variants = new List<string>();
        private List<string> AntiAliasing = new List<string>();
        private List<string> LODs = new List<string>();
        private List<string> IniSettings = new List<string>();

        public PerformanceFrame()
        {
            InitializeComponent();                      
        }
        public PerformanceFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();

            Variants.Add("Ultra");
            Variants.Add("Redux");

            AntiAliasing.Add("TAA");
            AntiAliasing.Add("DLAA");

            LODs.Add("Ultra");
            LODs.Add("Performance");
            LODs.Add("Ultra Performance");

            IniSettings.Add("Low");
            IniSettings.Add("Medium");
            IniSettings.Add("High");

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
       
        private int ResolutionIndex(List<string> Resolutions)
        {
            INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

            var Index = Resolutions.FindIndex(x => x == WorkingInstance.Performance.DownWidth + "x" + WorkingInstance.Performance.DownHeight);

            return Index == -1 ? 0 : Index;           
        }

        private int AntiAliasingIndex(List<string> AntiAliasing)
        {            
            var Index = AntiAliasing.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing);
            return Index == -1 ? 0 : Index;            
        }

        private int VariantIndex(List<string> Variants)
        {            
            var Index = Variants.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Performance.Variant);
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

        protected override void OnLoad()
        {           
            var Instance = ServiceSingleton.Instances.WorkingInstance;            

            DrpDwnLstScreenRes.DataSource = ServiceSingleton.Globals.WindowsResolutions;
            DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
            DrpDwnLstScreenRes.Enabled = false;

            this.TglBtnPhysics.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Performance.AdvancedPhysics == "TRUE")
            {
                this.TglBtnPhysics.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnRayTracing.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Performance.RayTracing == "TRUE")
            {
                this.TglBtnRayTracing.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Performance.DownScaling == "TRUE")
            {
                this.TglBtnDownScale.ToggleState = ToggleButtonState.Active;
            }

            this.TglBtnFPSStabilizer.ToggleState = ToggleButtonState.Inactive;

            if (Instance.Performance.FPSStabilizer == "TRUE")
            {
                this.TglBtnFPSStabilizer.ToggleState = ToggleButtonState.Active;
            }

            DrpDwnLstAntiAliasing.DataSource = AntiAliasing;

            if (!ServiceSingleton.Settings.ForceAA)
            {
                LblGPUs.Text = string.Join(Environment.NewLine, ServiceSingleton.Globals.GetVideoAdapters().ToArray());

                if (!IsNvidiaRTX())
                {
                    ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing = "TAA";
                    DrpDwnLstAntiAliasing.Enabled = false;
                }
            }
            else
            {
                LblGPUs.Text = "NOT LISTED (CHECK BYPASSED)";
            }
            
            DrpDwnLstAntiAliasing.SelectedIndex = AntiAliasingIndex(AntiAliasing);

            DrpDwnLstIni.DataSource = IniSettings;

            DrpDwnLstIni.SelectedIndex = System.Convert.ToInt16(Instance.Performance.IniSettings);

            DrpDwnLstVariant.DataSource = Variants;

            DrpDwnLstVariant.SelectedIndex = VariantIndex(Variants);

            DrpDwnLstLODs.DataSource = LODs;

            DrpDwnLstLODs.SelectedIndex = LODsIndex(LODs);

            ServiceSingleton.Dashboard.Info("Performance Settings");            
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
                if (NolvusMessageBox.ShowConfirmation("Confirmation", "Some of the options you selected (like the variant, LODs quality, Advanced physics, Global Illumination or FPS stabilizer) can not be changed after installation. Are you sure you want to continue?") == DialogResult.Yes)
                {                    
                    if ((Performance.Variant == "Redux") && (Performance.AdvancedPhysics == "TRUE" || Performance.RayTracing == "TRUE" || Performance.AntiAliasing == "DLAA"))
                    {
                        if (NolvusMessageBox.ShowConfirmation("Confirmation", "You selected the Redux variant with other effects that are normally disabled by default with this variant. Be sure you have more than the minimum requirement. Are you sure you want to continue?") == DialogResult.Yes)
                        {
                            ServiceSingleton.Dashboard.LoadFrame<OptionsFrame>();
                        }
                    }
                    else
                    {                        
                        ServiceSingleton.Dashboard.LoadFrame<OptionsFrame>();
                    }                                      
                }                
            }
        }       

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                DrpDwnLstScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                ServiceSingleton.Instances.WorkingInstance.Performance.DownScaling = "TRUE";
                DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(ServiceSingleton.Globals.WindowsResolutions);
            }
            else
            {
                DrpDwnLstScreenRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                ServiceSingleton.Instances.WorkingInstance.Performance.DownScaling = "FALSE";                
            }                                         
        }

        private void TglBtnPhysics_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.AdvancedPhysics = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.AdvancedPhysics = "FALSE";
            }
        }

        private void TglBtnRayTracing_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.RayTracing = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.RayTracing = "FALSE";
            }
        }

        private void TglBtnFPSStabilizer_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (e.ToggleState == ToggleButtonState.Active)
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.FPSStabilizer = "TRUE";
            }
            else
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.FPSStabilizer = "FALSE";
            }
        }

        private void DrpDwnLstScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstScreenRes.SelectedValue != null)
            {
                string Resolution = DrpDwnLstScreenRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                ServiceSingleton.Instances.WorkingInstance.Performance.DownWidth = Reso[0];
                ServiceSingleton.Instances.WorkingInstance.Performance.DownHeight = Reso[1];
            }
        }

        private void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();            
        }       

        private void DrpDwnLstVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.Variant = DrpDwnLstVariant.SelectedValue.ToString();

            if (DrpDwnLstVariant.SelectedValue.ToString() == "Ultra")
            {                
                LblLods.ForeColor = Color.White;
                DrpDwnLstLODs.Enabled = true;
            }
            else
            {                
                LblLods.ForeColor = Color.Gray;
                DrpDwnLstLODs.Enabled = false;
            }

            this.DisplayHardwareRequirement();
        }

        private void DrpDwnLstAntiAliasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrpDwnLstAntiAliasing.SelectedValue != null)
            {
                ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing = DrpDwnLstAntiAliasing.SelectedValue.ToString();

                if (ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing == "DLAA")
                {
                    GrpBxDownScaling.Enabled = false;
                    TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;
                }
                else
                {
                    GrpBxDownScaling.Enabled = true;                    
                }
            }

            DisplayHardwareRequirement();
        }

        private void DrpDwnLstLODs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Performance.LODs = DrpDwnLstLODs.SelectedValue.ToString();

            DisplayHardwareRequirement();
        }

        private void DisplayHardwareRequirement()
        {
            if (ServiceSingleton.Instances.WorkingInstance.Performance.Variant == "Ultra")
            {
                if (ServiceSingleton.Instances.WorkingInstance.Performance.LODs == "Ultra")
                {
                    this.LblGPU.Text = "Recommended : RTX 3080 Ti";
                    LblVRAM.Text = "Recommended : 12Gb @1440p (HIGHER GPU with HIGHER VRAM needed beyond 1440p)";
                    LblSTORAGE.Text = "Mods: 277 Gb, Archives: 115Gb, Total: 392Gb";
                }
                else if (ServiceSingleton.Instances.WorkingInstance.Performance.LODs == "Performance")
                {
                    this.LblGPU.Text = "Recommended : RTX 3080";
                    LblVRAM.Text = "Recommended : 10Gb @1440p (HIGHER GPU with HIGHER VRAM needed beyond 1440p)";
                    LblSTORAGE.Text = "Mods: 254 Gb, Archives: 115Gb, Total: 369Gb";
                }
                else if (ServiceSingleton.Instances.WorkingInstance.Performance.LODs == "Ultra Performance")
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

       
    }
}
