using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IMod : IMOElement
    {
        ICategory Category { get; }
        bool HasTag(string Tag);
        string GetFieldValueByKey(string Key);
        List<IEnvironmentCondition> Conditions { get; }
    }
}
