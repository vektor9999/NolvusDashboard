using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.Themes;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Services.Game;
using Vcc.Nolvus.Services.Files;
using Vcc.Nolvus.Services.Logger;

namespace Vcc.Nolvus.Downgrader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM2MTU4QDMxMzgyZTMzMmUzMFBiTkxiV0dEMEhlWnowK3IxVUFsYkdGM2VnR0d6RDVBdGNYOEVFK2VqNVk9");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SfSkinManager.LoadAssembly(typeof(Office2016Theme).Assembly);
            SfSkinManager.LoadAssembly(typeof(Office2019Theme).Assembly);

            ServiceSingleton.RegisterService<IGameService>(new GameService());
            ServiceSingleton.RegisterService<IFileService>(new FileService());
            ServiceSingleton.RegisterService<ILogService>(new LogService());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}

