using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;


namespace Vcc.Nolvus.Services.Files
{
    public abstract class BaseFileDownloader : IDisposable
    {
        protected Stopwatch SW = new Stopwatch();
        private WebClient Wcli;       

        event DownloadProgressChangedHandler DownloadProgressChangedEvent;

        public event DownloadProgressChangedHandler DownloadProgressChanged
        {
            add
            {
                if (DownloadProgressChangedEvent != null)
                {
                    lock (DownloadProgressChangedEvent)
                    {
                        DownloadProgressChangedEvent += value;
                    }
                }
                else
                {
                    DownloadProgressChangedEvent = value;
                }
            }
            remove
            {
                if (DownloadProgressChangedEvent != null)
                {
                    lock (DownloadProgressChangedEvent)
                    {
                        DownloadProgressChangedEvent -= value;
                    }
                }
            }
        }

        protected readonly DownloadProgress Progress;
        protected string FileName;

        protected WebClient Client
        {
            get
            {
                return Wcli;
            }
        }

        protected virtual WebClient CreateWebClient()
        {
            var Client = new WebClient();            

            return Client;
        }

        public BaseFileDownloader()
        {
            Wcli = CreateWebClient();

            Wcli.Proxy = null;

            Wcli.UseDefaultCredentials = true;            

            Wcli.DownloadProgressChanged += ProgressChanged;
            Wcli.DownloadFileCompleted += FileCompleted;

            Progress = new DownloadProgress();
            Progress.TotalBytesToReceive = -1L;           
        }
        
        public abstract Task DownloadFile(string UrlAddress, string Location);

        protected void NotifyProgress()
        {
            DownloadProgressChangedHandler Handler = this.DownloadProgressChangedEvent;
            if (Handler != null) Handler(this, Progress);
        }

        protected virtual void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress.BytesReceived = e.BytesReceived;

            if (e.TotalBytesToReceive > 0L)
            {
                Progress.TotalBytesToReceive = e.TotalBytesToReceive;
            }

            Progress.ProgressPercentage = e.ProgressPercentage;

            Progress.Speed = e.BytesReceived / 1024d / 1024d / SW.Elapsed.TotalSeconds;

            Progress.BytesReceivedAsString = (e.BytesReceived / 1024d / 1024d).ToString("0.00");
            Progress.TotalBytesToReceiveAsString = (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00");

            Progress.FileName = FileName;

            NotifyProgress();            
        }        

        protected virtual void FileCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
