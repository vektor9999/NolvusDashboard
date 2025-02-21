﻿using System;
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
        string NolvusUserAgent { get; }
        List<string> WindowsResolutions { get; }
        List<string> GetVideoAdapters();
        Task<string> GetCPUInfo();
        Task<string> GetRamCount();
        string GetVersion(string FilePath);
        bool MegaAnonymousConnection { get; set; }
        string MegaEmail { get; set; }
        string MegaPassword { get; set; }
    }
}
