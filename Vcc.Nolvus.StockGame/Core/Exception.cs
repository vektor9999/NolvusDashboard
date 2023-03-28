using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.StockGame.Core
{
    public class GameFileIntegrityException : Exception
    {
        public GameFileIntegrityException(string Message):
            base(Message)
        {

        }
    }

    public class GameFileMissingException : Exception
    {
        public GameFileMissingException(string Message) :
            base(Message)
        {

        }
    }

    public class GameFilePatchingException : Exception
    {
        public string Reason { get; set; }
        public GameFilePatchingException(string Message, string Output) 
            : base(Message)
        {
            Reason = Output;
        }
    }
}
