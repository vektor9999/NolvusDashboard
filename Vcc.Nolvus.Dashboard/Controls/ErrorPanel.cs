using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Dashboard.Forms;

namespace Vcc.Nolvus.Dashboard.Controls
{
    public partial class ErrorPanel : UserControl
    {
        public string ModName
        {
            get
            {
                return LblModName.Text;
            }

            set
            {
                LblModName.Text = value;
            }
        }

        public string ErrorText
        {
            get
            {
                return LnkLblError.Text;
            }

            set
            {
                LnkLblError.Text = value;
            }
        }

        public void SetImage(Image Image)
        {
            pictureBox1.Image = Image;
        }

        public ErrorPanel()
        {
            InitializeComponent();
        }

        private void LnkLblError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NolvusMessageBox.ShowMessage(ModName, ErrorText, Nolvus.Core.Enums.MessageBoxType.Error, 300, 700, Color.Red);
        }
    }
}
