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
    public partial class MessageBar : UserControl
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

        public MessageBar()
        {
            InitializeComponent();

            this.LblTitle.MouseDown += LblTitle_MouseDown;
        }

        private void LblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }
    }
}
