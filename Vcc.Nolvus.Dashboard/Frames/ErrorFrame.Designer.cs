namespace Vcc.Nolvus.Dashboard.Frames
{
    partial class ErrorFrame
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
            this.PnlTitle = new System.Windows.Forms.Panel();
            this.LblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LblError = new System.Windows.Forms.Label();
            this.LblTrace = new System.Windows.Forms.Label();
            this.BtnRetry = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnHelp = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.PnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlTitle
            // 
            this.PnlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlTitle.Controls.Add(this.LblTitle);
            this.PnlTitle.Controls.Add(this.pictureBox1);
            this.PnlTitle.Location = new System.Drawing.Point(3, 3);
            this.PnlTitle.Name = "PnlTitle";
            this.PnlTitle.Size = new System.Drawing.Size(836, 80);
            this.PnlTitle.TabIndex = 0;
            // 
            // LblTitle
            // 
            this.LblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTitle.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Location = new System.Drawing.Point(55, 23);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(765, 32);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.Text = "[Title]";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.pictureBox1.Location = new System.Drawing.Point(17, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LblError
            // 
            this.LblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblError.ForeColor = System.Drawing.Color.Red;
            this.LblError.Location = new System.Drawing.Point(3, 98);
            this.LblError.Name = "LblError";
            this.LblError.Size = new System.Drawing.Size(836, 109);
            this.LblError.TabIndex = 1;
            this.LblError.Text = "[Error]";
            this.LblError.UseMnemonic = false;
            // 
            // LblTrace
            // 
            this.LblTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTrace.ForeColor = System.Drawing.Color.White;
            this.LblTrace.Location = new System.Drawing.Point(3, 207);
            this.LblTrace.Name = "LblTrace";
            this.LblTrace.Size = new System.Drawing.Size(836, 231);
            this.LblTrace.TabIndex = 2;
            this.LblTrace.Text = "[Trace]";
            this.LblTrace.UseMnemonic = false;
            // 
            // BtnRetry
            // 
            this.BtnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnRetry.BorderColor = System.Drawing.Color.White;
            this.BtnRetry.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnRetry.ForeColor = System.Drawing.Color.White;
            this.BtnRetry.Location = new System.Drawing.Point(711, 441);
            this.BtnRetry.Name = "BtnRetry";
            this.BtnRetry.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnRetry.Size = new System.Drawing.Size(113, 35);
            this.BtnRetry.TabIndex = 4;
            this.BtnRetry.Text = "Retry";
            this.BtnRetry.UseVisualStyleBackColor = false;
            this.BtnRetry.Click += new System.EventHandler(this.BtnRetry_Click);
            // 
            // BtnHelp
            // 
            this.BtnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnHelp.BorderColor = System.Drawing.Color.White;
            this.BtnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnHelp.ForeColor = System.Drawing.Color.White;
            this.BtnHelp.Location = new System.Drawing.Point(592, 441);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnHelp.Size = new System.Drawing.Size(113, 35);
            this.BtnHelp.TabIndex = 5;
            this.BtnHelp.Text = "Online Help";
            this.BtnHelp.UseVisualStyleBackColor = false;
            this.BtnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // ErrorFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.BtnHelp);
            this.Controls.Add(this.BtnRetry);
            this.Controls.Add(this.LblTrace);
            this.Controls.Add(this.LblError);
            this.Controls.Add(this.PnlTitle);
            this.Name = "ErrorFrame";
            this.Size = new System.Drawing.Size(842, 499);
            this.PnlTitle.ResumeLayout(false);
            this.PnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblError;
        private System.Windows.Forms.Label LblTrace;
        private Components.Controls.FlatButton BtnRetry;
        private Components.Controls.FlatButton BtnHelp;
    }
}
