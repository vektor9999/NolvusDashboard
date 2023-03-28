using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vcc.Nolvus.Components.Controls
{
    public partial class DashboardTitleBar : UserControl
    {

        public string Title
        {
            get
            {
                return this.LblTitle.Text;
            }
            set
            {
                this.LblTitle.Text = value;
            }
        }

        public string InfoCaption
        {
            get
            {
                return this.LblInfo.Text;
            }
            set
            {
                this.LblInfo.Text = value;
            }
        }

        public void SetAccountImage(string Url)
        {
            this.AccountImage.Load(Url);
        }

        public DashboardTitleBar()
        {
            InitializeComponent();

            this.LblTitle.MouseDown += Label1_MouseDown;
        }

        private void Label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }
       

        public void SetStepInfo(string Text)
        {
            this.LblStepText.Text = Text;
        }
    }
}
