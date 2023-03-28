namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class InstallFrame
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
            this.ModsPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LoadingBox = new System.Windows.Forms.PictureBox();
            this.ModsBox = new Vcc.Nolvus.Components.Controls.ModsBox();
            this.ModsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ModsPanel
            // 
            this.ModsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModsPanel.Controls.Add(this.panel1);
            this.ModsPanel.Location = new System.Drawing.Point(3, 3);
            this.ModsPanel.Name = "ModsPanel";
            this.ModsPanel.Size = new System.Drawing.Size(981, 653);
            this.ModsPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LoadingBox);
            this.panel1.Controls.Add(this.ModsBox);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 632);
            this.panel1.TabIndex = 4;
            // 
            // LoadingBox
            // 
            this.LoadingBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.cog_loader_alpha;
            this.LoadingBox.Location = new System.Drawing.Point(0, 3);
            this.LoadingBox.Name = "LoadingBox";
            this.LoadingBox.Size = new System.Drawing.Size(974, 643);
            this.LoadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LoadingBox.TabIndex = 5;
            this.LoadingBox.TabStop = false;
            // 
            // ModsBox
            // 
            this.ModsBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ModsBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModsBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ModsBox.FormattingEnabled = true;
            this.ModsBox.ItemHeight = 35;
            this.ModsBox.Location = new System.Drawing.Point(0, 0);
            this.ModsBox.Name = "ModsBox";
            this.ModsBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.ModsBox.Size = new System.Drawing.Size(973, 630);
            this.ModsBox.TabIndex = 4;
            // 
            // InstallFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.ModsPanel);
            this.Name = "InstallFrame";
            this.Size = new System.Drawing.Size(987, 659);
            this.ModsPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ModsPanel;
        private Components.Controls.ModsBox ModsBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox LoadingBox;
    }
}
