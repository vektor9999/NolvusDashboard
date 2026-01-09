using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstanceStatusField
    {
        string Key { get; set; }
        string Value { get; set; }
    }
}
