namespace Vcc.Nolvus.GrassCache
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtBxGrassCacheFolder = new System.Windows.Forms.TextBox();
            this.BtnBrowseGrassCache = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnBrowseCombinedGrassCache = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.TxtBxCombinedGrassCacheFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ProgressBar = new Syncfusion.Windows.Forms.Tools.ProgressBarAdv();
            this.BtnRename = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label3 = new System.Windows.Forms.Label();
            this.DrpDwnLstSeasons = new Syncfusion.WinForms.ListView.SfComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstSeasons)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grass Cache folder";
            // 
            // TxtBxGrassCacheFolder
            // 
            this.TxtBxGrassCacheFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxGrassCacheFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxGrassCacheFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxGrassCacheFolder.ForeColor = System.Drawing.Color.White;
            this.TxtBxGrassCacheFolder.Location = new System.Drawing.Point(156, 17);
            this.TxtBxGrassCacheFolder.Name = "TxtBxGrassCacheFolder";
            this.TxtBxGrassCacheFolder.Size = new System.Drawing.Size(357, 20);
            this.TxtBxGrassCacheFolder.TabIndex = 3;
            // 
            // BtnBrowseGrassCache
            // 
            this.BtnBrowseGrassCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseGrassCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseGrassCache.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseGrassCache.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseGrassCache.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseGrassCache.Location = new System.Drawing.Point(519, 17);
            this.BtnBrowseGrassCache.Name = "BtnBrowseGrassCache";
            this.BtnBrowseGrassCache.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseGrassCache.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseGrassCache.TabIndex = 28;
            this.BtnBrowseGrassCache.Text = "...";
            this.BtnBrowseGrassCache.UseVisualStyleBackColor = false;
            this.BtnBrowseGrassCache.Click += new System.EventHandler(this.BtnBrowseGrassCache_Click);
            // 
            // BtnBrowseCombinedGrassCache
            // 
            this.BtnBrowseCombinedGrassCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseCombinedGrassCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseCombinedGrassCache.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseCombinedGrassCache.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseCombinedGrassCache.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseCombinedGrassCache.Location = new System.Drawing.Point(519, 47);
            this.BtnBrowseCombinedGrassCache.Name = "BtnBrowseCombinedGrassCache";
            this.BtnBrowseCombinedGrassCache.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseCombinedGrassCache.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseCombinedGrassCache.TabIndex = 31;
            this.BtnBrowseCombinedGrassCache.Text = "...";
            this.BtnBrowseCombinedGrassCache.UseVisualStyleBackColor = false;
            this.BtnBrowseCombinedGrassCache.Click += new System.EventHandler(this.BtnBrowseCombinedGrassCache_Click);
            // 
            // TxtBxCombinedGrassCacheFolder
            // 
            this.TxtBxCombinedGrassCacheFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxCombinedGrassCacheFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxCombinedGrassCacheFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxCombinedGrassCacheFolder.ForeColor = System.Drawing.Color.White;
            this.TxtBxCombinedGrassCacheFolder.Location = new System.Drawing.Point(156, 47);
            this.TxtBxCombinedGrassCacheFolder.Name = "TxtBxCombinedGrassCacheFolder";
            this.TxtBxCombinedGrassCacheFolder.Size = new System.Drawing.Size(357, 20);
            this.TxtBxCombinedGrassCacheFolder.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Grass Cache Combined folder";
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
            this.ProgressBar.Location = new System.Drawing.Point(156, 129);
            this.ProgressBar.MultipleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Empty};
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.SegmentWidth = 12;
            this.ProgressBar.Size = new System.Drawing.Size(383, 5);
            this.ProgressBar.TabIndex = 32;
            this.ProgressBar.Text = "progressBarAdv1";
            this.ProgressBar.TextStyle = Syncfusion.Windows.Forms.Tools.ProgressBarTextStyles.Custom;
            this.ProgressBar.ThemeName = "Constant";
            this.ProgressBar.Value = 100;
            this.ProgressBar.Visible = false;
            this.ProgressBar.WaitingGradientWidth = 400;
            // 
            // BtnRename
            // 
            this.BtnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnRename.BorderColor = System.Drawing.Color.White;
            this.BtnRename.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnRename.ForeColor = System.Drawing.Color.White;
            this.BtnRename.Location = new System.Drawing.Point(451, 157);
            this.BtnRename.Name = "BtnRename";
            this.BtnRename.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnRename.Size = new System.Drawing.Size(88, 40);
            this.BtnRename.TabIndex = 33;
            this.BtnRename.Text = "Rename";
            this.BtnRename.UseVisualStyleBackColor = false;
            this.BtnRename.Click += new System.EventHandler(this.BtnRename_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Season";
            // 
            // DrpDwnLstSeasons
            // 
            this.DrpDwnLstSeasons.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstSeasons.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstSeasons.Location = new System.Drawing.Point(156, 73);
            this.DrpDwnLstSeasons.Name = "DrpDwnLstSeasons";
            this.DrpDwnLstSeasons.Size = new System.Drawing.Size(146, 28);
            this.DrpDwnLstSeasons.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLstSeasons.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstSeasons.TabIndex = 35;
            this.DrpDwnLstSeasons.ThemeName = "Office2016Black";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(550, 202);
            this.Controls.Add(this.DrpDwnLstSeasons);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnRename);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.BtnBrowseCombinedGrassCache);
            this.Controls.Add(this.TxtBxCombinedGrassCacheFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnBrowseGrassCache);
            this.Controls.Add(this.TxtBxGrassCacheFolder);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Nolvus Grass Cache Seasons Renamer";
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstSeasons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBxGrassCacheFolder;
        private Components.Controls.FlatButton BtnBrowseGrassCache;
        private Components.Controls.FlatButton BtnBrowseCombinedGrassCache;
        private System.Windows.Forms.TextBox TxtBxCombinedGrassCacheFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Syncfusion.Windows.Forms.Tools.ProgressBarAdv ProgressBar;
        private Components.Controls.FlatButton BtnRename;
        private System.Windows.Forms.Label label3;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstSeasons;
    }
}

