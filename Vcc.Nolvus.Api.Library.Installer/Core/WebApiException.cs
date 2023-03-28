using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Core
{
    public class WebApiException : Exception
    {
        private int _Code;        
        private ApiException _ServerException;

        public int Code
        {
            get { return _Code; }
        }             

        public ApiException ServerException
        {
            get { return _ServerException; }
        }

        public WebApiException(string Message, int Code, ApiException ServerException) :
            base(Message)
        {
            _Code = Code;
            _ServerException = ServerException;            
        }
    }

    public class WebApiControlException : Exception
    {
        public WebApiControlException(string Message) : 
            base(Message)
        {

        }
    }
}
