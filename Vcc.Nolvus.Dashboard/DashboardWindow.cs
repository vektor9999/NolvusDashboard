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
using Vcc.Nolvus.Dashboard.Frames.Installer;


namespace Vcc.Nolvus.Dashboard
{
    public partial class DashboardWindow : SfForm, IDashboard
    {
        private DashboardFrame LoadedFrame;
        private TitleBarControl TitleBarControl;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

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

        #endregion

        #region UI Methods

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
               
        private void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            //this.PicBoxLoading.Show();
        }

        private void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShowLoading);
                return;
            }

            //this.PicBoxLoading.Hide();
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
                Assembly CurrentAssembly = Assembly.GetEntryAssembly();
                if (CurrentAssembly == null) CurrentAssembly = Assembly.GetCallingAssembly();
                System.Version VersionNumber = CurrentAssembly.GetName().Version;

                return string.Format("{0}.{1}.{2}", VersionNumber.Major, VersionNumber.Minor, VersionNumber.Build);
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
        private void DoLoad(DashboardFrame Frame)
        {
            Frame.Height = this.ContentPanel.Height;
            Frame.Width = this.ContentPanel.Width;
            Frame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                   | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));

            AddFrame(Frame);
        }
        public async Task<T> LoadFrameAsync<T>(FrameParameters Parameters = null) where T : DashboardFrame
        {
            //TitleBarControl.ShowLoading();

            RemoveLoadedFrame();

            var Frame = await DashboardFrame.CreateAsync<T>(new object[] { this, Parameters });

            DoLoad(Frame);

            OnFrameLoadedAsyncEvent(this, new EventArgs());

            //TitleBarControl.HideLoading();

            return Frame;
        }
        public T LoadFrame<T>(FrameParameters Parameters = null) where T : DashboardFrame
        {
            //TitleBarControl.ShowLoading();

            RemoveLoadedFrame();

            var Frame = DashboardFrame.Create<T>(new object[] { this, Parameters });

            DoLoad(Frame);

            OnFrameLoadedEvent(this, new EventArgs());

            //TitleBarControl.HideLoading();

            return Frame;            
        }

        public async Task Error(string Title, string Message, string Trace = null, bool Retry = false)
        {
            ServiceSingleton.Dashboard.NoStatus();
            ServiceSingleton.Dashboard.ProgressCompleted();
            await LoadFrameAsync<ErrorFrame>(new FrameParameters(FrameParameter.Create("Title", Title), FrameParameter.Create("Message", Message), FrameParameter.Create("Trace", Trace), FrameParameter.Create("Retry", Retry)));
        }

        public void ShutDown()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)ShutDown);
                return;
            }

            this.Close();
        }

        #endregion

        public DashboardWindow()
        {
            InitializeComponent();            
            ServiceSingleton.RegisterService<IDashboard>(this);

            ServiceSingleton.Logger.LineBreak();
            ServiceSingleton.Logger.Log("***Nolvus Dashboard Installer v" + ServiceSingleton.Dashboard.Version + "***");
            ServiceSingleton.Logger.Log("Starting new session : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            ServiceSingleton.Logger.Log("Architecture : " + (Environment.Is64BitProcess ? "x64" : "x86"));

            this.StStripLblInfo.Text = string.Empty;
            this.StStripLblAdditionalInfo.Text = string.Empty;
            this.StripLblAccountType.Text = string.Empty;
            this.StripLblNexus.Text = string.Empty;           

            SkinManager.SetVisualStyle(this, "Office2016Black");
            this.Style.TitleBar.MaximizeButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.MinimizeButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.HelpButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.CloseButtonHoverBackColor = Color.DarkOrange;
            this.Style.TitleBar.MaximizeButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.MinimizeButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.HelpButtonPressedBackColor = Color.DarkOrange;
            this.Style.TitleBar.CloseButtonPressedBackColor = Color.DarkOrange;

            this.Style.TitleBar.BackColor = Color.FromArgb(54, 54, 54);
            this.Style.TitleBar.IconBackColor = Color.FromArgb(54, 54, 54);
            this.Style.TitleBar.Height = 50;
            this.Style.BackColor = Color.FromArgb(54, 54, 54);


            TitleBarControl = new TitleBarControl();
            TitleBarControl.Width = 3000;
            TitleBarControl.MouseDown += TitleBarControl_MouseDown;
            this.TitleBarTextControl = TitleBarControl;

            this.TitleBarControl.Title = "Nolvus Dashboard";
            this.TitleBarControl.InfoCaption = "v" + ServiceSingleton.Dashboard.Version + " | Not logged";

            this.LoadAccountImage("https://www.nolvus.net/assets/images/account/user-profile.png");                      

            ProgressBar.Value = 0;
            ProgressBar.Maximum = 100;                               
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
                TitleBarControl.Refresh();
                Application.DoEvents();
            }
            
            base.OnClientSizeChanged(e);
        }
    }
}
