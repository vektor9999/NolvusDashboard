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
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.NexusApi;

namespace Vcc.Nolvus.Dashboard.Frames.Installer.v6
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
        }

        protected override async void OnLoad()
        {
            try
            {
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                LblName.Text = Instance.Name;
                LblResolution.Text = Instance.Settings.Width + "x" + Instance.Settings.Height;
                LblRatio.Text = Instance.Settings.Ratio;
                LblInstallDir.Text = Instance.InstallDir;
                LblArchiveDir.Text = Instance.ArchiveDir;

                LblEnableArchiving.Text = Instance.Settings.EnableArchiving ? "Yes" : "No";

                LblDownscaling.Text = Instance.Performance.DownScaling == "TRUE" ? "Yes (" + Instance.Performance.DownScaledResolution + ")" : "No";
                LblVariant.Text = Instance.Performance.Variant;
                LblAA.Text = Instance.Performance.AntiAliasing;

                switch (Instance.Performance.IniSettings)
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

                LblSREX.Text = Instance.Performance.SREX == "TRUE" ? "Yes" : "No";
                LblLODs.Text = Instance.Performance.LODs;
                
                

                LblNudity.Text = Instance.Options.Nudity == "TRUE" ? "Yes" : "No";
                LblCombatAnims.Text = Instance.Options.CombatAnimation;
                LblLeveling.Text = Instance.Options.AlternateLeveling == "TRUE" ? "Yes" : "No";
                LblAltStart.Text = Instance.Options.AlternateStart == "TRUE" ? "Yes" : "No";
                LblGore.Text = Instance.Options.Gore == "TRUE" ? "Yes" : "No";
                LblStancesPerksTree.Text = Instance.Options.StancesPerksTree == "TRUE" ? "Yes" : "No";
                LblENB.Text = ENBs.GetENBByCode(Instance.Options.AlternateENB);

                LblCombatScaling.Text = Instance.Options.CombatScaling;
                LblExhaustion.Text = Instance.Options.Exhaustion == "TRUE" ? "Yes" : "No";
                LblNerfPA.Text = Instance.Options.NerfPA == "TRUE" ? "Yes" : "No";
                LblEnemiesResistance.Text = Instance.Options.EnemiesResistance == "TRUE" ? "Yes" : "No";
                LblBoss.Text = Instance.Options.Boss == "TRUE" ? "Yes" : "No";
                LblPoise.Text = Instance.Options.Poise == "TRUE" ? "Yes" : "No";

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
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages (max errors can be set up in the Nolvus Dashboard.ini file)", 50);
                }
                else if (ServiceSingleton.Settings.ErrorsThreshold == 0)
                {
                    RdBtnNoThreshold.Checked = true;
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages (max errors can be set up in the Nolvus Dashboard.ini file)", 50);
                }
                else
                {
                    RdBtnThreshold.Checked = true;
                    RdBtnThreshold.Text = string.Format("Stop the installation when {0} errors occured and display the error messages (max errors can be set up in the Nolvus Dashboard.ini file)", ServiceSingleton.Settings.ErrorsThreshold);
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
              Color.White, 3, ButtonBorderStyle.Solid, // left
              Color.White, 3, ButtonBorderStyle.Solid, // top
              Color.White, 3, ButtonBorderStyle.Solid, // right
              Color.White, 3, ButtonBorderStyle.Solid);// bottom
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            string Mo2Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ModOrganizer");            

            if (Directory.Exists(Mo2Path))
            {                
                ServiceSingleton.Dashboard.Error("Global ModOrganizer instance detected", "The installer can not proceed to the installation because a global ModOrganizer instance has been detected. Read the message below to fix", "READ THIS TO FIX!!!" + Environment.NewLine + "All automated mod lists use portable instances, this way you can have multiple lists installed together." + Environment.NewLine + "If you want to install Nolvus, you need to remove this installed ModOrganizer global instance to avoid issues(make a backup before if it's sensitive)." + Environment.NewLine + "To know where your global instance is installed go to " + Mo2Path + "." + Environment.NewLine + "This folder may be hidden (be sure you disable hidden files and folder in Windows folder options if you don't see it)" + Environment.NewLine + "If you made a backup of your global instance and want to continue, just delete the " + Mo2Path + " folder" + Environment.NewLine + "DON'T REACTIVATE THIS GLOBAL INSTANCE AFTER INSTALLATION!!! YOUR NOLVUS MOD ORGANIZER WILL NOT WORK!!!");
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
    }
}
