using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IBrowserInstance
    {
        Task AwaitUserDownload(string Link, string FileName, DownloadProgressChangedHandler Progress);
        Task<string> GetNexusManualDownloadLink(string Link, string NexusModId);
    }
}
