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
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Api.Installer;

namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    public partial class InstanceDetailFrame : DashboardFrame
    {
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

        }

        protected override async Task OnLoadedAsync()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                LblHeader.Text = Instance.Name + " v" + Instance.Version;
                ServiceSingleton.Dashboard.Info("Instance mods for " + Instance.Name + " v" + Instance.Version);

                LoadGrids(await LoadMods());
            }
            catch(Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during instance detail loading", ex.Message);
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

        private void LoadGrids(List<GridModObject> Mods)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<List<GridModObject>>)LoadGrids, Mods);
                return;
            }

            ModsGrid.SourceType = typeof(GridModObject);
            ModsGrid.DataSource = Mods;

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

        public async Task<List<GridModObject>> LoadMods()
        {
            return await Task.Run(() =>
            {
                try
                {
                    try
                    {
                        ShowLoading();
                        ServiceSingleton.Dashboard.Status("Loading mods...");
                        INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                        var InstallList = ServiceSingleton.Packages.GetInstallList();

                        var ModListFile = Path.Combine(Instance.InstallDir, "MODS", "profiles", Instance.Name, "modlist.txt");

                        List<string> Mods = System.IO.File.ReadAllLines(ModListFile).ToList();

                        Mods.Reverse();

                        Mods.RemoveAt(Mods.Count - 1);

                        List<GridModObject> ModList = new List<GridModObject>();
                        var Category = string.Empty;
                        var Counter = 0;

                        foreach (var Mod in Mods)
                        {
                            #region Mods

                            var ModLine = Mod.Substring(1, Mod.Length - 1);

                            if (ModLine.Contains("_separator"))
                            {
                                Category = ModLine.Replace("_separator", string.Empty);
                            }
                            else
                            {
                                GridModObject GridModObject = new GridModObject();

                                GridModObject.Priority = ModList.Count + 1;
                                GridModObject.Name = ModLine;
                                GridModObject.Category = Category;

                                try
                                {
                                    IMOElement MOElement = InstallList.Where(x => x.Name == ModLine).FirstOrDefault();

                                    GridModObject.Status = GridModObjectStatus.OK;
                                    GridModObject.StatusText = "OK";

                                    if (MOElement != null)
                                    {
                                        var MetaIniFile = Path.Combine(Instance.InstallDir, "MODS", "mods", ModLine, "meta.ini");

                                        if (File.Exists(MetaIniFile))
                                        {
                                            GridModObject.Version = ServiceSingleton.Settings.GetIniValue(Path.Combine(Instance.InstallDir, "MODS", "mods", ModLine, "meta.ini"), "General", "version");

                                            if (GridModObject.Version != MOElement.Version)
                                            {
                                                GridModObject.Status = GridModObjectStatus.VersionMisMatch;
                                                GridModObject.StatusText = string.Format("Version mismatch expected v{0}", MOElement.Version);
                                            }
                                        }
                                        else
                                        {
                                            GridModObject.Version = "NA";
                                            GridModObject.Status = GridModObjectStatus.NotInstalled;
                                            GridModObject.StatusText = "Not installed";
                                        }
                                    }
                                    else
                                    {
                                        if (ServiceSingleton.Packages.GameBaseModsList.Where(x => x.Contains(ModLine)).ToList().Count == 0)
                                        {
                                            GridModObject.Status = GridModObjectStatus.CustomInstalled;
                                            GridModObject.StatusText = "Not from the guide";
                                        }
                                        else
                                        {
                                            GridModObject.Version = "NA";
                                        }
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                    GridModObject.Status = GridModObjectStatus.Error;
                                    GridModObject.StatusText = ex.Message;
                                }

                                    ModList.Add(GridModObject);

                             }                            

                                ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Math.Round(((double)++Counter / Mods.Count * 100))));

                                #endregion
                        }

                        return ModList;
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
            ServiceSingleton.Instances.WorkingInstance = null;
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
               
                if ((e.DataRow.RowData as GridModObject).Status == GridModObjectStatus.OK)
                {
                    e.Style.TextColor = Color.LimeGreen;
                }
                else if ((e.DataRow.RowData as GridModObject).Status == GridModObjectStatus.VersionMisMatch)
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
            Process ModOrganizer = Process.Start(Path.Combine(ServiceSingleton.Instances.WorkingInstance.InstallDir, "MO2", "ModOrganizer.exe"));

            this.BtnPlay.Text = "Running...";
            this.BtnPlay.Enabled = false;

            Task.Run(() =>
            {
                ModOrganizer.WaitForExit();

                if (ModOrganizer.ExitCode == 0)
                {
                    this.SetPlayText("Play");
                }
            });
        }       

        private async void BtnSettings_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<InstanceSettingsFrame>();      
        }

        private async void BtnLoadOrder_Click(object sender, EventArgs e)
        {
            await ServiceSingleton.Dashboard.LoadFrameAsync<LoadOrderFrame>();
        }
    }
}
