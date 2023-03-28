namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    partial class InstancesFrame
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
            this.InstancesPanel = new Vcc.Nolvus.Dashboard.Controls.InstancesPanel();
            this.BtnNewInstance = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnDiscord = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnDonate = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnPatreon = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 1);
            this.panel1.TabIndex = 1;
            // 
            // InstancesPanel
            // 
            this.InstancesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstancesPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.InstancesPanel.Location = new System.Drawing.Point(3, 3);
            this.InstancesPanel.Name = "InstancesPanel";
            this.InstancesPanel.Size = new System.Drawing.Size(720, 404);
            this.InstancesPanel.TabIndex = 0;
            // 
            // BtnNewInstance
            // 
            this.BtnNewInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNewInstance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnNewInstance.BorderColor = System.Drawing.Color.White;
            this.BtnNewInstance.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnNewInstance.ForeColor = System.Drawing.Color.White;
            this.BtnNewInstance.Location = new System.Drawing.Point(585, 349);
            this.BtnNewInstance.Name = "BtnNewInstance";
            this.BtnNewInstance.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnNewInstance.Size = new System.Drawing.Size(114, 40);
            this.BtnNewInstance.TabIndex = 2;
            this.BtnNewInstance.Text = "Add Instance";
            this.BtnNewInstance.UseVisualStyleBackColor = false;
            this.BtnNewInstance.Click += new System.EventHandler(this.BtnNewInstance_Click);
            // 
            // BtnDiscord
            // 
            this.BtnDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnDiscord.BorderColor = System.Drawing.Color.White;
            this.BtnDiscord.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnDiscord.ForeColor = System.Drawing.Color.White;
            this.BtnDiscord.Location = new System.Drawing.Point(489, 349);
            this.BtnDiscord.Name = "BtnDiscord";
            this.BtnDiscord.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnDiscord.Size = new System.Drawing.Size(90, 40);
            this.BtnDiscord.TabIndex = 3;
            this.BtnDiscord.Text = "Discord";
            this.BtnDiscord.UseVisualStyleBackColor = false;
            this.BtnDiscord.Click += new System.EventHandler(this.BtnDiscord_Click);
            // 
            // BtnDonate
            // 
            this.BtnDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDonate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnDonate.BorderColor = System.Drawing.Color.White;
            this.BtnDonate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnDonate.ForeColor = System.Drawing.Color.White;
            this.BtnDonate.Location = new System.Drawing.Point(393, 349);
            this.BtnDonate.Name = "BtnDonate";
            this.BtnDonate.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnDonate.Size = new System.Drawing.Size(90, 40);
            this.BtnDonate.TabIndex = 4;
            this.BtnDonate.Text = "Donate";
            this.BtnDonate.UseVisualStyleBackColor = false;
            this.BtnDonate.Click += new System.EventHandler(this.BtnDonate_Click);
            // 
            // BtnPatreon
            // 
            this.BtnPatreon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPatreon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPatreon.BorderColor = System.Drawing.Color.White;
            this.BtnPatreon.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPatreon.ForeColor = System.Drawing.Color.White;
            this.BtnPatreon.Location = new System.Drawing.Point(297, 349);
            this.BtnPatreon.Name = "BtnPatreon";
            this.BtnPatreon.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPatreon.Size = new System.Drawing.Size(90, 40);
            this.BtnPatreon.TabIndex = 5;
            this.BtnPatreon.Text = "Patreon";
            this.BtnPatreon.UseVisualStyleBackColor = false;
            this.BtnPatreon.Click += new System.EventHandler(this.BtnPatreon_Click);
            // 
            // InstancesFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.BtnPatreon);
            this.Controls.Add(this.BtnDonate);
            this.Controls.Add(this.BtnDiscord);
            this.Controls.Add(this.BtnNewInstance);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InstancesPanel);
            this.Name = "InstancesFrame";
            this.Size = new System.Drawing.Size(726, 410);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.InstancesPanel InstancesPanel;
        private System.Windows.Forms.Panel panel1;
        private Components.Controls.FlatButton BtnNewInstance;
        private Components.Controls.FlatButton BtnDiscord;
        private Components.Controls.FlatButton BtnDonate;
        private Components.Controls.FlatButton BtnPatreon;
    }
}
