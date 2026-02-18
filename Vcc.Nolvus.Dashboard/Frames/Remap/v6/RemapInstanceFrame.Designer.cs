namespace Vcc.Nolvus.Dashboard.Frames.Remap.v6
{
    partial class RemapInstanceFrame
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnBrowseInstancePath = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.TxtBxInstancePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LblCurrentInstallPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LblInstance = new System.Windows.Forms.Label();
            this.PnlMessage = new System.Windows.Forms.Panel();
            this.LblMessage = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnRemap = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.LblStepText.Size = new System.Drawing.Size(1020, 24);
            this.LblStepText.TabIndex = 16;
            this.LblStepText.Text = "Remap Instance Paths";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.PnlMessage);
            this.panel3.Controls.Add(this.BtnCancel);
            this.panel3.Controls.Add(this.BtnRemap);
            this.panel3.Location = new System.Drawing.Point(6, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1013, 632);
            this.panel3.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(448, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Select a new directory to move your instance (must be empty) and click on the Rem" +
    "ap button.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(837, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "This feature allows you to move your instance into an other drive or directory (o" +
    "n the same drive). Usefull if you want to shorten your install path or move your" +
    " instance elsewhere.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BtnBrowseInstancePath);
            this.groupBox1.Controls.Add(this.TxtBxInstancePath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.LblCurrentInstallPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LblInstance);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(13, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(982, 195);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instance Info";
            // 
            // BtnBrowseInstancePath
            // 
            this.BtnBrowseInstancePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseInstancePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBrowseInstancePath.BorderColor = System.Drawing.Color.White;
            this.BtnBrowseInstancePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBrowseInstancePath.Location = new System.Drawing.Point(929, 145);
            this.BtnBrowseInstancePath.Name = "BtnBrowseInstancePath";
            this.BtnBrowseInstancePath.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBrowseInstancePath.Size = new System.Drawing.Size(20, 20);
            this.BtnBrowseInstancePath.TabIndex = 34;
            this.BtnBrowseInstancePath.Text = "...";
            this.BtnBrowseInstancePath.UseVisualStyleBackColor = false;
            this.BtnBrowseInstancePath.Click += new System.EventHandler(this.BtnBrowseInstancePath_Click);
            // 
            // TxtBxInstancePath
            // 
            this.TxtBxInstancePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBxInstancePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxInstancePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxInstancePath.ForeColor = System.Drawing.Color.White;
            this.TxtBxInstancePath.Location = new System.Drawing.Point(19, 145);
            this.TxtBxInstancePath.Name = "TxtBxInstancePath";
            this.TxtBxInstancePath.ReadOnly = true;
            this.TxtBxInstancePath.Size = new System.Drawing.Size(904, 20);
            this.TxtBxInstancePath.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "New installation path (select an empty directory)";
            // 
            // LblCurrentInstallPath
            // 
            this.LblCurrentInstallPath.AutoSize = true;
            this.LblCurrentInstallPath.ForeColor = System.Drawing.Color.Orange;
            this.LblCurrentInstallPath.Location = new System.Drawing.Point(151, 72);
            this.LblCurrentInstallPath.Name = "LblCurrentInstallPath";
            this.LblCurrentInstallPath.Size = new System.Drawing.Size(35, 13);
            this.LblCurrentInstallPath.TabIndex = 31;
            this.LblCurrentInstallPath.Text = "[Path]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Current Installation Path";
            // 
            // LblInstance
            // 
            this.LblInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInstance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblInstance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInstance.ForeColor = System.Drawing.Color.White;
            this.LblInstance.Location = new System.Drawing.Point(15, 26);
            this.LblInstance.Name = "LblInstance";
            this.LblInstance.Size = new System.Drawing.Size(941, 24);
            this.LblInstance.TabIndex = 29;
            this.LblInstance.Text = "Instance Paths";
            this.LblInstance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlMessage
            // 
            this.PnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlMessage.BackColor = System.Drawing.Color.Orange;
            this.PnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlMessage.Controls.Add(this.LblMessage);
            this.PnlMessage.Controls.Add(this.PicBox);
            this.PnlMessage.Location = new System.Drawing.Point(13, 319);
            this.PnlMessage.Name = "PnlMessage";
            this.PnlMessage.Size = new System.Drawing.Size(982, 75);
            this.PnlMessage.TabIndex = 22;
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(44, 7);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(929, 53);
            this.LblMessage.TabIndex = 1;
            this.LblMessage.Text = "DO NOT CLOSE YOUR DASHBOARD DURING THIS OPERATION, YOU WILL BREAK YOUR INSTANCE!!" +
    "!";
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PicBox
            // 
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.PicBox.Location = new System.Drawing.Point(6, 16);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(32, 32);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 0;
            this.PicBox.TabStop = false;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.White;
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.Location = new System.Drawing.Point(823, 587);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(88, 40);
            this.BtnCancel.TabIndex = 7;
            this.BtnCancel.Text = "Back";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnRemap
            // 
            this.BtnRemap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRemap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnRemap.BorderColor = System.Drawing.Color.White;
            this.BtnRemap.ForeColor = System.Drawing.Color.White;
            this.BtnRemap.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnRemap.Location = new System.Drawing.Point(917, 587);
            this.BtnRemap.Name = "BtnRemap";
            this.BtnRemap.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnRemap.Size = new System.Drawing.Size(88, 40);
            this.BtnRemap.TabIndex = 6;
            this.BtnRemap.Text = "Remap";
            this.BtnRemap.UseVisualStyleBackColor = false;
            this.BtnRemap.Click += new System.EventHandler(this.BtnRemap_Click);
            // 
            // RemapInstanceFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.LblStepText);
            this.Name = "RemapInstanceFrame";
            this.Size = new System.Drawing.Size(1026, 667);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PnlMessage.ResumeLayout(false);
            this.PnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Components.Controls.FlatButton BtnRemap;
        private Components.Controls.FlatButton BtnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel PnlMessage;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LblInstance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblCurrentInstallPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtBxInstancePath;
        private Components.Controls.FlatButton BtnBrowseInstancePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
