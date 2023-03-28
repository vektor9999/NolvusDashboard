using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Browser.Core;

namespace Vcc.Nolvus.Browser.Forms
{
    public partial class BrowserWindow : SfForm, IBrowserInstance
    {                
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;            
       
        private BrowserTitleControl TitleBarControl;
        private ChromiumDownloader ChromiumDownloader;

        private bool CanClose = false;

        public BrowserWindow()
        {
            InitializeComponent();

            StStripLblInfo.Text = string.Empty;                             

            SkinManager.SetVisualStyle(this, "Office2016Black");
            Style.TitleBar.MaximizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MaximizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonPressedBackColor = Color.DarkOrange;

            Style.TitleBar.BackColor = Color.FromArgb(54, 54, 54);
            Style.TitleBar.IconBackColor = Color.FromArgb(54, 54, 54);
            Style.TitleBar.Height = 50;
            Style.BackColor = Color.FromArgb(54, 54, 54);


            TitleBarControl = new BrowserTitleControl();
            TitleBarControl.Width = 3000;
            TitleBarControl.MouseDown += TitleBarControl_MouseDown;
            TitleBarTextControl = TitleBarControl;

            TitleBarControl.Title = "Browser";
            
            Show();
            ShowLoading();
        }

        #region Base Methods

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((Action)ShowLoading);
                return;
            }

            LoadingBox.Show();
        }

        public void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((Action)HideLoading);
                return;
            }

            LoadingBox.Hide();
        }

        private void ShowBrowserWindow()
        {
            if (InvokeRequired)
            {
                Invoke((Action)ShowBrowserWindow);
                return;
            }

            Show();
        }

        private void HideBrowserWindow()
        {
            if (InvokeRequired)
            {
                Invoke((Action)HideBrowserWindow);
                return;
            }

            Hide();
        }

        private void CloseBrowserWindow()
        {
            if (InvokeRequired)
            {
                Invoke((Action)CloseBrowserWindow);
                return;
            }

            ChromiumDownloader.OnFileDownloadRequest -= Downloader_OnFileDownloadRequest;           

            ChromiumDownloader = null;
            BrowserPanel.Controls.Remove(ChromiumDownloader);

            Close();
        }

        private void TitleBarControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void SetTitle(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetTitle, Value);
                return;
            }


            TitleBarControl.Title = Value;
        }

        #endregion
                             
        public ChromiumDownloader LoadBrowser(string Url, DownloadProgressChangedHandler Progress = null)
        {
            if (InvokeRequired)
            {
                return Invoke((Func<string, DownloadProgressChangedHandler, ChromiumDownloader>)LoadBrowser, Url, Progress) as ChromiumDownloader;                
            }

            ChromiumDownloader = new ChromiumDownloader(this, Url, false, Progress);

            ChromiumDownloader.OnFileDownloadRequest += Downloader_OnFileDownloadRequest;

            BrowserPanel.Controls.Add(ChromiumDownloader);

            return ChromiumDownloader;
        }

        public ChromiumDownloader LoadBrowser(string Url, bool LinkOnly)
        {
            if (InvokeRequired)
            {
                return Invoke((Func<string, bool, ChromiumDownloader>)LoadBrowser, Url, LinkOnly) as ChromiumDownloader;
            }

            ChromiumDownloader = new ChromiumDownloader(this, Url, LinkOnly, null);

            ChromiumDownloader.OnFileDownloadRequest += Downloader_OnFileDownloadLinkRequest;

            BrowserPanel.Controls.Add(ChromiumDownloader);

            return ChromiumDownloader;
        }

        private void Downloader_OnFileDownloadRequest(object sender, FileDownloadRequestEvent EventArgs)
        {
            CanClose = true;
            HideBrowserWindow();
        }

        private void Downloader_OnFileDownloadLinkRequest(object sender, FileDownloadRequestEvent EventArgs)
        {
            CanClose = true;
            CloseBrowserWindow();
        }

        private void BrowserWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void BrowserWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        public async Task AwaitUserDownload(string Link, string FileName, DownloadProgressChangedHandler Progress)
        {            
            await LoadBrowser(Link, Progress).AwaitDownLoad(FileName);
            CloseBrowserWindow();            
        }

        public async Task<string> GetNexusManualDownloadLink(string Link, string NexusModId)
        {            
            return await LoadBrowser(Link, true).AwaitDownloadLink(NexusModId);            
        } 
    }
}
