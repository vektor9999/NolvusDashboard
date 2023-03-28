using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public class MegaAuthentication
    {
        private string _Message;
        private bool _AuthenticationSucceeded;

        public string Message
        {
            get
            {
                return _Message;
            }
        }

        public bool AuthenticationResult
        {
            get
            {
                return _AuthenticationSucceeded;
            }
        }

        public MegaAuthentication(string Message, bool Result)
        {
            _Message = Message;
            _AuthenticationSucceeded = Result;
        }
    }
}
