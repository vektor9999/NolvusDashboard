using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Package.Mods;
using Syncfusion.Windows.Forms.Tools;
using ZetaLongPaths;

namespace Vcc.Nolvus.Dashboard.Frames.Remap.v6
{
    public partial class RemapInstanceFrame : DashboardFrame
    {
        public RemapInstanceFrame()
        {
            InitializeComponent();
        }

        public RemapInstanceFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)
        {
            InitializeComponent();

            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Remap Instance]");
            ServiceSingleton.Dashboard.Info("Choose a new path to move your instance to");
        }        

        protected override void OnLoaded()
        {
            LblInstance.Text  = string.Format("{0} - {1} v{2}{3}", ServiceSingleton.Instances.WorkingInstance.Name, ServiceSingleton.Instances.WorkingInstance.Performance.Variant, ServiceSingleton.Instances.WorkingInstance.Version, ServiceSingleton.Instances.WorkingInstance.Tag != string.Empty ? string.Format(" - ({0})", ServiceSingleton.Instances.WorkingInstance.Tag) : string.Empty);
            LblCurrentInstallPath.Text = ServiceSingleton.Instances.WorkingInstance.InstallDir;            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.UnloadWorkingIntance();
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private void CheckMO2Executables()
        {
            ServiceSingleton.Dashboard.Status("Checking Mod Organizer 2 shortcuts...");

            if (!ModOrganizer.CheckIfExecutableExists("Nolvus", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Nolvus shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("SKSE", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 SKSE shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("Skyrim Special Edition", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Skyrim Special Edition shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("Skyrim Special Edition Launcher", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Skyrim Special Edition Launcher shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("Explore Virtual Folder", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Explore Virtual Folder shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("Nemesis Unlimited Behavior Engine", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Nemesis Unlimited Behavior Engine shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("xEdit", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 xEdit shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("xEdit Cleaning", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 xEdit Cleaning shortcut doesn't exist!");
            }
            else if (!ModOrganizer.CheckIfExecutableExists("Body Slide", ServiceSingleton.Instances.WorkingInstance.InstallDir))
            {
                throw new Exception("The Mod Organizer 2 Body Slide shortcut doesn't exist!");
            }

            ServiceSingleton.Dashboard.Progress(100);
        } 
        
        private void ModifyMO2Executables(string NewInstallPath, string StockGamePath)
        {
            ServiceSingleton.Dashboard.Status("Modifying Mord Organizer 2 executables...");

            ModOrganizer.ModifyExecutable("Nolvus", NewInstallPath, Path.Combine(NewInstallPath, "MO2", "NolvusLauncher.exe").Replace(@"\", @"/"), @"\""" + StockGamePath.Replace(@"\", @"\\") + @"\""", Path.Combine(NewInstallPath, "MO2").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(10);
            ModOrganizer.ModifyExecutable("SKSE", NewInstallPath, Path.Combine(StockGamePath, "skse64_loader.exe").Replace(@"\", @"/"), string.Empty, StockGamePath.Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(20);
            ModOrganizer.ModifyExecutable("Skyrim Special Edition", NewInstallPath, Path.Combine(StockGamePath, "SkyrimSE.exe").Replace(@"\", @"/"), string.Empty, StockGamePath.Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(30);
            ModOrganizer.ModifyExecutable("Skyrim Special Edition Launcher", NewInstallPath, Path.Combine(StockGamePath, "SkyrimSELauncher.exe").Replace(@"\", @"/"), string.Empty, StockGamePath.Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(40);
            ModOrganizer.ModifyExecutable("Explore Virtual Folder", NewInstallPath, Path.Combine(NewInstallPath, "MO2", "explorer++", "Explorer++.exe").Replace(@"\", @"/"), @"\""" + StockGamePath.Replace(@"\", @"\\") + "\\\\Data\\\"", Path.Combine(NewInstallPath, "MO2", "explorer++").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(50);
            ModOrganizer.ModifyExecutable("Nemesis Unlimited Behavior Engine", NewInstallPath, Path.Combine(NewInstallPath, "MODS", "mods", "Nemesis Unlimited Behavior Engine", "Nemesis_Engine", "Nemesis Unlimited Behavior Engine.exe").Replace(@"\", @"/"), string.Empty, Path.Combine(NewInstallPath, "MODS", "mods", "Nemesis Unlimited Behavior Engine", "Nemesis_Engine").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(60);

            string Args = "-D:\\\"" + Path.Combine(StockGamePath, "Data").Replace("\\", "\\\\") + "\\\"" + " -c:\\\"" + NewInstallPath.Replace("\\", "\\\\") + "\\\\TOOLS\\\\SSE Edit\\\\Cache\\\\\\\"";

            ModOrganizer.ModifyExecutable("xEdit", NewInstallPath, Path.Combine(NewInstallPath + "\\TOOLS\\SSE Edit", "SSEEdit.exe").Replace(@"\", @"/"), Args, Path.Combine(NewInstallPath + "\\TOOLS\\SSE Edit").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(70);

            string ArgsAutoClean = "-DontCache -D:\\\"" + Path.Combine(StockGamePath, "Data").Replace("\\", "\\\\") + "\\\"";

            ModOrganizer.ModifyExecutable("xEdit Cleaning", NewInstallPath, Path.Combine(NewInstallPath + "\\TOOLS\\SSE Edit", "SSEEditQuickAutoClean.exe").Replace(@"\", @"/"), ArgsAutoClean, Path.Combine(NewInstallPath + "\\TOOLS\\SSE Edit").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(80);

            ModOrganizer.ModifyExecutable("Body Slide", NewInstallPath, Path.Combine(NewInstallPath, "MODS", "mods", "BodySlide and Outfit Studio", "CalienteTools", "BodySlide", "BodySlide x64.exe").Replace(@"\", @"/"), string.Empty, Path.Combine(NewInstallPath, "MODS", "mods", "BodySlide and Outfit Studio", "CalienteTools", "BodySlide").Replace(@"\", @"/"));
            ServiceSingleton.Dashboard.Progress(90);

            ServiceSingleton.Settings.StoreIniValue(Path.Combine(NewInstallPath, "MO2", "ModOrganizer.ini"), "General", "gamePath", string.Format("@ByteArray({0})", StockGamePath.Replace(@"\", @"\\")));
            ServiceSingleton.Dashboard.Progress(95);

            ServiceSingleton.Settings.StoreIniValue(Path.Combine(NewInstallPath, "MO2", "ModOrganizer.ini"), "Settings", "base_directory", (NewInstallPath + "\\MODS").Replace("\\", "/"));
            ServiceSingleton.Dashboard.Progress(100);
        }       

        private async Task Remap()
        {
            var Tsk = Task.Run(() =>
            {                                                                               
                CheckMO2Executables();

                var CurrentInstallPath = ServiceSingleton.Instances.WorkingInstance.InstallDir;
                var NewInstallPath = TxtBxInstancePath.Text;
                var StockGamePath = ZlpPathHelper.Combine(NewInstallPath, "STOCK GAME");

                var Files = ServiceSingleton.Files.GetFiles(CurrentInstallPath);

                ZlpIOHelper.CreateDirectory(ZlpPathHelper.Combine(NewInstallPath, "MODS", "mods", "0. MASTER FILES_separator"));

                int Counter = 0;

                foreach (var File in Files)
                {
                    var FileCurrentFullName = File.Directory.GetFullPath().FullName.Replace(@"\\?\", string.Empty);
                    var FileCurrentDirectoryToCreate = FileCurrentFullName.Replace(CurrentInstallPath, string.Empty);

                    ServiceSingleton.Files.MoveFile(File.FullName, ZlpPathHelper.Combine(NewInstallPath, FileCurrentDirectoryToCreate, File.Name));

                    int PercentDone = System.Convert.ToInt16(((double)++Counter / Files.Count) * 100);

                    ServiceSingleton.Dashboard.Progress(PercentDone);
                    ServiceSingleton.Dashboard.Status(string.Format("Moving file {0}", File.Name));
                    ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Moving instance ({0}%)", PercentDone));                        
                }

                ModifyMO2Executables(NewInstallPath, StockGamePath);

                ServiceSingleton.Instances.WorkingInstance.InstallDir = NewInstallPath;
                ServiceSingleton.Instances.WorkingInstance.StockGame = StockGamePath;

                ServiceSingleton.Instances.Save();

                ServiceSingleton.Files.RemoveDirectory(CurrentInstallPath, true);
                                              
            });

            await Tsk;
        }

        private async void BtnRemap_Click(object sender, EventArgs e)
        {
            if (TxtBxInstancePath.Text.Trim() != string.Empty)
            {
                try
                {
                    try
                    {
                        DisableButtons();
                        ServiceSingleton.Dashboard.DisableSettings();

                        await Remap();                        

                        ServiceSingleton.Instances.UnloadWorkingIntance();
                        ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
                    }
                    finally
                    {
                        ServiceSingleton.Dashboard.EnableSettings();
                        ServiceSingleton.Dashboard.NoStatus();
                        ServiceSingleton.Dashboard.ProgressCompleted();
                        ServiceSingleton.Dashboard.ClearInfo();
                    }
                }
                catch (Exception ex)
                {
                    EnableButtons();

                    NolvusMessageBox.ShowMessage("Error", string.Format("Error occured with message : {0}", ex.Message), MessageBoxType.Error);
                }            
            }
            else
            {
                NolvusMessageBox.ShowMessage("Invalid Installation Directory.", "The new installation path must not be empty!", MessageBoxType.Error);
            }
            
        }

        private void BtnBrowseInstancePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (ServiceSingleton.Files.IsDirectoryEmpty(folderBrowserDialog1.SelectedPath))
                {
                    TxtBxInstancePath.Text = folderBrowserDialog1.SelectedPath;
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Invalid Installation Directory.", "The specified directory is not empty. Please select an other directory.", MessageBoxType.Error);
                }
            }
        }        
    }
}
