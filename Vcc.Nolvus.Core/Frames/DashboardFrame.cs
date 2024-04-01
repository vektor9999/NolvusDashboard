using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Core.Frames
{
    public partial class DashboardFrame : UserControl, IDashboardFrame
    {       
        protected FrameParameters Parameters;
        IDashboard DashBoardInstance;
        public DashboardFrame()
        {
            InitializeComponent();            
        }

        public DashboardFrame(IDashboard Dashboard, FrameParameters Params)
        {            
            InitializeComponent();
            DashBoardInstance = Dashboard;
            DashBoardInstance.OnFrameLoaded += OnFrameLoaded;
            DashBoardInstance.OnFrameLoadedAsync += OnFrameLoadedSync;

            Parameters = Params;

            if (Parameters == null)
            {
                Parameters = new FrameParameters();
            }
        }

        private void OnFrameLoadedSync(object sender, EventArgs e)
        {
            Task.CompletedTask.ContinueWith(async T => { await OnLoadedAsync(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnFrameLoaded(object sender, EventArgs e)
        {
            OnLoaded();
        }

        protected virtual Task OnLoadAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void OnLoad()
        {            
        }

        protected virtual Task OnLoadedAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void OnLoaded()
        {
            
        }

        protected virtual async Task<T> InitializeAsync<T>() where T : DashboardFrame
        {            
            await OnLoadAsync();            
            return (T)this;
        }

        protected virtual T Initialize<T>() where T : DashboardFrame
        {            
            OnLoad();
            return (T)this;
        }

        public static Task<T> CreateAsync<T>() where T : DashboardFrame
        {
            return (Activator.CreateInstance(typeof(T)) as T).InitializeAsync<T>();
        }

        public static Task<T> CreateAsync<T>(object[] Args) where T : DashboardFrame
        {
            return (Activator.CreateInstance(typeof(T), Args) as T).InitializeAsync<T>();
        }

        public static T Create<T>(object[] Args) where T : DashboardFrame
        {
            return (Activator.CreateInstance(typeof(T), Args) as T).Initialize<T>();
        }

        public void Close()
        {
            if (InvokeRequired)
            {
                Invoke((System.Action)Close);
                return;
            }

            DashBoardInstance.OnFrameLoaded -= OnFrameLoaded;
            DashBoardInstance.OnFrameLoadedAsync -= OnFrameLoadedSync;

            Dispose();
        }

    }
}
