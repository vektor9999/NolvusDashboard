using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Utils;

namespace Vcc.Nolvus.Updater
{    
    public partial class Main : SfForm
    {
        public static class KnownFolder
        {
            public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        }

        private bool _Error = false;
        private bool _FreshInstall = false;

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
            string Version = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "NolvusDashBoard.exe").ProductVersion;

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

            Process[] AgentProcesses = Process.GetProcessesByName("NolvusAgent");

            if (AgentProcesses.Length > 0)
            {
                foreach (var Process in AgentProcesses)
                {
                    Process.Kill();
                }
            }
        }

        private void StopAgent()
        {            
            Process[] AgentProcesses = Process.GetProcessesByName("NolvusAgent");

            if (AgentProcesses.Length > 0)
            {
                foreach (var Process in AgentProcesses)
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
                Dashboard.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\NolvusDashboard.exe";
                Dashboard.Start();
            }

            Process[] DashBoardAgentProcesses = Process.GetProcessesByName("NolvusAgent");

            if (DashBoardAgentProcesses.Length == 0)
            {
                Process DashboardAgent = new Process();

                DashboardAgent.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                DashboardAgent.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\NolvusAgent.exe";
                DashboardAgent.Start();
            }
        }

        private void StartAgent()
        {           
            Process[] DashBoardAgentProcesses = Process.GetProcessesByName("NolvusAgent");

            if (DashBoardAgentProcesses.Length == 0)
            {
                Process DashboardAgent = new Process();

                DashboardAgent.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                DashboardAgent.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\NolvusAgent.exe";
                DashboardAgent.Start();
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
            return File.Exists(AppDomain.CurrentDomain.BaseDirectory + "NolvusDashBoard.exe");
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

        private void Extracting(object sender, SevenZip.ProgressEventArgs e)
        {
            this.SetInfo("Installating Application (" + e.PercentDone + "%)...");
            this.SetProgress(e.PercentDone);
        }

        public Main()
        {
            InitializeComponent();                        

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
            else if (BaseFolder.Length > 30)
            {
                SetError("Your Installation path (" + BaseFolder + ") is too long, try reduce it (less than 30 characters). Your installation path will be automatically the path you are starting this program from.");
                ShowButton(true);
            }
            else
            {
                Task.Run(() =>
                {
                    SetProgress(0);
                    InitApi();
                    SetProgress(50);

                    this.SetInfo("Check for updates...");

                    CheckForInstallerVersion().ContinueWith(Dashboard =>
                    {
                        SetProgress(100);

                        if (!Dashboard.IsFaulted)
                        {
                            _FreshInstall = !IsDashBoardInstalled();

                            if (!IsDashBoardInstalled() || this.IsNewerVersion(Dashboard.Result, this.GetDashboardVersion()))                            
                            {
                                this.SetProgress(0);

                                Process[] DashBoardProcesses = Process.GetProcessesByName("NolvusDashBoard");

                                //Dashboard running...
                                if (DashBoardProcesses.Length != 0)
                                {
                                    if (CloseApp)
                                    {
                                        StopDashBoard();
                                        StopAgent();
                                        DownloadAndInstall(Dashboard.Result);
                                    }
                                    else
                                    {
                                        this.SetError("Your Nolvus Dashboard is already running! Close it first.");
                                    }
                                }
                                else
                                {
                                    StopAgent();
                                    DownloadAndInstall(Dashboard.Result);
                                }
                            }
                            else
                            {
                                SetProgress(0);
                                this.SetInfo("Your Nolvus Dashboard is up to date.");
                                ShowButton(true);
                            }
                        }
                        else
                        {
                            this.SetError("Error (" + Dashboard.Exception.InnerException.Message + ")");
                        }
                    });
                });
            }                                                          
        }

        private void DownloadAndInstall(string Version)
        {
            this.SetInfo("Initializing download...");

            ApiManager.Service.Installer.GetLatestInstallerLink().ContinueWith(Link => 
            {
                string FileName = Path.GetTempPath() + "Binaries" + Version.Replace(".", string.Empty) + ".7z";

                DownloadHelper.DownloadFile(Link.Result, FileName, Downloading, 0).ContinueWith(Download =>
                {
                    if (Download.IsFaulted)
                    {
                        this.SetProgress(0);
                        SetError("Error During download (" + Download.Exception.InnerException.Message + ")");
                    }
                    else
                    {
                        this.SetProgress(0);

                        ExtractorHelper.ExtractFile(FileName, AppDomain.CurrentDomain.BaseDirectory, Extracting).ContinueWith(Extract =>
                        {
                            this.SetProgress(0);

                            if (Extract.IsFaulted)
                            {
                                this.SetProgress(0);
                                SetError("Error During installation (" + Extract.Exception.InnerException.Message + ")");
                            }
                            else
                            {
                                this.SetProgress(0);
                                File.Delete(FileName);

                                this.SetInfo("Your Nolvus Dashboard has been installed.");
                                ShowButton(true);
                            }
                        });
                    }
                });
            });            
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (!_Error)
            {
                if (CloseApp || _FreshInstall)
                {
                    StartDashBoard();
                }
                
                StartAgent();                                
            }

            this.Close();
        }
    }
}
