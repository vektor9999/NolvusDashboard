using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IENBService : INolvusService
    {
        bool EnbPresetsNeedUpdate { get; }
        Task<List<IENBPreset>> GetEnbPresets();
        Task<List<IMod>> PrepareModsToUpdate(string CurrentENB, string NewENB);
        IENBPreset CurrentPreset(string Preset);
        Task DeleteENB(Action<string, int> Progress = null);
    }
}
