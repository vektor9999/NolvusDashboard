using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public class ExtractException : Exception
    {
        public ExtractException(string Message)
            :base(Message)
        {

        }
    }
}
