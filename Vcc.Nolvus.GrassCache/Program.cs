using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.Themes;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Services.Files;

namespace Vcc.Nolvus.GrassCache
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

            SfSkinManager.LoadAssembly(typeof(Office2016Theme).Assembly);
            SfSkinManager.LoadAssembly(typeof(Office2019Theme).Assembly);

            ServiceSingleton.RegisterService<IFileService>(new FileService());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
