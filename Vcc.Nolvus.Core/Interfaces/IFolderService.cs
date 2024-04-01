using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IFolderService : INolvusService
    {
        string CacheDirectory { get; }       
        string DownloadDirectory { get; }
        string ExtractDirectory { get; }
        string LibDirectory { get; }
        string PatchDirectory { get; }
        string NexusCacheDirectory { get; }
        string WebCacheDirectory { get; }
        string ResourcesDirectory { get; }
        string InstancesDirectory { get; }
        string GameDirectory { get; }
        string ReportDirectory { get; }
        string UpdaterExe { get; }       
    }
}
