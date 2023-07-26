using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Browser.Forms;

namespace Vcc.Nolvus.Browser.Core
{
    public class ChromeDownloaderHandler : IDownloadHandler
    {
        private bool _IsDownloadComplete = false;
        private bool LinkOnly;
        public bool IsDownloadComplete
        {
            get
            {
                return _IsDownloadComplete;
            }
        }

        private Stopwatch SW = new Stopwatch();        

        event OnFileDownloadRequestedHandler OnFileDownloadRequestEvent;
        public event OnFileDownloadRequestedHandler OnFileDownloadRequest
        {
            add
            {
                if (OnFileDownloadRequestEvent != null)
                {
                    lock (OnFileDownloadRequestEvent)
                    {
                        OnFileDownloadRequestEvent += value;
                    }
                }
                else
                {
                    OnFileDownloadRequestEvent = value;
                }
            }
            remove
            {
                if (OnFileDownloadRequestEvent != null)
                {
                    lock (OnFileDownloadRequestEvent)
                    {
                        OnFileDownloadRequestEvent -= value;
                    }
                }
            }
        }

        event OnFileDownloadRequestedHandler OnFileDownloadCompletedEvent;
        public event OnFileDownloadRequestedHandler OnFileDownloadCompleted
        {
            add
            {
                if (OnFileDownloadCompletedEvent != null)
                {
                    lock (OnFileDownloadCompletedEvent)
                    {
                        OnFileDownloadCompletedEvent += value;
                    }
                }
                else
                {
                    OnFileDownloadCompletedEvent = value;
                }
            }
            remove
            {
                if (OnFileDownloadCompletedEvent != null)
                {
                    lock (OnFileDownloadCompletedEvent)
                    {
                        OnFileDownloadCompletedEvent -= value;
                    }
                }
            }
        }

        public event EventHandler<DownloadItem> OnBeforeDownloadFired;
        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        private readonly DownloadProgress DownloadProgress;
        public event DownloadProgressChangedHandler DownloadProgressChanged;

        public ChromeDownloaderHandler(bool DownloadLinkOnly, DownloadProgressChangedHandler OnProgress = null)
        {       
            if (OnProgress != null) DownloadProgressChanged += OnProgress;
            LinkOnly = DownloadLinkOnly;
            DownloadProgress = new DownloadProgress();            
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    string DownloadsDirectoryPath = ServiceSingleton.Folders.DownloadDirectory;

                    OnFileDownloadRequestEvent(this, new FileDownloadRequestEvent(downloadItem.Url));

                    SW.Start();

                    callback.Continue(
                        Path.Combine(
                            DownloadsDirectoryPath,
                            downloadItem.SuggestedFileName
                        ),
                        showDialog: false
                    );
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);

            if (downloadItem.IsValid)
            {
                DownloadProgress.BytesReceived = downloadItem.ReceivedBytes;

                if (downloadItem.TotalBytes > 0L)
                {
                    DownloadProgress.TotalBytesToReceive = downloadItem.TotalBytes;
                }

                DownloadProgress.ProgressPercentage = downloadItem.PercentComplete;

                DownloadProgress.Speed = downloadItem.ReceivedBytes / 1024d / 1024d / SW.Elapsed.TotalSeconds;

                DownloadProgress.BytesReceivedAsString = (downloadItem.ReceivedBytes / 1024d / 1024d).ToString("0.00");
                DownloadProgress.TotalBytesToReceiveAsString = (downloadItem.TotalBytes / 1024d / 1024d).ToString("0.00");

                DownloadProgress.FileName = downloadItem.SuggestedFileName;
                
                if (downloadItem.IsInProgress && (downloadItem.PercentComplete != 0))
                {
                    if (DownloadProgressChanged != null)
                    {
                        DownloadProgressChanged(this, DownloadProgress);
                    }
                    
                }

                if (downloadItem.IsComplete)
                {
                    SW.Stop();
                    _IsDownloadComplete = true;
                    OnFileDownloadCompletedEvent(this, new FileDownloadRequestEvent(downloadItem.Url));
                }
            }
        }

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            if (!LinkOnly)
            {
                return true;
            }
            else
            {                
                OnFileDownloadRequestEvent(this, new FileDownloadRequestEvent(url));
                return false;
            }
            
        }        
    }

    public class ChromiumDownloader : ChromiumWebBrowser
    {
        private BrowserWindow Browser;        
        private WebSite WebSite;
        private string File;
        private string ModId;
        private TaskCompletionSource<object> TaskCompletionDownload = new TaskCompletionSource<object>();
        private TaskCompletionSource<string> TaskCompletionDownloadLink = new TaskCompletionSource<string>();
        event OnFileDownloadRequestedHandler OnFileDownloadRequestEvent;
        public event OnFileDownloadRequestedHandler OnFileDownloadRequest
        {
            add
            {
                if (OnFileDownloadRequestEvent != null)
                {
                    lock (OnFileDownloadRequestEvent)
                    {
                        OnFileDownloadRequestEvent += value;
                    }
                }
                else
                {
                    OnFileDownloadRequestEvent = value;
                }
            }
            remove
            {
                if (OnFileDownloadRequestEvent != null)
                {
                    lock (OnFileDownloadRequestEvent)
                    {
                        OnFileDownloadRequestEvent -= value;
                    }
                }
            }
        }

        private string _Url;
        public string Url { get { return _Url; } }

        public ChromiumDownloader(BrowserWindow Window, string address, bool LinkOnly, DownloadProgressChangedHandler OnProgress)
            :base(address)
        {
            _Url = address;
            Browser = Window;

            Dock = DockStyle.Fill;

            if (_Url.Contains("nexusmods.com"))
            {
                WebSite = WebSite.Nexus;
            }           
            else if (Url.Contains("enbdev.com"))
            {
                WebSite = WebSite.EnbDev;
            }
            else
            {
                WebSite = WebSite.Other;
            }

            DownloadHandler = new ChromeDownloaderHandler(LinkOnly, OnProgress);
            (DownloadHandler as ChromeDownloaderHandler).OnFileDownloadRequest += DownloadRequested;
            (DownloadHandler as ChromeDownloaderHandler).OnFileDownloadCompleted += DownloadCompleted;

            this.LoadingStateChanged += Browser_LoadingStateChanged;
            this.FrameLoadEnd += Browser_FrameLoadEnd;

            TaskCompletionDownload = new TaskCompletionSource<object>();
            TaskCompletionDownloadLink = new TaskCompletionSource<string>();
        }       

        private void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {                
                switch (WebSite)
                {
                    case WebSite.Nexus:
                        HandleNexusLoadEnd(e.Url);
                        break;                    
                }
            }
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                UnRegisterLoadingStateEvent();

                switch (WebSite)
                {
                    case WebSite.Nexus:
                        HandleNexusLoadState();
                        break;                  
                    case WebSite.EnbDev:
                        HandleEnbDev();
                        break;
                    case WebSite.Other:
                        HandleOthers();
                        break;
                }
            }
        }

        private void RegisterFrameLoadEnd()
        {
            this.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        private void UnRegisterFrameLoadEnd()
        {
            this.FrameLoadEnd -= Browser_FrameLoadEnd;
        }

        private void RegisterLoadingStateEvent()
        {
            this.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        private void UnRegisterLoadingStateEvent()
        {
            this.LoadingStateChanged -= Browser_LoadingStateChanged;
        }

        private void DownloadRequested(object sender, FileDownloadRequestEvent EventArgs)
        {
            TaskCompletionDownloadLink.SetResult(EventArgs.DownloadUrl);
            OnFileDownloadRequestEvent(this, EventArgs);
        }

        private void DownloadCompleted(object sender, FileDownloadRequestEvent EventArgs)
        {
            TaskCompletionDownload.SetResult(null);
        }       

        public bool IsDownloadComplete
        {
            get
            {
                return (DownloadHandler as ChromeDownloaderHandler).IsDownloadComplete;
            }            
        }        

        public Task AwaitDownLoad(string FileName)
        {
            File = FileName;          
            return TaskCompletionDownload.Task;
        }

        public Task<string> AwaitDownloadLink(string NexusModId)
        {
            ModId = NexusModId;
            return TaskCompletionDownloadLink.Task;
        }

        #region Scripts

        private async Task<bool> EvaluateScriptWithResponse(string Script)
        {
            var ScriptExecution = await this.GetMainFrame().EvaluateScriptAsync(Script);

            return ScriptExecution.Success && (int)ScriptExecution.Result == 1;
        }

        private async Task EvaluateScript(string Script)
        {
            await this.GetMainFrame().EvaluateScriptAsync(Script);
        }

        private void ExecuteScript(string Script)
        {
            this.GetMainFrame().ExecuteJavaScriptAsync(Script);
        }

        #endregion        

        #region Nexus

        private async Task<bool> IsLoginNeeded()
        {
            return await EvaluateScriptWithResponse(ScriptManager.GetIsLoginNeeded());
        }

        private void RedirectToLogin()
        {
            ExecuteScript(ScriptManager.GetRedirectToLogin());
        }

        private async Task<bool> IsModNotFound()
        {
            return await EvaluateScriptWithResponse(ScriptManager.GetIsModNotFound());
        }

        private async Task<bool> IsDownloadAvailable()
        {
            return await EvaluateScriptWithResponse(ScriptManager.GetIsDownloadAvailable());
        }

        private async void InitializeNexusManualDownload()
        {            
            await EvaluateScript(ScriptManager.GetNexusManualDownloadInit());

            await Task.Delay(100).ContinueWith(T =>
            {
                ExecuteScript(ScriptManager.GetNexusManualDownload());
            });
        }

        private async void HandleNexusLoadState()
        {
            if (await IsLoginNeeded())
            {
                RedirectToLogin();
            }            
            else if (await IsDownloadAvailable())
            {
                InitializeNexusManualDownload();
            }
        }

        private void HandleNexusLoadEnd(string Url)
        {
            if (Url == "https://users.nexusmods.com/auth/sign_in")
            {
                Browser.HideLoading();                
            }
            else if (Url.Contains("https://users.nexusmods.com/auth/continue?"))
            {
                Browser.HideLoading();                
            }
            else if (Url == "https://www.nexusmods.com/skyrimspecialedition/mods/" + ModId + "?tab=files")
            {
                RegisterLoadingStateEvent();
                Load(_Url);
            }
            else
            {
                Browser.HideLoading();
            }
        }


        #endregion

        #region ENB
        private void HandleEnbDev()
        {                       
            var Script = ScriptManager.GetHandleENBDev(File);

            ExecuteScript(Script);
        }
        #endregion

        #region Others

        private void HandleOthers()
        {
            Browser.HideLoading();
        }

        #endregion
    }
}
