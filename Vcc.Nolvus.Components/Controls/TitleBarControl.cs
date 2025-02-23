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
    public delegate void SettingsHandler(object sender, EventArgs e);

    public partial class TitleBarControl : UserControl
    {
        private bool _SettingsEnabled;

        event SettingsHandler OnSettingsClickedEvent;

        public event SettingsHandler OnSettingsClicked
        {
            add
            {
                if (OnSettingsClickedEvent != null)
                {
                    lock (OnSettingsClickedEvent)
                    {
                        OnSettingsClickedEvent += value;
                    }
                }
                else
                {
                    OnSettingsClickedEvent = value;
                }
            }
            remove
            {
                if (OnSettingsClickedEvent != null)
                {
                    lock (OnSettingsClickedEvent)
                    {
                        OnSettingsClickedEvent -= value;
                    }
                }
            }
        }

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
            SettingsBox.Show();
        }

        public void HideLoading()
        {
            SettingsBox.Hide();
        }

        private void LabelMouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void SettingsBox_Click(object sender, EventArgs e)
        {
            SettingsHandler Handler = OnSettingsClickedEvent;            
            if (Handler != null) Handler(this, e);
        }

        private void SettingsBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();
            ToolTip.SetToolTip(SettingsBox, "Global settings");
        }

        public void EnableSettings()
        {
            _SettingsEnabled = true;
        }

        public void DisableSettings()
        {
            _SettingsEnabled = false;
        }

        public bool SettingsEnabled
        {
            get
            {
                return _SettingsEnabled;
            }            
        }
    }
}
