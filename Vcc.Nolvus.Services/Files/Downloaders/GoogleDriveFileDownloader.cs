using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using Vcc.Nolvus.Core.Events;

namespace Vcc.Nolvus.Services.Files.Downloaders
{
    public class GoogleDriveFileDownloader : BaseFileDownloader
    {
        private TaskCompletionSource<object> Tcs;        
        private int DriveDownloadAttempt;
        private Uri DownloadAddress;
        private string DownloadPath;
        private bool DownloadingDriveFile;
        private const int GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT = 3;

        public event AsyncCompletedEventHandler DownloadFileCompleted;        

        protected new CookieAwareWebClient Client
        {
            get
            {
                return base.Client as CookieAwareWebClient;
            }
        }

        protected override WebClient CreateWebClient()
        {
            return new CookieAwareWebClient();
        }

        private string GetGoogleDriveDownloadAddress(string Address)
        {
            int index = Address.IndexOf("id=");
            int closingIndex;
            if (index > 0)
            {
                index += 3;
                closingIndex = Address.IndexOf('&', index);
                if (closingIndex < 0)
                    closingIndex = Address.Length;
            }
            else
            {
                index = Address.IndexOf("file/d/");
                if (index < 0)
                    return string.Empty;

                index += 7;

                closingIndex = Address.IndexOf('/', index);
                if (closingIndex < 0)
                {
                    closingIndex = Address.IndexOf('?', index);
                    if (closingIndex < 0)
                        closingIndex = Address.Length;
                }
            }

            string fileID = Address.Substring(index, closingIndex - index);

            index = Address.IndexOf("resourcekey=");
            if (index > 0)
            {
                index += 12;
                closingIndex = Address.IndexOf('&', index);
                if (closingIndex < 0)
                    closingIndex = Address.Length;

                string resourceKey = Address.Substring(index, closingIndex - index);
                return string.Concat("https://drive.google.com/uc?id=", fileID, "&export=download&resourcekey=", resourceKey, "&confirm=t");
            }
            else
                return string.Concat("https://drive.google.com/uc?id=", fileID, "&export=download&confirm=t");
        }

        private bool ProcessDriveDownload()
        {
            FileInfo DownloadedFile = new FileInfo(DownloadPath);
            if (DownloadedFile == null)
                return true;

            if (DownloadedFile.Length > 60000L)
                return true;

            string content;
            using (var reader = DownloadedFile.OpenText())
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
                    DownloadAddress = new Uri("https://drive.google.com" + content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&"));
                    return false;
                }
            }

            return true;
        }

        private void DownloadFileInternal()
        {            
            SW.Start();
            Client.DownloadFileAsync(DownloadAddress, DownloadPath);           
        }

        private void DoDownloadFile(string UrlAddress, string Location)
        {
            DownloadingDriveFile = UrlAddress.StartsWith("drive.google.com") || UrlAddress.StartsWith("https://drive.google.com");

            UrlAddress = GetGoogleDriveDownloadAddress(UrlAddress);
            DriveDownloadAttempt = 1;
            Client.ContentRangeTarget = Progress;
            DownloadAddress = new Uri(UrlAddress);
            DownloadPath = Location;
            Progress.FileName = new FileInfo(Location).Name;

            DownloadFileInternal();           
        }

        public override Task DownloadFile(string UrlAddress, string Location)
        {
            Tcs = new TaskCompletionSource<object>();

            DoDownloadFile(UrlAddress, Location);

            return Tcs.Task;
        }

        protected override void FileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!DownloadingDriveFile)
            {
                if (DownloadFileCompleted != null)
                    DownloadFileCompleted(this, e);

                SW.Stop();
                Tcs.SetResult(new object());
            }
            else
            {
                if (DriveDownloadAttempt < GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT && !ProcessDriveDownload())
                {
                    DriveDownloadAttempt++;
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
    }
}
