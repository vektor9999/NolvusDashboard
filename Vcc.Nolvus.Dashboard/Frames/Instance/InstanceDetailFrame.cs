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
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Api.Installer;

namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    public partial class InstanceDetailFrame : DashboardFrame
    {
        private ModObjectList ModListStatus = new ModObjectList();
        private string _CurrentProfile;

        public InstanceDetailFrame()
        {
            InitializeComponent();
        }

        public InstanceDetailFrame(IDashboard Dashboard, FrameParameters Params) 
            :base(Dashboard, Params)            
        {
            InitializeComponent();

            PnlHeader.BackColor = Color.FromArgb(92, 184, 92);
            PnlHeader.Paint += PnlHeader_Paint;            

            ModsGrid.ThemeName = "Office2016Black";
            ModsGrid.ShowRowHeader = true;
            ModsGrid.Style.RowHeaderStyle.SelectionMarkerThickness = 100;
            ModsGrid.Style.RowHeaderStyle.SelectionMarkerColor = Color.Orange;
            ModsGrid.Style.RowHeaderStyle.SelectionBackColor = Color.FromArgb(68, 68, 68);

            ModsGrid.Style.CurrentCellStyle.BackColor = Color.Orange;
            ModsGrid.Style.CurrentCellStyle.TextColor = Color.White;
            ModsGrid.Style.CurrentCellStyle.BorderColor = Color.Orange;

            ModsGrid.GroupColumnDescriptions.Add(new GroupColumnDescription() { ColumnName = "Category", SortGroupRecords = false });

            ModsGrid.AllowSorting = false;

            PicBox.Location = new Point((int)Math.Round(PicBox.Location.X * ServiceSingleton.Dashboard.ScalingFactor), (int)Math.Round(PicBox.Location.Y * ServiceSingleton.Dashboard.ScalingFactor));
            PicBox.Size = new Size((int)Math.Round(PicBox.Size.Width * ServiceSingleton.Dashboard.ScalingFactor), (int)Math.Round(PicBox.Size.Height * ServiceSingleton.Dashboard.ScalingFactor));

        }

        private string SelectedProfile
        {
            get
            {
                return DrpDwnLstProfiles.SelectedValue.ToString();
            }
        }

        protected override void OnLoaded()
        {            
            INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

            LblHeader.Text = Instance.Name + " v" + Instance.Version;
            ServiceSingleton.Dashboard.Info("Instance mods for " + Instance.Name + " v" + Instance.Version);

            DrpDwnLstProfiles.Visible = false;
            DrpDwnLstProfiles.DataSource = ServiceSingleton.Packages.ModOrganizer2.GetProfiles();
            DrpDwnLstProfiles.SelectedIndex = 0;                
        }

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, PnlHeader.ClientRectangle,
              Color.White, 3, ButtonBorderStyle.Solid, // left
              Color.White, 3, ButtonBorderStyle.Solid, // top
              Color.White, 3, ButtonBorderStyle.Solid, // right
              Color.White, 3, ButtonBorderStyle.Solid);// bottom
        }

        private void LoadGrids(ModObjectList Mods)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<ModObjectList>)LoadGrids, Mods);
                return;
            }

            ModsGrid.SourceType = typeof(ModObject);
            ModsGrid.DataSource = Mods.List;

            ModsGrid.ExpandAllGroup();                              
        }

        private void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            PicBoxLoading.Show();
        }

        private void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)HideLoading);
                return;
            }

            PicBoxLoading.Hide();
        }

        private void SetPlayText(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetPlayText, Text);
                return;
            }

            this.BtnPlay.Text = Text;
            this.BtnPlay.Enabled = true;
        }       

        public async Task<ModObjectList> LoadModStatus()
        {
            return await Task.Run(async() =>
            {
                try
                {
                    try
                    {                        
                        ShowLoading();

                        ServiceSingleton.Dashboard.Status("Loading mods...");

                        ModListStatus = await ServiceSingleton.CheckerService.CheckModList(                            
                            await ServiceSingleton.SoftwareProvider.ModOrganizer2.GetModsMetaData(SelectedProfile, (s, p) =>
                            {
                                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                ServiceSingleton.Dashboard.Progress(p);
                            }),
                            await ServiceSingleton.Packages.GetModsMetaData((s, p) =>
                            {
                                ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                ServiceSingleton.Dashboard.Progress(p);
                            }),
                            (s) => 
                            {
                                ServiceSingleton.Dashboard.Status(s);
                            }
                        );

                        ModListStatus.Profile = SelectedProfile;

                        return ModListStatus;
                    }
                    catch (Exception ex)
                    {
                        throw ex;                        
                    }
                }
                finally
                {
                    HideLoading();
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();
                }
            });            
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

        private void ModsGrid_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
        {
            if (e.DataRow.RowType == RowType.CaptionCoveredRow && !string.IsNullOrEmpty(e.DisplayText))
            {
                var displayText = string.Empty;
                var group = (e.DataRow.RowData as Syncfusion.Data.Group);

                if (group != null)
                {
                    if (group.Groups != null)
                    {
                        int Mods = 0;
                        Mods = GetTotalRecordsCount(group);
                        displayText = group.Key.ToString() + " ( " + Mods + " Mods (" + group.Groups.Count + " Sub Categories)";
                    }
                    else if (group.Records != null)
                    {
                        displayText = group.Key.ToString() + " ( " + group.Records.Count + " Mods )";                        
                    }

                    e.DisplayText = displayText;
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.UnloadWorkingIntance();
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private void BtnExpand_Click(object sender, EventArgs e)
        {
            ModsGrid.ExpandAllGroup();
        }

        private void BtnCollapse_Click(object sender, EventArgs e)
        {
            ModsGrid.CollapseAllGroup();
        }

        private void ModsGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "StatusText")
            {               
                if ((e.DataRow.RowData as ModObject).Status == ModObjectStatus.OK)
                {
                    e.Style.TextColor = Color.LimeGreen;
                }
                else if ((e.DataRow.RowData as ModObject).Status == ModObjectStatus.VersionMisMatch || (e.DataRow.RowData as ModObject).Status == ModObjectStatus.InstalledIniMissing)
                {
                    e.Style.TextColor = Color.Orange;
                }
                else
                {
                    e.Style.TextColor = Color.Red;
                }             
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (!ModOrganizer.IsRunning)
            {
                Process MO2 = ModOrganizer.Start(ServiceSingleton.Instances.WorkingInstance.InstallDir);

                this.BtnPlay.Text = "Running...";
                this.BtnPlay.Enabled = false;

                Task.Run(() =>
                {
                    MO2.WaitForExit();

                    if (MO2.ExitCode == 0)
                    {
                        this.SetPlayText("Play");
                    }
                });
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is already running!", MessageBoxType.Error);
            }
        }       

        private async void BtnSettings_Click(object sender, EventArgs e)
        {
            switch (ServiceSingleton.Instances.WorkingInstance.Name)
            {
                case Strings.NolvusAscension:
                    await ServiceSingleton.Dashboard.LoadFrameAsync<v5.InstanceSettingsFrame>();
                    break;
                case Strings.NolvusAwakening:
                    await ServiceSingleton.Dashboard.LoadFrameAsync<v6.InstanceSettingsFrame>();
                    break;
            }            
        }

        private async void BtnLoadOrder_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<LoadOrderFrame>();
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            popupMenu1.Show(BtnReport, new Point(0, BtnReport.Height));                        
        }

        private async void BrItmClipboardReport_Click(object sender, EventArgs e)
        {
            ShowLoading();

            try
            {
                try
                {
                    Clipboard.SetText(await ServiceSingleton.Report.GenerateReportToClipBoard(ModListStatus, (s,p)=> {
                        ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                        ServiceSingleton.Dashboard.Progress(p);
                    }));

                    HideLoading();
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();

                    NolvusMessageBox.ShowMessage("Information", "Configuration has been copied to the clipboard", MessageBoxType.Info);                                
                }
                catch (Exception ex)
                {
                    HideLoading();
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();
                    NolvusMessageBox.ShowMessage("Error during report generation", ex.Message, MessageBoxType.Error);
                }
            }
            finally
            {                                
            }
        }

        private async void BrItmPDFReport_Click(object sender, EventArgs e)
        {
            ShowLoading();

            try
            {
                try
                {                                   
                    await ServiceSingleton.Report.GenerateReportToPdf(ModListStatus, Properties.Resources.background_nolvus, (s, p) =>
                    {
                        ServiceSingleton.Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                        ServiceSingleton.Dashboard.Progress(p);
                    });

                    HideLoading();
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();

                    NolvusMessageBox.ShowMessage("Information", string.Format("PDF report has been generated in {0}", ServiceSingleton.Folders.ReportDirectory), MessageBoxType.Info);

                    Process.Start(ServiceSingleton.Folders.ReportDirectory);
                }
                catch (Exception ex)
                {
                    HideLoading();
                    ServiceSingleton.Dashboard.NoStatus();
                    ServiceSingleton.Dashboard.ProgressCompleted();
                    NolvusMessageBox.ShowMessage("Error during report generation", ex.Message, MessageBoxType.Error);
                }
            }
            finally
            {                                
            }
        }

        private async void DrpDwnLstProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentProfile != SelectedProfile)
                {
                    _CurrentProfile = SelectedProfile;

                    LoadGrids(await LoadModStatus());

                    if (ModListStatus.HasMods)
                    {
                        if (ModListStatus.AddedModsCount > 0 || ModListStatus.RemovedModsCount > 0 || ModListStatus.IniParsingErrorCount > 0)
                        {
                            PnlHeader.BackColor = Color.FromArgb(255, 0, 0);
                            LblHeader.Text += " - Errors Detected";
                        }
                        else if (ModListStatus.VersionMismatchCount > 0 || ModListStatus.InstalledIniMissingCount > 0)
                        {
                            PnlHeader.BackColor = Color.FromArgb(255, 128, 0);
                            LblHeader.Text += " - Warning Detected";
                        }
                    }

                    DrpDwnLstProfiles.Visible = true;
                }
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during instance detail loading", ex.Message);
            }   
        }

        private void ModsGrid_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            var Mod = e.DataRow.RowData as ModObject;

            var Info = string.Format("Mod : {0}", Mod.Name, Mod.Version);

            Info += Environment.NewLine;
            Info += string.Format("Version : {0}", Mod.Version);
            Info += Environment.NewLine;
            Info += string.Format("Status : {0}", Mod.StatusText);

            NolvusMessageBox.ShowMessage("Mod information", Info, MessageBoxType.Info);
        }
    }
}
