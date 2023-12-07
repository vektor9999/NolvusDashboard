namespace Vcc.Nolvus.Dashboard.Controls
{
    partial class ErrorPanel
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LblModName = new System.Windows.Forms.Label();
            this.LnkLblError = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 30);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LblModName
            // 
            this.LblModName.AutoSize = true;
            this.LblModName.BackColor = System.Drawing.Color.Transparent;
            this.LblModName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblModName.ForeColor = System.Drawing.Color.White;
            this.LblModName.Location = new System.Drawing.Point(109, 3);
            this.LblModName.Name = "LblModName";
            this.LblModName.Size = new System.Drawing.Size(75, 13);
            this.LblModName.TabIndex = 1;
            this.LblModName.Text = "[Mod Name]";
            // 
            // LnkLblError
            // 
            this.LnkLblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LnkLblError.AutoEllipsis = true;
            this.LnkLblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LnkLblError.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.LnkLblError.LinkColor = System.Drawing.Color.Red;
            this.LnkLblError.Location = new System.Drawing.Point(109, 19);
            this.LnkLblError.Name = "LnkLblError";
            this.LnkLblError.Size = new System.Drawing.Size(735, 15);
            this.LnkLblError.TabIndex = 2;
            this.LnkLblError.TabStop = true;
            this.LnkLblError.Text = "[Error...]";
            this.LnkLblError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLblError_LinkClicked);
            // 
            // ErrorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.LnkLblError);
            this.Controls.Add(this.LblModName);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ErrorPanel";
            this.Size = new System.Drawing.Size(850, 35);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LblModName;
        private System.Windows.Forms.LinkLabel LnkLblError;
    }
}
