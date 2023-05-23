namespace Vcc.Nolvus.Dashboard.Frames
{
    partial class DeleteFrame
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
            this.parentBarItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnBack = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.LblInfo = new System.Windows.Forms.Label();
            this.BtnAction = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LblInstance = new System.Windows.Forms.Label();
            this.LblInstanceInfo = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.LblDeleteInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // parentBarItem1
            // 
            this.parentBarItem1.BarName = "parentBarItem1";
            this.parentBarItem1.MetroColor = System.Drawing.Color.LightSkyBlue;
            this.parentBarItem1.ShowToolTipInPopUp = false;
            this.parentBarItem1.SizeToFit = true;
            this.parentBarItem1.WrapLength = 20;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Controls.Add(this.LblInfo);
            this.panel1.Controls.Add(this.BtnAction);
            this.panel1.Location = new System.Drawing.Point(448, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 312);
            this.panel1.TabIndex = 18;
            // 
            // BtnBack
            // 
            this.BtnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBack.BorderColor = System.Drawing.Color.White;
            this.BtnBack.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBack.ForeColor = System.Drawing.Color.White;
            this.BtnBack.Location = new System.Drawing.Point(148, 257);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBack.Size = new System.Drawing.Size(88, 40);
            this.BtnBack.TabIndex = 3;
            this.BtnBack.Text = "Back";
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // LblInfo
            // 
            this.LblInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.ForeColor = System.Drawing.Color.White;
            this.LblInfo.Location = new System.Drawing.Point(3, 2);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(336, 35);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Click on the Delete button to delete your instance. WARNING, this will not be rev" +
    "ersible.";
            // 
            // BtnAction
            // 
            this.BtnAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnAction.BorderColor = System.Drawing.Color.White;
            this.BtnAction.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnAction.ForeColor = System.Drawing.Color.White;
            this.BtnAction.Location = new System.Drawing.Point(242, 257);
            this.BtnAction.Name = "BtnAction";
            this.BtnAction.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnAction.Size = new System.Drawing.Size(88, 40);
            this.BtnAction.TabIndex = 1;
            this.BtnAction.Text = "[Action]";
            this.BtnAction.UseVisualStyleBackColor = false;
            this.BtnAction.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.LblDeleteInfo);
            this.panel2.Controls.Add(this.LblInstance);
            this.panel2.Controls.Add(this.LblInstanceInfo);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(435, 312);
            this.panel2.TabIndex = 17;
            // 
            // LblInstance
            // 
            this.LblInstance.AutoSize = true;
            this.LblInstance.ForeColor = System.Drawing.Color.Orange;
            this.LblInstance.Location = new System.Drawing.Point(104, 12);
            this.LblInstance.Name = "LblInstance";
            this.LblInstance.Size = new System.Drawing.Size(67, 13);
            this.LblInstance.TabIndex = 1;
            this.LblInstance.Text = "[INSTANCE]";
            // 
            // LblInstanceInfo
            // 
            this.LblInstanceInfo.AutoSize = true;
            this.LblInstanceInfo.ForeColor = System.Drawing.Color.White;
            this.LblInstanceInfo.Location = new System.Drawing.Point(3, 12);
            this.LblInstanceInfo.Name = "LblInstanceInfo";
            this.LblInstanceInfo.Size = new System.Drawing.Size(95, 13);
            this.LblInstanceInfo.TabIndex = 0;
            this.LblInstanceInfo.Text = "Instance to delete ";
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
            this.LblStepText.Size = new System.Drawing.Size(807, 20);
            this.LblStepText.TabIndex = 14;
            this.LblStepText.Text = "Delete Nolvus Instance";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblDeleteInfo
            // 
            this.LblDeleteInfo.AutoSize = true;
            this.LblDeleteInfo.ForeColor = System.Drawing.Color.White;
            this.LblDeleteInfo.Location = new System.Drawing.Point(3, 43);
            this.LblDeleteInfo.Name = "LblDeleteInfo";
            this.LblDeleteInfo.Size = new System.Drawing.Size(98, 13);
            this.LblDeleteInfo.TabIndex = 2;
            this.LblDeleteInfo.Text = "Deleting instance...";
            this.LblDeleteInfo.Visible = false;
            // 
            // DeleteFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblStepText);
            this.Name = "DeleteFrame";
            this.Size = new System.Drawing.Size(799, 350);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblStepText;
        private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblInfo;
        private Components.Controls.FlatButton BtnAction;
        private System.Windows.Forms.Label LblInstance;
        private System.Windows.Forms.Label LblInstanceInfo;
        private Components.Controls.FlatButton BtnBack;
        private System.Windows.Forms.Label LblDeleteInfo;
    }
}
