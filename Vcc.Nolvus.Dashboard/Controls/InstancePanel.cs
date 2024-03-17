using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Api.Installer.Services;

namespace Vcc.Nolvus.Dashboard.Controls
{
    public partial class InstancePanel : UserControl
    {
        INolvusInstance _Instance;
        InstancesPanel _Panel;        

        public InstancePanel(InstancesPanel Panel)
        {
            InitializeComponent();
            _Panel = Panel;
        }

        private void LockButtons()
        {
            BtnPlay.Enabled = false;
            BtnUpdate.Enabled = false;
            BtnView.Enabled = false;
            (_Panel.ContainerFrame as InstancesFrame).LockButtons();
        }

        private void UnLockButtons()
        {
            BtnPlay.Enabled = true;
            BtnUpdate.Enabled = true;
            BtnView.Enabled = true;
            (_Panel.ContainerFrame as InstancesFrame).UnLockButtons();
        }

        private void InstancePane_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
             Color.Gray, 1, ButtonBorderStyle.Solid, // left
             Color.Gray, 1, ButtonBorderStyle.Solid, // top
             Color.Gray, 1, ButtonBorderStyle.Solid, // right
             Color.Gray, 1, ButtonBorderStyle.Solid);// bottom
        }

        private void SetPlayText(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetPlayText, Text);
                return;
            }

            this.BtnPlay.Text = Text;
            this.BtnPlay.Enabled = true;
        }
        public async void LoadInstance(INolvusInstance Instance)
        {
            _Instance = Instance;            

            LblInstanceName.Text = _Instance.Name;
            LblVersion.Text = _Instance.Version;
            LblDesc.Text = _Instance.Description;

            LblBeta.Text = string.Empty;

            if (await _Instance.IsBeta())
            {
                LblBeta.Text = "(Beta)";
            }

            LblStatus.Text = await _Instance.GetState();

            if (_Instance.Name == "Nolvus Ascension")
            {
                PicInstanceImage.Image = Properties.Resources.Nolvus_V5;
            }            

            if (LblStatus.Text == "Installed")
            {
                LblStatus.ForeColor = Color.Orange;
            }
            else if (LblStatus.Text.Contains("New version available"))
            {
                LblStatus.ForeColor = Color.Orange;
                BtnUpdate.Visible = true;
            }
        }
        private void BtnPlay_Click(object sender, EventArgs e)
        {            

            if (!ModOrganizer.IsRunning)
            {
                Process MO2 = ModOrganizer.Start(_Instance.InstallDir);

                this.BtnPlay.Text = "Running...";
                this.BtnPlay.Enabled = false;

                Task.Run(() =>
                {
                    MO2.WaitForExit();

                    if (MO2.ExitCode == 0)
                    {
                        this.SetPlayText("Play");
                    }
                });
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is already running!", MessageBoxType.Error);
            }                       
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {            
            if (!ModOrganizer.IsRunning)
            {                
                ServiceSingleton.Dashboard.LoadFrame<ChangeLogFrame>(new FrameParameters(new FrameParameter() {Key = "Instance", Value =_Instance }));
            }
            else
            {
                NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is running! Please close it before updating.", MessageBoxType.Error);
            }
        }
        private void BtnView_Click(object sender, EventArgs e)
        {            
            popupMenu1.Show(BtnView, new Point(0, BtnView.Height));
        }
        private async void BrItmMods_Click(object sender, EventArgs e)
        {            
            ServiceSingleton.Instances.WorkingInstance = _Instance;
            await ServiceSingleton.Dashboard.LoadFrameAsync<PackageFrame>();
        }
        private async void BrItmReport_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Instances.WorkingInstance = _Instance;
            IDashboard Dashboard = ServiceSingleton.Dashboard;

            LockButtons();

            await ServiceSingleton.Packages.Load(await ApiManager.Service.Installer.GetPackage(_Instance.Id, _Instance.Version), (s, p) =>
            {
                Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                Dashboard.Progress(p);
            })
            .ContinueWith(async T=> {
                if (T.IsFaulted)
                {
                    UnLockButtons();
                    NolvusMessageBox.ShowMessage("Error during package loading", T.Exception.InnerException.Message, MessageBoxType.Error);
                }
                else
                {
                    try
                    {
                        try
                        {
                            await ServiceSingleton.Report.GenerateReportToPdf(await ServiceSingleton.CheckerService.CheckModList(
                                await ServiceSingleton.Packages.ModOrganizer2.GetMods((s, p) =>
                                {
                                    Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                    Dashboard.Progress(p);
                                }),
                                await ServiceSingleton.Packages.GetMods((s, p) =>
                                {
                                    Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                    Dashboard.Progress(p);
                                }),
                                (s) =>
                                {
                                    Dashboard.Status(s);
                                }
                            ), Properties.Resources.background_nolvus,
                            (s, p) =>
                            {
                                Dashboard.Status(string.Format("{0} ({1}%)", s, p));
                                Dashboard.Progress(p);
                            });

                            Dashboard.NoStatus();
                            Dashboard.ProgressCompleted();

                            NolvusMessageBox.ShowMessage("Information", string.Format("PDF report has been generated in {0}", ServiceSingleton.Folders.ReportDirectory), MessageBoxType.Info);
                        }
                        catch (Exception ex)
                        {
                            Dashboard.NoStatus();
                            Dashboard.ProgressCompleted();


                            NolvusMessageBox.ShowMessage("Error during report generation", ex.Message, MessageBoxType.Error);
                        }
                    }
                    finally
                    {
                        UnLockButtons();
                        ServiceSingleton.Instances.UnloadWorkingIntance();
                    }
                }
                
            }, TaskScheduler.FromCurrentSynchronizationContext());                                    
        }
        private void BrItmDelete_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<DeleteFrame>(new FrameParameters(new FrameParameter() { Key = "Instance", Value = _Instance as INolvusInstance }, new FrameParameter() { Key = "Action", Value = InstanceAction.Delete }));
        }
        private void BrItmShortCut_Click(object sender, EventArgs e)
        {
            try
            {
                IShellLink Link = (IShellLink)new ShellLink();

                Link.SetDescription(string.Format("Desktop shortcut for your {0} instance.", _Instance.Name));
                Link.SetPath(Path.Combine(_Instance.InstallDir, "MO2", "ModOrganizer.exe"));

                var IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nolvus-ico.ico");

                Link.SetIconLocation(IconPath, 0);

                IPersistFile File = (IPersistFile)Link;
                string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                File.Save(Path.Combine(DesktopPath, _Instance.Name + ".lnk"), false);

                NolvusMessageBox.ShowMessage("Desktop shortcut", string.Format("Your {0} shortcut has been added to your desktop.", _Instance.Name), MessageBoxType.Info);
            }
            catch(Exception ex)
            {
                NolvusMessageBox.ShowMessage("Error", ex.Message, MessageBoxType.Error);
            }
        }

       
    }
}
