using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.NexusApi.SSO
{
    public class ConnectionToken
    {
        public string connection_token { get; set; } = string.Empty;
    }

    public class NexusSSORequest
    {
        public string id { get; set; } = string.Empty;
        public ConnectionToken token { get; set; } = null;
        public int protocol { get; set; } = 2;

        public void SetToken(string Token)
        {
            if (token == null)
            {
                token = new ConnectionToken { connection_token = Token };
            }
            else
            {
                token.connection_token = Token;
            }
        }
    }
}
