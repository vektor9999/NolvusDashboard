using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.StockGame.Core;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class StockGameFrame : DashboardFrame
    {
        string AEMSG1 = "You need to buy Skyrim Anniversary Edition or if you already have Skyrim Special Edition, buy the Anniversary Upgrade";
        string AEMSG2 = "If you already have the Anniversary Edition, be sure you ran the game once from steam and when prompted download all content then close Skyrim";
        string AEMSG3 = "If it still does not work, do an integrity check in Steam";
        string AEMSG4 = "More info here ==> https://www.nolvus.net/appendix/installer/skyrim_setup";        


        public StockGameFrame()
        {
            InitializeComponent();
        }

        public StockGameFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)
        {
            InitializeComponent();
        }

        protected override async Task OnLoadedAsync()
        {
                            
            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Stock Game Installation]");
            ServiceSingleton.Dashboard.Info("Stock Game Installation");

            IFolderService Folders = ServiceSingleton.Folders;
            INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

            LstBxOutput.ItemHeight = (int)Math.Round(LstBxOutput.ItemHeight * ServiceSingleton.Dashboard.ScalingFactor);

            var StockGameManager = new StockGameManager(Folders.DownloadDirectory, Folders.LibDirectory, Folders.PatchDirectory, Folders.GameDirectory, Instance, await ApiManager.Service.Installer.GetGamePackage(Instance.Version), true);

            StockGameManager.OnDownload += StockGameManager_OnDownload;
            StockGameManager.OnExtract += StockGameManager_OnExtract;
            StockGameManager.OnItemProcessed += StockGameManager_OnItemProcessed;
            StockGameManager.OnStepProcessed += StockGameManager_OnStepProcessed;                           

            try
            {
                await StockGameManager.Load();
                ServiceSingleton.Dashboard.Info("Stock Game Installation 1/4 (25%)");
                await StockGameManager.CheckIntegrity();
                ServiceSingleton.Dashboard.Info("Stock Game Installation 2/4 (50%)");
                await StockGameManager.CopyGameFiles();
                ServiceSingleton.Dashboard.Info("Stock Game Installation 3/4 (75%)");
                await StockGameManager.PatchGameFiles();
                ServiceSingleton.Dashboard.Info("Stock Game Installation 4/4 (100%)");

                ServiceSingleton.Dashboard.ProgressCompleted();
                ServiceSingleton.Instances.PrepareInstanceForInstall();

                await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();
            }
            catch (Exception ex)
            {
                RollBack();
                    
                if (ex is GameFileMissingException)
                {
                    await ServiceSingleton.Dashboard.Error("Error during game file checking", "Skyrim Anniversary Edition is not installed", AEMSG1 + Environment.NewLine + AEMSG2 + Environment.NewLine + AEMSG3 + Environment.NewLine + AEMSG4 + Environment.NewLine + "Original error : " + ex.Message);
                }
                else if (ex is GameFileIntegrityException)
                {
                    await ServiceSingleton.Dashboard.Error("Error during game integrity checking", ex.Message, "Possible fix is to do an integrity check for Skyrim in Steam");
                }
                else if (ex is GameFilePatchingException)
                {
                    await ServiceSingleton.Dashboard.Error("Error during game files patching", ex.Message);
                }
                else
                {
                    await ServiceSingleton.Dashboard.Error("Error during stock game installation", ex.Message);
                }
            }
        }   

        public void AddItemToList(string Item)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)AddItemToList, Item);
                return;
            }

            LstBxOutput.Items.Add(Item);

            int VisibleItems = LstBxOutput.ClientSize.Height / LstBxOutput.ItemHeight;
            LstBxOutput.TopIndex = Math.Max(LstBxOutput.Items.Count - VisibleItems + 1, 0);            
            ServiceSingleton.Logger.Log(Item);
        }

        private void StockGameManager_OnDownload(object sender, DownloadProgress e)
        {
            ServiceSingleton.Dashboard.Status("Downloading file (" + e.ProgressPercentage + "%)...");
            ServiceSingleton.Dashboard.Progress(e.ProgressPercentage);
        }

        private void StockGameManager_OnExtract(object sender, ExtractProgress e)
        {
            ServiceSingleton.Dashboard.Status("Extracting game meta (" + e.ProgressPercentage + "%)...");
            ServiceSingleton.Dashboard.Progress(e.ProgressPercentage);
        }

        private void StockGameManager_OnItemProcessed(object sender, ItemProcessedEventArgs e)
        {
            double Percent = ((double)e.Value / (double)e.Total) * 100;

            Percent = Math.Round(Percent, 0);            

            switch (e.Step)
            {
                case StockGameProcessStep.GameFileInfoLoading:
                    ServiceSingleton.Dashboard.Status(string.Format("Loading game files info for {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Loading game files info {0}", Percent));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;
                case StockGameProcessStep.PatchingInfoLoading:
                    ServiceSingleton.Dashboard.Status(string.Format("Loading patching info for {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Loading patching info ({0}%)", Percent));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;
                case StockGameProcessStep.GameFilesChecking:
                    ServiceSingleton.Dashboard.Status(string.Format("Checking game file {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Game files checking ({0}%)", Percent));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;
                case StockGameProcessStep.GameFilesCopy:
                    ServiceSingleton.Dashboard.Status(string.Format("Copying game file {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Copying game files ({0}%)", Percent));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;
                case StockGameProcessStep.GameFilesPatching:
                    ServiceSingleton.Dashboard.Status("Awaiting game file to patch...");
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Patching game files ({0}%)", Percent));
                    break;
                case StockGameProcessStep.PatchGameFile:
                    ServiceSingleton.Dashboard.Status(string.Format("Patching game files {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;
                case StockGameProcessStep.CheckPatchedGameFile:
                    ServiceSingleton.Dashboard.Status(string.Format("Checking patched game files {0}...", e.ItemName));
                    ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));
                    break;

            }                           
        }

        private void StockGameManager_OnStepProcessed(object sender, StepProcessedEventArgs e)
        {
            AddItemToList(e.Step);            
        }

        private void LstBxOutput_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font Font = e.Font;

            if ( ServiceSingleton.Dashboard.ScalingFactor > 1)
            {
                Font = new Font(e.Font.FontFamily, (float)(e.Font.Size * ServiceSingleton.Dashboard.ScalingFactor), GraphicsUnit.Pixel);
            }            
            
            if (e.Index < 0) return;            
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          Color.FromArgb(54,54,54),
                                          Color.Orange);
            
            e.DrawBackground();            
            e.Graphics.DrawString(LstBxOutput.Items[e.Index].ToString(), Font, Brushes.White, e.Bounds, StringFormat.GenericDefault);            
            e.DrawFocusRectangle();
        }

        private void RollBack()
        {
            this.AddItemToList("Error detected, rollbacking changes...");
            ServiceSingleton.Dashboard.Status("Error detected, rollbacking changes, please wait...");
            ServiceSingleton.Files.RemoveDirectory(ServiceSingleton.Instances.WorkingInstance.InstallDir, true);
        }        
    }
}
