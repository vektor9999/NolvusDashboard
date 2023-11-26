using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Components.Controls
{
    public partial class TitleBarControl : UserControl
    {
       

        public string Title
        {
            get
            {
                return LblTitle.Text;
            }
            set
            {
                LblTitle.Text = value;
            }
        }
       
        public string InfoCaption
        {
            get
            {
                return LblInfo.Text;
            }
            set
            {
                LblInfo.Text = value;
            }
        }

        public void SetAccountImage(string Url)
        {
            AccountImage.Load(Url);            
        }

        public void SetAccountImage(System.Drawing.Image Image)
        {            
            AccountImage.Image = Image;            
        }

        public TitleBarControl()
        {
            InitializeComponent();                        

            LblTitle.MouseDown += LabelMouseDown;
            LblInfo.MouseDown += LabelMouseDown;
        }

        public void ShowLoading()
        {
            LoadingBox.Show();
        }

        public void HideLoading()
        {
            LoadingBox.Hide();
        }

        private void LabelMouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }
    }
}
