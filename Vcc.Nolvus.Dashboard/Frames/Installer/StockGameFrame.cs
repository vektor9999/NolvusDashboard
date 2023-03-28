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
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.StockGame.Core;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class StockGameFrame : DashboardFrame
    {        
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
            try
            {                
                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Stock Game Installation]");
                ServiceSingleton.Dashboard.Info("Stock Game Installation");

                IFolderService Folders = ServiceSingleton.Folders;
                INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                var StockGameManager = new StockGameManager(Folders.DownloadDirectory, Folders.LibDirectory, Folders.GameDirectory, Instance, await ApiManager.Service.Installer.GetGamePackage(Instance.Version));

                StockGameManager.OnDownload += StockGameManager_OnDownload;
                StockGameManager.OnExtract += StockGameManager_OnExtract;
                StockGameManager.OnItemProcessed += StockGameManager_OnGamePackageLoad;
                StockGameManager.OnStepProcessed += StockGameManager_OnStepProcessed;

                await StockGameManager.Load();                

                try
                {
                    await StockGameManager.CheckIntegrity();                    
                }
                catch (Exception ex)
                {
                    RollBack();
                    await ServiceSingleton.Dashboard.Error("Error during game integrity checking", ex.Message, "Be sure the specified Skyrim directory (" + Folders.GameDirectory + ") is the one from Steam wtih Anniversary Edition content, freshly installed, not downgraded, with the right language you selected and without any modification. You may have to reinstall the game properly from Steam and launch it once to download all AE content. If the path does not point to your Steam path, you can modify it in the NolbusDashboard.ini under GamePath.");
                }

                await StockGameManager.CopyGameFiles();                

                try
                {
                    await StockGameManager.PatchGameFiles();                    
                }
                catch (Exception ex)
                {
                    RollBack();
                    await ServiceSingleton.Dashboard.Error("Error during game files patching", ex.Message);                                      
                }

                ServiceSingleton.Dashboard.ProgressCompleted();
                ServiceSingleton.Instances.PrepareInstanceForInstall();
                await ServiceSingleton.Dashboard.LoadFrameAsync<InstallFrame>();
            }
            catch (Exception ex)
            {
                RollBack();                
                await ServiceSingleton.Dashboard.Error("Error during stock game creation", ex.Message);
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
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Log.txt", Environment.NewLine + Item);
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

        private void StockGameManager_OnGamePackageLoad(object sender, ItemProcessedEventArgs e)
        {
            double Percent = ((double)e.Value / (double)e.Total) * 100;

            Percent = Math.Round(Percent, 0);

            ServiceSingleton.Dashboard.Status(e.Step + " (" + Percent.ToString() + "%)...");            
            ServiceSingleton.Dashboard.Progress(System.Convert.ToInt16(Percent));            
        }

        private void StockGameManager_OnStepProcessed(object sender, ItemProcessedEventArgs e)
        {
            AddItemToList(e.Step);            
        }

        private void LstBxOutput_DrawItem(object sender, DrawItemEventArgs e)
        {
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
            e.Graphics.DrawString(LstBxOutput.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds, StringFormat.GenericDefault);            
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
