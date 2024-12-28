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
using CefSharp;
using CefSharp.WinForms;

namespace Vcc.Nolvus.Browser.Forms
{
    public partial class BrowserWindow : SfForm, IBrowserInstance
    {                
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;            
       
        private BrowserTitleControl TitleBarControl;
        private ChromiumWebBrowser ChromiumDownloader;

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

        #region Events

        event OnBrowserClosedHandler OnBrowserClosedEvent;
        public event OnBrowserClosedHandler OnBrowserClosed
        {
            add
            {
                if (OnBrowserClosedEvent != null)
                {
                    lock (OnBrowserClosedEvent)
                    {
                        OnBrowserClosedEvent += value;
                    }
                }
                else
                {
                    OnBrowserClosedEvent = value;
                }
            }
            remove
            {
                if (OnBrowserClosedEvent != null)
                {
                    lock (OnBrowserClosedEvent)
                    {
                        OnBrowserClosedEvent -= value;
                    }
                }
            }
        }

        #endregion

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

        public void CloseBrowser()
        {
            if (InvokeRequired)
            {
                Invoke((Action)CloseBrowser);
                return;
            }

            if (ChromiumDownloader is ChromiumDownloader)
            {
                (ChromiumDownloader as ChromiumDownloader).OnFileDownloadRequest -= Downloader_OnFileDownloadRequest;                
            }
            
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

        public void SetInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetInfo, Value);
                return;
            }


            StStripLblInfo.Text = Value;
        }

        #endregion

        public ChromiumWebBrowser LoadBrowser(string Url, DownloadProgressChangedHandler Progress = null)
        {
            if (InvokeRequired)
            {
                return Invoke((Func<string, DownloadProgressChangedHandler, ChromiumWebBrowser>)LoadBrowser, Url, Progress) as ChromiumWebBrowser;                
            }

            ChromiumDownloader = new ChromiumDownloader(this, Url, false, Progress);

            (ChromiumDownloader as ChromiumDownloader).OnFileDownloadRequest += Downloader_OnFileDownloadRequest;

            BrowserPanel.Controls.Add(ChromiumDownloader);

            return ChromiumDownloader;
        }

        public ChromiumWebBrowser LoadBrowser(string Url, bool LinkOnly)
        {
            if (InvokeRequired)
            {
                return Invoke((Func<string, bool, ChromiumWebBrowser>)LoadBrowser, Url, LinkOnly) as ChromiumWebBrowser;
            }

            ChromiumDownloader = new ChromiumDownloader(this, Url, LinkOnly, null);

            (ChromiumDownloader as ChromiumDownloader).OnFileDownloadRequest += Downloader_OnFileDownloadLinkRequest;

            BrowserPanel.Controls.Add(ChromiumDownloader);

            return ChromiumDownloader;
        }

        public ChromiumWebBrowser LoadBrowser(string Url)
        {
            if (InvokeRequired)
            {
                return Invoke((Func<string, ChromiumWebBrowser>)LoadBrowser, Url) as ChromiumWebBrowser;
            }

            ChromiumDownloader = new ChromiumWebBrowser(Url);            

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
            CloseBrowser();
        }

        private void BrowserWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnBrowserClosedHandler Handler = this.OnBrowserClosedEvent;
            EventArgs Event = new EventArgs();
            if (Handler != null) Handler(this, Event);
        }

        private void BrowserWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        public async Task AwaitUserDownload(string Link, string FileName, DownloadProgressChangedHandler Progress)
        {            
            await (LoadBrowser(Link, Progress) as ChromiumDownloader).AwaitDownLoad(FileName);
            CloseBrowser();            
        }

        public async Task<string> GetNexusManualDownloadLink(string ModName, string Link, string NexusModId)
        {
            SetTitle("Manual download [" + ModName + "]");
            return await (LoadBrowser(Link, true) as ChromiumDownloader).AwaitDownloadLink(NexusModId);            
        } 

        public void Navigate(string Link, string Title)
        {
            CanClose = true;
            SetTitle(Title);                        
            HideLoading();
            LoadBrowser(Link);
        }

        public async Task NexusSSOAuthentication(string Id, string Slug)
        {
            CanClose = true;
            SetTitle("Nexus SSO Authentication");
            await (LoadBrowser(string.Format("https://www.nexusmods.com/sso?id={0}&application={1}", Id, Slug), false) as ChromiumDownloader).AwaitNexusSSOAuthentication();                        
        }
    }
}
