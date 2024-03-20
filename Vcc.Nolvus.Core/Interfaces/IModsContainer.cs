using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Misc;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IModsContainer
    {
        Task<List<ModObject>> GetModsMetaData(Action<string, int> Progress = null);
    }
}
