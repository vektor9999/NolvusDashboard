using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Dashboard.Frames;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Installer;
using Vcc.Nolvus.Dashboard.Frames.Settings;


namespace Vcc.Nolvus.Dashboard
{
    public partial class DashboardWindow : SfForm, IDashboard
    {
        private int DefaultDpi = 96;
        private DashboardFrame LoadedFrame;
        private TitleBarControl TitleBarControl;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private PictureBox PicBox;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();      

        #region Events

        event OnFrameLoadedHandler OnFrameLoadedEvent;
        public event OnFrameLoadedHandler OnFrameLoaded
        {
            add
            {
                if (OnFrameLoadedEvent != null)
                {
                    lock (OnFrameLoadedEvent)
                    {
                        OnFrameLoadedEvent += value;
                    }
                }
                else
                {
                    OnFrameLoadedEvent = value;
                }
            }
            remove
            {
                if (OnFrameLoadedEvent != null)
                {
                    lock (OnFrameLoadedEvent)
                    {
                        OnFrameLoadedEvent -= value;
                    }
                }
            }
        }

        event OnFrameLoadedHandler OnFrameLoadedAsyncEvent;
        public event OnFrameLoadedHandler OnFrameLoadedAsync
        {
            add
            {
                if (OnFrameLoadedAsyncEvent != null)
                {
                    lock (OnFrameLoadedAsyncEvent)
                    {
                        OnFrameLoadedAsyncEvent += value;
                    }
                }
                else
                {
                    OnFrameLoadedAsyncEvent = value;
                }
            }
            remove
            {
                if (OnFrameLoadedAsyncEvent != null)
                {
                    lock (OnFrameLoadedAsyncEvent)
                    {
                        OnFrameLoadedAsyncEvent -= value;
                    }
                }
            }
        }

        #endregion

        #region Properties

        public double ScalingFactor
        {
            get
            {
                return CreateGraphics().DpiX / DefaultDpi;
            }
        }

        private string DashboardExe
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusDashboard.exe");
            }            
        }

        #endregion

        #region UI Methods

        private void ShowLoadingIndicator()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoadingIndicator);
                return;
            }

            PicBox = new PictureBox();
            PicBox.Image = Properties.Resources.cog_loader_alpha;
            PicBox.SizeMode = PictureBoxSizeMode.CenterImage;

            PicBox.Height = ContentPanel.Height;
            PicBox.Width = ContentPanel.Width;
            PicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                   | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));

            ContentPanel.Controls.Add(PicBox);            
        }

        private void UnloadLoadingIndicator()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)UnloadLoadingIndicator);
                return;
            }

            ContentPanel.Controls.Remove(PicBox);
            PicBox = null;            
        }

        private void AddFrame(DashboardFrame Frame)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<DashboardFrame>)AddFrame, Frame);
                return;
            }

            ContentPanel.Controls.Add(Frame);
            LoadedFrame = Frame;
        }

        private void DoLoad(DashboardFrame Frame)
        {
            Frame.Height = ContentPanel.Height;
            Frame.Width = ContentPanel.Width;
            Frame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                   | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));

            AddFrame(Frame);
        }

        private void RemoveLoadedFrame()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)RemoveLoadedFrame);
                return;
            }

            if (LoadedFrame != null)
            {                
                LoadedFrame.Close();
            }            
        }    
        
        public void SetMaximumProgress(int Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<int>)SetMaximumProgress, Value);
                return;
            }

            this.ProgressBar.Maximum = Value;
        }        

        public void Title(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)Title, Value);
                return;
            }


            this.TitleBarControl.Title = Value;
        }       

        #endregion

        #region Interface Implementation

        public bool IsOlder(string LatestVersion)
        {
            string[] v1List = LatestVersion.Split(new char[] { '.' });
            string[] v2List = Version.Split(new char[] { '.' });

            for (int i = 0; i < v1List.Length; i++)
            {
                int _v1 = System.Convert.ToInt16(v1List[i]);
                int _v2 = System.Convert.ToInt16(v2List[i]);

                if (_v1 > _v2)
                {
                    return true;
                }
                else if (_v1 < _v2)
                {
                    return false;
                }
            }

            return false;
        }        
        public string Version
        {
            get
            {                               
                return ServiceSingleton.Globals.GetVersion(DashboardExe);              
            }
        }        
        public void LoadAccountImage(string Url)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)LoadAccountImage, Url);
                return;
            }

            TitleBarControl.SetAccountImage(Url);
        }
        public void LoadAccountImage(System.Drawing.Image Image)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<System.Drawing.Image>)LoadAccountImage, Image);
                return;
            }

            TitleBarControl.SetAccountImage(Image);
        }
        public void Status(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)Status, Value);
                return;
            }

            this.LblStatus.Visible = true;
            this.LblStatus.Text = Value;
        }
        public void NoStatus()
        {
            Status(string.Empty);
        }
        public void Progress(int Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<int>)Progress, Value);
                return;
            }

            this.ProgressBar.Visible = true;
            this.ProgressBar.Value = Value;
        }
        public void ProgressCompleted()
        {
            Progress(0);
        }
        public void Info(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)Info, Value);
                return;
            }

            StStripLblInfo.Text = Value;
        }
        public void AdditionalInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)AdditionalInfo, Value);
                return;
            }
            StatusStripEx.Visible = true;
            StStripLblAdditionalInfo.Text = Value;
        }
        public void AdditionalSecondaryInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)AdditionalSecondaryInfo, Value);
                return;
            }
            StatusStripEx.Visible = true;
            StStripLblAdditionalInfo2.Text = Value;
        }
        public void AdditionalTertiaryInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)AdditionalTertiaryInfo, Value);
                return;
            }
            StatusStripEx.Visible = true;
            StStripLblAdditionalInfo3.Text = Value;
        }
        public void ClearInfo()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ClearInfo);
                return;
            }
            StStripLblInfo.Text = string.Empty;
            StStripLblAdditionalInfo.Text = string.Empty;
            StStripLblAdditionalInfo2.Text = string.Empty;
            StStripLblAdditionalInfo3.Text = string.Empty;
        }
        public void TitleInfo(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)TitleInfo, Value);
                return;
            }

            TitleBarControl.InfoCaption = "v" + ServiceSingleton.Dashboard.Version + " | " + Value;
        }
        public void NexusAccount(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)NexusAccount, Value);
                return;
            }

            StatusStripEx.Visible = true;
            StripLblNexus.Text = Value;
        }
        public void AccountType(string Value)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)AccountType, Value);
                return;
            }

            StatusStripEx.Visible = true;
            StripLblAccountType.Text = Value;
        }       
        public async Task<T> LoadFrameAsync<T>(FrameParameters Parameters = null) where T : DashboardFrame
        {            
            RemoveLoadedFrame();

            ShowLoadingIndicator();

            var Frame = await DashboardFrame.CreateAsync<T>(new object[] { this, Parameters });

            UnloadLoadingIndicator();

            DoLoad(Frame);

            OnFrameLoadedAsyncEvent(this, new EventArgs());

            return Frame;                        
        }
        public T LoadFrame<T>(FrameParameters Parameters = null) where T : DashboardFrame
        {            
            RemoveLoadedFrame();

            ShowLoadingIndicator();

            var Frame = DashboardFrame.Create<T>(new object[] { this, Parameters });

            UnloadLoadingIndicator();

            DoLoad(Frame);

            OnFrameLoadedEvent(this, new EventArgs());            

            return Frame;            
        }
        public async Task Error(string Title, string Message, string Trace = null, bool Retry = false)
        {
            UnloadLoadingIndicator();

            ServiceSingleton.Dashboard.NoStatus();
            ServiceSingleton.Dashboard.ProgressCompleted();
            ServiceSingleton.Logger.Log("Error Form => " + Message);

            await LoadFrameAsync<ErrorFrame>(new FrameParameters(FrameParameter.Create("Title", Title), FrameParameter.Create("Message", Message), FrameParameter.Create("Trace", Trace), FrameParameter.Create("Retry", Retry)));
        }
        public void ShutDown()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShutDown);
                return;
            }

            Close();
        }

        public void EnableSettings()
        {
            TitleBarControl.EnableSettings();
        }

        public void DisableSettings()
        {
            TitleBarControl.DisableSettings();
        }

        #endregion

        public DashboardWindow()
        {
            InitializeComponent();
            
            ServiceSingleton.RegisterService<IDashboard>(this);
            ServiceSingleton.Logger.Log(string.Format("Nolvus Dashboard Installer v{0} loaded", ServiceSingleton.Dashboard.Version));

            StStripLblInfo.Text = string.Empty;
            StStripLblAdditionalInfo.Text = string.Empty;
            StStripLblAdditionalInfo2.Text = string.Empty;
            StStripLblAdditionalInfo3.Text = string.Empty;
            StripLblAccountType.Text = string.Empty;
            StripLblNexus.Text = string.Empty;

            SkinManager.SetVisualStyle(this, "Office2016Black");
            Style.TitleBar.MaximizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MaximizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonPressedBackColor = Color.DarkOrange;

            Style.TitleBar.BackColor = Color.FromArgb(54, 54, 54);
            Style.TitleBar.IconBackColor = Color.FromArgb(54, 54, 54);
            Style.TitleBar.Height = 50;            

            Padding = new Padding(0, 50, 0, 0);
            Style.BackColor = Color.FromArgb(54, 54, 54);            

            TitleBarControl = new TitleBarControl();
            TitleBarControl.Width = 3000;
            TitleBarControl.MouseDown += TitleBarControl_MouseDown;
            TitleBarTextControl = TitleBarControl;
            TitleBarControl.OnSettingsClicked += TitleBarControl_OnSettingsClicked;                 

            TitleBarControl.Title = "Nolvus Dashboard";
            TitleBarControl.InfoCaption = string.Format("v{0} | Not logged", ServiceSingleton.Dashboard.Version);

            LoadAccountImage("https://www.nolvus.net/assets/images/account/user-profile.png");                      

            ProgressBar.Value = 0;
            ProgressBar.Maximum = 100;

            IconSize = new Size((int)Math.Round(IconSize.Width * ScalingFactor), (int)Math.Round(IconSize.Height * ScalingFactor));
            StripLblScaling.Text = "[DPI:" + this.ScalingFactor * 100 + "%" + "]";
        }

        private void TitleBarControl_OnSettingsClicked(object sender, EventArgs e)
        {
            if (!ServiceSingleton.Packages.Processing)
            {
                if (TitleBarControl.SettingsEnabled)
                {
                    ServiceSingleton.Dashboard.LoadFrame<GlobalSettingsFrame>();
                }
                else
                {
                    NolvusMessageBox.ShowMessage("Error", "This action can not be done now, please finish the Dashboard pre setup (Game path, Nexus and Nolvus connection)", Nolvus.Core.Enums.MessageBoxType.Error);
                }
                
            }
            else
            {
                NolvusMessageBox.ShowMessage("Settings", "This action is not allowed during mod list installation!", Nolvus.Core.Enums.MessageBoxType.Error);
            }                
        }

        private void TitleBarControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {                
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }        
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServiceSingleton.Packages.Processing)
            {
                e.Cancel = NolvusMessageBox.ShowConfirmation("Stop Installation", "Are you sure you want to stop the installation? You will be able to resume where it left off") == DialogResult.No;               
            }                        
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {            
        }

        private void DashboardWindow_Load(object sender, EventArgs e)
        {            
            ServiceSingleton.Dashboard.LoadFrameAsync<StartFrame>();
        }               
        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (TitleBarControl != null)
            {
                var h = Convert.ToInt32(Math.Round(50 * ScalingFactor, 0)); ;
                Style.TitleBar.Height = h;
                Padding = new Padding(0, h, 0, 0);
                TitleBarControl.Refresh();
                Application.DoEvents();
            }

            base.OnClientSizeChanged(e);
        }      
    }
}

