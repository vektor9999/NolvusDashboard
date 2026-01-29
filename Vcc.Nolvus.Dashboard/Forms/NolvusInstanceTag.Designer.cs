namespace Vcc.Nolvus.Dashboard.Forms
{
    partial class NolvusInstanceTag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NolvusInstanceTag));
            this.label1 = new System.Windows.Forms.Label();
            this.TxtBxTag = new System.Windows.Forms.TextBox();
            this.LblError = new System.Windows.Forms.Label();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnOK = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(443, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter a tag to differentiate your new instance (max 15 caracters, no special cara" +
    "cter allowed)";
            // 
            // TxtBxTag
            // 
            this.TxtBxTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxTag.ForeColor = System.Drawing.Color.White;
            this.TxtBxTag.Location = new System.Drawing.Point(5, 90);
            this.TxtBxTag.MaxLength = 15;
            this.TxtBxTag.Name = "TxtBxTag";
            this.TxtBxTag.Size = new System.Drawing.Size(218, 20);
            this.TxtBxTag.TabIndex = 5;
            this.TxtBxTag.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBxTag_KeyPress);
            // 
            // LblError
            // 
            this.LblError.AutoSize = true;
            this.LblError.ForeColor = System.Drawing.Color.Red;
            this.LblError.Location = new System.Drawing.Point(2, 123);
            this.LblError.Name = "LblError";
            this.LblError.Size = new System.Drawing.Size(35, 13);
            this.LblError.TabIndex = 6;
            this.LblError.Text = "[Error]";
            this.LblError.Visible = false;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(292, 181);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(75, 39);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnOK.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.BtnOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnOK.ForeColor = System.Drawing.Color.White;
            this.BtnOK.Location = new System.Drawing.Point(373, 181);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnOK.Size = new System.Drawing.Size(75, 39);
            this.BtnOK.TabIndex = 2;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = false;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // NolvusInstanceTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(453, 230);
            this.Controls.Add(this.LblError);
            this.Controls.Add(this.TxtBxTag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NolvusInstanceTag";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "FrmMessageBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.Controls.FlatButton BtnCancel;
        private Components.Controls.FlatButton BtnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBxTag;
        private System.Windows.Forms.Label LblError;
    }
}