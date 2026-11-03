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
using Syncfusion.WinForms.Controls;
using Syncfusion.Data;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView.Enums;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.NexusApi;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class SummaryFrame : DashboardFrame
    {
        public SummaryFrame()
        {
            InitializeComponent();
        }

        public SummaryFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            OptionsGrid.ThemeName = "Office2016Black";            

            OptionsGrid.Style.CurrentCellStyle.BackColor = Color.Orange;
            OptionsGrid.Style.CurrentCellStyle.TextColor = Color.White;
            OptionsGrid.Style.CurrentCellStyle.BorderColor = Color.Orange;            

            OptionsGrid.GroupColumnDescriptions.Add(new GroupColumnDescription() { ColumnName = "Category", SortGroupRecords = false });
        }

        protected override async void OnLoad()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                LblName.Text = Instance.Name;
                LblResolution.Text = Instance.SettingsManager.Width + "x" + Instance.SettingsManager.Height;                
                LblInstallDir.Text = Instance.SettingsManager.InstallDir;
                LblArchiveDir.Text = Instance.SettingsManager.ArchiveDir;

                LblEnableArchiving.Text = Instance.SettingsManager.EnableArchiving ? "Yes" : "No";

                LblDownscaling.Text = Instance.SettingsManager.DownScaling == "TRUE" ? "Yes (" + Instance.SettingsManager.DownScaledResolution + ")" : "No";
                
                LblVariant.Text = Instance.SettingsManager.Variant;                
                LblCDN.Text = Instance.SettingsManager.CDN;
                LblLanguage.Text = Instance.SettingsManager.LgName;

                switch (Instance.SettingsManager.IniSettings)
                {
                    case "0": LblIni.Text = "Low";
                        break;
                    case "1":
                        LblIni.Text = "Medium";
                        break;
                    case "2":
                        LblIni.Text = "High";
                        break;
                }

                OptionsGrid.DataSource = Instance.OptionsManager.GetOptions();

                OptionsGrid.ExpandAllGroup();                          

                if (!ApiManager.AccountInfo.IsPremium)
                {
                    PnlMessage.BackColor = Color.Orange;
                    PicBox.Image = Properties.Resources.Warning_Message;
                    LblMessage.Text = "You are not a Nexus Premium user. Download will not be automatic (you will have to click the download button for each mods) and bandwidth will be limited to 2 MB/s.";
                }
                else
                {
                    PnlMessage.BackColor = Color.FromArgb(92, 184, 92);
                    PicBox.Image = Properties.Resources.Info;
                    LblMessage.Text = "You are a Nexus Premium user. Download will be automatic and bandwidth will be unlimited.";
                }

                PnlMessage.Paint += PnlMessage_Paint;

                if (ServiceSingleton.Settings.ErrorsThreshold == 1 || !NexusApi.ApiManager.AccountInfo.IsPremium)
                {
                    RdBtnOneError.Checked = true;
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages.", 50);
                }
                else if (ServiceSingleton.Settings.ErrorsThreshold == 0)
                {
                    RdBtnNoThreshold.Checked = true;
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages.", 50);
                }
                else
                {
                    RdBtnThreshold.Checked = true;
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages.", ServiceSingleton.Settings.ErrorsThreshold);
                }

                ServiceSingleton.Dashboard.Info("Review your selections");
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during summary checking", ex.Message, ex.StackTrace);
            }
        }         

        private void PnlMessage_Paint(object sender, PaintEventArgs e)
        {            
            ControlPaint.DrawBorder(e.Graphics, PnlMessage.ClientRectangle,
              Color.White, 1, ButtonBorderStyle.Solid, // left
              Color.White, 1, ButtonBorderStyle.Solid, // top
              Color.White, 1, ButtonBorderStyle.Solid, // right
              Color.White, 1, ButtonBorderStyle.Solid);// bottom
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {            
            string Mo2Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ModOrganizer");            

            if (Directory.Exists(Mo2Path))
            {
                NolvusMessageBox.ShowMessage("Global ModOrganizer instance detected", "The installer can not proceed to the installation because a global ModOrganizer instance has been detected. Read the message below to fix\n\nREAD THIS TO FIX!!!\n\nAll automated mod lists use portable instances, this way you can have multiple lists installed together.\n\nIf you want to install Nolvus, you need to remove this installed ModOrganizer global instance to avoid issues(make a backup before if it's sensitive).\n\nTo know where your global instance is installed go to \n\n" + Mo2Path + ".\n\nThis folder may be hidden (be sure you disable hidden files and folder in Windows folder options if you don't see it)\n\nIf you made a backup of your global instance and want to continue, just delete the folder\n\n" + Mo2Path + "\n\nDON'T REACTIVATE THIS GLOBAL INSTANCE AFTER INSTALLATION!!! YOUR NOLVUS MOD ORGANIZER WILL NOT WORK!!!", MessageBoxType.Error, 600, 620, Color.White);
            }
            else
            {         
                if (RdBtnOneError.Checked)
                {
                    ServiceSingleton.Settings.StoreIniValue("Process", "ErrorsThreshold", "1");
                }   
                else if (RdBtnNoThreshold.Checked)
                {
                    ServiceSingleton.Settings.StoreIniValue("Process", "ErrorsThreshold", "0");
                }
                else
                {
                    if (ServiceSingleton.Settings.ErrorsThreshold == 1 || ServiceSingleton.Settings.ErrorsThreshold == 0)
                    {
                        ServiceSingleton.Settings.StoreIniValue("Process", "ErrorsThreshold", "50");
                    }                    
                }               
                                                    
                ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>();
            }
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (ApiManager.AccountInfo.IsPremium)
            {
                ServiceSingleton.Dashboard.LoadFrame<CDNFrame>();
            }
            else
            {
                ServiceSingleton.Dashboard.LoadFrame<PageFileFrame>();
            }
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
    }
}
