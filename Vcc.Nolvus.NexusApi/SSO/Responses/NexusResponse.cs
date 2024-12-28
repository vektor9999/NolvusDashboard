using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vcc.Nolvus.NexusApi.SSO.Responses
{
    public class NexusSSOData 
    {
        [JsonProperty(PropertyName = "connection_token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "api_key")]
        public string ApiKey { get; set; }
    }

    public class NexusSSOResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public NexusSSOData Data { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
