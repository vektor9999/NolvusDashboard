namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class PathFrame
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
            Syncfusion.Windows.Forms.Tools.ActiveStateCollection activeStateCollection3 = new Syncfusion.Windows.Forms.Tools.ActiveStateCollection();
            Syncfusion.Windows.Forms.Tools.InactiveStateCollection inactiveStateCollection3 = new Syncfusion.Windows.Forms.Tools.InactiveStateCollection();
            Syncfusion.Windows.Forms.Tools.SliderCollection sliderCollection3 = new Syncfusion.Windows.Forms.Tools.SliderCollection();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathFrame));
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PnlMessage = new System.Windows.Forms.Panel();
            this.LblMessage = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TglBtnEnableArchive = new Syncfusion.Windows.Forms.Tools.ToggleButton();
            this.BtnBrowseArchivePath = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnBrowseInstancePath = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.TxtBxArchivePath = new System.Windows.Forms.TextBox();
            this.TxtBxInstancePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnPrevious = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnContinue = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel2.SuspendLayout();
            this.PnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnEnableArchive)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.LblStepText.Size = new System.Drawing.Size(804, 20);
            this.LblStepText.TabIndex = 8;
            this.LblStepText.Text = "Instance Paths";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.PnlMessage);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.TglBtnEnableArchive);
            this.panel2.Controls.Add(this.BtnBrowseArchivePath);
            this.panel2.Controls.Add(this.BtnBrowseInstancePath);
            this.panel2.Controls.Add(this.TxtBxArchivePath);
            this.panel2.Controls.Add(this.TxtBxInstancePath);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(484, 374);
            this.panel2.TabIndex = 14;
            // 
            // PnlMessage
            // 
            this.PnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlMessage.BackColor = System.Drawing.Color.Orange;
            this.PnlMessage.Controls.Add(this.LblMessage);
            this.PnlMessage.Controls.Add(this.PicBox);
            this.PnlMessage.Location = new System.Drawing.Point(16, 154);
            this.PnlMessage.Name = "PnlMessage";
            this.PnlMessage.Size = new System.Drawing.Size(439, 45);
            this.PnlMessage.TabIndex = 31;
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(44, 7);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(388, 32);
            this.LblMessage.TabIndex = 1;
            this.LblMessage.Text = "Be sure the installation path is not too long to avoid issues (ex D:\\Nolvus)";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Enable Archiving";
            // 
            // TglBtnEnableArchive
            // 
            activeStateCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            activeStateCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(176)))));
            this.TglBtnEnableArchive.ActiveState = activeStateCollection3;
            this.TglBtnEnableArchive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TglBtnEnableArchive.ForeColor = System.Drawing.Color.Black;
            inactiveStateCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            inactiveStateCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            inactiveStateCollection3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            inactiveStateCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.TglBtnEnableArchive.InactiveState = inactiveStateCollection3;
            this.TglBtnEnableArchive.Location = new System.Drawing.Point(128, 99);
            this.TglBtnEnableArchive.MinimumSize = new System.Drawing.Size(52, 20);
            this.TglBtnEnableArchive.Name = "TglBtnEnableArchive";
            this.TglBtnEnableArchive.Size = new System.Drawing.Size(63, 24);
            sliderCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            sliderCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection3.InactiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            sliderCollection3.InactiveHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TglBtnEnableArchive.Slider = sliderCollection3;
            this.TglBtnEnableArchive.TabIndex = 27;
            this.TglBtnEnableArchive.Text = "toggleButton1";
            this.TglBtnEnableArchive.ThemeName = "Office2016Black";
            this.TglBtnEnableArchive.ToggleStateChanged += new Syncfusion.Windows.Forms.Tools.ToggleStateChangedEventHandler(this.TglBtnEnableArchive_ToggleStateChanged);
            // 
            // BtnBrowseArchivePath
            // 
            this.BtnBrowseArchivePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseArchivePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseArchivePath.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseArchivePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseArchivePath.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseArchivePath.Location = new System.Drawing.Point(435, 63);
            this.BtnBrowseArchivePath.Name = "BtnBrowseArchivePath";
            this.BtnBrowseArchivePath.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseArchivePath.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseArchivePath.TabIndex = 26;
            this.BtnBrowseArchivePath.Text = "...";
            this.BtnBrowseArchivePath.UseVisualStyleBackColor = false;
            this.BtnBrowseArchivePath.Click += new System.EventHandler(this.BtnBrowseArchivePath_Click);
            // 
            // BtnBrowseInstancePath
            // 
            this.BtnBrowseInstancePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseInstancePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseInstancePath.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseInstancePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseInstancePath.ForeColor = System.Drawing.Color.Orange;
            this.BtnBrowseInstancePath.Location = new System.Drawing.Point(435, 21);
            this.BtnBrowseInstancePath.Name = "BtnBrowseInstancePath";
            this.BtnBrowseInstancePath.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseInstancePath.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseInstancePath.TabIndex = 25;
            this.BtnBrowseInstancePath.Text = "...";
            this.BtnBrowseInstancePath.UseVisualStyleBackColor = false;
            this.BtnBrowseInstancePath.Click += new System.EventHandler(this.BtnBrowseInstancePath_Click);
            // 
            // TxtBxArchivePath
            // 
            this.TxtBxArchivePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxArchivePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxArchivePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxArchivePath.ForeColor = System.Drawing.Color.White;
            this.TxtBxArchivePath.Location = new System.Drawing.Point(128, 63);
            this.TxtBxArchivePath.Name = "TxtBxArchivePath";
            this.TxtBxArchivePath.ReadOnly = true;
            this.TxtBxArchivePath.Size = new System.Drawing.Size(301, 20);
            this.TxtBxArchivePath.TabIndex = 24;
            // 
            // TxtBxInstancePath
            // 
            this.TxtBxInstancePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxInstancePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxInstancePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxInstancePath.ForeColor = System.Drawing.Color.White;
            this.TxtBxInstancePath.Location = new System.Drawing.Point(128, 21);
            this.TxtBxInstancePath.Name = "TxtBxInstancePath";
            this.TxtBxInstancePath.ReadOnly = true;
            this.TxtBxInstancePath.Size = new System.Drawing.Size(301, 20);
            this.TxtBxInstancePath.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Archive Directory";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Instance Directory";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.BtnPrevious);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.BtnContinue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(497, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 374);
            this.panel1.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 80);
            this.label5.TabIndex = 5;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPrevious.BorderColor = System.Drawing.Color.White;
            this.BtnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPrevious.ForeColor = System.Drawing.Color.White;
            this.BtnPrevious.Location = new System.Drawing.Point(114, 319);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPrevious.Size = new System.Drawing.Size(88, 40);
            this.BtnPrevious.TabIndex = 4;
            this.BtnPrevious.Text = "Previous";
            this.BtnPrevious.UseVisualStyleBackColor = false;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(293, 55);
            this.label6.TabIndex = 2;
            this.label6.Text = "You can choose to disable archiving to save some disk space. WARNING if you don\'t" +
    " keep archive files and want to reinstall the mod list, the installer will downl" +
    "oad all mods again.";
            // 
            // BtnContinue
            // 
            this.BtnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnContinue.BorderColor = System.Drawing.Color.White;
            this.BtnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnContinue.ForeColor = System.Drawing.Color.White;
            this.BtnContinue.Location = new System.Drawing.Point(208, 319);
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
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 66);
            this.label4.TabIndex = 0;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // PathFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblStepText);
            this.Name = "PathFrame";
            this.Size = new System.Drawing.Size(810, 409);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PnlMessage.ResumeLayout(false);
            this.PnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnEnableArchive)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBxArchivePath;
        private System.Windows.Forms.TextBox TxtBxInstancePath;
        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label4;
        private Components.Controls.FlatButton BtnBrowseArchivePath;
        private Components.Controls.FlatButton BtnBrowseInstancePath;
        private Syncfusion.Windows.Forms.Tools.ToggleButton TglBtnEnableArchive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label6;
        private Components.Controls.FlatButton BtnPrevious;
        private System.Windows.Forms.Panel PnlMessage;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.Label label5;
    }
}
