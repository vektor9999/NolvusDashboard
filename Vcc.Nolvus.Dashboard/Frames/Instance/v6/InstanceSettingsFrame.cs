﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Data;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView.Enums;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Controls;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Installer;

namespace Vcc.Nolvus.Dashboard.Frames.Instance.v6
{
    public partial class InstanceSettingsFrame : DashboardFrame
    {
        private bool Initializing = true;

        private int RatioIndex(List<string> Ratios)
        {
            int Index = 0;

            if (ServiceSingleton.Instances.WorkingInstance.Settings.Ratio != string.Empty)
            {
                foreach (var Ratio in Ratios)
                {
                    if (Ratio == ServiceSingleton.Instances.WorkingInstance.Settings.Ratio)
                    {
                        break;
                    }

                    Index++;
                }
            }

            return Index;
        }

        private int ResolutionIndex(List<string> Resolutions, out bool Error)
        {
            int Index = Resolutions.Count - 1;

            Error = false;

            bool Found = false;

            string Resolution = ServiceSingleton.Instances.WorkingInstance.Settings.Width + "x" + ServiceSingleton.Instances.WorkingInstance.Settings.Height;

            if (ServiceSingleton.Instances.WorkingInstance.Settings.Height != string.Empty && ServiceSingleton.Instances.WorkingInstance.Settings.Width != string.Empty)
            {
                Index = 0;

                foreach (var Reso in Resolutions)
                {
                    if (Resolution == Reso)
                    {
                        Found = true;
                        break;
                    }

                    Index++;
                }
            }

            if (Found)
            {
                return Index;
            }
            else
            {
                Error = true;
                NolvusMessageBox.ShowMessage("Error", "The resolution you set up (" + Resolution + ") is no more compatible with your current Windows available resolutions. It can happen if you changed your monitor or modified the instancesdata.xml file manually. The dashboard will revert to the first available resolution. Be sure to change it.", MessageBoxType.Error);
                return 0;                
            }

        }

        private int DownScaledResolutionIndex(List<string> Resolutions, out bool Error)
        {
            int Index = Resolutions.Count - 1;

            Error = false;

            bool Found = false;

            if (ServiceSingleton.Instances.WorkingInstance.Performance.DownWidth != string.Empty && ServiceSingleton.Instances.WorkingInstance.Performance.DownHeight != string.Empty)
            {

                string Resolution = ServiceSingleton.Instances.WorkingInstance.Performance.DownWidth + "x" + ServiceSingleton.Instances.WorkingInstance.Performance.DownHeight;

                if (ServiceSingleton.Instances.WorkingInstance.Performance.DownHeight != string.Empty && ServiceSingleton.Instances.WorkingInstance.Performance.DownWidth != string.Empty)
                {
                    Index = 0;

                    foreach (var Reso in Resolutions)
                    {
                        if (Resolution == Reso)
                        {
                            Found = true;
                            break;
                        }

                        Index++;
                    }
                }

                if (Found)
                {
                    return Index;
                }
                else
                {
                    Error = true;
                    NolvusMessageBox.ShowMessage("Error", "The downscale resolution you set up (" + Resolution + ") is no more compatible with your current Windows available resolutions. It can happen if you changed your monitor or modified the instancesdata.xml file manually. The dashboard will revert to the first available resolution. Be sure to change it.", MessageBoxType.Error);
                    return 0;                    
                }
            }
            else
            {
                return 0;
            }
        }
       

        private int DownloadLocationIndex(List<string> Locations)
        {
            var Index = Locations.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Settings.CDN);

            return Index == -1 ? 0 : Index;            
        }

        private int AntiAliasingIndex(List<string> AntiAliasing)
        {
            var Index = AntiAliasing.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.Performance.AntiAliasing);
            return Index == -1 ? 0 : Index;            
        }

        private void EnableFlatButton(FlatButton Button, bool Enabled)
        {
            Button.Enabled = Enabled;

            if (Enabled)
            {
                Button.ForeColor = Color.Orange;
                Button.BorderColor = Color.White;
            }
            else
            {
                Button.ForeColor = Color.Gray;
                Button.BorderColor = Color.Gray;
            }

            this.ActiveControl = null;
        }

        public InstanceSettingsFrame()
        {
            InitializeComponent();
        }

        public InstanceSettingsFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {
            InitializeComponent();

            PnlHeader.BackColor = Color.FromArgb(92, 184, 92);
            PnlHeader.Paint += PnlHeader_Paint;

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

            PicBox.Location = new Point((int)Math.Round(PicBox.Location.X * ServiceSingleton.Dashboard.ScalingFactor), (int)Math.Round(PicBox.Location.Y * ServiceSingleton.Dashboard.ScalingFactor));
            PicBox.Size = new Size((int)Math.Round(PicBox.Size.Width * ServiceSingleton.Dashboard.ScalingFactor), (int)Math.Round(PicBox.Size.Height * ServiceSingleton.Dashboard.ScalingFactor));
        }
      
        protected override async Task OnLoadAsync()
        {
            try
            {
                bool ResError = false;

                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                LblHeader.Text = "Settings for " + Instance.Name + " v" + Instance.Version;
                List<string> Resolutions = ServiceSingleton.Globals.WindowsResolutions;
                DrpDwnLstScreenRes.DataSource = Resolutions;
                DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(Resolutions, out ResError);

                if (ResError)
                {
                    ApplyResolution();
                }

                List<string> Ratios = new List<string>();

                Ratios.Add("16:9");                
                Ratios.Add("21:9");

                DrpDwnLstRatios.DataSource = Ratios;

                DrpDwnLstRatios.SelectedIndex = RatioIndex(Ratios);

                DrpDwnLstDownRes.DataSource = Resolutions;
                DrpDwnLstDownRes.SelectedIndex = DownScaledResolutionIndex(Resolutions, out ResError);

                if (ResError)
                {
                    ApplyDowncalingResolution();
                }

                this.TxtBxInstancePath.Text = Instance.InstallDir;
                this.TxtBxArchivePath.Text = Instance.ArchiveDir;

                this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Inactive;

                if (Instance.Settings.EnableArchiving)
                {
                    this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Active;
                }

                this.TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;

                if (Instance.Performance.DownScaling == "TRUE")
                {
                    this.TglBtnDownScale.ToggleState = ToggleButtonState.Active;
                    DrpDwnLstDownRes.Enabled = this.TglBtnDownScale.ToggleState == ToggleButtonState.Active;
                }

                DrpDwnLstDownLoc.DataSource = CDN.Get();
                DrpDwnLstDownLoc.SelectedIndex = DownloadLocationIndex(CDN.Get());

                EnableFlatButton(BtnApplyRes, false);
                EnableFlatButton(BtnApplyDownScaling, false);

                LblVariant.Text = Instance.Performance.Variant;
                LblAntiAliasing.Text = Instance.Performance.AntiAliasing;

                if (Instance.Performance.Variant == "Redux")
                {
                    LblLODs.Text = "Redux";
                }
                else
                {
                    LblLODs.Text = Instance.Performance.LODs;
                }

                LblSREX.Text = Instance.Performance.SREX == "TRUE" ? "Yes" : "No";
                LblCombatAnims.Text = Instance.Options.CombatAnimation;


                LblStances.Text = Instance.Options.StancesPerksTree == "TRUE" ? "Yes" : "No";
                LblLeveling.Text = Instance.Options.AlternateLeveling == "TRUE" ? "Yes" : "No";
                LblNude.Text = Instance.Options.Nudity == "TRUE" ? "Yes" : "No";
                LblGore.Text = Instance.Options.Gore == "TRUE" ? "Yes" : "No";
                LblAltStart.Text = Instance.Options.AlternateStart == "TRUE" ? "Yes" : "No";

                LblENB.Text = ENBs.GetENBByCode(Instance.Options.AlternateENB);

                LblCombatScaling.Text = Instance.Options.CombatScaling;
                LblExhaustion.Text = Instance.Options.Exhaustion == "TRUE" ? "Yes" : "No";
                LblNerfPA.Text = Instance.Options.NerfPA == "TRUE" ? "Yes" : "No";
                LblEnemiesResistance.Text = Instance.Options.EnemiesResistance == "TRUE" ? "Yes" : "No";
                LblBoss.Text = Instance.Options.Boss == "TRUE" ? "Yes" : "No";
                LblPoise.Text = Instance.Options.Poise == "TRUE" ? "Yes" : "No";

               
                List<string> IniSettings = new List<string>();

                IniSettings.Add("Low");
                IniSettings.Add("Medium");
                IniSettings.Add("High");

                DrpDwnLstIni.DataSource = IniSettings;

                DrpDwnLstIni.SelectedIndex = System.Convert.ToInt16(Instance.Performance.IniSettings);

                GrpBxDownscaling.Enabled = Instance.Performance.AntiAliasing != "DLAA" && Instance.Performance.AntiAliasing != "FSR";

                Initializing = false;                
            }
            catch(Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during instance settings loading", ex.Message);
            }
        }
        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, PnlHeader.ClientRectangle,
              Color.White, 3, ButtonBorderStyle.Solid, // left
              Color.White, 3, ButtonBorderStyle.Solid, // top
              Color.White, 3, ButtonBorderStyle.Solid, // right
              Color.White, 3, ButtonBorderStyle.Solid);// bottom
        }

        private void TglBtnEnableArchive_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.EnableArchiving = e.ToggleState == ToggleButtonState.Active;

            ServiceSingleton.Instances.Save();
        }      

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (!Initializing)
            {
                DrpDwnLstDownRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                EnableFlatButton(BtnApplyDownScaling, true);
            }            
        }

        private void DrpDwnLstScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                EnableFlatButton(BtnApplyRes, true);
            }
        }

        private void DrpDwnLstDownRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                EnableFlatButton(BtnApplyDownScaling, true);
            }
        }

        private void DrpDwnLstRatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                EnableFlatButton(BtnApplyRes, true);

                ServiceSingleton.Instances.WorkingInstance.Settings.Ratio = DrpDwnLstRatios.SelectedValue.ToString();

                ServiceSingleton.Instances.Save();
            }            
        }

        private void DrpDwnLstDownLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.Settings.CDN = DrpDwnLstDownLoc.SelectedItem.ToString();

            ServiceSingleton.Instances.Save();
        }

        private async void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                try
                {
                    INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                    Instance.Performance.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();

                    File.WriteAllText(Path.Combine(Instance.InstallDir, "MODS", "profiles", Instance.Name, "Skyrim.ini"), ModOrganizer.GetIni(false, (IniLevel)System.Convert.ToInt16(Instance.Performance.IniSettings), Instance));
                    File.WriteAllText(Path.Combine(Instance.InstallDir, "MODS", "profiles", Instance.Name, "SkyrimPrefs.ini"), ModOrganizer.GetIni(true, (IniLevel)System.Convert.ToInt16(Instance.Performance.IniSettings), Instance));

                    ServiceSingleton.Instances.Save();
                }
                catch(Exception ex)
                {                    
                    await ServiceSingleton.Dashboard.Error("Error during skyrim ini file configuration saving", ex.Message);
                }
            }
        }        

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<InstanceDetailFrame>();
        }

        private void ApplyResolution()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                string Resolution = DrpDwnLstScreenRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                Instance.Settings.Width = Reso[0];
                Instance.Settings.Height = Reso[1];

                ServiceSingleton.Instances.Save();

                string FileName = Path.Combine(Instance.InstallDir, "MODS", "profiles", Instance.Name, "SkyrimPrefs.ini");

                ServiceSingleton.Settings.StoreIniValue(FileName, "Display", "iSize W", Instance.Settings.Width);
                ServiceSingleton.Settings.StoreIniValue(FileName, "Display", "iSize H", Instance.Settings.Height);

                EnableFlatButton(BtnApplyRes, false);
            }
            catch (Exception ex)
            {
                NolvusMessageBox.ShowMessage("Error", "Error applying resolution with message : " + ex.Message, MessageBoxType.Error);
            }            
        }

        private void BtnApplyRes_Click(object sender, EventArgs e)
        {
            ApplyResolution();
        }
       
        private void ApplyDowncalingResolution()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                string Resolution = DrpDwnLstDownRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                if (TglBtnDownScale.ToggleState == ToggleButtonState.Active)
                {
                    Instance.Performance.DownScaling = "TRUE";
                }
                else
                {
                    Instance.Performance.DownScaling = "FALSE";
                }

                Instance.Performance.DownWidth = Reso[0];
                Instance.Performance.DownHeight = Reso[1];

                ServiceSingleton.Instances.Save();

                #region Apply Settings

                string SettingsFile = Path.Combine(Instance.InstallDir, "MODS", "Mods", "SSE Display Tweaks", "SKSE", "Plugins", "SSEDisplayTweaks.ini");

                if (File.Exists(SettingsFile))
                {
                    string[] Lines = System.IO.File.ReadAllLines(SettingsFile);

                    List<string> NewLines = new List<string>();

                    bool FoundResolution = false;
                    bool FoundUpscale = false;

                    foreach (string Line in Lines)
                    {
                        string _Line = Line;

                        if (Line.Contains("Resolution") && Line.Substring(0, 1) != "# " && !FoundResolution)
                        {
                            if (Instance.Performance.DownScaling == "TRUE")
                            {
                                _Line = "Resolution" + "=" + Instance.Performance.DownScaledResolution;
                            }
                            else
                            {
                                _Line = "#Resolution" + "=" + Instance.Performance.DownScaledResolution;
                            }

                            FoundResolution = true;
                        }
                        else if (Line.Contains("BorderlessUpscale") && Line.Substring(0, 1) != "# " && !FoundUpscale)
                        {
                            if (Instance.Performance.DownScaling == "TRUE")
                            {
                                _Line = "BorderlessUpscale" + "=true";
                            }
                            else
                            {
                                _Line = "#BorderlessUpscale" + "=false";
                            }

                            FoundUpscale = true;
                        }

                        NewLines.Add(_Line);
                    }

                    System.IO.File.WriteAllLines(SettingsFile, NewLines.ToArray());
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Error", "The needed ini file (SSEDisplayTweaks.ini) is missing", MessageBoxType.Error);
                }

                #endregion

                EnableFlatButton(BtnApplyDownScaling, false);
            }
            catch (Exception ex)
            {
                NolvusMessageBox.ShowMessage("Error", "Error applying downscaling with message : " + ex.Message, MessageBoxType.Error);
            }
        }

        private void BtnApplyDownScaling_Click(object sender, EventArgs e)
        {
            ApplyDowncalingResolution();
        }

        
    }
}
