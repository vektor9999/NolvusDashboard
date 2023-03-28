using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstanceSettings
    {
        string Ratio { get; set; }
        string Height { get; set; }
        string Width { get; set; }
        bool EnableArchiving { get; set; }
        string CDN { get; set; }
        string LgCode { get; set; }
        string LgName { get; set; }
        string GameDataDir { get; }
    }
}
