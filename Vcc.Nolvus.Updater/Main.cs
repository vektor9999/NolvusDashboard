using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Core;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Services.Files;
using Vcc.Nolvus.Services.Globals;

namespace Vcc.Nolvus.Updater
{    
    public partial class Main : SfForm
    {
        public static class KnownFolder
        {
            public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        }

        private bool _Error = false;        

        MessageBar MessageBar;

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

        private void InitApi()
        {
            try
            {
                ApiManager.Init("https://www.nolvus.net/rest/", "v1", string.Empty, string.Empty);
            }
            catch (Exception e)
            {
                SetError(e.Message);
            }
        }

        private string WindowsDownloadFolder
        {
            get
            {
                string Result;

                SHGetKnownFolderPath(KnownFolder.Downloads, 0, IntPtr.Zero, out Result);

                return Result;
            }
        }

        private string WindowsDesktopFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        private string BaseFolder
        {
            get
            {
                return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        private string GetDashboardVersion()
        {
            string Version = FileVersionInfo.GetVersionInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashBoard.exe")).ProductVersion;

            return Version.Substring(0, Version.LastIndexOf('.'));
        }

        private bool IsNewerVersion(string v1, string v2)
        {
            string[] v1List = v1.Split(new char[] { '.' });
            string[] v2List = v2.Split(new char[] { '.' });

            for (int i = 0; i < v1List.Length; i++)
            {
                int _v1 = System.Convert.ToInt16(v1List[i]);
                int _v2 = System.Convert.ToInt16(v2List[i]);

                if (_v1 > _v2)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CloseApp
        {
            get
            {
                string[] Args = Environment.GetCommandLineArgs();

                if (Args.Length > 1)
                {
                    return Args[1] == "1";
                }
                else
                {
                    return false;
                }
            }
        }

        async Task<string> CheckForInstallerVersion()
        {
            return await ApiManager.Service.Installer.GetLatestInstallerVersion();
        }

        private void StopDashBoard()
        {
            Process[] DashBoardProcesses = Process.GetProcessesByName("NolvusDashBoard");

            if (DashBoardProcesses.Length > 0)
            {
                foreach (var Process in DashBoardProcesses)
                {
                    Process.Kill();
                }
            }
        }

        private void StartDashBoard()
        {
            Process[] DashBoardProcesses = Process.GetProcessesByName("NolvusDashBoard");

            if (DashBoardProcesses.Length == 0)
            {
                Process Dashboard = new Process();

                Dashboard.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Dashboard.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashboard.exe");
                Dashboard.Start();
            }
        }

        public void SetInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetInfo, Value);
                return;
            }

            this.LblInfo.Text = Value;
        }

        public void SetError(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetError, Value);
                return;
            }

            this.LblInfo.Text = string.Empty;
            SetProgress(0);

            this.LblError.Visible = true;
            this.LblError.Text = Value;

            _Error = true;
        }

        private bool IsDashBoardInstalled()
        {
            return File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashBoard.exe"));
        }

        public void SetProgress(int Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<int>)SetProgress, Value);
                return;
            }


            this.ProgressBar.Value = Value;
        }

        public void ShowButton(bool Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<bool>)ShowButton, Value);
                return;
            }


            this.BtnClose.Visible = Value;
        }

        private void Downloading(object sender, DownloadProgress e)
        {
            this.SetInfo("Downloading latest Nolvus Dashboard (" + e.ProgressPercentage + "%)...");
            this.SetProgress(e.ProgressPercentage);
        }

        private void Extracting(object sender, ExtractProgress e)
        {
            this.SetInfo("Installating Application (" + e.ProgressPercentage + "%)...");
            this.SetProgress(e.ProgressPercentage);
        }

        public Main()
        {
            InitializeComponent();
            
            ServiceSingleton.RegisterService<IGlobalsService>(new GlobalsService());

            SkinManager.SetVisualStyle(this, "Office2016Black");
            this.Style.TitleBar.MaximizeButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.MinimizeButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.HelpButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.CloseButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.MaximizeButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.MinimizeButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.HelpButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.CloseButtonPressedBackColor = Color.DarkOrange;

            this.Style.TitleBar.BackColor = Color.FromArgb(54, 54, 54);
            this.Style.TitleBar.IconBackColor = Color.FromArgb(54, 54, 54);
            this.Style.TitleBar.Height = 50;
            this.Style.BackColor = Color.FromArgb(54, 54, 54);            

            MessageBar = new MessageBar();
            MessageBar.Width = 3000;
            MessageBar.Title = "Nolvus Dashboard Updater";

            this.TitleBarTextControl = MessageBar;

            this.LblInfo.Text = "Initializing...";

            if (BaseFolder == WindowsDownloadFolder)
            {
                SetError("Nolvus Dashboard can not be installed inside Windows download folder, please copy the updater.exe into an other folder (like D:\\Nolvus).");
                ShowButton(true);
            }
            else if (BaseFolder == WindowsDesktopFolder)
            {
                SetError("Nolvus Dashboard can not be installed on your desktop, please copy the updater.exe into an other folder (like D:\\Nolvus).");
                ShowButton(true);
            }
            else if (new DirectoryInfo(BaseFolder).Parent == null)
            {
                SetError("Nolvus Dashboard can not be installed on a root drive, please copy the updater.exe into an other folder (like D:\\Nolvus).");
                ShowButton(true);
            }
            else
            {
                Task.Run(async () =>
                {
                    SetProgress(0);
                    InitApi();
                    SetProgress(50);

                    SetInfo("Check for updates...");

                    try
                    {

                        var DashboardVersion = await CheckForInstallerVersion();                        

                        if (!IsDashBoardInstalled() || IsNewerVersion(DashboardVersion, GetDashboardVersion()))
                        {
                            SetProgress(0);

                            Process[] DashBoardProcesses = Process.GetProcessesByName("NolvusDashBoard");

                            if (DashBoardProcesses.Length != 0)
                            {
                                if (CloseApp)
                                {
                                    StopDashBoard();
                                    await DownloadAndInstall(DashboardVersion);
                                }
                                else
                                {
                                    SetError("Your Nolvus Dashboard is already running! Close it first.");
                                }
                            }
                            else
                            {
                                await DownloadAndInstall(DashboardVersion);
                            }
                        }
                        else
                        {
                            SetProgress(0);
                            SetInfo("Your Nolvus Dashboard is up to date.");
                            ShowButton(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        SetError("Error (" + ex.Message + ")");
                    }
                });
            }
        }

        private Task ExtractFiles(string FileName, string ExtractPath)
        {
            TaskCompletionSource<object> source = new TaskCompletionSource<object>();

            using (ZipArchive archive = ZipFile.OpenRead(FileName))
            {
                int num = 0;

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(ExtractPath, entry.FullName));
                    if (fullPath.StartsWith(ExtractPath, StringComparison.Ordinal))
                    {
                        if (entry.FullName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                        else
                        {
                            entry.ExtractToFile(fullPath, true);
                        }
                        num++;
                    }

                    int num2 = Convert.ToInt32((int)((100 * num) / archive.Entries.Count));
                    this.SetInfo("Installating Application (" + num2 + "%)...");
                    this.SetProgress(num2);
                }
                source.SetResult(new object());
            }
            return source.Task;
        }

        private async Task DownloadAndInstall(string Version)
        {
            var Tsk = Task.Run(async () =>
            {
                SetInfo("Initializing download...");

                try
                {
                    var FileName = Path.GetTempPath() + "Binaries" + Version.Replace(".", string.Empty) + ".7z";

                    if (File.Exists(FileName))
                    {
                        File.Delete(FileName);
                    }

                    try
                    {
                        var FileService = new FileService();

                        await FileService.DownloadFile(await ApiManager.Service.Installer.GetLatestInstallerLink(), FileName, Downloading);                        
                        await ExtractFiles(FileName, AppDomain.CurrentDomain.BaseDirectory);

                        SetInfo("Your Nolvus Dashboard has been installed.");

                        ShowButton(true);
                    }
                    finally
                    {
                        File.Delete(FileName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            });

            await Tsk;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (!_Error)
            {
                if (CloseApp || IsDashBoardInstalled())
                {
                    StartDashBoard();
                }
            }

            this.Close();
        }
    }
}
