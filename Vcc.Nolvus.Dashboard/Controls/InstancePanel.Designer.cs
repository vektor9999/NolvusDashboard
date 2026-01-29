namespace Vcc.Nolvus.Dashboard.Controls
{
    partial class InstancePanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstancePanel));
            this.LblInstanceName = new System.Windows.Forms.Label();
            this.LblVersion = new System.Windows.Forms.Label();
            this.LblStatus = new System.Windows.Forms.Label();
            this.BtnUpdate = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnPlay = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.PicInstanceImage = new System.Windows.Forms.PictureBox();
            this.LblDesc = new System.Windows.Forms.Label();
            this.BtnView = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.popupMenusManager1 = new Syncfusion.Windows.Forms.Tools.XPMenus.PopupMenusManager(this.components);
            this.popupMenu1 = new Syncfusion.Windows.Forms.Tools.XPMenus.PopupMenu(this.components);
            this.parentBarItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.BrItmMods = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmShortCut = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmReport = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem2 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmKeyBinds = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem3 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmManual = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem4 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmENBManager = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem6 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.BrItmDelete = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.LblImageLoading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicInstanceImage)).BeginInit();
            this.SuspendLayout();
            // 
            // LblInstanceName
            // 
            this.LblInstanceName.BackColor = System.Drawing.Color.Transparent;
            this.LblInstanceName.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInstanceName.ForeColor = System.Drawing.Color.White;
            this.LblInstanceName.Location = new System.Drawing.Point(258, 12);
            this.LblInstanceName.Name = "LblInstanceName";
            this.LblInstanceName.Size = new System.Drawing.Size(353, 28);
            this.LblInstanceName.TabIndex = 1;
            this.LblInstanceName.Text = "Nolvus Regular";
            // 
            // LblVersion
            // 
            this.LblVersion.AutoSize = true;
            this.LblVersion.BackColor = System.Drawing.Color.Transparent;
            this.LblVersion.ForeColor = System.Drawing.Color.White;
            this.LblVersion.Location = new System.Drawing.Point(260, 75);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(31, 13);
            this.LblVersion.TabIndex = 2;
            this.LblVersion.Text = "5.0.2";
            this.LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblStatus
            // 
            this.LblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStatus.BackColor = System.Drawing.Color.Transparent;
            this.LblStatus.ForeColor = System.Drawing.Color.White;
            this.LblStatus.Location = new System.Drawing.Point(260, 92);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(257, 14);
            this.LblStatus.TabIndex = 3;
            this.LblStatus.Text = "Loading status...";
            this.LblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnUpdate.BackColor = System.Drawing.Color.Black;
            this.BtnUpdate.BorderColor = System.Drawing.Color.White;
            this.BtnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnUpdate.ForeColor = System.Drawing.Color.White;
            this.BtnUpdate.Location = new System.Drawing.Point(319, 115);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnUpdate.Size = new System.Drawing.Size(92, 35);
            this.BtnUpdate.TabIndex = 5;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.UseVisualStyleBackColor = false;
            this.BtnUpdate.Visible = false;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnPlay
            // 
            this.BtnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPlay.BorderColor = System.Drawing.Color.White;
            this.BtnPlay.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPlay.ForeColor = System.Drawing.Color.White;
            this.BtnPlay.Location = new System.Drawing.Point(515, 115);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPlay.Size = new System.Drawing.Size(92, 35);
            this.BtnPlay.TabIndex = 4;
            this.BtnPlay.Text = "Play";
            this.BtnPlay.UseVisualStyleBackColor = false;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // PicInstanceImage
            // 
            this.PicInstanceImage.BackColor = System.Drawing.Color.Black;
            this.PicInstanceImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicInstanceImage.Location = new System.Drawing.Point(46, 20);
            this.PicInstanceImage.Name = "PicInstanceImage";
            this.PicInstanceImage.Size = new System.Drawing.Size(206, 128);
            this.PicInstanceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicInstanceImage.TabIndex = 0;
            this.PicInstanceImage.TabStop = false;
            // 
            // LblDesc
            // 
            this.LblDesc.AutoEllipsis = true;
            this.LblDesc.BackColor = System.Drawing.Color.Transparent;
            this.LblDesc.ForeColor = System.Drawing.Color.White;
            this.LblDesc.Location = new System.Drawing.Point(261, 42);
            this.LblDesc.Name = "LblDesc";
            this.LblDesc.Size = new System.Drawing.Size(368, 31);
            this.LblDesc.TabIndex = 6;
            this.LblDesc.Text = "[Description]";
            // 
            // BtnView
            // 
            this.BtnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnView.BorderColor = System.Drawing.Color.White;
            this.BtnView.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnView.ForeColor = System.Drawing.Color.White;
            this.BtnView.Location = new System.Drawing.Point(417, 115);
            this.BtnView.Name = "BtnView";
            this.BtnView.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnView.Size = new System.Drawing.Size(92, 35);
            this.BtnView.TabIndex = 7;
            this.BtnView.Text = "Manage";
            this.BtnView.UseVisualStyleBackColor = false;
            this.BtnView.Click += new System.EventHandler(this.BtnView_Click);
            // 
            // popupMenu1
            // 
            this.popupMenu1.ParentBarItem = this.parentBarItem1;
            this.popupMenu1.ThemeName = "Office2016Black";
            // 
            // parentBarItem1
            // 
            this.parentBarItem1.BarName = "parentBarItem1";
            this.parentBarItem1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.BrItmMods,
            this.BrItmShortCut,
            this.barItem1,
            this.BrItmReport,
            this.barItem2,
            this.BrItmKeyBinds,
            this.barItem3,
            this.BrItmManual,
            this.barItem4,
            this.BrItmENBManager,
            this.barItem6,
            this.BrItmDelete});
            this.parentBarItem1.MetroColor = System.Drawing.Color.LightSkyBlue;
            this.parentBarItem1.ShowToolTipInPopUp = false;
            this.parentBarItem1.SizeToFit = true;
            this.parentBarItem1.WrapLength = 20;
            // 
            // BrItmMods
            // 
            this.BrItmMods.BarName = "BrItmMods";
            this.BrItmMods.ID = "Instance";
            this.BrItmMods.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmMods.Image")));
            this.BrItmMods.ShowToolTipInPopUp = false;
            this.BrItmMods.SizeToFit = true;
            this.BrItmMods.Text = "Instance";
            this.BrItmMods.Click += new System.EventHandler(this.BrItmMods_Click);
            // 
            // BrItmShortCut
            // 
            this.BrItmShortCut.BarName = "BrItmShortCut";
            this.BrItmShortCut.ID = "Add a desktop shortcut for this instance";
            this.BrItmShortCut.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmShortCut.Image")));
            this.BrItmShortCut.ShowToolTipInPopUp = false;
            this.BrItmShortCut.SizeToFit = true;
            this.BrItmShortCut.Text = "Add a desktop shortcut for this instance";
            this.BrItmShortCut.Click += new System.EventHandler(this.BrItmShortCut_Click);
            // 
            // barItem1
            // 
            this.barItem1.BarName = "barItem1";
            this.barItem1.ID = "-";
            this.barItem1.ShowToolTipInPopUp = false;
            this.barItem1.SizeToFit = true;
            this.barItem1.Text = "-";
            // 
            // BrItmReport
            // 
            this.BrItmReport.BarName = "BrItmReport";
            this.BrItmReport.ID = "Report to PDF";
            this.BrItmReport.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmReport.Image")));
            this.BrItmReport.ShowToolTipInPopUp = false;
            this.BrItmReport.SizeToFit = true;
            this.BrItmReport.Text = "Report to PDF";
            this.BrItmReport.Click += new System.EventHandler(this.BrItmReport_Click);
            // 
            // barItem2
            // 
            this.barItem2.BarName = "barItem2";
            this.barItem2.ID = "-";
            this.barItem2.ShowToolTipInPopUp = false;
            this.barItem2.SizeToFit = true;
            this.barItem2.Text = "-";
            // 
            // BrItmKeyBinds
            // 
            this.BrItmKeyBinds.BarName = "BrItmKeyBinds";
            this.BrItmKeyBinds.ID = "Key Binds";
            this.BrItmKeyBinds.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmKeyBinds.Image")));
            this.BrItmKeyBinds.ShowToolTipInPopUp = false;
            this.BrItmKeyBinds.SizeToFit = true;
            this.BrItmKeyBinds.Text = "Key Binds";
            this.BrItmKeyBinds.Click += new System.EventHandler(this.BrItmKeyBinds_Click);
            // 
            // barItem3
            // 
            this.barItem3.BarName = "barItem3";
            this.barItem3.ID = "-";
            this.barItem3.ShowToolTipInPopUp = false;
            this.barItem3.SizeToFit = true;
            this.barItem3.Text = "-";
            // 
            // BrItmManual
            // 
            this.BrItmManual.BarName = "BrItmManual";
            this.BrItmManual.ID = "Manual";
            this.BrItmManual.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmManual.Image")));
            this.BrItmManual.ImageSize = new System.Drawing.Size(24, 24);
            this.BrItmManual.ShowToolTipInPopUp = false;
            this.BrItmManual.SizeToFit = true;
            this.BrItmManual.Text = "User Manual";
            this.BrItmManual.Click += new System.EventHandler(this.BrItmManual_Click);
            // 
            // barItem4
            // 
            this.barItem4.BarName = "barItem4";
            this.barItem4.ID = "-";
            this.barItem4.ShowToolTipInPopUp = false;
            this.barItem4.SizeToFit = true;
            this.barItem4.Text = "-";
            // 
            // BrItmENBManager
            // 
            this.BrItmENBManager.BarName = "BrItmENBManager";
            this.BrItmENBManager.ID = "ENB Manager";
            this.BrItmENBManager.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmENBManager.Image")));
            this.BrItmENBManager.ImageSize = new System.Drawing.Size(24, 24);
            this.BrItmENBManager.ShowToolTipInPopUp = false;
            this.BrItmENBManager.SizeToFit = true;
            this.BrItmENBManager.Text = "ENB Manager";
            this.BrItmENBManager.Click += new System.EventHandler(this.BrItmENBManager_Click);
            // 
            // barItem6
            // 
            this.barItem6.BarName = "barItem6";
            this.barItem6.ID = "-";
            this.barItem6.ShowToolTipInPopUp = false;
            this.barItem6.SizeToFit = true;
            this.barItem6.Text = "-";
            // 
            // BrItmDelete
            // 
            this.BrItmDelete.BarName = "BrItmDelete";
            this.BrItmDelete.ID = "Delete Instance";
            this.BrItmDelete.Image = ((Syncfusion.Windows.Forms.Tools.XPMenus.ImageExt)(resources.GetObject("BrItmDelete.Image")));
            this.BrItmDelete.ShowToolTipInPopUp = false;
            this.BrItmDelete.SizeToFit = true;
            this.BrItmDelete.Text = "Delete Instance";
            this.BrItmDelete.Click += new System.EventHandler(this.BrItmDelete_Click);
            // 
            // LblImageLoading
            // 
            this.LblImageLoading.BackColor = System.Drawing.Color.Black;
            this.LblImageLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblImageLoading.ForeColor = System.Drawing.Color.White;
            this.LblImageLoading.Location = new System.Drawing.Point(43, 20);
            this.LblImageLoading.Name = "LblImageLoading";
            this.LblImageLoading.Size = new System.Drawing.Size(209, 130);
            this.LblImageLoading.TabIndex = 8;
            this.LblImageLoading.Text = "Loading...";
            this.LblImageLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InstancePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BackgroundImage = global::Vcc.Nolvus.Dashboard.Properties.Resources.InstanceBoard;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.LblImageLoading);
            this.Controls.Add(this.BtnView);
            this.Controls.Add(this.LblDesc);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.BtnPlay);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this.LblInstanceName);
            this.Controls.Add(this.PicInstanceImage);
            this.DoubleBuffered = true;
            this.Name = "InstancePanel";
            this.Size = new System.Drawing.Size(659, 171);
            ((System.ComponentModel.ISupportInitialize)(this.PicInstanceImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicInstanceImage;
        private System.Windows.Forms.Label LblInstanceName;
        private System.Windows.Forms.Label LblVersion;
        private System.Windows.Forms.Label LblStatus;
        private Components.Controls.FlatButton BtnPlay;
        private Components.Controls.FlatButton BtnUpdate;
        private System.Windows.Forms.Label LblDesc;
        private Components.Controls.FlatButton BtnView;
        private Syncfusion.Windows.Forms.Tools.XPMenus.PopupMenusManager popupMenusManager1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.PopupMenu popupMenu1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmMods;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmDelete;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmShortCut;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmReport;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem2;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmKeyBinds;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem3;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmManual;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem4;
        private System.Windows.Forms.Label LblImageLoading;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem BrItmENBManager;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem6;
    }
}
