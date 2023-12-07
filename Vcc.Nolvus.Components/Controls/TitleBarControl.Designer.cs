namespace Vcc.Nolvus.Components.Controls
{
    partial class TitleBarControl
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
            this.LblTitle = new System.Windows.Forms.Label();
            this.LblInfo = new System.Windows.Forms.Label();
            this.AccountImage = new System.Windows.Forms.PictureBox();
            this.LoadingBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.AccountImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LblTitle
            // 
            this.LblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblTitle.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Location = new System.Drawing.Point(3, 8);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(139, 21);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Nolvus Dashboard";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblInfo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblInfo.ForeColor = System.Drawing.Color.White;
            this.LblInfo.Location = new System.Drawing.Point(255, 0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(269, 36);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "| ?";
            this.LblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AccountImage
            // 
            this.AccountImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountImage.Location = new System.Drawing.Point(530, 3);
            this.AccountImage.Name = "AccountImage";
            this.AccountImage.Size = new System.Drawing.Size(39, 34);
            this.AccountImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AccountImage.TabIndex = 5;
            this.AccountImage.TabStop = false;
            this.AccountImage.Visible = false;
            // 
            // LoadingBox
            // 
            this.LoadingBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingBox.Image = global::Vcc.Nolvus.Components.Properties.Resources.cog_loader_alpha;
            this.LoadingBox.Location = new System.Drawing.Point(575, 3);
            this.LoadingBox.Name = "LoadingBox";
            this.LoadingBox.Size = new System.Drawing.Size(34, 34);
            this.LoadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LoadingBox.TabIndex = 11;
            this.LoadingBox.TabStop = false;
            this.LoadingBox.Visible = false;
            // 
            // TitleBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.LoadingBox);
            this.Controls.Add(this.AccountImage);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.LblTitle);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "TitleBarControl";
            this.Size = new System.Drawing.Size(612, 40);
            ((System.ComponentModel.ISupportInitialize)(this.AccountImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.PictureBox AccountImage;
        private System.Windows.Forms.PictureBox LoadingBox;
    }
}
