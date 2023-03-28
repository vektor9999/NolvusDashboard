using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Services
{
    public interface ITokenService
    {
        Task<string> GetAuthenticationToken();

        Task<bool> Authenticate();

        string Url { get; }
    }
}
