namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class CDNFrame
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
            this.DrpDwnLstDownLoc = new Syncfusion.WinForms.ListView.SfComboBox();
            this.LblDownLoc = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstDownLoc)).BeginInit();
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
            this.panel1.Location = new System.Drawing.Point(497, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 374);
            this.panel1.TabIndex = 18;
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
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 35);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select your download area.";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.DrpDwnLstDownLoc);
            this.panel2.Controls.Add(this.LblDownLoc);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(484, 374);
            this.panel2.TabIndex = 17;
            // 
            // DrpDwnLstDownLoc
            // 
            this.DrpDwnLstDownLoc.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstDownLoc.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstDownLoc.Location = new System.Drawing.Point(105, 10);
            this.DrpDwnLstDownLoc.Name = "DrpDwnLstDownLoc";
            this.DrpDwnLstDownLoc.Size = new System.Drawing.Size(261, 28);
            this.DrpDwnLstDownLoc.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstDownLoc.TabIndex = 31;
            this.DrpDwnLstDownLoc.ThemeName = "Office2016Black";
            this.DrpDwnLstDownLoc.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstDownLoc_SelectedIndexChanged);
            // 
            // LblDownLoc
            // 
            this.LblDownLoc.AutoSize = true;
            this.LblDownLoc.ForeColor = System.Drawing.Color.White;
            this.LblDownLoc.Location = new System.Drawing.Point(4, 17);
            this.LblDownLoc.Name = "LblDownLoc";
            this.LblDownLoc.Size = new System.Drawing.Size(95, 13);
            this.LblDownLoc.TabIndex = 30;
            this.LblDownLoc.Text = "Download location";
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
            this.LblStepText.TabIndex = 16;
            this.LblStepText.Text = "CDN Location";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Orange;
            this.label5.Location = new System.Drawing.Point(4, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(431, 31);
            this.label5.TabIndex = 34;
            this.label5.Text = "Only work for nexus mods and not for other file hosting web sites Nolvus connects" +
    " onto";
            // 
            // CDNFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblStepText);
            this.Name = "CDNFrame";
            this.Size = new System.Drawing.Size(810, 409);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstDownLoc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnPrevious;
        private Components.Controls.FlatButton BtnContinue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label LblStepText;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstDownLoc;
        private System.Windows.Forms.Label LblDownLoc;
        private System.Windows.Forms.Label label5;
    }
}
