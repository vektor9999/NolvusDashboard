using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.Themes;


namespace Vcc.Nolvus.Updater
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

            Application.EnableVisualStyles();

            SfSkinManager.LoadAssembly(typeof(Office2016Theme).Assembly);
            SfSkinManager.LoadAssembly(typeof(Office2019Theme).Assembly);


            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
