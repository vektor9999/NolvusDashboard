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
            if (args.Length > 0)
            {
                Process SKSEProcess = new Process();

                SKSEProcess.StartInfo.WorkingDirectory = args[0];
                SKSEProcess.StartInfo.FileName = Path.Combine(args[0], "skse64_loader.exe");
                SKSEProcess.StartInfo.CreateNoWindow = true;
                SKSEProcess.StartInfo.UseShellExecute = false;
                SKSEProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                SKSEProcess.Start();
            }
            else
            {
                Console.WriteLine("Nolvus can only be launched through Mod Organizer 2. Execute NolvusDashboard.exe, click on play. When Mod Organizer 2 has started, be sure Nolvus is selected in the right drop down list and click on Run.");
            }
            
        }
    }
}
