using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using CG.Web.MegaApiClient;
using Vcc.Nolvus.Core.Services;


namespace Vcc.Nolvus.Services.Files.Downloaders
{
    public class MegaFileDownloader : BaseFileDownloader
    {
        private MegaApiClient MegaApiClient;

        public MegaFileDownloader()
        {
            MegaApiClient = new MegaApiClient();
            MegaApiClient.ApiRequestFailed += MegaApiClient_ApiRequestFailed;  
        }

        private void MegaApiClient_ApiRequestFailed(object sender, ApiRequestFailedEventArgs e)
        {
            ServiceSingleton.Logger.Log($"Mega Api Request Failed: {e.ApiResult}, {e.ApiUrl}, {e.AttemptNum}, {e.RetryDelay}, {e.ResponseJson}, {e.Exception} {e.Exception?.Message}");
        }

        public override async Task DownloadFile(string UrlAddress, string Location)
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {

                    try
                    {
                        await MegaApiClient.LoginAnonymousAsync();

                        Uri FileLink = new Uri(UrlAddress);
                        FileName = new FileInfo(Location).Name;

                        INode FileNode = await MegaApiClient.GetNodeFromLinkAsync(FileLink);

                        SW.Start();

                        await MegaApiClient.DownloadFileAsync(FileLink, Location, new Progress<double>(x =>
                        {
                            Progress.ProgressPercentage = System.Convert.ToInt16(Math.Round(x, 0));
                            Progress.TotalBytesToReceive = FileNode.Size;

                            if (Progress.ProgressPercentage != 0)
                            {
                                Progress.BytesReceived = (Progress.TotalBytesToReceive / 100) * Progress.ProgressPercentage;
                            }

                            Progress.Speed = Progress.BytesReceived / 1024d / 1024d / SW.Elapsed.TotalSeconds;

                            Progress.BytesReceivedAsString = (Progress.BytesReceived / 1024d / 1024d).ToString("0.00");
                            Progress.TotalBytesToReceiveAsString = (Progress.TotalBytesToReceive / 1024d / 1024d).ToString("0.00");

                            Progress.FileName = FileName;

                            NotifyProgress();
                        }));

                        SW.Stop();
                    }
                    catch(Exception ex)
                    {
                        if (ex.Message.Contains("509"))
                        {
                            throw new Exception("Your daily mega.nz limit of 5gb by day has been reached. Wait until the limit (24 hours) has been reset or use a VPN to bypass this limit.");
                        }
                        else
                        {
                            throw ex;
                        }
                    }                  
                }
                finally
                {
                    await MegaApiClient.LogoutAsync();
                }
            });

            await Tsk;            
        }
    }
}
