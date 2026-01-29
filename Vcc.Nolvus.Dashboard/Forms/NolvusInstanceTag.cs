using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Dashboard.Forms
{
    public partial class NolvusInstanceTag : SfForm
    {
        MessageBar MessageBar;
        public NolvusInstanceTag(string Title)
        {
            InitializeComponent();

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


            MessageBar = new MessageBar();
            MessageBar.Width = 3000;            
            TitleBarTextControl = MessageBar;

            MessageBar.Title = Title;            
        }

        public string InstanceTag
        {
            get
            {
                return TxtBxTag.Text.Trim();
            }
        }

        public static NolvusInstanceTag EnterTag(string Title)
        {
            NolvusInstanceTag InstanceTagSelection = new NolvusInstanceTag(Title);

            InstanceTagSelection.TxtBxTag.Focus();

            return InstanceTagSelection;
        }



        private void BtnOK_Click(object sender, EventArgs e)
        {
            LblError.Hide();

            if (TxtBxTag.Text.Trim() == string.Empty)
            {
                LblError.Text = "You must enter a tag!";
                LblError.Show();
            }
            else if (ServiceSingleton.Instances.InstanceExists(ServiceSingleton.Instances.WorkingInstance.Name, TxtBxTag.Text.Trim()))
            {
                LblError.Text = string.Format("The Tag {0} already exists for instance {1}", TxtBxTag.Text.Trim(), ServiceSingleton.Instances.WorkingInstance.Name);
                LblError.Show();                
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TxtBxTag_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                var regex = new Regex(@"[^a-zA-Z0-9\s]");
                if (regex.IsMatch(e.KeyChar.ToString()))
                {
                    e.Handled = true;
                }
            }

            
        }
    }
}
