using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Services.Logger
{
    public class LogService : ILogService
    {        
        private static readonly object SyncRoot = new object();

        public void Log(string Message)
        {
            lock (SyncRoot)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", Environment.NewLine + Message);
            }
        }

        public void LineBreak()
        {
            Log(Environment.NewLine);
        }
    }
}
