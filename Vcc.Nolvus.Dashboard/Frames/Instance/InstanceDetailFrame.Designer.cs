namespace Vcc.Nolvus.Dashboard.Frames.Instance
{
    partial class InstanceDetailFrame
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
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn1 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn2 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn3 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn4 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            Syncfusion.WinForms.DataGrid.GridTextColumn gridTextColumn5 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            this.ModsGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.BtnBack = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.PnlHeader = new System.Windows.Forms.Panel();
            this.LblHeader = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.PicBoxLoading = new System.Windows.Forms.PictureBox();
            this.BtnExpand = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnCollapse = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnPlay = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnSettings = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnLoadOrder = new Vcc.Nolvus.Components.Controls.FlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.ModsGrid)).BeginInit();
            this.PnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // ModsGrid
            // 
            this.ModsGrid.AccessibleName = "Table";
            this.ModsGrid.AllowResizingColumns = true;
            this.ModsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModsGrid.AutoGenerateColumns = false;
            gridTextColumn1.AllowResizing = true;
            gridTextColumn1.HeaderText = "Priority";
            gridTextColumn1.MappingName = "Priority";
            gridTextColumn1.Width = 75D;
            gridTextColumn2.AllowResizing = true;
            gridTextColumn2.HeaderText = "Mod";
            gridTextColumn2.MappingName = "Name";
            gridTextColumn2.Width = 400D;
            gridTextColumn3.AllowResizing = true;
            gridTextColumn3.HeaderText = "Version";
            gridTextColumn3.MappingName = "Version";
            gridTextColumn4.AllowResizing = true;
            gridTextColumn4.HeaderText = "Category";
            gridTextColumn4.MappingName = "Category";
            gridTextColumn4.Visible = false;
            gridTextColumn5.AllowResizing = true;
            gridTextColumn5.HeaderText = "Status";
            gridTextColumn5.MappingName = "StatusText";
            gridTextColumn5.Width = 200D;
            this.ModsGrid.Columns.Add(gridTextColumn1);
            this.ModsGrid.Columns.Add(gridTextColumn2);
            this.ModsGrid.Columns.Add(gridTextColumn3);
            this.ModsGrid.Columns.Add(gridTextColumn4);
            this.ModsGrid.Columns.Add(gridTextColumn5);
            this.ModsGrid.Location = new System.Drawing.Point(7, 76);
            this.ModsGrid.Name = "ModsGrid";
            this.ModsGrid.Size = new System.Drawing.Size(889, 347);
            this.ModsGrid.TabIndex = 0;
            this.ModsGrid.Text = "sfDataGrid1";
            this.ModsGrid.QueryCellStyle += new Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventHandler(this.ModsGrid_QueryCellStyle);
            this.ModsGrid.DrawCell += new Syncfusion.WinForms.DataGrid.Events.DrawCellEventHandler(this.ModsGrid_DrawCell);
            // 
            // BtnBack
            // 
            this.BtnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBack.BorderColor = System.Drawing.Color.Gray;
            this.BtnBack.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBack.ForeColor = System.Drawing.Color.Orange;
            this.BtnBack.Location = new System.Drawing.Point(907, 306);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBack.Size = new System.Drawing.Size(127, 40);
            this.BtnBack.TabIndex = 9;
            this.BtnBack.Text = "Back to instances";
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // PnlHeader
            // 
            this.PnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.PnlHeader.Controls.Add(this.LblHeader);
            this.PnlHeader.Controls.Add(this.PicBox);
            this.PnlHeader.Location = new System.Drawing.Point(7, 5);
            this.PnlHeader.Name = "PnlHeader";
            this.PnlHeader.Size = new System.Drawing.Size(1027, 45);
            this.PnlHeader.TabIndex = 21;
            // 
            // LblHeader
            // 
            this.LblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblHeader.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHeader.ForeColor = System.Drawing.Color.White;
            this.LblHeader.Location = new System.Drawing.Point(44, 5);
            this.LblHeader.Name = "LblHeader";
            this.LblHeader.Size = new System.Drawing.Size(976, 32);
            this.LblHeader.TabIndex = 1;
            this.LblHeader.Text = "Instance";
            this.LblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PicBox
            // 
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.nolvus_ico;
            this.PicBox.Location = new System.Drawing.Point(6, 7);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(32, 32);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 0;
            this.PicBox.TabStop = false;
            // 
            // PicBoxLoading
            // 
            this.PicBoxLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBoxLoading.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.cog_loader_alpha;
            this.PicBoxLoading.Location = new System.Drawing.Point(7, 76);
            this.PicBoxLoading.Name = "PicBoxLoading";
            this.PicBoxLoading.Size = new System.Drawing.Size(1027, 349);
            this.PicBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PicBoxLoading.TabIndex = 22;
            this.PicBoxLoading.TabStop = false;
            // 
            // BtnExpand
            // 
            this.BtnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExpand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnExpand.BorderColor = System.Drawing.Color.Gray;
            this.BtnExpand.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnExpand.ForeColor = System.Drawing.Color.Orange;
            this.BtnExpand.Location = new System.Drawing.Point(907, 122);
            this.BtnExpand.Name = "BtnExpand";
            this.BtnExpand.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnExpand.Size = new System.Drawing.Size(127, 40);
            this.BtnExpand.TabIndex = 23;
            this.BtnExpand.Text = "Expand";
            this.BtnExpand.UseVisualStyleBackColor = false;
            this.BtnExpand.Click += new System.EventHandler(this.BtnExpand_Click);
            // 
            // BtnCollapse
            // 
            this.BtnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCollapse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCollapse.BorderColor = System.Drawing.Color.Gray;
            this.BtnCollapse.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCollapse.ForeColor = System.Drawing.Color.Orange;
            this.BtnCollapse.Location = new System.Drawing.Point(907, 168);
            this.BtnCollapse.Name = "BtnCollapse";
            this.BtnCollapse.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCollapse.Size = new System.Drawing.Size(127, 40);
            this.BtnCollapse.TabIndex = 24;
            this.BtnCollapse.Text = "Collapse";
            this.BtnCollapse.UseVisualStyleBackColor = false;
            this.BtnCollapse.Click += new System.EventHandler(this.BtnCollapse_Click);
            // 
            // BtnPlay
            // 
            this.BtnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnPlay.BorderColor = System.Drawing.Color.Gray;
            this.BtnPlay.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnPlay.ForeColor = System.Drawing.Color.Orange;
            this.BtnPlay.Location = new System.Drawing.Point(907, 76);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnPlay.Size = new System.Drawing.Size(127, 40);
            this.BtnPlay.TabIndex = 25;
            this.BtnPlay.Text = "Play";
            this.BtnPlay.UseVisualStyleBackColor = false;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // BtnSettings
            // 
            this.BtnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnSettings.BorderColor = System.Drawing.Color.Gray;
            this.BtnSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnSettings.ForeColor = System.Drawing.Color.Orange;
            this.BtnSettings.Location = new System.Drawing.Point(907, 214);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnSettings.Size = new System.Drawing.Size(127, 40);
            this.BtnSettings.TabIndex = 28;
            this.BtnSettings.Text = "Settings";
            this.BtnSettings.UseVisualStyleBackColor = false;
            this.BtnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // BtnLoadOrder
            // 
            this.BtnLoadOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoadOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnLoadOrder.BorderColor = System.Drawing.Color.Gray;
            this.BtnLoadOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnLoadOrder.ForeColor = System.Drawing.Color.Orange;
            this.BtnLoadOrder.Location = new System.Drawing.Point(907, 260);
            this.BtnLoadOrder.Name = "BtnLoadOrder";
            this.BtnLoadOrder.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnLoadOrder.Size = new System.Drawing.Size(127, 40);
            this.BtnLoadOrder.TabIndex = 29;
            this.BtnLoadOrder.Text = "Apply Order";
            this.BtnLoadOrder.UseVisualStyleBackColor = false;
            this.BtnLoadOrder.Click += new System.EventHandler(this.BtnLoadOrder_Click);
            // 
            // InstanceDetailFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.PicBoxLoading);
            this.Controls.Add(this.BtnSettings);
            this.Controls.Add(this.BtnPlay);
            this.Controls.Add(this.BtnCollapse);
            this.Controls.Add(this.BtnExpand);
            this.Controls.Add(this.PnlHeader);
            this.Controls.Add(this.BtnBack);
            this.Controls.Add(this.ModsGrid);
            this.Controls.Add(this.BtnLoadOrder);
            this.Name = "InstanceDetailFrame";
            this.Size = new System.Drawing.Size(1042, 448);
            ((System.ComponentModel.ISupportInitialize)(this.ModsGrid)).EndInit();
            this.PnlHeader.ResumeLayout(false);
            this.PnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.WinForms.DataGrid.SfDataGrid ModsGrid;
        private Components.Controls.FlatButton BtnBack;
        private System.Windows.Forms.Panel PnlHeader;
        private System.Windows.Forms.Label LblHeader;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.PictureBox PicBoxLoading;
        private Components.Controls.FlatButton BtnExpand;
        private Components.Controls.FlatButton BtnCollapse;
        private Components.Controls.FlatButton BtnPlay;
        private Components.Controls.FlatButton BtnSettings;
        private Components.Controls.FlatButton BtnLoadOrder;
    }
}
