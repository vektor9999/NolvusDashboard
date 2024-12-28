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
        Task<string> GetNexusManualDownloadLink(string ModName, string Link, string NexusModId);
        Task NexusSSOAuthentication(string Id, string Slug);
        void Navigate(string Link, string Title = null);
        void CloseBrowser();
        event OnBrowserClosedHandler OnBrowserClosed;
    }
}
