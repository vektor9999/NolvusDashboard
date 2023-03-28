using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Services
{
    public static class ApiManager
    {
        private static IApiService _Api;

        public static void Init(string Url, string Version, string UserName, string Password)
        {
            ApiManager._Api = new ApiService(new TokenService(Url, UserName, Password), Version);            
        }

        public static void Init(string Url, string Version)
        {
            ApiManager._Api = new ApiService(Url, Version);
        }

        public static IApiService Service
        {
            get
            {
                return _Api;
            }
        }
    }
}
