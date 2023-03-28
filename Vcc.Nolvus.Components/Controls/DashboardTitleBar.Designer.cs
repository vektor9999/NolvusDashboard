namespace Vcc.Nolvus.Components.Controls
{
    partial class DashboardTitleBar
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
            this.AccountImage = new System.Windows.Forms.PictureBox();
            this.LblInfo = new System.Windows.Forms.Label();
            this.LblTitle = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.AccountImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountImage
            // 
            this.AccountImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountImage.Location = new System.Drawing.Point(559, 3);
            this.AccountImage.Name = "AccountImage";
            this.AccountImage.Size = new System.Drawing.Size(39, 34);
            this.AccountImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AccountImage.TabIndex = 8;
            this.AccountImage.TabStop = false;
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblInfo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblInfo.ForeColor = System.Drawing.Color.White;
            this.LblInfo.Location = new System.Drawing.Point(284, 0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(269, 36);
            this.LblInfo.TabIndex = 7;
            this.LblInfo.Text = "| ?";
            this.LblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblTitle
            // 
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblTitle.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Location = new System.Drawing.Point(0, 0);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(189, 24);
            this.LblTitle.TabIndex = 6;
            this.LblTitle.Text = "Nolvus Dashboard";
            // 
            // LblStepText
            // 
            this.LblStepText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStepText.ForeColor = System.Drawing.Color.White;
            this.LblStepText.Location = new System.Drawing.Point(3, 23);
            this.LblStepText.Name = "LblStepText";
            this.LblStepText.Size = new System.Drawing.Size(225, 14);
            this.LblStepText.TabIndex = 9;
            this.LblStepText.Text = "Initializing...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Vcc.Nolvus.Components.Properties.Resources.cog_loader_alpha;
            this.pictureBox1.Location = new System.Drawing.Point(246, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // DashboardTitleBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LblStepText);
            this.Controls.Add(this.AccountImage);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.LblTitle);
            this.Name = "DashboardTitleBar";
            this.Size = new System.Drawing.Size(601, 40);
            ((System.ComponentModel.ISupportInitialize)(this.AccountImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox AccountImage;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
