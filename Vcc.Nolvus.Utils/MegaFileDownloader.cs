using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CG.Web.MegaApiClient;

namespace Vcc.Nolvus.Utils
{
    public class MegaFileDownloader
    {
        private Stopwatch SW = new Stopwatch();
        private MegaApiClient MegaApiClient;
        private string _FileName;

        private readonly DownloadProgress _DownloadProgress;

        private DownloadProgressChangedEventHandler _DownloadProgressChanged;

        public event DownloadProgressChangedEventHandler DownloadProgressChanged
        {
            add
            {
                if (_DownloadProgressChanged == null || !_DownloadProgressChanged.GetInvocationList().Contains(value))
                {
                    _DownloadProgressChanged += value;
                }
            }
            remove
            {
                _DownloadProgressChanged -= value;
            }
        }

        private CancellationTokenSource CancelKeepAlive = new CancellationTokenSource();

        public MegaFileDownloader()
        {
            MegaApiClient = new MegaApiClient();                       

            MegaApiClient.ApiRequestFailed += MegaApiClient_ApiRequestFailed;

            _DownloadProgress = new DownloadProgress();

            //Init();                       
        }     
        
        private async void Init()
        {
            try
            {
                await PeriodicTask.Run(KeepAlive, new TimeSpan(0, 1, 0), CancelKeepAlive.Token);
            }
            catch
            {

            }
        }

        private void KeepAlive()
        {
            if (MegaApiClient.IsLoggedIn)
                MegaApiClient.GetNodes().Single(x => x.Type == NodeType.Root);            
        }  

        public void StopSurvey()
        {
            this.CancelKeepAlive.Cancel();
        }

        public async Task<MegaAuthentication> MegaLogin(string Username, string Password)
        {
            return await Task.Run(async () => 
            {
                MegaApiClient.AuthInfos AuthInfos = null;

                if (MegaApiClient.IsLoggedIn)
                {
                    await MegaApiClient.LogoutAsync();
                }

                try
                {
                    AuthInfos = MegaApiClient.GenerateAuthInfos(Username, Password);
                }
                catch (ApiException e)
                {
                    switch (e.ApiResultCode)
                    {
                        case ApiResultCode.BadArguments: return new MegaAuthentication("Invalid email or password!", false);
                        case ApiResultCode.InternalError: return new MegaAuthentication("Internal error!", false);
                    }
                }

                try
                {
                    await MegaApiClient.LoginAsync(AuthInfos);
                }
                catch (ApiException e)
                {
                    switch (e.ApiResultCode)
                    {
                        case ApiResultCode.RequestIncomplete:
                        case ApiResultCode.ResourceNotExists:
                            return new MegaAuthentication("Invalid email or password!", false);
                        case ApiResultCode.InternalError: return new MegaAuthentication("Internal error!", false);
                    }
                }

                return new MegaAuthentication("Authentication successfull", true);
            });
            
            
        }

        private async Task _InternalLogin(string Username, string Password)
        {
            var Tsk = Task.Run(async () => 
            {
                if (MegaApiClient.IsLoggedIn)
                {
                    //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega.nz already logged");
                    return;
                }

                if (Username == string.Empty && Password == string.Empty)
                {
                    //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega.nz anonymous login");
                    await MegaApiClient.LoginAnonymousAsync();
                }
                else
                {
                    //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega.nz credentials login");
                    await MegaApiClient.LoginAsync(MegaApiClient.GenerateAuthInfos(Username, Password));
                }
            });

            await Tsk;
            
        }

        public async Task DownloadFile(string UrlAddress, string Location, int Size, string UserName, string Password)
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {

                    try
                    {

                        _DownloadProgress.TotalBytesToReceive = -1L;

                        await _InternalLogin(UserName, Password);

                        Uri FileLink = new Uri(UrlAddress);

                        _FileName = new FileInfo(Location).Name;

                        IProgress<double> ProgressHandler = new Progress<double>(x =>
                        {
                            if (_DownloadProgressChanged != null)
                            {
                                _DownloadProgress.ProgressPercentage = System.Convert.ToInt16(Math.Round(x, 0));
                                _DownloadProgress.TotalBytesToReceive = (long)Size * 1024;

                                if (_DownloadProgress.ProgressPercentage != 0)
                                {
                                    _DownloadProgress.BytesReceived = (_DownloadProgress.TotalBytesToReceive / 100) * _DownloadProgress.ProgressPercentage;
                                }

                                _DownloadProgress.Speed = _DownloadProgress.BytesReceived / 1024d / 1024d / SW.Elapsed.TotalSeconds;

                                _DownloadProgress.BytesReceivedAsString = (_DownloadProgress.BytesReceived / 1024d / 1024d).ToString("0.00");
                                _DownloadProgress.TotalBytesToReceiveAsString = (_DownloadProgress.TotalBytesToReceive / 1024d / 1024d).ToString("0.00");

                                _DownloadProgress.FileName = _FileName;

                                _DownloadProgressChanged(this, _DownloadProgress);
                            }

                        });

                        SW.Start();

                        await MegaApiClient.DownloadFileAsync(FileLink, Location, ProgressHandler);                        

                        SW.Stop();
                    }
                    catch (Exception e)
                    {
                        if (e.InnerException != null)
                        {
                            if (e.InnerException is System.Net.Http.HttpRequestException && e.InnerException.Message.Contains("509"))
                            {
                                throw new MegaLimitReachedException();
                            }
                            else
                            {
                                throw new MegaException(e.InnerException.Message);
                            }

                        }
                        else
                        {
                            throw e;
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

        private void MegaApiClient_ApiRequestFailed(object sender, ApiRequestFailedEventArgs e)
        {                  
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", $"ApiRequestFailed: {e.ApiResult}, {e.ApiUrl}, {e.AttemptNum}, {e.RetryDelay}, {e.ResponseJson}, {e.Exception} {e.Exception?.Message}");
        }
    }
}
