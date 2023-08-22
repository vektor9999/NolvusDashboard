namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class ENBFrame
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnPrevious = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnContinue = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LnkLblENB = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DrpDwnLstENB = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstENB)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnPrevious);
            this.panel1.Controls.Add(this.BtnContinue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(654, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 531);
            this.panel1.TabIndex = 18;
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPrevious.BorderColor = System.Drawing.Color.White;
            this.BtnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPrevious.ForeColor = System.Drawing.Color.White;
            this.BtnPrevious.Location = new System.Drawing.Point(114, 476);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPrevious.Size = new System.Drawing.Size(88, 40);
            this.BtnPrevious.TabIndex = 4;
            this.BtnPrevious.Text = "Previous";
            this.BtnPrevious.UseVisualStyleBackColor = false;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnContinue
            // 
            this.BtnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnContinue.BorderColor = System.Drawing.Color.White;
            this.BtnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnContinue.ForeColor = System.Drawing.Color.White;
            this.BtnContinue.Location = new System.Drawing.Point(208, 476);
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
            this.label4.Size = new System.Drawing.Size(293, 54);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select the ENB you want to install. PI-CHO is the default Nolvus ENB preset and t" +
    "he one that works better with the list.";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.LnkLblENB);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.DrpDwnLstENB);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(641, 531);
            this.panel2.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Orange;
            this.label7.Location = new System.Drawing.Point(13, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(511, 35);
            this.label7.TabIndex = 47;
            this.label7.Text = "BE SURE TO FOLLOW THIS TUTORIAL AFTER THE LIST IS INSTALLED.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(66, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(224, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "for the tutorial on how to install Cabbage ENB.";
            // 
            // LnkLblENB
            // 
            this.LnkLblENB.AutoSize = true;
            this.LnkLblENB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LnkLblENB.ForeColor = System.Drawing.Color.Orange;
            this.LnkLblENB.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LnkLblENB.LinkColor = System.Drawing.Color.Orange;
            this.LnkLblENB.Location = new System.Drawing.Point(40, 182);
            this.LnkLblENB.Name = "LnkLblENB";
            this.LnkLblENB.Size = new System.Drawing.Size(28, 13);
            this.LnkLblENB.TabIndex = 45;
            this.LnkLblENB.TabStop = true;
            this.LnkLblENB.Text = "here";
            this.LnkLblENB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLblENB_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "Click";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(511, 29);
            this.label2.TabIndex = 43;
            this.label2.Text = "A preset with Cabbage ENB is also available, but the Cabbage ENB must be installe" +
    "d manually.";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Orange;
            this.label3.Location = new System.Drawing.Point(13, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(511, 35);
            this.label3.TabIndex = 42;
            this.label3.Text = "This application will auto install ENB and Reshade. Don\'t change ENB and Reshade " +
    "presets if you want to keep the intended look.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.pictureBox1.Location = new System.Drawing.Point(16, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // DrpDwnLstENB
            // 
            this.DrpDwnLstENB.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstENB.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstENB.Location = new System.Drawing.Point(48, 17);
            this.DrpDwnLstENB.Name = "DrpDwnLstENB";
            this.DrpDwnLstENB.Size = new System.Drawing.Size(355, 28);
            this.DrpDwnLstENB.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstENB.TabIndex = 32;
            this.DrpDwnLstENB.ThemeName = "Office2016Black";
            this.DrpDwnLstENB.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstENB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ENB";
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
            this.LblStepText.Size = new System.Drawing.Size(961, 20);
            this.LblStepText.TabIndex = 16;
            this.LblStepText.Text = "Select your ENB";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ENBFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblStepText);
            this.Name = "ENBFrame";
            this.Size = new System.Drawing.Size(967, 566);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstENB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnPrevious;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Label label1;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstENB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel LnkLblENB;
    }
}
