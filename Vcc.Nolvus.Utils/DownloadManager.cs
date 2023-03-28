using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public class DownloadManager
    {
        private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";
        private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";

        private const string MEGA_DOMAIN = "mega.nz";
        private const string MEGA_DOMAIN2 = "https://mega.nz";        

        private async Task DoDownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress, int Size, string MegaLogin, string MegaPassword)
        {
            if (UrlAddress.StartsWith(GOOGLE_DRIVE_DOMAIN) || UrlAddress.StartsWith(GOOGLE_DRIVE_DOMAIN2))
            {
                using (GoogleDriveFileDownloader Downloader = new GoogleDriveFileDownloader())
                {
                    Downloader.DownloadProgressChanged += OnProgress;

                    await Downloader.DownloadFileTaskAsync(UrlAddress, Location);

                    //Downloader.DownloadProgressChanged -= OnProgress;
                }
            }
            else if (UrlAddress.StartsWith(MEGA_DOMAIN) || UrlAddress.StartsWith(MEGA_DOMAIN2))
            {
                MegaFileDownloader MegaDownloader = new MegaFileDownloader();

                MegaDownloader.DownloadProgressChanged += OnProgress;

                await MegaDownloader.DownloadFile(UrlAddress, Location, Size, MegaLogin, MegaPassword);

                //MegaDownloader.DownloadProgressChanged -= OnProgress;
            }
            else
            {
                FileDownloader FileDownloader = new FileDownloader();

                FileDownloader.DownloadProgressChanged += OnProgress;

                await FileDownloader.DownloadFileTaskAsync(UrlAddress, Location);

                //FileDownloader.DownloadProgressChanged -= OnProgress;
            }
        }

        public async Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress, int Size, string MegaLogin, string MegaPassword)
        {
            await this.DoDownloadFile(UrlAddress, Location, OnProgress, Size, MegaLogin, MegaPassword);
        }

        public async Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedEventHandler OnProgress)
        {
            await this.DoDownloadFile(UrlAddress, Location, OnProgress, 0, string.Empty, string.Empty);
        }
    }
}
