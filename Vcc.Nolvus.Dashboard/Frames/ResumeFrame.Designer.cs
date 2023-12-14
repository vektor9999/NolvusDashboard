namespace Vcc.Nolvus.Dashboard.Frames
{ 
    partial class ResumeFrame
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.DrpDwnLstDownLoc = new Syncfusion.WinForms.ListView.SfComboBox();
            this.LblDownLoc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DrpDwnLstInstances = new Syncfusion.WinForms.ListView.SfComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label5 = new System.Windows.Forms.Label();
            this.Resume = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RdBtnNoThreshold = new System.Windows.Forms.RadioButton();
            this.RdBtnThreshold = new System.Windows.Forms.RadioButton();
            this.RdBtnOneError = new System.Windows.Forms.RadioButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstDownLoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstInstances)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.DrpDwnLstDownLoc);
            this.panel2.Controls.Add(this.LblDownLoc);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.DrpDwnLstInstances);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(707, 642);
            this.panel2.TabIndex = 16;
            // 
            // DrpDwnLstDownLoc
            // 
            this.DrpDwnLstDownLoc.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstDownLoc.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstDownLoc.Location = new System.Drawing.Point(116, 55);
            this.DrpDwnLstDownLoc.Name = "DrpDwnLstDownLoc";
            this.DrpDwnLstDownLoc.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstDownLoc.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstDownLoc.TabIndex = 33;
            this.DrpDwnLstDownLoc.ThemeName = "Office2016Black";
            this.DrpDwnLstDownLoc.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstDownLoc_SelectedIndexChanged);
            // 
            // LblDownLoc
            // 
            this.LblDownLoc.AutoSize = true;
            this.LblDownLoc.ForeColor = System.Drawing.Color.White;
            this.LblDownLoc.Location = new System.Drawing.Point(13, 62);
            this.LblDownLoc.Name = "LblDownLoc";
            this.LblDownLoc.Size = new System.Drawing.Size(95, 13);
            this.LblDownLoc.TabIndex = 32;
            this.LblDownLoc.Text = "Download location";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Instance to resume";
            // 
            // DrpDwnLstInstances
            // 
            this.DrpDwnLstInstances.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstInstances.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstInstances.Location = new System.Drawing.Point(116, 17);
            this.DrpDwnLstInstances.Name = "DrpDwnLstInstances";
            this.DrpDwnLstInstances.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstInstances.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstInstances.TabIndex = 20;
            this.DrpDwnLstInstances.ThemeName = "Office2016Black";
            this.DrpDwnLstInstances.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstInstances_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.Resume);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(720, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 642);
            this.panel1.TabIndex = 15;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.White;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(148, 587);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(88, 40);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(336, 35);
            this.label5.TabIndex = 2;
            this.label5.Text = "A previous instance installation has not been completed.";
            // 
            // Resume
            // 
            this.Resume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Resume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Resume.BorderColor = System.Drawing.Color.White;
            this.Resume.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.Resume.ForeColor = System.Drawing.Color.White;
            this.Resume.Location = new System.Drawing.Point(242, 587);
            this.Resume.Name = "Resume";
            this.Resume.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.Resume.Size = new System.Drawing.Size(88, 40);
            this.Resume.TabIndex = 1;
            this.Resume.Text = "Resume";
            this.Resume.UseVisualStyleBackColor = false;
            this.Resume.Click += new System.EventHandler(this.Resume_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(336, 35);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select the instance you want to resume and click on Resume";
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
            this.LblStepText.Size = new System.Drawing.Size(1065, 20);
            this.LblStepText.TabIndex = 14;
            this.LblStepText.Text = "Resume Nolvus Instance installation";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.RdBtnNoThreshold);
            this.groupBox4.Controls.Add(this.RdBtnThreshold);
            this.groupBox4.Controls.Add(this.RdBtnOneError);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(16, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(673, 163);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Errors Management";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(22, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action to take when an error occurs";
            // 
            // RdBtnNoThreshold
            // 
            this.RdBtnNoThreshold.AutoSize = true;
            this.RdBtnNoThreshold.Location = new System.Drawing.Point(25, 123);
            this.RdBtnNoThreshold.Name = "RdBtnNoThreshold";
            this.RdBtnNoThreshold.Size = new System.Drawing.Size(442, 17);
            this.RdBtnNoThreshold.TabIndex = 2;
            this.RdBtnNoThreshold.Text = "Don\'t stop installation and display all error messages at the end of the process " +
    "(overnight)";
            this.RdBtnNoThreshold.UseVisualStyleBackColor = true;
            // 
            // RdBtnThreshold
            // 
            this.RdBtnThreshold.AutoSize = true;
            this.RdBtnThreshold.Checked = true;
            this.RdBtnThreshold.Location = new System.Drawing.Point(25, 88);
            this.RdBtnThreshold.Name = "RdBtnThreshold";
            this.RdBtnThreshold.Size = new System.Drawing.Size(85, 17);
            this.RdBtnThreshold.TabIndex = 1;
            this.RdBtnThreshold.TabStop = true;
            this.RdBtnThreshold.Text = "radioButton2";
            this.RdBtnThreshold.UseVisualStyleBackColor = true;
            // 
            // RdBtnOneError
            // 
            this.RdBtnOneError.AutoSize = true;
            this.RdBtnOneError.ForeColor = System.Drawing.Color.White;
            this.RdBtnOneError.Location = new System.Drawing.Point(24, 56);
            this.RdBtnOneError.Name = "RdBtnOneError";
            this.RdBtnOneError.Size = new System.Drawing.Size(526, 17);
            this.RdBtnOneError.TabIndex = 0;
            this.RdBtnOneError.Text = "Stop installation and display the error message (recommended if you don\'t have a " +
    "Nexus Premium account)";
            this.RdBtnOneError.UseVisualStyleBackColor = true;
            // 
            // ResumeFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblStepText);
            this.Name = "ResumeFrame";
            this.Size = new System.Drawing.Size(1071, 679);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstDownLoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstInstances)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstInstances;
        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton Resume;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Label label5;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstDownLoc;
        private System.Windows.Forms.Label LblDownLoc;
        private Components.Controls.FlatButton BtnCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton RdBtnNoThreshold;
        private System.Windows.Forms.RadioButton RdBtnThreshold;
        private System.Windows.Forms.RadioButton RdBtnOneError;
    }
}
