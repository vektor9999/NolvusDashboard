using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface INolvusSettingsManager
    {
        string InstallDir { get; set; }
        string ArchiveDir { get; set; }
        string StockGame { get; set; }
        string Variant { get; set; }
        string Height { get; set; }
        string Width { get; set; }
        string DownScaling { get; set; }
        string DownHeight { get; set; }
        string DownWidth { get; set; }
        string IniSettings { get; set; }
        bool EnableArchiving { get; set; }
        string CDN { get; set; }
        string LgCode { get; set; }
        string LgName { get; set; }                
        string GameDataDir { get; }
        string GameDataDirSl { get; }                       
        string DownScaledResolution { get; }
        string GetSelectedResolution();
        string GetSelectedHeight();
        string GetSelectedWidth();
    }
}
