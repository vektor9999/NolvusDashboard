namespace Vcc.Nolvus.Downgrader
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnBrowseOutputPath = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnBrowseSkyrimPath = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.TxtBxOutputDir = new System.Windows.Forms.TextBox();
            this.TxtBxSkyrimDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LstBxOutput = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LblStatus = new System.Windows.Forms.Label();
            this.ProgressBar = new Syncfusion.Windows.Forms.Tools.ProgressBarAdv();
            this.BtnDowngrade = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnBrowseOutputPath);
            this.panel1.Controls.Add(this.BtnBrowseSkyrimPath);
            this.panel1.Controls.Add(this.TxtBxOutputDir);
            this.panel1.Controls.Add(this.TxtBxSkyrimDir);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 79);
            this.panel1.TabIndex = 1;
            // 
            // BtnBrowseOutputPath
            // 
            this.BtnBrowseOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseOutputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseOutputPath.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseOutputPath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseOutputPath.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseOutputPath.Location = new System.Drawing.Point(742, 49);
            this.BtnBrowseOutputPath.Name = "BtnBrowseOutputPath";
            this.BtnBrowseOutputPath.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseOutputPath.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseOutputPath.TabIndex = 28;
            this.BtnBrowseOutputPath.Text = "...";
            this.BtnBrowseOutputPath.UseVisualStyleBackColor = false;
            this.BtnBrowseOutputPath.Click += new System.EventHandler(this.BtnBrowseOutputPath_Click);
            // 
            // BtnBrowseSkyrimPath
            // 
            this.BtnBrowseSkyrimPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseSkyrimPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseSkyrimPath.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseSkyrimPath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseSkyrimPath.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseSkyrimPath.Location = new System.Drawing.Point(742, 16);
            this.BtnBrowseSkyrimPath.Name = "BtnBrowseSkyrimPath";
            this.BtnBrowseSkyrimPath.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseSkyrimPath.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseSkyrimPath.TabIndex = 27;
            this.BtnBrowseSkyrimPath.Text = "...";
            this.BtnBrowseSkyrimPath.UseVisualStyleBackColor = false;
            this.BtnBrowseSkyrimPath.Click += new System.EventHandler(this.BtnBrowseSkyrimPath_Click);
            // 
            // TxtBxOutputDir
            // 
            this.TxtBxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxOutputDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxOutputDir.ForeColor = System.Drawing.Color.White;
            this.TxtBxOutputDir.Location = new System.Drawing.Point(125, 49);
            this.TxtBxOutputDir.Name = "TxtBxOutputDir";
            this.TxtBxOutputDir.Size = new System.Drawing.Size(613, 20);
            this.TxtBxOutputDir.TabIndex = 3;
            // 
            // TxtBxSkyrimDir
            // 
            this.TxtBxSkyrimDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxSkyrimDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxSkyrimDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxSkyrimDir.ForeColor = System.Drawing.Color.White;
            this.TxtBxSkyrimDir.Location = new System.Drawing.Point(125, 16);
            this.TxtBxSkyrimDir.Name = "TxtBxSkyrimDir";
            this.TxtBxSkyrimDir.Size = new System.Drawing.Size(613, 20);
            this.TxtBxSkyrimDir.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output Directory";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Skyrim Steam Directory";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.LstBxOutput);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(763, 502);
            this.panel2.TabIndex = 3;
            // 
            // LstBxOutput
            // 
            this.LstBxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LstBxOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LstBxOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LstBxOutput.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LstBxOutput.ForeColor = System.Drawing.Color.White;
            this.LstBxOutput.FormattingEnabled = true;
            this.LstBxOutput.Location = new System.Drawing.Point(2, 3);
            this.LstBxOutput.Name = "LstBxOutput";
            this.LstBxOutput.Size = new System.Drawing.Size(757, 494);
            this.LstBxOutput.TabIndex = 4;
            this.LstBxOutput.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LstBxOutput_DrawItem);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.LblStatus);
            this.panel3.Controls.Add(this.ProgressBar);
            this.panel3.Controls.Add(this.BtnDowngrade);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(2, 583);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(763, 50);
            this.panel3.TabIndex = 4;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.ForeColor = System.Drawing.Color.White;
            this.LblStatus.Location = new System.Drawing.Point(-2, 20);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(84, 13);
            this.LblStatus.TabIndex = 32;
            this.LblStatus.Text = "Output Directory";
            this.LblStatus.Visible = false;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ProgressBar.BackMultipleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Empty};
            this.ProgressBar.BackSegments = false;
            this.ProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProgressBar.CanApplyTheme = false;
            this.ProgressBar.CanOverrideStyle = true;
            this.ProgressBar.CustomText = null;
            this.ProgressBar.CustomWaitingRender = false;
            this.ProgressBar.ForeColor = System.Drawing.Color.Orange;
            this.ProgressBar.ForegroundImage = null;
            this.ProgressBar.ForeSegments = false;
            this.ProgressBar.GradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.ProgressBar.GradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.ProgressBar.Location = new System.Drawing.Point(0, 7);
            this.ProgressBar.MultipleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Empty};
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.SegmentWidth = 12;
            this.ProgressBar.Size = new System.Drawing.Size(669, 5);
            this.ProgressBar.TabIndex = 31;
            this.ProgressBar.Text = "progressBarAdv1";
            this.ProgressBar.TextStyle = Syncfusion.Windows.Forms.Tools.ProgressBarTextStyles.Custom;
            this.ProgressBar.ThemeName = "Constant";
            this.ProgressBar.Value = 100;
            this.ProgressBar.Visible = false;
            this.ProgressBar.WaitingGradientWidth = 400;
            // 
            // BtnDowngrade
            // 
            this.BtnDowngrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDowngrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnDowngrade.BorderColor = System.Drawing.Color.White;
            this.BtnDowngrade.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnDowngrade.ForeColor = System.Drawing.Color.White;
            this.BtnDowngrade.Location = new System.Drawing.Point(675, 7);
            this.BtnDowngrade.Name = "BtnDowngrade";
            this.BtnDowngrade.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnDowngrade.Size = new System.Drawing.Size(88, 40);
            this.BtnDowngrade.TabIndex = 2;
            this.BtnDowngrade.Text = "Downgrade";
            this.BtnDowngrade.UseVisualStyleBackColor = false;
            this.BtnDowngrade.Click += new System.EventHandler(this.BtnDowngrade_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(767, 635);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Nolvus - Skyrim Downgrade Patcher";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtBxOutputDir;
        private System.Windows.Forms.TextBox TxtBxSkyrimDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Components.Controls.FlatButton BtnBrowseOutputPath;
        private Components.Controls.FlatButton BtnBrowseSkyrimPath;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox LstBxOutput;
        private Components.Controls.FlatButton BtnDowngrade;
        private Syncfusion.Windows.Forms.Tools.ProgressBarAdv ProgressBar;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

