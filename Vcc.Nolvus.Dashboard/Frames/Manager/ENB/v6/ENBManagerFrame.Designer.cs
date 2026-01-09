namespace Vcc.Nolvus.Dashboard.Frames.Manager.ENB.v6
{
    partial class ENBManagerFrame
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnInstall = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PicLoading = new System.Windows.Forms.PictureBox();
            this.ENBListBox = new Vcc.Nolvus.Components.Controls.ENBListBox();
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.BtnInstall);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(713, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 698);
            this.panel1.TabIndex = 18;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.White;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(114, 642);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(88, 40);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnInstall
            // 
            this.BtnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnInstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnInstall.BorderColor = System.Drawing.Color.White;
            this.BtnInstall.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnInstall.ForeColor = System.Drawing.Color.White;
            this.BtnInstall.Location = new System.Drawing.Point(208, 642);
            this.BtnInstall.Name = "BtnInstall";
            this.BtnInstall.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnInstall.Size = new System.Drawing.Size(88, 40);
            this.BtnInstall.TabIndex = 4;
            this.BtnInstall.Text = "Install";
            this.BtnInstall.UseVisualStyleBackColor = false;
            this.BtnInstall.Click += new System.EventHandler(this.BtnInstall_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 54);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select the ENB you want to install and click on Install. To cancel, click on Canc" +
    "el";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 698);
            this.panel2.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.PicLoading);
            this.panel3.Controls.Add(this.ENBListBox);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(697, 698);
            this.panel3.TabIndex = 1;
            // 
            // PicLoading
            // 
            this.PicLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicLoading.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.cog_loader_alpha;
            this.PicLoading.Location = new System.Drawing.Point(3, 3);
            this.PicLoading.Name = "PicLoading";
            this.PicLoading.Size = new System.Drawing.Size(689, 690);
            this.PicLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PicLoading.TabIndex = 4;
            this.PicLoading.TabStop = false;
            // 
            // ENBListBox
            // 
            this.ENBListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ENBListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ENBListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ENBListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ENBListBox.FormattingEnabled = true;
            this.ENBListBox.ItemHeight = 110;
            this.ENBListBox.Location = new System.Drawing.Point(0, 0);
            this.ENBListBox.Name = "ENBListBox";
            this.ENBListBox.ScalingFactor = 1D;
            this.ENBListBox.Size = new System.Drawing.Size(695, 696);
            this.ENBListBox.TabIndex = 1;
            this.ENBListBox.Visible = false;
            // 
            // LblStepText
            // 
            this.LblStepText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStepText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblStepText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStepText.ForeColor = System.Drawing.Color.White;
            this.LblStepText.Location = new System.Drawing.Point(3, 0);
            this.LblStepText.Name = "LblStepText";
            this.LblStepText.Size = new System.Drawing.Size(1020, 24);
            this.LblStepText.TabIndex = 16;
            this.LblStepText.Text = "Available ENBs";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ENBManagerFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblStepText);
            this.Name = "ENBManagerFrame";
            this.Size = new System.Drawing.Size(1026, 733);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Panel panel3;
        private Components.Controls.ENBListBox ENBListBox;
        private System.Windows.Forms.PictureBox PicLoading;
        private Components.Controls.FlatButton BtnCancel;
        private Components.Controls.FlatButton BtnInstall;
    }
}
