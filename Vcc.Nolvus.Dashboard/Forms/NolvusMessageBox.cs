using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Vcc.Nolvus.Components.Controls;
using Vcc.Nolvus.Core.Enums;

namespace Vcc.Nolvus.Dashboard.Forms
{
    public partial class NolvusMessageBox : SfForm
    {
        MessageBar MessageBar;
        public NolvusMessageBox(string Title, string Message, MessageBoxType Type)
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
            this.TitleBarTextControl = MessageBar;

            this.MessageBar.Title = Title;
            this.LblMessage.Text = Message;

            if (Type == MessageBoxType.Question)
            {
                BtnOK.Visible = false;
                BtnYes.Visible = true;
                BtnNo.Visible = true;
            }
            else
            {
                BtnOK.Visible = true;
                BtnYes.Visible = false;
                BtnNo.Visible = false;
            }

            switch (Type)
            {
                case MessageBoxType.Info:
                    PicBox.Image = Properties.Resources.Info;
                    break;
                case MessageBoxType.Warning:
                    PicBox.Image = Properties.Resources.Warning_Message;
                    break;
                case MessageBoxType.Error:
                    PicBox.Image = Properties.Resources.Wrong_WF;
                    break;
                case MessageBoxType.Question:
                    PicBox.Image = Properties.Resources.Question;
                    break;
            }
        }

        public static DialogResult ShowMessage(string Title, string Message, MessageBoxType Type)
        {
            NolvusMessageBox MessageBox = new NolvusMessageBox(Title, Message, Type);

            return MessageBox.ShowDialog();
        }

        public static DialogResult ShowMessage(string Title, string Message, MessageBoxType Type, int Height, int Width, Color MessageColor)
        {
            NolvusMessageBox MessageBox = new NolvusMessageBox(Title, Message, Type);

            MessageBox.Height = Height;
            MessageBox.Width = Width;

            MessageBox.LblMessage.ForeColor = MessageColor;

            return MessageBox.ShowDialog();
        }

        public static DialogResult ShowConfirmation(string Title, string Message)
        {
            NolvusMessageBox MessageBox = new NolvusMessageBox(Title, Message, MessageBoxType.Question);

            return MessageBox.ShowDialog();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
