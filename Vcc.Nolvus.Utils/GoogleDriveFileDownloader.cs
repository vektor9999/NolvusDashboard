using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Vcc.Nolvus.Utils
{
    public class CookieAwareWebClient : WebClient
    {
        private class CookieContainer
        {
            private readonly Dictionary<string, string> cookies = new Dictionary<string, string>();

            public string this[Uri address]
            {
                get
                {
                    string cookie;
                    if (cookies.TryGetValue(address.Host, out cookie))
                        return cookie;

                    return null;
                }
                set
                {
                    cookies[address.Host] = value;
                }
            }
        }

        private readonly CookieContainer cookies = new CookieContainer();
        public DownloadProgress ContentRangeTarget;

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                string cookie = cookies[address];
                if (cookie != null)
                    ((HttpWebRequest)request).Headers.Set("cookie", cookie);

                if (ContentRangeTarget != null)
                    ((HttpWebRequest)request).AddRange(0);
            }

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            return ProcessResponse(base.GetWebResponse(request, result));
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return ProcessResponse(base.GetWebResponse(request));
        }

        private WebResponse ProcessResponse(WebResponse response)
        {
            string[] cookies = response.Headers.GetValues("Set-Cookie");
            if (cookies != null && cookies.Length > 0)
            {
                int length = 0;
                for (int i = 0; i < cookies.Length; i++)
                    length += cookies[i].Length;

                StringBuilder cookie = new StringBuilder(length);
                for (int i = 0; i < cookies.Length; i++)
                    cookie.Append(cookies[i]);

                this.cookies[response.ResponseUri] = cookie.ToString();
            }

            if (ContentRangeTarget != null)
            {
                string[] rangeLengthHeader = response.Headers.GetValues("Content-Range");
                if (rangeLengthHeader != null && rangeLengthHeader.Length > 0)
                {
                    int splitIndex = rangeLengthHeader[0].LastIndexOf('/');
                    if (splitIndex >= 0 && splitIndex < rangeLengthHeader[0].Length - 1)
                    {
                        long length;
                        if (long.TryParse(rangeLengthHeader[0].Substring(splitIndex + 1), out length))
                            ContentRangeTarget.TotalBytesToReceive = length;
                    }
                }
            }

            return response;
        }
    }

    public class GoogleDriveFileDownloader : IDisposable
    {
        private Stopwatch SW = new Stopwatch();        

        private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";        
        private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";       

        private TaskCompletionSource<object> Tcs;
        
        private const int GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT = 3;        
        
        

        private readonly CookieAwareWebClient webClient;        
        private readonly DownloadProgress downloadProgress;

        private Uri downloadAddress;
        private string downloadPath;

        private bool asyncDownload;
        private object userToken;

        private bool downloadingDriveFile;        
        private int driveDownloadAttempt;

        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        public GoogleDriveFileDownloader()
        {
            webClient = new CookieAwareWebClient();
            webClient.Proxy = null;
            webClient.DownloadProgressChanged += DownloadProgressChangedCallback;
            webClient.DownloadFileCompleted += DownloadFileCompletedCallback;

            webClient.UseDefaultCredentials = true;            

            downloadProgress = new DownloadProgress();
        }

        public void DownloadFile(string address, string fileName)
        {
            DownloadFile(address, fileName, false, null);
        }

        public void DownloadFileAsync(string address, string fileName, object userToken = null)
        {
            DownloadFile(address, fileName, true, userToken);
        }

        public Task DownloadFileTaskAsync(string address, string fileName)
        {
            Tcs = new TaskCompletionSource<object>();            
            DownloadFile(address, fileName, true, userToken);

            return Tcs.Task;
        }

        private void DownloadFile(string address, string fileName, bool asyncDownload, object userToken)
        {
            downloadingDriveFile = address.StartsWith(GOOGLE_DRIVE_DOMAIN) || address.StartsWith(GOOGLE_DRIVE_DOMAIN2);            

            if (downloadingDriveFile)
            {
                address = GetGoogleDriveDownloadAddress(address);
                driveDownloadAttempt = 1;

                webClient.ContentRangeTarget = downloadProgress;
            }           
            else
                webClient.ContentRangeTarget = null;

            downloadAddress = new Uri(address);
            downloadPath = fileName;

            downloadProgress.TotalBytesToReceive = -1L;
            //downloadProgress.UserState = userToken;
            downloadProgress.FileName = new FileInfo(fileName).Name;
            this.asyncDownload = asyncDownload;
            this.userToken = userToken;

            DownloadFileInternal();
        }

        private void DownloadFileInternal()
        {
            if (!asyncDownload)
            {
                webClient.DownloadFile(downloadAddress, downloadPath);

                DownloadFileCompletedCallback(webClient, new AsyncCompletedEventArgs(null, false, null));
            }
            else if (userToken == null)
            {
                SW.Start();
                webClient.DownloadFileAsync(downloadAddress, downloadPath);
            }
            else
                webClient.DownloadFileAsync(downloadAddress, downloadPath, userToken);
        }

        private void DownloadProgressChangedCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
            {
                downloadProgress.BytesReceived = e.BytesReceived;
                if (e.TotalBytesToReceive > 0L)
                    downloadProgress.TotalBytesToReceive = e.TotalBytesToReceive;

                downloadProgress.ProgressPercentage = e.ProgressPercentage;

                downloadProgress.Speed = e.BytesReceived / 1024d / 1024d / SW.Elapsed.TotalSeconds;

                downloadProgress.BytesReceivedAsString = (e.BytesReceived / 1024d / 1024d).ToString("0.00");
                downloadProgress.TotalBytesToReceiveAsString = (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00");                

                DownloadProgressChanged(this, downloadProgress);
            }
        }

        private void DownloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {            
            if (!downloadingDriveFile)
            {
                if (DownloadFileCompleted != null)
                    DownloadFileCompleted(this, e);

                SW.Stop();
                Tcs.SetResult(new object());                
            }
            else
            {
                if (driveDownloadAttempt < GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT && !ProcessDriveDownload())
                {                    
                    driveDownloadAttempt++;
                    DownloadFileInternal();
                }
                else
                {
                    if (DownloadFileCompleted != null)
                        DownloadFileCompleted(this, e);

                    SW.Stop();
                    Tcs.SetResult(new object());                    
                }
            }
        }

        private bool ProcessDriveDownload()
        {
            FileInfo downloadedFile = new FileInfo(downloadPath);
            if (downloadedFile == null)
                return true;
            
            if (downloadedFile.Length > 60000L)
                return true;
            
            string content;
            using (var reader = downloadedFile.OpenText())
            {                
                char[] header = new char[20];
                int readCount = reader.ReadBlock(header, 0, 20);
                if (readCount < 20 || !(new string(header).Contains("<!DOCTYPE html>")))
                    return true;

                content = reader.ReadToEnd();
            }

            int linkIndex = content.LastIndexOf("href=\"/uc?");
            if (linkIndex >= 0)
            {
                linkIndex += 6;
                int linkEnd = content.IndexOf('"', linkIndex);
                if (linkEnd >= 0)
                {
                    downloadAddress = new Uri("https://drive.google.com" + content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&"));
                    return false;
                }
            }

            return true;
        }

        private string GetGoogleDriveDownloadAddress(string address)
        {
            int index = address.IndexOf("id=");
            int closingIndex;
            if (index > 0)
            {
                index += 3;
                closingIndex = address.IndexOf('&', index);
                if (closingIndex < 0)
                    closingIndex = address.Length;
            }
            else
            {
                index = address.IndexOf("file/d/");
                if (index < 0)
                    return string.Empty;

                index += 7;

                closingIndex = address.IndexOf('/', index);
                if (closingIndex < 0)
                {
                    closingIndex = address.IndexOf('?', index);
                    if (closingIndex < 0)
                        closingIndex = address.Length;
                }
            }

            string fileID = address.Substring(index, closingIndex - index);

            index = address.IndexOf("resourcekey=");
            if (index > 0)
            {
                index += 12;
                closingIndex = address.IndexOf('&', index);
                if (closingIndex < 0)
                    closingIndex = address.Length;

                string resourceKey = address.Substring(index, closingIndex - index);
                return string.Concat("https://drive.google.com/uc?id=", fileID, "&export=download&resourcekey=", resourceKey, "&confirm=t");
            }
            else
                return string.Concat("https://drive.google.com/uc?id=", fileID, "&export=download&confirm=t");
        }

        public void Dispose()
        {
            webClient.Dispose();
        }
    }
}
