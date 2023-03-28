using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Dashboard.Frames
{
    public partial class ErrorFrame : DashboardFrame
    {
        private string Title
        {
            get
            {
                return Parameters["Title"].ToString();
            }
        }

        private string Message
        {
            get
            {
                return Parameters["Message"].ToString();
            }
        }

        private string Trace
        {
            get
            {
                if (Parameters["Trace"] != null)
                {
                    return Parameters["Trace"].ToString();
                }

                return string.Empty;
            }
        }

        private bool Retry
        {
            get
            {
                if (Parameters["Retry"] != null)
                {
                    return System.Convert.ToBoolean(Parameters["Retry"]);
                }

                return false;
            }
        }

        public ErrorFrame()
        {
            InitializeComponent();
        }       

        public ErrorFrame(IDashboard Dashboard, FrameParameters Params) :
            base(Dashboard, Params)
        {
            InitializeComponent();

            PnlTitle.BackColor = Color.FromArgb(217, 83, 79);
            LblTitle.Text = Title;
            LblError.Text = Message;
            LblTrace.Text = Trace;
            BtnRetry.Visible = Retry;            
        }                 
                     
        private void BtnRetry_Click(object sender, EventArgs e)
        {
            //ShowFrame(typeof(ResumeInstall), true);
        }                
    }
}
