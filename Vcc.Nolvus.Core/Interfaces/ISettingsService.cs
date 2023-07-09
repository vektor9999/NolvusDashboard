using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface ISettingsService : INolvusService
    {
        void StoreIniValue(string Section, string Parameter, string Value);
        void StoreIniValue(string File, string Section, string Parameter, string Value);
        string GetIniValue(string Section, string Parameter);        
        string GetIniValue(string File, string Section, string Parameter);
        int ProcessCount { get; }
        int RetryCount { get; }
        bool ForceAA { get; }
    }
}
