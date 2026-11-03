namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class SummaryFrame
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
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn1 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn2 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn3 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.RdBtnNoThreshold = new System.Windows.Forms.RadioButton();
            this.RdBtnThreshold = new System.Windows.Forms.RadioButton();
            this.RdBtnOneError = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnPrevious = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnStart = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.OptionsGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LblLanguage = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.LblCDN = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.LblIni = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.LblDownscaling = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblEnableArchiving = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LblArchiveDir = new System.Windows.Forms.Label();
            this.LblInstallDir = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LblVariant = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LblResolution = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.PnlMessage = new System.Windows.Forms.Panel();
            this.LblMessage = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptionsGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.PnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.SuspendLayout();
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
            this.LblStepText.Size = new System.Drawing.Size(1104, 20);
            this.LblStepText.TabIndex = 8;
            this.LblStepText.Text = "Installation Summary";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.PnlMessage);
            this.panel1.Location = new System.Drawing.Point(7, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 802);
            this.panel1.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.RdBtnNoThreshold);
            this.panel4.Controls.Add(this.RdBtnThreshold);
            this.panel4.Controls.Add(this.RdBtnOneError);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Location = new System.Drawing.Point(0, 442);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(650, 360);
            this.panel4.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 81;
            this.label2.Text = "Action to take when an error occurs";
            // 
            // RdBtnNoThreshold
            // 
            this.RdBtnNoThreshold.AutoSize = true;
            this.RdBtnNoThreshold.ForeColor = System.Drawing.Color.White;
            this.RdBtnNoThreshold.Location = new System.Drawing.Point(30, 138);
            this.RdBtnNoThreshold.Name = "RdBtnNoThreshold";
            this.RdBtnNoThreshold.Size = new System.Drawing.Size(442, 17);
            this.RdBtnNoThreshold.TabIndex = 80;
            this.RdBtnNoThreshold.Text = "Don\'t stop installation and display all error messages at the end of the process " +
    "(overnight)";
            this.RdBtnNoThreshold.UseVisualStyleBackColor = true;
            // 
            // RdBtnThreshold
            // 
            this.RdBtnThreshold.AutoSize = true;
            this.RdBtnThreshold.Checked = true;
            this.RdBtnThreshold.ForeColor = System.Drawing.Color.White;
            this.RdBtnThreshold.Location = new System.Drawing.Point(30, 108);
            this.RdBtnThreshold.Name = "RdBtnThreshold";
            this.RdBtnThreshold.Size = new System.Drawing.Size(85, 17);
            this.RdBtnThreshold.TabIndex = 79;
            this.RdBtnThreshold.TabStop = true;
            this.RdBtnThreshold.Text = "radioButton2";
            this.RdBtnThreshold.UseVisualStyleBackColor = true;
            // 
            // RdBtnOneError
            // 
            this.RdBtnOneError.AutoSize = true;
            this.RdBtnOneError.ForeColor = System.Drawing.Color.White;
            this.RdBtnOneError.Location = new System.Drawing.Point(30, 78);
            this.RdBtnOneError.Name = "RdBtnOneError";
            this.RdBtnOneError.Size = new System.Drawing.Size(526, 17);
            this.RdBtnOneError.TabIndex = 78;
            this.RdBtnOneError.Text = "Stop installation and display the error message (recommended if you don\'t have a " +
    "Nexus Premium account)";
            this.RdBtnOneError.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Orange;
            this.label13.Location = new System.Drawing.Point(5, 5);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(160, 21);
            this.label13.TabIndex = 77;
            this.label13.Text = "Errors Management";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.BtnPrevious);
            this.panel3.Controls.Add(this.BtnStart);
            this.panel3.Controls.Add(this.OptionsGrid);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Location = new System.Drawing.Point(659, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(431, 802);
            this.panel3.TabIndex = 39;
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPrevious.BorderColor = System.Drawing.Color.Gray;
            this.BtnPrevious.ForeColor = System.Drawing.Color.White;
            this.BtnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPrevious.Location = new System.Drawing.Point(234, 747);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPrevious.Size = new System.Drawing.Size(88, 40);
            this.BtnPrevious.TabIndex = 79;
            this.BtnPrevious.Text = "Previous";
            this.BtnPrevious.UseVisualStyleBackColor = false;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnStart.BorderColor = System.Drawing.Color.Gray;
            this.BtnStart.ForeColor = System.Drawing.Color.White;
            this.BtnStart.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnStart.Location = new System.Drawing.Point(329, 747);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnStart.Size = new System.Drawing.Size(88, 40);
            this.BtnStart.TabIndex = 78;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // OptionsGrid
            // 
            this.OptionsGrid.AccessibleName = "Table";
            this.OptionsGrid.AllowEditing = false;
            this.OptionsGrid.AllowResizingColumns = true;
            this.OptionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsGrid.AutoGenerateColumns = false;
            this.OptionsGrid.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            gridTextColumn1.AllowEditing = false;
            gridTextColumn1.AllowResizing = true;
            gridTextColumn1.HeaderText = "Name";
            gridTextColumn1.MappingName = "FriendlyName";
            gridTextColumn1.Width = 200D;
            gridTextColumn2.AllowEditing = false;
            gridTextColumn2.AllowResizing = true;
            gridTextColumn2.HeaderText = "Category";
            gridTextColumn2.MappingName = "Category";
            gridTextColumn2.Visible = false;
            gridTextColumn3.AllowEditing = false;
            gridTextColumn3.AllowResizing = true;
            gridTextColumn3.HeaderText = "Value";
            gridTextColumn3.MappingName = "FormattedValue";
            gridTextColumn3.Width = 135D;
            this.OptionsGrid.Columns.Add(gridTextColumn1);
            this.OptionsGrid.Columns.Add(gridTextColumn2);
            this.OptionsGrid.Columns.Add(gridTextColumn3);
            this.OptionsGrid.HeaderRowHeight = 0;
            this.OptionsGrid.Location = new System.Drawing.Point(11, 45);
            this.OptionsGrid.Name = "OptionsGrid";
            this.OptionsGrid.Size = new System.Drawing.Size(406, 690);
            this.OptionsGrid.TabIndex = 77;
            this.OptionsGrid.Text = "sfDataGrid1";
            this.OptionsGrid.ThemeName = "Office2016Black";
            this.OptionsGrid.DrawCell += new Syncfusion.WinForms.DataGrid.Events.DrawCellEventHandler(this.OptionsGrid_DrawCell);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Orange;
            this.label9.Location = new System.Drawing.Point(5, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 21);
            this.label9.TabIndex = 76;
            this.label9.Text = "Options";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.LblLanguage);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.LblCDN);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.LblIni);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.LblDownscaling);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.LblEnableArchiving);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.LblArchiveDir);
            this.panel2.Controls.Add(this.LblInstallDir);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.LblVariant);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.LblResolution);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.LblName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(650, 345);
            this.panel2.TabIndex = 34;
            // 
            // LblLanguage
            // 
            this.LblLanguage.AutoSize = true;
            this.LblLanguage.ForeColor = System.Drawing.Color.Orange;
            this.LblLanguage.Location = new System.Drawing.Point(142, 269);
            this.LblLanguage.Name = "LblLanguage";
            this.LblLanguage.Size = new System.Drawing.Size(61, 13);
            this.LblLanguage.TabIndex = 97;
            this.LblLanguage.Text = "[Language]";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(10, 269);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 96;
            this.label14.Text = "Language";
            // 
            // LblCDN
            // 
            this.LblCDN.AutoSize = true;
            this.LblCDN.ForeColor = System.Drawing.Color.Orange;
            this.LblCDN.Location = new System.Drawing.Point(142, 244);
            this.LblCDN.Name = "LblCDN";
            this.LblCDN.Size = new System.Drawing.Size(36, 13);
            this.LblCDN.TabIndex = 95;
            this.LblCDN.Text = "[CDN]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(10, 244);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 94;
            this.label10.Text = "CDN";
            // 
            // LblIni
            // 
            this.LblIni.AutoSize = true;
            this.LblIni.ForeColor = System.Drawing.Color.Orange;
            this.LblIni.Location = new System.Drawing.Point(142, 219);
            this.LblIni.Name = "LblIni";
            this.LblIni.Size = new System.Drawing.Size(24, 13);
            this.LblIni.TabIndex = 93;
            this.LblIni.Text = "[Ini]";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(10, 219);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 92;
            this.label11.Text = "Skyrim Ini";
            // 
            // LblDownscaling
            // 
            this.LblDownscaling.AutoSize = true;
            this.LblDownscaling.ForeColor = System.Drawing.Color.Orange;
            this.LblDownscaling.Location = new System.Drawing.Point(142, 195);
            this.LblDownscaling.Name = "LblDownscaling";
            this.LblDownscaling.Size = new System.Drawing.Size(74, 13);
            this.LblDownscaling.TabIndex = 89;
            this.LblDownscaling.Text = "[Downscaling]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 88;
            this.label3.Text = "Donwscaling";
            // 
            // LblEnableArchiving
            // 
            this.LblEnableArchiving.AutoSize = true;
            this.LblEnableArchiving.ForeColor = System.Drawing.Color.Orange;
            this.LblEnableArchiving.Location = new System.Drawing.Point(142, 170);
            this.LblEnableArchiving.Name = "LblEnableArchiving";
            this.LblEnableArchiving.Size = new System.Drawing.Size(93, 13);
            this.LblEnableArchiving.TabIndex = 87;
            this.LblEnableArchiving.Text = "[Enable Archiving]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(10, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 86;
            this.label6.Text = "Enable archving";
            // 
            // LblArchiveDir
            // 
            this.LblArchiveDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblArchiveDir.AutoEllipsis = true;
            this.LblArchiveDir.ForeColor = System.Drawing.Color.Orange;
            this.LblArchiveDir.Location = new System.Drawing.Point(142, 145);
            this.LblArchiveDir.Name = "LblArchiveDir";
            this.LblArchiveDir.Size = new System.Drawing.Size(500, 13);
            this.LblArchiveDir.TabIndex = 85;
            this.LblArchiveDir.Text = "[ArchiveDir]";
            // 
            // LblInstallDir
            // 
            this.LblInstallDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInstallDir.AutoEllipsis = true;
            this.LblInstallDir.ForeColor = System.Drawing.Color.Orange;
            this.LblInstallDir.Location = new System.Drawing.Point(142, 120);
            this.LblInstallDir.Name = "LblInstallDir";
            this.LblInstallDir.Size = new System.Drawing.Size(500, 13);
            this.LblInstallDir.TabIndex = 84;
            this.LblInstallDir.Text = "[InstallDir]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 83;
            this.label5.Text = "Archive directory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 82;
            this.label4.Text = "Installation directory";
            // 
            // LblVariant
            // 
            this.LblVariant.AutoSize = true;
            this.LblVariant.ForeColor = System.Drawing.Color.Orange;
            this.LblVariant.Location = new System.Drawing.Point(142, 95);
            this.LblVariant.Name = "LblVariant";
            this.LblVariant.Size = new System.Drawing.Size(46, 13);
            this.LblVariant.TabIndex = 81;
            this.LblVariant.Text = "[Variant]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 80;
            this.label7.Text = "Variant";
            // 
            // LblResolution
            // 
            this.LblResolution.AutoSize = true;
            this.LblResolution.ForeColor = System.Drawing.Color.Orange;
            this.LblResolution.Location = new System.Drawing.Point(142, 70);
            this.LblResolution.Name = "LblResolution";
            this.LblResolution.Size = new System.Drawing.Size(63, 13);
            this.LblResolution.TabIndex = 79;
            this.LblResolution.Text = "[Resolution]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(10, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 78;
            this.label8.Text = "Resolution";
            // 
            // LblName
            // 
            this.LblName.AutoSize = true;
            this.LblName.ForeColor = System.Drawing.Color.Orange;
            this.LblName.Location = new System.Drawing.Point(142, 45);
            this.LblName.Name = "LblName";
            this.LblName.Size = new System.Drawing.Size(41, 13);
            this.LblName.TabIndex = 77;
            this.LblName.Text = "[Name]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 76;
            this.label1.Text = "Instance Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Orange;
            this.label12.Location = new System.Drawing.Point(5, 5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 21);
            this.label12.TabIndex = 75;
            this.label12.Text = "Settings";
            // 
            // PnlMessage
            // 
            this.PnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlMessage.BackColor = System.Drawing.Color.Orange;
            this.PnlMessage.Controls.Add(this.LblMessage);
            this.PnlMessage.Controls.Add(this.PicBox);
            this.PnlMessage.Location = new System.Drawing.Point(1, 369);
            this.PnlMessage.Name = "PnlMessage";
            this.PnlMessage.Size = new System.Drawing.Size(649, 45);
            this.PnlMessage.TabIndex = 20;
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(44, 7);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(598, 32);
            this.LblMessage.TabIndex = 1;
            this.LblMessage.Text = "[Message]";
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PicBox
            // 
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.PicBox.Location = new System.Drawing.Point(6, 7);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(32, 32);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 0;
            this.PicBox.TabStop = false;
            // 
            // SummaryFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblStepText);
            this.Name = "SummaryFrame";
            this.Size = new System.Drawing.Size(1107, 837);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptionsGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PnlMessage.ResumeLayout(false);
            this.PnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PnlMessage;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LblResolution;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LblVariant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LblInstallDir;
        private System.Windows.Forms.Label LblArchiveDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LblEnableArchiving;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblDownscaling;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label LblIni;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private Syncfusion.WinForms.DataGrid.SfDataGrid OptionsGrid;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label LblCDN;
        private System.Windows.Forms.Label LblLanguage;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton RdBtnOneError;
        private System.Windows.Forms.RadioButton RdBtnThreshold;
        private System.Windows.Forms.RadioButton RdBtnNoThreshold;
        private System.Windows.Forms.Label label2;
        private Components.Controls.FlatButton BtnStart;
        private Components.Controls.FlatButton BtnPrevious;
    }
}
