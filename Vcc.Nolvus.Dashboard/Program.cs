using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.Themes;
using System.Reflection;
using System.Runtime.CompilerServices;
using CefSharp;
using CefSharp.WinForms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Services.Globals;
using Vcc.Nolvus.Services.Settings;
using Vcc.Nolvus.Services.Folders;
using Vcc.Nolvus.Services.Updater;
using Vcc.Nolvus.Services.Lib;
using Vcc.Nolvus.Services.Logger;
using Vcc.Nolvus.Services.Game;
using Vcc.Nolvus.Services.Files;
using Vcc.Nolvus.Services.Report;
using Vcc.Nolvus.Services.Checker;
using Vcc.Nolvus.Instance.Services;
using Vcc.Nolvus.Package.Services;
using Vcc.Nolvus.Dashboard.Forms;

namespace Vcc.Nolvus.Dashboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            ServiceSingleton.RegisterService<ILogService>(new LogService());
            ServiceSingleton.Logger.LineBreak();
            ServiceSingleton.Logger.Log("***Nolvus Dashboard Initialization***");
            ServiceSingleton.Logger.Log("Starting new session : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            ServiceSingleton.Logger.Log("Architecture : " + (Environment.Is64BitProcess ? "x64" : "x86"));

            var PackageService = new PackageService();

            ServiceSingleton.RegisterService<IGlobalsService>(new GlobalsService());
            ServiceSingleton.RegisterService<ISettingsService>(new SettingsService());
            ServiceSingleton.RegisterService<IFolderService>(new FolderService());            
            ServiceSingleton.RegisterService<IInstanceService>(new InstanceService());
            ServiceSingleton.RegisterService<IUpdaterService>(new UpdaterService());
            ServiceSingleton.RegisterService<IPackageService>(PackageService);            
            ServiceSingleton.RegisterService<ILibService>(new LibService());
            ServiceSingleton.RegisterService<IGameService>(new GameService());
            ServiceSingleton.RegisterService<IFileService>(new FileService());
            ServiceSingleton.RegisterService<ISoftwareProvider>(PackageService);
            ServiceSingleton.RegisterService<IReportService>(new ReportService());
            ServiceSingleton.RegisterService<ICheckerService>(new CheckerService());            

            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            AppDomain.CurrentDomain.AssemblyLoad += Loader;
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM2MTU4QDMxMzgyZTMzMmUzMFBiTkxiV0dEMEhlWnowK3IxVUFsYkdGM2VnR0d6RDVBdGNYOEVFK2VqNVk9");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;            

            SfSkinManager.LoadAssembly(typeof(Office2016Theme).Assembly);
            SfSkinManager.LoadAssembly(typeof(Office2019Theme).Assembly);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process[] DashBoardProcesses = Process.GetProcessesByName("NolvusDashBoard");

            if (DashBoardProcesses.Length > 1)
            {
                NolvusMessageBox.ShowMessage("Error", "An other Nolvus Dashboard instance is already running, please close it first. If you don't see it, open your Windows task manager and close the process (NolvusDashboard.exe)", Nolvus.Core.Enums.MessageBoxType.Error);                
            }
            else
            {
                LoadApp();
            }            
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            ServiceSingleton.Logger.Log(ex.Message + Environment.NewLine + "Stack =>" + ex.StackTrace);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadApp()
        {                                                         
            ServicePointManager.DefaultConnectionLimit = 100;

            var CefSettings = new CefSettings();

            CefSettings.CachePath = ServiceSingleton.Folders.WebCacheDirectory;
            CefSettings.LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CefSharpLog.txt");

            LogSeverity LogSeverity;

            Enum.TryParse<LogSeverity>(ServiceSingleton.Settings.BrowserLogSeverity, out LogSeverity);

            CefSettings.LogSeverity = LogSeverity;

            CefSettings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   System.Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");
            
            Cef.Initialize(CefSettings, performDependencyCheck: false, browserProcessHandler: null);

            
            Application.Run(new DashboardWindow());            
        }

        private static void Loader(object sender, AssemblyLoadEventArgs args)
        {
            ServiceSingleton.Logger.Log("Assembly loader : ==> " + args.LoadedAssembly.FullName);
        }

        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       System.Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                if (!File.Exists(archSpecificPath))
                {
                    ServiceSingleton.Logger.Log("Assembly loader : Unable to load assembly ==> " + args.Name);
                    return null;
                }

                
                return Assembly.LoadFile(archSpecificPath);
            }

            ServiceSingleton.Logger.Log("Assembly loader : Unable to load assembly ==> " + args.Name);
            return null;
        }
    }
}
