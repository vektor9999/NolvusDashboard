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
            this.LblCheck = new System.Windows.Forms.Label();
            this.TxtBxNexusApiKey = new System.Windows.Forms.TextBox();
            this.BtnPrevious = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnContinue = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(224, 15);
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
            this.LnkLblInfo.Location = new System.Drawing.Point(199, 15);
            this.LnkLblInfo.Name = "LnkLblInfo";
            this.LnkLblInfo.Size = new System.Drawing.Size(28, 13);
            this.LnkLblInfo.TabIndex = 15;
            this.LnkLblInfo.TabStop = true;
            this.LnkLblInfo.Text = "here";
            this.LnkLblInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLblInfo_LinkClicked);
            // 
            // LblCheck
            // 
            this.LblCheck.AutoSize = true;
            this.LblCheck.ForeColor = System.Drawing.Color.White;
            this.LblCheck.Location = new System.Drawing.Point(13, 93);
            this.LblCheck.Name = "LblCheck";
            this.LblCheck.Size = new System.Drawing.Size(195, 13);
            this.LblCheck.TabIndex = 14;
            this.LblCheck.Text = "You must select an installation directory!";
            // 
            // TxtBxNexusApiKey
            // 
            this.TxtBxNexusApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxNexusApiKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxNexusApiKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxNexusApiKey.ForeColor = System.Drawing.Color.White;
            this.TxtBxNexusApiKey.Location = new System.Drawing.Point(16, 57);
            this.TxtBxNexusApiKey.Name = "TxtBxNexusApiKey";
            this.TxtBxNexusApiKey.Size = new System.Drawing.Size(487, 20);
            this.TxtBxNexusApiKey.TabIndex = 13;
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPrevious.BorderColor = System.Drawing.Color.White;
            this.BtnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPrevious.ForeColor = System.Drawing.Color.White;
            this.BtnPrevious.Location = new System.Drawing.Point(373, 174);
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
            this.BtnContinue.Location = new System.Drawing.Point(473, 174);
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
            this.label1.Size = new System.Drawing.Size(190, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Please enter your Nexus Api Key. Click";
            // 
            // NexusFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LnkLblInfo);
            this.Controls.Add(this.LblCheck);
            this.Controls.Add(this.TxtBxNexusApiKey);
            this.Controls.Add(this.BtnPrevious);
            this.Controls.Add(this.BtnContinue);
            this.Controls.Add(this.label1);
            this.Name = "NexusFrame";
            this.Size = new System.Drawing.Size(579, 231);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel LnkLblInfo;
        private System.Windows.Forms.Label LblCheck;
        private System.Windows.Forms.TextBox TxtBxNexusApiKey;
        private Components.Controls.FlatButton BtnPrevious;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label1;
    }
}
