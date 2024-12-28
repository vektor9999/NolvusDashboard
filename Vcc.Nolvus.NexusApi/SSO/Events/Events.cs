using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.NexusApi.SSO.Events
{
    public class AuthenticationEventArgs : EventArgs
    {
        private string _ApiKey = string.Empty;

        public string ApiKey
        {
            get { return _ApiKey; }
        }

        public AuthenticationEventArgs(string ApiKey)
        {
            _ApiKey = ApiKey;
        }
    }

    public class AuthenticatingEventArgs : EventArgs
    {
        private string _Id = string.Empty;

        public string Id
        {
            get { return _Id; }
        }

        public AuthenticatingEventArgs(string Id)
        {
            _Id = Id;
        }
    }

    public class RequestErrorEventArgs : EventArgs
    {
        private string _Message = string.Empty;

        public string Message
        {
            get { return _Message; }
        }

        public RequestErrorEventArgs(string Message)
        {
            _Message = Message;
        }
    }

    public delegate void OnAuthenticatedHandler(object sender, AuthenticationEventArgs EventArgs);
    public delegate void OnAuthenticatingHandler(object sender, AuthenticatingEventArgs EventArgs);
    public delegate void OnRequestErrorHandler(object sender, RequestErrorEventArgs EventArgs);
}
