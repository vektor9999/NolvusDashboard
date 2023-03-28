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

        public event DownloadProgressChangedHandler DownloadProgressChanged;
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
            Wcli.Headers["Accept"] = "*/*";
            Wcli.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36";


            Wcli.DownloadProgressChanged += ProgressChanged;
            Wcli.DownloadFileCompleted += FileCompleted;

            Progress = new DownloadProgress();
            Progress.TotalBytesToReceive = -1L;           
        }
        
        public abstract Task DownloadFile(string UrlAddress, string Location);

        protected virtual void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
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

                DownloadProgressChanged(this, Progress);
            }
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
