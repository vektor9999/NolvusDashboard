using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Vcc.Nolvus.Core.Events;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Services.Files.Downloaders
{
    public class CookieAwareWebClient : WebClient
    {
        private TaskCompletionSource<object> _TaskCompletion;

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

        public CookieAwareWebClient(TaskCompletionSource<object> TaskCompletion)
        {
            _TaskCompletion = TaskCompletion;
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
            WebResponse Response = null;

            try
            {
                Response = ProcessResponse(base.GetWebResponse(request, result));
            }
            catch (Exception ex)
            {
                var CaughtException = ex;

                if (CaughtException.Message.Contains("429"))
                {
                    CaughtException = new Exception("The server returned error code 429 (too many requests) which means that the google drive server where this file is hosted is currently overloaded by the number of simultaneous downloads. Please try again later");
                }
                
                _TaskCompletion.SetException(CaughtException);
            }

            return Response;
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
}
