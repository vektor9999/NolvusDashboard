using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Core.Errors
{
    public class FaultyMod
    {
        public IInstallableElement Mod { get; set; }
        public Exception Error { get; set; }
    }
}
