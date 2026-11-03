using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;


namespace Vcc.Nolvus.Services.Logger
{
    public class LogService : ILogService
    {        
        private static readonly object SyncRoot = new object();
        private static string Filename = string.Empty;

        public void Log(string Message)
        {
            lock (SyncRoot)
            {
                File.AppendAllText(Path.Combine(ServiceSingleton.Folders.LogsDirectory, Filename), Environment.NewLine + Message);
            }
        }

        public void LineBreak()
        {
            Log(Environment.NewLine);
        }

        public void InitializeLog()
        {            
            Filename = string.Format("Log-{0}.txt", DateTime.Now.ToString("yyyydd-M-HHmmss"));
        }

        public void ClearLogs()
        {
            var LogsToDelete = ServiceSingleton.Files.GetFiles(ServiceSingleton.Folders.LogsDirectory).Where(x => x.Name != Filename).ToList();

            foreach(var LogFile in LogsToDelete)
            {
                LogFile.Delete();
            }                   
        }
    }
}
