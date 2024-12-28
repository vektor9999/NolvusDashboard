namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    partial class NexusFrame
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.LnkLblInfo = new System.Windows.Forms.LinkLabel();
            this.BtnPrevious = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnContinue = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnAuthenticate = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.LblMessage = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.PnlMessage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.PnlMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(333, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "for more info";
            // 
            // LnkLblInfo
            // 
            this.LnkLblInfo.AutoSize = true;
            this.LnkLblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LnkLblInfo.ForeColor = System.Drawing.Color.Orange;
            this.LnkLblInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LnkLblInfo.LinkColor = System.Drawing.Color.Orange;
            this.LnkLblInfo.Location = new System.Drawing.Point(309, 15);
            this.LnkLblInfo.Name = "LnkLblInfo";
            this.LnkLblInfo.Size = new System.Drawing.Size(28, 13);
            this.LnkLblInfo.TabIndex = 15;
            this.LnkLblInfo.TabStop = true;
            this.LnkLblInfo.Text = "here";
            this.LnkLblInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLblInfo_LinkClicked);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPrevious.BorderColor = System.Drawing.Color.White;
            this.BtnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPrevious.ForeColor = System.Drawing.Color.White;
            this.BtnPrevious.Location = new System.Drawing.Point(763, 391);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPrevious.Size = new System.Drawing.Size(94, 42);
            this.BtnPrevious.TabIndex = 12;
            this.BtnPrevious.Text = "Previous";
            this.BtnPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPrevious.UseVisualStyleBackColor = false;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnContinue
            // 
            this.BtnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnContinue.BorderColor = System.Drawing.Color.White;
            this.BtnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnContinue.ForeColor = System.Drawing.Color.White;
            this.BtnContinue.Location = new System.Drawing.Point(863, 391);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnContinue.Size = new System.Drawing.Size(94, 42);
            this.BtnContinue.TabIndex = 11;
            this.BtnContinue.Text = "Next";
            this.BtnContinue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnContinue.UseVisualStyleBackColor = false;
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Click the button below to authenticate using Nexus SSO. Click";
            // 
            // BtnAuthenticate
            // 
            this.BtnAuthenticate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnAuthenticate.BorderColor = System.Drawing.Color.White;
            this.BtnAuthenticate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnAuthenticate.ForeColor = System.Drawing.Color.White;
            this.BtnAuthenticate.Location = new System.Drawing.Point(16, 47);
            this.BtnAuthenticate.Name = "BtnAuthenticate";
            this.BtnAuthenticate.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnAuthenticate.Size = new System.Drawing.Size(192, 42);
            this.BtnAuthenticate.TabIndex = 17;
            this.BtnAuthenticate.Text = "Nexus SSO Authentication";
            this.BtnAuthenticate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAuthenticate.UseVisualStyleBackColor = false;
            this.BtnAuthenticate.Click += new System.EventHandler(this.BtnAuthenticate_Click);
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(44, 7);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(821, 32);
            this.LblMessage.TabIndex = 1;
            this.LblMessage.Text = "[Message]";
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PicBox
            // 
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.PicBox.Location = new System.Drawing.Point(6, 7);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(32, 32);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 0;
            this.PicBox.TabStop = false;
            // 
            // PnlMessage
            // 
            this.PnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlMessage.BackColor = System.Drawing.Color.Orange;
            this.PnlMessage.Controls.Add(this.LblMessage);
            this.PnlMessage.Controls.Add(this.PicBox);
            this.PnlMessage.Location = new System.Drawing.Point(16, 116);
            this.PnlMessage.Name = "PnlMessage";
            this.PnlMessage.Size = new System.Drawing.Size(872, 45);
            this.PnlMessage.TabIndex = 21;
            // 
            // NexusFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.PnlMessage);
            this.Controls.Add(this.BtnAuthenticate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LnkLblInfo);
            this.Controls.Add(this.BtnPrevious);
            this.Controls.Add(this.BtnContinue);
            this.Controls.Add(this.label1);
            this.Name = "NexusFrame";
            this.Size = new System.Drawing.Size(969, 448);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.PnlMessage.ResumeLayout(false);
            this.PnlMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel LnkLblInfo;
        private Components.Controls.FlatButton BtnPrevious;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label1;
        private Components.Controls.FlatButton BtnAuthenticate;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.Panel PnlMessage;
    }
}
