using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Vcc.Nolvus.Api.Installer.Token;

namespace Vcc.Nolvus.Api.Installer.Services
{
    public class TokenService : ITokenService
    {
        private AuthenticationToken Token = new AuthenticationToken();
        private readonly HttpClient Client = new HttpClient();

        private string _BaseUrl;
        private string _UserName;
        private string _Password;

        public string Url
        {
            get { return $"{_BaseUrl}"; }
        }

        public TokenService(string BaseUrl, string UserName, string Password)
        {
            _BaseUrl = BaseUrl;
            _UserName = UserName;
            _Password = Password;

            Client.BaseAddress = new Uri(Url);

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAuthenticationToken()
        {
            if (!Token.IsValidAndNotExpiring)
            {
                Token = await GetNewAuthenticationToken();
            }

            return Token.AccessToken;
        }

        public async Task<bool> Authenticate()
        {
            bool Result = true;

            try
            {
                await GetNewAuthenticationToken();
            }
            catch
            {
                Result = false;
            }

            return Result;
        }

        private async Task<AuthenticationToken> GetNewAuthenticationToken()
        {
            var Token = new AuthenticationToken();           
            
            HttpResponseMessage Response = new HttpResponseMessage();                          

            var RequestBody = new Dictionary<string, string>{
                {"grant_type", "password"},
                {"username", _UserName},
                {"password", _Password},
            };
                
            HttpContent RequestParams = new FormUrlEncodedContent(RequestBody);

            Response =  await Client.PostAsync("Token", RequestParams);

            var JsonResult = Response.Content.ReadAsStringAsync().Result;

            Token = JsonConvert.DeserializeObject<AuthenticationToken>(JsonResult);

            if (Response.IsSuccessStatusCode)
            {                                   
                Token.ExpiresAt = DateTime.UtcNow.AddSeconds(Token.ExpiresIn);                    
            }
            else
            {               
                throw new ApplicationException(Token.Error);
            }
            

            return Token;
        }

    }
}
