namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class SelectInstanceFrame
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
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnContinue = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DrpDwnLg = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LblDesc = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LblVRAM = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LblStorage = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LblRAM = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LblGPU = new System.Windows.Forms.Label();
            this.LblCPU = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DrpDwnLstRatios = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DrpDwnLstScreenRes = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DrpDwnLstGuides = new Syncfusion.WinForms.ListView.SfComboBox();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLg)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstRatios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstScreenRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstGuides)).BeginInit();
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
            this.LblStepText.Size = new System.Drawing.Size(965, 20);
            this.LblStepText.TabIndex = 7;
            this.LblStepText.Text = "Instance Prerequisites";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.BtnContinue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(655, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 686);
            this.panel1.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(3, 89);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(293, 49);
            this.label11.TabIndex = 3;
            this.label11.Text = "Language only affects base game not the mods. Be sure you have setup the same lan" +
    "guage for Skyrim SE in steam too.";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 27);
            this.label5.TabIndex = 2;
            this.label5.Text = "Please select your Nolvus instance as well as your screen resolution and ratio.";
            // 
            // BtnContinue
            // 
            this.BtnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnContinue.BorderColor = System.Drawing.Color.White;
            this.BtnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnContinue.ForeColor = System.Drawing.Color.White;
            this.BtnContinue.Location = new System.Drawing.Point(208, 631);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnContinue.Size = new System.Drawing.Size(88, 40);
            this.BtnContinue.TabIndex = 1;
            this.BtnContinue.Text = "Continue";
            this.BtnContinue.UseVisualStyleBackColor = false;
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Welcome to Nolvus mod list installation.";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.DrpDwnLg);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.DrpDwnLstRatios);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.DrpDwnLstScreenRes);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.DrpDwnLstGuides);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(642, 686);
            this.panel2.TabIndex = 13;
            // 
            // DrpDwnLg
            // 
            this.DrpDwnLg.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLg.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLg.Location = new System.Drawing.Point(114, 136);
            this.DrpDwnLg.Name = "DrpDwnLg";
            this.DrpDwnLg.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLg.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLg.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLg.TabIndex = 29;
            this.DrpDwnLg.ThemeName = "Office2016Black";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(13, 142);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Language";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.LblDesc);
            this.groupBox2.Controls.Add(this.PicBox);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(16, 347);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(601, 321);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // LblDesc
            // 
            this.LblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblDesc.Location = new System.Drawing.Point(10, 16);
            this.LblDesc.Name = "LblDesc";
            this.LblDesc.Size = new System.Drawing.Size(580, 39);
            this.LblDesc.TabIndex = 29;
            this.LblDesc.Text = "label10";
            this.LblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PicBox
            // 
            this.PicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Nolvus_V5;
            this.PicBox.Location = new System.Drawing.Point(9, 60);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(583, 251);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBox.TabIndex = 28;
            this.PicBox.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.LblVRAM);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.LblStorage);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.LblRAM);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.LblGPU);
            this.groupBox1.Controls.Add(this.LblCPU);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(16, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(601, 162);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hardware requirements";
            // 
            // LblVRAM
            // 
            this.LblVRAM.AutoSize = true;
            this.LblVRAM.ForeColor = System.Drawing.Color.Orange;
            this.LblVRAM.Location = new System.Drawing.Point(56, 108);
            this.LblVRAM.Name = "LblVRAM";
            this.LblVRAM.Size = new System.Drawing.Size(44, 13);
            this.LblVRAM.TabIndex = 9;
            this.LblVRAM.Text = "[VRAM]";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 108);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "VRAM";
            // 
            // LblStorage
            // 
            this.LblStorage.AutoSize = true;
            this.LblStorage.ForeColor = System.Drawing.Color.Orange;
            this.LblStorage.Location = new System.Drawing.Point(56, 133);
            this.LblStorage.Name = "LblStorage";
            this.LblStorage.Size = new System.Drawing.Size(65, 13);
            this.LblStorage.TabIndex = 7;
            this.LblStorage.Text = "[STORAGE]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Storage";
            // 
            // LblRAM
            // 
            this.LblRAM.AutoSize = true;
            this.LblRAM.ForeColor = System.Drawing.Color.Orange;
            this.LblRAM.Location = new System.Drawing.Point(56, 82);
            this.LblRAM.Name = "LblRAM";
            this.LblRAM.Size = new System.Drawing.Size(37, 13);
            this.LblRAM.TabIndex = 5;
            this.LblRAM.Text = "[RAM]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "RAM";
            // 
            // LblGPU
            // 
            this.LblGPU.AutoSize = true;
            this.LblGPU.ForeColor = System.Drawing.Color.Orange;
            this.LblGPU.Location = new System.Drawing.Point(56, 55);
            this.LblGPU.Name = "LblGPU";
            this.LblGPU.Size = new System.Drawing.Size(36, 13);
            this.LblGPU.TabIndex = 3;
            this.LblGPU.Text = "[GPU]";
            // 
            // LblCPU
            // 
            this.LblCPU.AutoSize = true;
            this.LblCPU.ForeColor = System.Drawing.Color.Orange;
            this.LblCPU.Location = new System.Drawing.Point(57, 29);
            this.LblCPU.Name = "LblCPU";
            this.LblCPU.Size = new System.Drawing.Size(35, 13);
            this.LblCPU.TabIndex = 2;
            this.LblCPU.Text = "[CPU]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "GPU";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "CPU";
            // 
            // DrpDwnLstRatios
            // 
            this.DrpDwnLstRatios.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstRatios.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstRatios.Location = new System.Drawing.Point(114, 97);
            this.DrpDwnLstRatios.Name = "DrpDwnLstRatios";
            this.DrpDwnLstRatios.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstRatios.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLstRatios.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstRatios.TabIndex = 25;
            this.DrpDwnLstRatios.ThemeName = "Office2016Black";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Screen ratio";
            // 
            // DrpDwnLstScreenRes
            // 
            this.DrpDwnLstScreenRes.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstScreenRes.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstScreenRes.Location = new System.Drawing.Point(114, 58);
            this.DrpDwnLstScreenRes.Name = "DrpDwnLstScreenRes";
            this.DrpDwnLstScreenRes.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstScreenRes.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLstScreenRes.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstScreenRes.TabIndex = 23;
            this.DrpDwnLstScreenRes.ThemeName = "Office2016Black";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Resolution";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Version to install";
            // 
            // DrpDwnLstGuides
            // 
            this.DrpDwnLstGuides.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstGuides.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstGuides.Location = new System.Drawing.Point(114, 18);
            this.DrpDwnLstGuides.Name = "DrpDwnLstGuides";
            this.DrpDwnLstGuides.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstGuides.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLstGuides.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstGuides.TabIndex = 20;
            this.DrpDwnLstGuides.ThemeName = "Office2016Black";
            this.DrpDwnLstGuides.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstGuides_SelectedIndexChanged);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.White;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(114, 631);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(88, 40);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Visible = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SelectInstanceFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblStepText);
            this.Name = "SelectInstanceFrame";
            this.Size = new System.Drawing.Size(968, 721);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLg)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstRatios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstScreenRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstGuides)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstRatios;
        private System.Windows.Forms.Label label3;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstScreenRes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstGuides;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LblGPU;
        private System.Windows.Forms.Label LblCPU;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LblRAM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LblStorage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.Label LblDesc;
        private System.Windows.Forms.Label label11;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLg;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label LblVRAM;
        private System.Windows.Forms.Label label13;
        private Components.Controls.FlatButton BtnCancel;
    }
}
