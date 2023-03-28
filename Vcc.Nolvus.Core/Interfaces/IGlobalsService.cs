using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IGlobalsService : INolvusService
    {        
        string ApiUrl { get; }
        string ApiVersion { get; }
        string NolvusUserName { get; }
        string NolvusPassword { get; }
        string NexusApiKey { get; }
        string NexusUserAgent { get; }
        List<string> WindowsResolutions { get; }
    }
}
