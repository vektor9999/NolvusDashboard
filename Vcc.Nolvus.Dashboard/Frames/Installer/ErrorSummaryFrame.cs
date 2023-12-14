using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Styles;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Browser.Forms;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Controls;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class ErrorSummaryFrame : DashboardFrame
    {                                
        public ErrorSummaryFrame()
        {
            InitializeComponent();
        }

        public ErrorSummaryFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            ServiceSingleton.Dashboard.ClearInfo();

            ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Installation Failed]");
            ServiceSingleton.Dashboard.Status("Installation is not completed, please review errors.");
            ServiceSingleton.Dashboard.Info(string.Format("Error(s) : {0}", ServiceSingleton.Packages.ErrorHandler.ErrorsCount));            

            if (ServiceSingleton.Packages.ErrorHandler.ThresholdEnabled)
            {
                LblMessage.Text = string.Format("The installation has not been completed because {0} error(s) on {1} maximum error(s) allowed occured", ServiceSingleton.Packages.ErrorHandler.ErrorsCount, ServiceSingleton.Settings.ErrorsThreshold);
                ServiceSingleton.Dashboard.AdditionalInfo(string.Format("Error threshold : {0}", ServiceSingleton.Settings.ErrorsThreshold));
            }
            else
            {
                LblMessage.Text = string.Format("The installation has not been completed because {0} error(s) occured", ServiceSingleton.Packages.ErrorHandler.ErrorsCount);
                ServiceSingleton.Dashboard.AdditionalInfo("Error threshold : Disabled");
            }            

            ErrorsPanel.LoadMods(ServiceSingleton.Packages.ErrorHandler.List);

            ErrorsPanel.AutoScroll = false;
            ErrorsPanel.HorizontalScroll.Enabled = false;
            ErrorsPanel.HorizontalScroll.Visible = false;
            ErrorsPanel.HorizontalScroll.Maximum = 0;
            ErrorsPanel.AutoScroll = true;

        }

        private void BtnRetry_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.ClearInfo();            
            ServiceSingleton.Dashboard.LoadFrame<ResumeFrame>();
        }

        private void BtnFix_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nolvus.net/appendix/installer/faq");
        }

        private void ErrorsPanel_Resize(object sender, EventArgs e)
        {
            ErrorsPanel.LoadMods(ServiceSingleton.Packages.ErrorHandler.List);
        }
    }
}
