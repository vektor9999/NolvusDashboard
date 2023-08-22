using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Vcc.Nolvus.Api.Installer.Core
{
    public class ApiException
    {
        [JsonProperty(PropertyName = "ExceptionMessage")]
        public string ExceptionMessage { get; set; }

        [JsonProperty(PropertyName = "ExceptionType")]
        public string ExceptionType { get; set; }

        [JsonProperty(PropertyName = "StackTrace")]
        public string StackTrace { get; set; }
    }

    public static class ExceptionHandler
    {
        public static void ThrowWebApiException(HttpResponseMessage Response)
        {
            try
            {
                var ErrorAsJson = JsonConvert.DeserializeObject<ApiException>(Response.Content.ReadAsStringAsync().Result);

                throw new WebApiException(ErrorAsJson.ExceptionMessage, (int)Response.StatusCode, ErrorAsJson);
            }
            catch
            {
                throw new Exception(Response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
