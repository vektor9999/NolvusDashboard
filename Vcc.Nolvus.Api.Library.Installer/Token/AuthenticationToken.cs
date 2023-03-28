using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vcc.Nolvus.Api.Installer.Token
{
    public class AuthenticationToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string Error { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring
        {
            get
            {
                if (!String.IsNullOrEmpty(this.AccessToken))
                {
                    DateTime Dt = DateTime.UtcNow.AddSeconds(30);
                    if ( this.ExpiresAt > Dt)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
                //return !String.IsNullOrEmpty(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
            }
        }
    }
}
