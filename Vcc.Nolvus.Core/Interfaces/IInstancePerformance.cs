using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstancePerformance
    {
        string DownScaling { get; set; }
        string DownHeight { get; set; }
        string DownWidth { get; set; }       
        string AdvancedPhysics { get; set; }        
        string IniSettings { get; set; }
        string AntiAliasing { get; set; }
        string Variant { get; set; }
        string LODs { get; set; }
        string RayTracing { get; set; }
        string DownScaledResolution { get; }       
    }
}
