using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.NexusApi.SSO.Events;
using Vcc.Nolvus.NexusApi.SSO.Responses;

namespace Vcc.Nolvus.NexusApi.SSO
{
    public class NexusSSOSettings
    {
        public Func<IBrowserInstance> Browser { get; set; }
    }

    public class NexusSSOManager
    {
        #region Field

        private bool IsAuthenticated = false;        
        private ClientWebSocket WebSocket;
        private NexusSSORequest Request;
        private NexusSSOSettings SocketSettings;
        private IBrowserInstance _Browser;

        #endregion

        #region Properties

        private IBrowserInstance Browser
        {
            get
            {
                if (_Browser == null)
                {
                    _Browser = SocketSettings.Browser();
                    _Browser.OnBrowserClosed += TriggerBrowserClose;
                }

                return _Browser;
            }
        }

        public bool Authenticated
        {
            get { return IsAuthenticated; }
        }

        #endregion

        #region Events

        event OnAuthenticatedHandler OnAuthenticatedEvent;

        public event OnAuthenticatedHandler OnAuthenticated
        {
            add
            {
                if (OnAuthenticatedEvent != null)
                {
                    lock (OnAuthenticatedEvent)
                    {
                        OnAuthenticatedEvent += value;
                    }
                }
                else
                {
                    OnAuthenticatedEvent = value;
                }
            }
            remove
            {
                if (OnAuthenticatedEvent != null)
                {
                    lock (OnAuthenticatedEvent)
                    {
                        OnAuthenticatedEvent -= value;
                    }
                }
            }
        }

        event OnAuthenticatingHandler OnAuthenticatingEvent;

        public event OnAuthenticatingHandler OnAuthenticating
        {
            add
            {
                if (OnAuthenticatingEvent != null)
                {
                    lock (OnAuthenticatingEvent)
                    {
                        OnAuthenticatingEvent += value;
                    }
                }
                else
                {
                    OnAuthenticatingEvent = value;
                }
            }
            remove
            {
                if (OnAuthenticatingEvent != null)
                {
                    lock (OnAuthenticatingEvent)
                    {
                        OnAuthenticatingEvent -= value;
                    }
                }
            }
        }

        event OnRequestErrorHandler OnRequestErrorEvent;

        public event OnRequestErrorHandler OnRequestError
        {
            add
            {
                if (OnRequestErrorEvent != null)
                {
                    lock (OnRequestErrorEvent)
                    {
                        OnRequestErrorEvent += value;
                    }
                }
                else
                {
                    OnRequestErrorEvent = value;
                }
            }
            remove
            {
                if (OnRequestErrorEvent != null)
                {
                    lock (OnRequestErrorEvent)
                    {
                        OnRequestErrorEvent -= value;
                    }
                }
            }
        }

        event OnBrowserClosedHandler OnBrowserClosedEvent;

        public event OnBrowserClosedHandler OnBrowserClosed
        {
            add
            {
                if (OnBrowserClosedEvent != null)
                {
                    lock (OnBrowserClosedEvent)
                    {
                        OnBrowserClosedEvent += value;
                    }
                }
                else
                {
                    OnBrowserClosedEvent = value;
                }
            }
            remove
            {
                if (OnBrowserClosedEvent != null)
                {
                    lock (OnBrowserClosedEvent)
                    {
                        OnBrowserClosedEvent -= value;
                    }
                }
            }
        }

        #endregion

        public NexusSSOManager(NexusSSOSettings Settings = null)
        {
            if (Settings != null)
            {
                SocketSettings = Settings;
            }
        }

        #region Methods

        private void TriggerAuthenticated(string ApiKey)
        {
            OnAuthenticatedHandler Handler = OnAuthenticatedEvent;
            AuthenticationEventArgs Event = new AuthenticationEventArgs(ApiKey);
            if (Handler != null) Handler(this, Event);
        }

        private void TriggerAuthenticating(string UuId)
        {
            OnAuthenticatingHandler Handler = OnAuthenticatingEvent;
            AuthenticatingEventArgs Event = new AuthenticatingEventArgs(UuId);
            if (Handler != null) Handler(this, Event);
        }

        private void TriggerError(string Message)
        {
            OnRequestErrorHandler Handler = OnRequestErrorEvent;
            RequestErrorEventArgs Event = new RequestErrorEventArgs(Message);
            if (Handler != null) Handler(this, Event);
        }

        private void TriggerBrowserClose(object sender, EventArgs EventArgs)
        {
            _Browser.OnBrowserClosed -= TriggerBrowserClose;
            _Browser = null;

            Close();

            OnBrowserClosedHandler Handler = OnBrowserClosedEvent;
            EventArgs Event = new EventArgs();
            if (Handler != null) Handler(this, Event);
        }

        public async Task Connect()
        {
            try
            {
                WebSocket = new ClientWebSocket();
                await WebSocket.ConnectAsync(new Uri("wss://sso.nexusmods.com"), CancellationToken.None);
                StartListenerThread();
            }
            catch (Exception e)
            {
                TriggerError(e.Message);
            }
        }

        public async Task Authenticate()
        {
            if (!IsAuthenticated)
            {
                try
                {
                    if (WebSocket.State == WebSocketState.Open)
                    {

                        Request = new NexusSSORequest
                        {
                            id = Guid.NewGuid().ToString()
                        };

                        await WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Request))), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                catch (Exception e)
                {
                    TriggerError(e.Message);
                }
            }
        }

        public void Close()
        {            
            WebSocket.Dispose();
            WebSocket = null;
        }

        private void StartListenerThread()
        {
            Task.Run(async () =>
            {
                var Buffer = new byte[1024 * 4];

                while (WebSocket.State == WebSocketState.Open || WebSocket.State == WebSocketState.CloseSent)                
                {
                    try
                    {
                        var Result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(Buffer), CancellationToken.None);

                        var NexusResponse = JsonConvert.DeserializeObject<NexusSSOResponse>(Encoding.UTF8.GetString(Buffer, 0, Result.Count));

                        if (NexusResponse != null && NexusResponse.Success)
                        {
                            if (NexusResponse.Data.ApiKey != null)
                            {
                                TriggerAuthenticated(NexusResponse.Data.ApiKey);
                                IsAuthenticated = true;

                                Browser.CloseBrowser();
                            }
                            else if (NexusResponse.Data.Token != string.Empty)
                            {
                                Request.SetToken(NexusResponse.Data.Token);

                                TriggerAuthenticating(Request.id);

                                await Browser.NexusSSOAuthentication(Request.id, Strings.NolvusSlug);
                            }
                        }
                        else
                        {
                            TriggerError(NexusResponse.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        TriggerError(ex.Message);
                    }
                }
            });
        }

        #endregion
    }
}
