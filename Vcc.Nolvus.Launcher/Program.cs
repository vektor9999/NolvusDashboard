using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Vcc.Nolvus.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Process SKSEProcess = new Process();

            SKSEProcess.StartInfo.WorkingDirectory = args[0];            
            SKSEProcess.StartInfo.FileName =  Path.Combine(args[0], "skse64_loader.exe");
            SKSEProcess.StartInfo.CreateNoWindow = true;
            SKSEProcess.StartInfo.UseShellExecute = false;
            SKSEProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            SKSEProcess.Start();
        }
    }
}
