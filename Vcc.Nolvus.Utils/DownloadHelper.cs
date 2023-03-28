using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgress progress);

    public static class DownloadHelper
    {
        private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";
        private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";

        private const string MEGA_DOMAIN = "mega.nz";
        private const string MEGA_DOMAIN2 = "https://mega.nz";

        public static MegaFileDownloader MegaDownloader = new MegaFileDownloader();        

        private static async Task DoDownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress, int Size, string MegaLogin, string MegaPassword)
        {
            if (UrlAddress.StartsWith(DownloadHelper.GOOGLE_DRIVE_DOMAIN) || UrlAddress.StartsWith(DownloadHelper.GOOGLE_DRIVE_DOMAIN2))
            {
                using (GoogleDriveFileDownloader Downloader = new GoogleDriveFileDownloader())
                {
                    Downloader.DownloadProgressChanged += OnProgress;

                    await Downloader.DownloadFileTaskAsync(UrlAddress, Location);

                    Downloader.DownloadProgressChanged -= OnProgress;
                }
            }
            else if (UrlAddress.StartsWith(DownloadHelper.MEGA_DOMAIN) || UrlAddress.StartsWith(DownloadHelper.MEGA_DOMAIN2))
            {                
                MegaDownloader.DownloadProgressChanged += OnProgress;

                await MegaDownloader.DownloadFile(UrlAddress, Location, Size, MegaLogin, MegaPassword);

                MegaDownloader.DownloadProgressChanged -= OnProgress;
            }
            else
            {
                FileDownloader FileDownloader = new FileDownloader();

                FileDownloader.DownloadProgressChanged += OnProgress;                

                await FileDownloader.DownloadFileTaskAsync(UrlAddress, Location);

                FileDownloader.DownloadProgressChanged -= OnProgress;
            }
        }

        public static async Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress, int Size)
        {
            await DownloadHelper.DoDownloadFile(UrlAddress, Location, OnProgress,  Size, string.Empty, string.Empty);        
        }

        public static async Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress, int Size, string MegaLogin, string MegaPassword)
        {
            await DownloadHelper.DoDownloadFile(UrlAddress, Location, OnProgress, Size, MegaLogin, MegaPassword);
        }
    }
}
