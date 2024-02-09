using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Vcc.Nolvus.Api.Installer.Core;
using Vcc.Nolvus.Api.Installer.Controllers;


namespace Vcc.Nolvus.Api.Installer.Services
{
    public class ApiService : IApiService
    {
        private readonly ITokenService _TokenService;
        private readonly HttpClient Client = new HttpClient();
        private Dictionary<Type, object> _Services = new Dictionary<Type, object>();
        private string _Version;

        #region Controllers
               
        public IInstallerController Installer
        {
            get
            {
                return GetController<IInstallerController>();
            }
        }

        #endregion

        public ApiService(ITokenService TokenService, string Version)
        {
            _TokenService = TokenService;
            _Version = Version;
            
            RegisterController<IInstallerController>(new InstallerController(this, string.Format("api/{0}/installer", Version)));


            Client.BaseAddress = new Uri(_TokenService.Url);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
        }

        public ApiService(string Url, string Version)
        {
            _Version = Version;

            RegisterController<IInstallerController>(new InstallerController(this, string.Format("api/{0}/installer", Version)));

            Client.BaseAddress = new Uri(Url);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
        }
        
        public void RegisterController<T>(T Controller) where T : class
        {            
            _Services[typeof(T)] = Controller;
        }

        public T GetController<T>() where T : class
        {
            object Result = null;
            _Services.TryGetValue(typeof(T), out Result);
            return (T)Result;
        }
        
        public async Task<T> GetUnRestricted<T>(string ApiMethod, Dictionary<string, object> Parameters)
        {
            T Result = default(T);

            string Params = ParametersBuilder.GetQueryStringFromParameters(Parameters);

            var Response = await Client.GetAsync(ApiMethod + Params);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T> GetUnRestricted<T>(string ApiMethod)
        {
            T Result = default(T);          

            var Response = await Client.GetAsync(ApiMethod);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T> Get<T>(string ApiMethod)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();
           
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var Response = await Client.GetAsync(ApiMethod);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }
            

            return Result;
        }

        public async Task<T> GetPolyMorphic<T>(string ApiMethod)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var Response = await Client.GetAsync(ApiMethod);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T> Get<T>(string ApiMethod, string Id)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();
            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var Response = await Client.GetAsync(ApiMethod + "/" + Id);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }
            

            return Result;
        }

        public async Task<T> GetPolyMorphic<T>(string ApiMethod, string Id)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var Response = await Client.GetAsync(ApiMethod + "/" + Id);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T> Get<T>(string ApiMethod, Dictionary<string, object> Parameters)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();
                                        
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string Params = ParametersBuilder.GetQueryStringFromParameters(Parameters);

            var Response = await Client.GetAsync(ApiMethod + Params);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }                            

            return Result;
        }

        public async Task<T> GetPolyMorphic<T>(string ApiMethod, Dictionary<string, object> Parameters)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string Params = ParametersBuilder.GetQueryStringFromParameters(Parameters);

            var Response = await Client.GetAsync(ApiMethod + Params);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }

            return Result;
        }

        public async Task<T> Post<T>(string ApiMethod, T Object)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();
           
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            Response = await Client.PostAsJsonAsync(ApiMethod, Object);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }
            
            return Result;
        }

        public async Task<T> PostPolyMorphic<T>(string ApiMethod, T Object)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            string jsonTypeNameAll = JsonConvert.SerializeObject(Object, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Response = await Client.PostAsJsonAsync(ApiMethod, jsonTypeNameAll);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }

            return Result;
        }

        public async Task<T> Post<T>(string ApiMethod, Dictionary<string, object> Object)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            Response = await Client.PostAsJsonAsync(ApiMethod, Object);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T> PostPolyMorphic<T>(string ApiMethod, Dictionary<string, object> Object)
        {
            T Result = default(T);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            string jsonTypeNameAll = JsonConvert.SerializeObject(Object, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Response = await Client.PostAsJsonAsync(ApiMethod, Object);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }

        public async Task<T2> Post<T1, T2>(string ApiMethod, T1 Object)
        {
            T2 Result = default(T2);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            Response = await Client.PostAsJsonAsync(ApiMethod, Object);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T2>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }

            return Result;
        }

        public async Task<T2> PostPolyMorphic<T1, T2>(string ApiMethod, T1 Object, Dictionary<string, object> Parameters)
        {
            T2 Result = default(T2);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            string jsonTypeNameAll = JsonConvert.SerializeObject(Object, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            string Params = ParametersBuilder.GetQueryStringFromParameters(Parameters);

            Response = await Client.PostAsJsonAsync(ApiMethod + Params, jsonTypeNameAll);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T2>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }
        
        public async Task<T2> PostPolyMorphic<T1, T2>(string ApiMethod, T1 Object)
        {
            T2 Result = default(T2);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            string jsonTypeNameAll = JsonConvert.SerializeObject(Object, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Response = await Client.PostAsJsonAsync(ApiMethod, jsonTypeNameAll);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T2>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }

            return Result;
        }

        public async Task<HttpResponseMessage> PostPolyMorphic(string ApiMethod, Dictionary<string, object> Parameters)
        {            
            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            Response = await Client.PostAsJsonAsync(ApiMethod, Parameters);

            if (!Response.IsSuccessStatusCode)
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }

            return Response;         
        }

        public async Task<T2> Put<T1, T2>(string ApiMethod, string Id, T1 Object)
        {
            T2 Result = default(T2);

            var token = await _TokenService.GetAuthenticationToken();

            
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            Response = await Client.PutAsJsonAsync(ApiMethod + "/" + Id, Object);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;
                Result = JsonConvert.DeserializeObject<T2>(json);
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }
           

            return Result;
        }

        public async Task<T2> PutPolyMorphic<T1, T2>(string ApiMethod, string Id, T1 Object)
        {
            T2 Result = default(T2);

            var token = await _TokenService.GetAuthenticationToken();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = new HttpResponseMessage();

            string jsonTypeNameAll = JsonConvert.SerializeObject(Object, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });            

            Response = await Client.PutAsJsonAsync(ApiMethod + "/" + Id, jsonTypeNameAll);

            if (Response.IsSuccessStatusCode)
            {
                var json = Response.Content.ReadAsStringAsync().Result;

                var formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
                };

                Result = await Response.Content.ReadAsAsync<T2>(
                    new List<MediaTypeFormatter> { formatter });
            }
            else
            {
                ExceptionHandler.ThrowWebApiException(Response);
            }


            return Result;
        }       
    }
}
