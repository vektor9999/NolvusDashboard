namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    partial class ErrorSummaryFrame
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
            this.ModsPanel = new System.Windows.Forms.Panel();
            this.BtnFix = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label1 = new System.Windows.Forms.Label();
            this.LblMessage = new System.Windows.Forms.Label();
            this.BtnRetry = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrorsPanel = new Vcc.Nolvus.Dashboard.Controls.ErrorsPanel();
            this.ModsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModsPanel
            // 
            this.ModsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModsPanel.Controls.Add(this.BtnFix);
            this.ModsPanel.Controls.Add(this.label1);
            this.ModsPanel.Controls.Add(this.LblMessage);
            this.ModsPanel.Controls.Add(this.BtnRetry);
            this.ModsPanel.Controls.Add(this.panel1);
            this.ModsPanel.Location = new System.Drawing.Point(3, 3);
            this.ModsPanel.Name = "ModsPanel";
            this.ModsPanel.Size = new System.Drawing.Size(981, 653);
            this.ModsPanel.TabIndex = 0;
            // 
            // BtnFix
            // 
            this.BtnFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnFix.BorderColor = System.Drawing.Color.White;
            this.BtnFix.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnFix.ForeColor = System.Drawing.Color.White;
            this.BtnFix.Location = new System.Drawing.Point(745, 609);
            this.BtnFix.Name = "BtnFix";
            this.BtnFix.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnFix.Size = new System.Drawing.Size(113, 35);
            this.BtnFix.TabIndex = 8;
            this.BtnFix.Text = "Fix";
            this.BtnFix.UseVisualStyleBackColor = false;
            this.BtnFix.Click += new System.EventHandler(this.BtnFix_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 631);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Click on the Help button to fix";
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblMessage.AutoSize = true;
            this.LblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessage.ForeColor = System.Drawing.Color.Orange;
            this.LblMessage.Location = new System.Drawing.Point(3, 609);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(485, 13);
            this.LblMessage.TabIndex = 6;
            this.LblMessage.Text = "The installation has not been completed because {0} error(s) on {1} maximum error" +
    "(s) allowed occured";
            // 
            // BtnRetry
            // 
            this.BtnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnRetry.BorderColor = System.Drawing.Color.White;
            this.BtnRetry.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnRetry.ForeColor = System.Drawing.Color.White;
            this.BtnRetry.Location = new System.Drawing.Point(864, 609);
            this.BtnRetry.Name = "BtnRetry";
            this.BtnRetry.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnRetry.Size = new System.Drawing.Size(113, 35);
            this.BtnRetry.TabIndex = 5;
            this.BtnRetry.Text = "Retry";
            this.BtnRetry.UseVisualStyleBackColor = false;
            this.BtnRetry.Click += new System.EventHandler(this.BtnRetry_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ErrorsPanel);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 596);
            this.panel1.TabIndex = 4;
            // 
            // ErrorsPanel
            // 
            this.ErrorsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ErrorsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsPanel.Location = new System.Drawing.Point(0, 0);
            this.ErrorsPanel.Name = "ErrorsPanel";
            this.ErrorsPanel.Size = new System.Drawing.Size(973, 594);
            this.ErrorsPanel.TabIndex = 0;
            this.ErrorsPanel.Resize += new System.EventHandler(this.ErrorsPanel_Resize);
            // 
            // ErrorSummaryFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.ModsPanel);
            this.Name = "ErrorSummaryFrame";
            this.Size = new System.Drawing.Size(987, 659);
            this.ModsPanel.ResumeLayout(false);
            this.ModsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ModsPanel;
        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnRetry;
        private Controls.ErrorsPanel ErrorsPanel;
        private System.Windows.Forms.Label LblMessage;
        private Components.Controls.FlatButton BtnFix;
        private System.Windows.Forms.Label label1;
    }
}
