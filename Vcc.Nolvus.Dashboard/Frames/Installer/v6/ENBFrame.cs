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
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Installer.v6
{
    public partial class ENBFrame : DashboardFrame
    {
        public ENBFrame()
        {
            InitializeComponent();
        }

        public ENBFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {            
            DrpDwnLstENB.ValueMember = "Code";
            DrpDwnLstENB.DisplayMember = "Name";
            DrpDwnLstENB.DataSource = ENBs.GetAvailableENBsForV6();

            DrpDwnLstENB.SelectedIndex = 0;

            ServiceSingleton.Dashboard.Info("ENB Selection");
        }               
               
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<v6.DifficultyFrame>();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {                       
            ServiceSingleton.Dashboard.LoadFrame<PageFileFrame>();            
        }

        private void DrpDwnLstENB_SelectedIndexChanged(object sender, EventArgs e)
        {        
            ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB = DrpDwnLstENB.SelectedValue.ToString();

            var ENB = ENBs.GetAvailableENBsForV6().Where(x => x.Code == DrpDwnLstENB.SelectedValue.ToString()).FirstOrDefault();

            LblENBDesc.Text = ENB.Description;

            switch (ENB.Code)
            {
                case "CABBAGE":
                    PicBoxENB.Image = Properties.Resources.Cabbage_ENB_01;
                    break;
                case "CABBAVAL":
                    PicBoxENB.Image = Properties.Resources.Cabbaval_ENB;
                    break;
                case "KAUZ":
                    PicBoxENB.Image = Properties.Resources.Kauz_ENB;
                    break;
                case "PICHO":
                    PicBoxENB.Image = Properties.Resources.PiCho_ENB;
                    break;

            }           
        }
    }
}
