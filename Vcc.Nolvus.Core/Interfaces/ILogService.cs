using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface ILogService : INolvusService
    {
        void Log(string Message);
        void LineBreak();
    }
}
