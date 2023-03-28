namespace Vcc.Nolvus.Dashboard.Frames
{
    partial class PackageFrame
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
            this.PicLoading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // PicLoading
            // 
            this.PicLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicLoading.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.cog_loader_alpha;
            this.PicLoading.Location = new System.Drawing.Point(3, 3);
            this.PicLoading.Name = "PicLoading";
            this.PicLoading.Size = new System.Drawing.Size(672, 431);
            this.PicLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PicLoading.TabIndex = 1;
            this.PicLoading.TabStop = false;
            // 
            // CheckingFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.PicLoading);
            this.Name = "CheckingFrame";
            this.Size = new System.Drawing.Size(678, 437);
            ((System.ComponentModel.ISupportInitialize)(this.PicLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox PicLoading;
    }
}
