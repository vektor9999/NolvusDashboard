using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public class MegaException : Exception
    {
        public MegaException(string Message) 
            : base(Message)
        {

        }
    }
}
