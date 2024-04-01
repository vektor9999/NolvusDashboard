using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IProgressQueue
    {
        void Add(ModProgress ModProgress);
        void Remove(ModProgress ModProgress);
        void Clear();
        List<ModProgress> GetList();
        double Sum();
    }
}
