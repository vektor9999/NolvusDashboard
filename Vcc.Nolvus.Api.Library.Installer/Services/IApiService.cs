using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer.Controllers;

namespace Vcc.Nolvus.Api.Installer.Services
{
    public interface IApiService
    {
        Task<T> GetUnRestricted<T>(string ApiMethod);
        Task<T> GetUnRestricted<T>(string ApiMethod, Dictionary<string, object> Parameters);        
        Task<T> Get<T>(string ApiMethod);
        Task<T> GetPolyMorphic<T>(string ApiMethod);
        Task<T> Get<T>(string ApiMethod, string Id);
        Task<T> GetPolyMorphic<T>(string ApiMethod, string Id);
        Task<T> Get<T>(string ApiMethod, Dictionary<string, object> Parameters);
        Task<T> GetPolyMorphic<T>(string ApiMethod, Dictionary<string, object> Parameters);
        Task<T> Post<T>(string ApiMethod, T Object);
        Task<T> PostPolyMorphic<T>(string ApiMethod, T Object);
        Task<T> Post<T>(string ApiMethod, Dictionary<string, object> Object);
        Task<T> PostPolyMorphic<T>(string ApiMethod, Dictionary<string, object> Object);        
        Task<HttpResponseMessage> PostPolyMorphic(string ApiMethod, Dictionary<string, object> Parameters);
        Task<T2> Post<T1, T2>(string ApiMethod, T1 Object);
        Task<T2> PostPolyMorphic<T1, T2>(string ApiMethod, T1 Object);
        Task<T2> PostPolyMorphic<T1, T2>(string ApiMethod, T1 Object, Dictionary<string, object> Parameters);
        Task<T2> Put<T1, T2>(string ApiMethod, string Id, T1 Object);
        Task<T2> PutPolyMorphic<T1, T2>(string ApiMethod, string Id, T1 Object);             
        IInstallerController Installer { get; }
    }
}
