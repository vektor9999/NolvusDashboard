using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer.Services;

namespace Vcc.Nolvus.Api.Installer.Controllers
{
    public class BaseController
    {
        protected IApiService _Service;
        protected string _Api;

        public BaseController(IApiService Service, string Api)
        {
            this._Service = Service;
            this._Api = Api;
        }
    }
}
