using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace Vcc.Nolvus.Utils
{
    //public class CustomWebClient : WebClient
    //{
    //    public CustomWebClient() { }

    //    protected override WebRequest GetWebRequest(Uri address)
    //    {
    //        var request = base.GetWebRequest(address) as HttpWebRequest;
    //        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36";
    //        request.Accept = "*/*";

    //        return request;
    //    }
    //}

    public class FileDownloader
    {
        private Stopwatch SW = new Stopwatch();
        private WebClient WebClient;

        private readonly DownloadProgress DownloadProgress;        

        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        private string _FileName;

        public FileDownloader()
        {
            DownloadProgress = new DownloadProgress();
        }

        public async Task DownloadFileTaskAsync(string UrlAddress, string Location)
        {
            DownloadProgress.TotalBytesToReceive = -1L;          

            try
            {
                _FileName = new FileInfo(Location).Name;

                WebClient = new WebClient();

                WebClient.Proxy = null;

                WebClient.Credentials = System.Net.CredentialCache.DefaultCredentials;
                WebClient.Headers["Accept"] = "*/*";
                WebClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36";                
                

                WebClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                

                SW.Start();

                try
                {
                    await WebClient.DownloadFileTaskAsync(new Uri(UrlAddress), Location);
                }
                catch(Exception ex)
                {
                    string test = ex.Message;
                    throw ex;
                }

                SW.Stop();               
            }
            finally
            {
                WebClient.Dispose();
            }            
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {            
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
            {
                DownloadProgress.BytesReceived = e.BytesReceived;

                if (e.TotalBytesToReceive > 0L)
                {
                    DownloadProgress.TotalBytesToReceive = e.TotalBytesToReceive;
                }

                DownloadProgress.ProgressPercentage = e.ProgressPercentage;

                DownloadProgress.Speed = e.BytesReceived / 1024d / 1024d / SW.Elapsed.TotalSeconds;

                DownloadProgress.BytesReceivedAsString = (e.BytesReceived / 1024d / 1024d).ToString("0.00");
                DownloadProgress.TotalBytesToReceiveAsString = (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00");

                DownloadProgress.FileName = _FileName;

                DownloadProgressChanged(this, DownloadProgress);
            }
        }
    }
}
