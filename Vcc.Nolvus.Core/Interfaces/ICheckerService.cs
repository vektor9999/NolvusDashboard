using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Misc;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface ICheckerService : INolvusService
    {
        Task<ModObjectList> CheckModList(List<ModObject> Mo2List, List<ModObject> NolvusList, Action<string> Status);
    }
}
