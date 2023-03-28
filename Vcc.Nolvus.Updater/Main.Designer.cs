namespace Vcc.Nolvus.Updater
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
            this.LblInfo = new System.Windows.Forms.Label();
            this.ProgressBar = new Syncfusion.Windows.Forms.Tools.ProgressBarAdv();
            this.BtnClose = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.LblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // LblInfo
            // 
            this.LblInfo.AutoSize = true;
            this.LblInfo.ForeColor = System.Drawing.Color.White;
            this.LblInfo.Location = new System.Drawing.Point(24, 83);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(61, 13);
            this.LblInfo.TabIndex = 0;
            this.LblInfo.Text = "Initializing...";
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
            this.ProgressBar.Location = new System.Drawing.Point(27, 109);
            this.ProgressBar.MultipleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Empty};
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.SegmentWidth = 12;
            this.ProgressBar.Size = new System.Drawing.Size(547, 6);
            this.ProgressBar.TabIndex = 12;
            this.ProgressBar.Text = "progressBarAdv1";
            this.ProgressBar.TextStyle = Syncfusion.Windows.Forms.Tools.ProgressBarTextStyles.Custom;
            this.ProgressBar.ThemeName = "Constant";
            this.ProgressBar.Value = 100;
            this.ProgressBar.WaitingGradientWidth = 400;
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnClose.BorderColor = System.Drawing.Color.White;
            this.BtnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnClose.ForeColor = System.Drawing.Color.White;
            this.BtnClose.Location = new System.Drawing.Point(494, 145);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnClose.Size = new System.Drawing.Size(99, 37);
            this.BtnClose.TabIndex = 13;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Visible = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // LblError
            // 
            this.LblError.ForeColor = System.Drawing.Color.Red;
            this.LblError.Location = new System.Drawing.Point(24, 122);
            this.LblError.Name = "LblError";
            this.LblError.Size = new System.Drawing.Size(464, 60);
            this.LblError.TabIndex = 14;
            this.LblError.Text = "Initializing...";
            this.LblError.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(603, 190);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.LblError);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.LblInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Nolvus Dashboard Updater";
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblInfo;
        private Syncfusion.Windows.Forms.Tools.ProgressBarAdv ProgressBar;
        private Components.Controls.FlatButton BtnClose;
        private System.Windows.Forms.Label LblError;
    }
}

