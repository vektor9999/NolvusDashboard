using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstallCondition
    {
        int Operator { get; set; }
        bool IsValid();
        bool IsValid(string Value);
    }
}
