using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using CG.Web.MegaApiClient;


namespace Vcc.Nolvus.Utils
{
    public class MegaWebClient : IWebClient
    {
        private const int DefaultResponseTimeout = Timeout.Infinite;

        private static readonly HttpClient s_sharedHttpClient = CreateHttpClient(DefaultResponseTimeout, GenerateUserAgent());

        private readonly HttpClient _httpClient;

        public MegaWebClient(int responseTimeout = DefaultResponseTimeout, ProductInfoHeaderValue userAgent = null)
        {
            if (responseTimeout == DefaultResponseTimeout && userAgent == null)
            {
                _httpClient = s_sharedHttpClient;
            }
            else
            {
                _httpClient = CreateHttpClient(responseTimeout, userAgent ?? GenerateUserAgent());
            }
        }

        public int BufferSize { get; set; } = Options.DefaultBufferSize;

        public string PostRequestJson(Uri url, string jsonData)
        {
            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
            {
                using (var responseStream = PostRequest(url, jsonStream, "application/json"))
                {
                    return StreamToString(responseStream);
                }
            }
        }

        public string PostRequestRaw(Uri url, Stream dataStream)
        {
            using (var responseStream = PostRequest(url, dataStream, "application/json"))
            {
                return StreamToString(responseStream);
            }
        }

        public Stream PostRequestRawAsStream(Uri url, Stream dataStream)
        {
            return PostRequest(url, dataStream, "application/octet-stream");
        }

        public Stream GetRequestRaw(Uri url)
        {
            return _httpClient.GetStreamAsync(url).Result;
        }

        private Stream PostRequest(Uri url, Stream dataStream, string contentType)
        {
            using (var content = new StreamContent(dataStream, BufferSize))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega Api : Before Response");

                var response = _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).Result;

                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega Api : After Response");

                if (!response.IsSuccessStatusCode
                    && response.StatusCode == HttpStatusCode.InternalServerError
                    && response.ReasonPhrase == "Server Too Busy")
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(((long)ApiResultCode.RequestFailedRetry).ToString()));
                }

                response.EnsureSuccessStatusCode();

                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + "Mega Api : EnsureSuccessStatusCode");

                return response.Content.ReadAsStreamAsync().Result;
            }
        }

        private string StreamToString(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static HttpClient CreateHttpClient(int timeout, ProductInfoHeaderValue userAgent)
        {
            var httpClient = new HttpClient();

            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
            httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

            return httpClient;
        }

        private static ProductInfoHeaderValue GenerateUserAgent()
        {
            var assemblyName = typeof(MegaWebClient).GetTypeInfo().Assembly.GetName();
            return new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString(2));
        }
    }
}

