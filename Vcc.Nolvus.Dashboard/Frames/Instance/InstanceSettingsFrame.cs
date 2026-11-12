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
using Vcc.Nolvus.Dashboard.Frames.Manager.Options.v6;
using Vcc.Nolvus.Dashboard.Frames.Remap.v6;

namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    public partial class InstanceSettingsFrame : DashboardFrame
    {
        private bool Initializing = true;        

        private int ResolutionIndex(List<string> Resolutions, out bool Error)
        {
            int Index = Resolutions.Count - 1;

            Error = false;

            bool Found = false;

            string Resolution = ServiceSingleton.Instances.WorkingInstance.SettingsManager.Width + "x" + ServiceSingleton.Instances.WorkingInstance.SettingsManager.Height;

            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.Height != string.Empty && ServiceSingleton.Instances.WorkingInstance.SettingsManager.Width != string.Empty)
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

            if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownWidth != string.Empty && ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownHeight != string.Empty)
            {

                string Resolution = ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownWidth + "x" + ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownHeight;

                if (ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownHeight != string.Empty && ServiceSingleton.Instances.WorkingInstance.SettingsManager.DownWidth != string.Empty)
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
            var Index = Locations.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.SettingsManager.CDN);

            return Index == -1 ? 0 : Index;            
        }

        private int AntiAliasingIndex(List<string> AntiAliasing)
        {
            var Index = AntiAliasing.FindIndex(x => x == ServiceSingleton.Instances.WorkingInstance.OptionsManager.GetOptionValue("AntiAliasing"));
            return Index == -1 ? 0 : Index;            
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

            OptionsGrid.ThemeName = "Office2016Black";                        

            OptionsGrid.Style.CurrentCellStyle.BackColor = Color.Orange;
            OptionsGrid.Style.CurrentCellStyle.TextColor = Color.White;
            OptionsGrid.Style.CurrentCellStyle.BorderColor = Color.Orange;            

            OptionsGrid.GroupColumnDescriptions.Add(new GroupColumnDescription() { ColumnName = "Category", SortGroupRecords = false });

            OptionsGrid.AllowSorting = false;
        }
      
        protected override async Task OnLoadAsync()
        {
            try
            {
                bool ResError = false;

                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                LblHeader.Text = string.Format("Settings for {0}", Instance.FullName);

                ServiceSingleton.Dashboard.Info(string.Format("Instance settings for {0}", Instance.FullName));

                List<string> Resolutions = ServiceSingleton.Globals.WindowsResolutions;
                DrpDwnLstScreenRes.DataSource = Resolutions;
                DrpDwnLstScreenRes.SelectedIndex = ResolutionIndex(Resolutions, out ResError);

                if (ResError)
                {
                    ApplyResolution();
                }

                LblLanguage.Text = string.Format("{0} - ({1})", Instance.SettingsManager.LgName, Instance.SettingsManager.LgCode);

                DrpDwnLstDownRes.DataSource = Resolutions;
                DrpDwnLstDownRes.SelectedIndex = DownScaledResolutionIndex(Resolutions, out ResError);

                if (ResError)
                {
                    ApplyDowncalingResolution();
                }

                LblInstallDir.Text = Instance.SettingsManager.InstallDir;
                LblArchiveDir.Text = Instance.SettingsManager.ArchiveDir;

                this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Inactive;

                if (Instance.SettingsManager.EnableArchiving)
                {
                    this.TglBtnEnableArchive.ToggleState = ToggleButtonState.Active;
                }                

                this.TglBtnDownScale.ToggleState = ToggleButtonState.Inactive;

                if (Instance.SettingsManager.DownScaling == "TRUE")
                {
                    this.TglBtnDownScale.ToggleState = ToggleButtonState.Active;
                    DrpDwnLstDownRes.Enabled = this.TglBtnDownScale.ToggleState == ToggleButtonState.Active;
                }

                DrpDwnLstDownLoc.DataSource = CDN.Get();
                DrpDwnLstDownLoc.SelectedIndex = DownloadLocationIndex(CDN.Get());

                BtnApplyRes.Enabled = false;
                BtnApplyDownScaling.Enabled = false;

                

                List <string> IniSettings = new List<string>();

                IniSettings.Add("Low");
                IniSettings.Add("Medium");
                IniSettings.Add("High");

                DrpDwnLstIni.DataSource = IniSettings;

                DrpDwnLstIni.SelectedIndex = System.Convert.ToInt16(Instance.SettingsManager.IniSettings);

                OptionsGrid.DataSource = Instance.OptionsManager.GetOptions();

                OptionsGrid.ExpandAllGroup();

                TglBtnDownScale.Enabled = Instance.OptionsManager.GetOptionValue("AntiAliasing") != "DLAA" && Instance.OptionsManager.GetOptionValue("AntiAliasing") != "FSR";

                if (!TglBtnDownScale.Enabled)
                {
                    LblDownscalingInfo.Text = " (Disabled when using DLAA/FSR/Frame Generation)";
                }
                else
                {
                    LblDownscalingInfo.Text = string.Empty;
                }

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
              Color.White, 1, ButtonBorderStyle.Solid, // left
              Color.White, 1, ButtonBorderStyle.Solid, // top
              Color.White, 1, ButtonBorderStyle.Solid, // right
              Color.White, 1, ButtonBorderStyle.Solid);// bottom
        }

        private void TglBtnEnableArchive_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.SettingsManager.EnableArchiving = e.ToggleState == ToggleButtonState.Active;

            ServiceSingleton.Instances.Save();
        }      

        private void TglBtnDownScale_ToggleStateChanged(object sender, ToggleStateChangedEventArgs e)
        {
            if (!Initializing)
            {
                DrpDwnLstDownRes.Enabled = e.ToggleState == ToggleButtonState.Active;
                BtnApplyDownScaling.Enabled = true;
            }            
        }

        private void DrpDwnLstScreenRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                BtnApplyRes.Enabled = true;
            }
        }

        private void DrpDwnLstDownRes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DrpDwnLstDownLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance.SettingsManager.CDN = DrpDwnLstDownLoc.SelectedItem.ToString();

            ServiceSingleton.Instances.Save();
        }

        private async void DrpDwnLstIni_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {
                try
                {
                    INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                    Instance.SettingsManager.IniSettings = DrpDwnLstIni.SelectedIndex.ToString();

                    File.WriteAllText(Path.Combine(Instance.SettingsManager.InstallDir, "MODS", "profiles", Instance.Name, "Skyrim.ini"), ModOrganizer.GetIni(false, (IniLevel)System.Convert.ToInt16(Instance.SettingsManager.IniSettings), Instance));
                    File.WriteAllText(Path.Combine(Instance.SettingsManager.InstallDir, "MODS", "profiles", Instance.Name, "SkyrimPrefs.ini"), ModOrganizer.GetIni(true, (IniLevel)System.Convert.ToInt16(Instance.SettingsManager.IniSettings), Instance));

                    ServiceSingleton.Instances.Save();
                }
                catch(Exception ex)
                {                    
                    await ServiceSingleton.Dashboard.Error("Error during skyrim ini file configuration saving", ex.Message);
                }
            }
        }        

        private async void BtnBack_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<InstanceDetailFrame>();
        }

        private void ApplyResolution()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                string Resolution = DrpDwnLstScreenRes.SelectedValue.ToString();

                string[] Reso = Resolution.Split(new char[] { 'x' });

                Instance.SettingsManager.Width = Reso[0];
                Instance.SettingsManager.Height = Reso[1];

                ServiceSingleton.Instances.Save();

                string FileName = Path.Combine(Instance.SettingsManager.InstallDir, "MODS", "profiles", Instance.Name, "SkyrimPrefs.ini");

                ServiceSingleton.Settings.StoreIniValue(FileName, "Display", "iSize W", Instance.SettingsManager.Width);
                ServiceSingleton.Settings.StoreIniValue(FileName, "Display", "iSize H", Instance.SettingsManager.Height);

                BtnApplyRes.Enabled = false;
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
                    Instance.SettingsManager.DownScaling = "TRUE";
                }
                else
                {
                    Instance.SettingsManager.DownScaling = "FALSE";
                }

                Instance.SettingsManager.DownWidth = Reso[0];
                Instance.SettingsManager.DownHeight = Reso[1];

                ServiceSingleton.Instances.Save();

                #region Apply Settings

                string SettingsFile = Path.Combine(Instance.SettingsManager.InstallDir, "MODS", "Mods", "SSE Display Tweaks", "SKSE", "Plugins", "SSEDisplayTweaks.ini");

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
                            if (Instance.SettingsManager.DownScaling == "TRUE")
                            {
                                _Line = "Resolution" + "=" + Instance.SettingsManager.DownScaledResolution;
                            }
                            else
                            {
                                _Line = "#Resolution" + "=" + Instance.SettingsManager.DownScaledResolution;
                            }

                            FoundResolution = true;
                        }
                        else if (Line.Contains("BorderlessUpscale") && Line.Substring(0, 1) != "# " && !FoundUpscale)
                        {
                            if (Instance.SettingsManager.DownScaling == "TRUE")
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

                BtnApplyDownScaling.Enabled = false;
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

        private int GetTotalRecordsCount(Group group)
        {
            int count = 0;

            if (group.Groups != null)
            {
                foreach (var g in group.Groups)
                {
                    if (g.Groups != null)
                        foreach (var g1 in g.Groups)
                            count += GetTotalRecordsCount(g1);

                    if (g.Records != null)
                        count += g.Records.Count;
                }
            }
            else
                if (group.Records != null)
                count += group.Records.Count;

            return count;
        }

        private void OptionsGrid_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
        {
            if (e.DataRow.RowType == RowType.CaptionCoveredRow && !string.IsNullOrEmpty(e.DisplayText))
            {
                var displayText = string.Empty;
                var group = (e.DataRow.RowData as Syncfusion.Data.Group);

                if (group != null)
                {
                    if (group.Groups != null)
                    {
                        int Options = 0;
                        Options = GetTotalRecordsCount(group);
                        displayText = group.Key.ToString() + " ( " + Options + " Options (" + group.Groups.Count + " Sub Categories)";
                    }
                    else if (group.Records != null)
                    {
                        displayText = group.Key.ToString() + " ( " + group.Records.Count + " Options )";                        
                    }

                    e.DisplayText = displayText;
                }
            }
        }

        private async void LnkLblOptionsManager_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var Instance = ServiceSingleton.Instances.WorkingInstance;

            if (Instance.Name == Strings.NolvusAwakening)
            {
                await ServiceSingleton.Dashboard.LoadFrameAsync<OptionsManagerFrame>();
            }
            else
            {
                NolvusMessageBox.ShowMessage("Options Manager", string.Format("This feature is not available for {0}.", Instance.Name), MessageBoxType.Info);
            }
            
        }

        private void LnkLblRemap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<RemapInstanceFrame>();
        }
    }
}
