namespace Vcc.Nolvus.Dashboard.Frames.Manager.Options.v6
{
    partial class OptionsManagerFrame
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
            Syncfusion.Windows.Forms.Tools.ActiveStateCollection activeStateCollection1 = new Syncfusion.Windows.Forms.Tools.ActiveStateCollection();
            Syncfusion.Windows.Forms.Tools.InactiveStateCollection inactiveStateCollection1 = new Syncfusion.Windows.Forms.Tools.InactiveStateCollection();
            Syncfusion.Windows.Forms.Tools.SliderCollection sliderCollection1 = new Syncfusion.Windows.Forms.Tools.SliderCollection();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label35 = new System.Windows.Forms.Label();
            this.BtnCancel = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.BtnChange = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label4 = new System.Windows.Forms.Label();
            this.LblStepText = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DrpDwnLstOption = new Syncfusion.WinForms.ListView.SfComboBox();
            this.DrpDwnLstCategory = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TglBtnNewValue = new Syncfusion.Windows.Forms.Tools.ToggleButton();
            this.DrpDwnLstNewValue = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LblCurrentValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstCategory)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnNewValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstNewValue)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.BtnChange);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(713, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 698);
            this.panel1.TabIndex = 18;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Orange;
            this.label35.Location = new System.Drawing.Point(5, 5);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 21);
            this.label35.TabIndex = 7;
            this.label35.Text = "Info";
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCancel.BorderColor = System.Drawing.Color.Gray;
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCancel.Location = new System.Drawing.Point(114, 642);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCancel.Size = new System.Drawing.Size(88, 40);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnChange
            // 
            this.BtnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnChange.BorderColor = System.Drawing.Color.Gray;
            this.BtnChange.ForeColor = System.Drawing.Color.White;
            this.BtnChange.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnChange.Location = new System.Drawing.Point(208, 642);
            this.BtnChange.Name = "BtnChange";
            this.BtnChange.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnChange.Size = new System.Drawing.Size(88, 40);
            this.BtnChange.TabIndex = 4;
            this.BtnChange.Text = "Change";
            this.BtnChange.UseVisualStyleBackColor = false;
            this.BtnChange.Click += new System.EventHandler(this.BtnChange_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 54);
            this.label4.TabIndex = 0;
            this.label4.Text = "Select the option you want to change and click on Change. To cancel, click on Can" +
    "cel";
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
            this.LblStepText.Text = "Available Options";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.DrpDwnLstOption);
            this.panel4.Controls.Add(this.DrpDwnLstCategory);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Location = new System.Drawing.Point(7, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(700, 149);
            this.panel4.TabIndex = 66;
            // 
            // DrpDwnLstOption
            // 
            this.DrpDwnLstOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrpDwnLstOption.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstOption.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstOption.Location = new System.Drawing.Point(102, 84);
            this.DrpDwnLstOption.Name = "DrpDwnLstOption";
            this.DrpDwnLstOption.Size = new System.Drawing.Size(214, 28);
            this.DrpDwnLstOption.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstOption.TabIndex = 64;
            this.DrpDwnLstOption.ThemeName = "Office2016Black";
            this.DrpDwnLstOption.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstOption_SelectedIndexChanged);
            // 
            // DrpDwnLstCategory
            // 
            this.DrpDwnLstCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrpDwnLstCategory.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstCategory.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstCategory.Location = new System.Drawing.Point(102, 37);
            this.DrpDwnLstCategory.Name = "DrpDwnLstCategory";
            this.DrpDwnLstCategory.Size = new System.Drawing.Size(214, 28);
            this.DrpDwnLstCategory.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstCategory.TabIndex = 63;
            this.DrpDwnLstCategory.ThemeName = "Office2016Black";
            this.DrpDwnLstCategory.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstCategory_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Option";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Category";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Orange;
            this.label6.Location = new System.Drawing.Point(5, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 21);
            this.label6.TabIndex = 8;
            this.label6.Text = "Categories and Options";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.TglBtnNewValue);
            this.panel2.Controls.Add(this.DrpDwnLstNewValue);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.LblCurrentValue);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(7, 185);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 543);
            this.panel2.TabIndex = 72;
            // 
            // TglBtnNewValue
            // 
            activeStateCollection1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            activeStateCollection1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(176)))));
            this.TglBtnNewValue.ActiveState = activeStateCollection1;
            this.TglBtnNewValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TglBtnNewValue.ForeColor = System.Drawing.Color.Black;
            inactiveStateCollection1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            inactiveStateCollection1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            inactiveStateCollection1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            inactiveStateCollection1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.TglBtnNewValue.InactiveState = inactiveStateCollection1;
            this.TglBtnNewValue.Location = new System.Drawing.Point(102, 85);
            this.TglBtnNewValue.MinimumSize = new System.Drawing.Size(52, 20);
            this.TglBtnNewValue.Name = "TglBtnNewValue";
            this.TglBtnNewValue.Size = new System.Drawing.Size(63, 24);
            sliderCollection1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            sliderCollection1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection1.InactiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            sliderCollection1.InactiveHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TglBtnNewValue.Slider = sliderCollection1;
            this.TglBtnNewValue.TabIndex = 76;
            this.TglBtnNewValue.Text = "toggleButton1";
            this.TglBtnNewValue.ThemeName = "Office2016Black";
            // 
            // DrpDwnLstNewValue
            // 
            this.DrpDwnLstNewValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrpDwnLstNewValue.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstNewValue.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstNewValue.Location = new System.Drawing.Point(102, 83);
            this.DrpDwnLstNewValue.Name = "DrpDwnLstNewValue";
            this.DrpDwnLstNewValue.Size = new System.Drawing.Size(214, 28);
            this.DrpDwnLstNewValue.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstNewValue.TabIndex = 75;
            this.DrpDwnLstNewValue.ThemeName = "Office2016Black";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 74;
            this.label5.Text = "New Value";
            // 
            // LblCurrentValue
            // 
            this.LblCurrentValue.AutoSize = true;
            this.LblCurrentValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblCurrentValue.ForeColor = System.Drawing.Color.Orange;
            this.LblCurrentValue.Location = new System.Drawing.Point(99, 45);
            this.LblCurrentValue.Name = "LblCurrentValue";
            this.LblCurrentValue.Size = new System.Drawing.Size(54, 13);
            this.LblCurrentValue.TabIndex = 73;
            this.LblCurrentValue.Text = "Loading...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 72;
            this.label3.Text = "Current Value";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Orange;
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 21);
            this.label7.TabIndex = 9;
            this.label7.Text = "Values";
            // 
            // OptionsManagerFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblStepText);
            this.Name = "OptionsManagerFrame";
            this.Size = new System.Drawing.Size(1026, 733);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstCategory)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnNewValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstNewValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LblStepText;
        private Components.Controls.FlatButton BtnCancel;
        private Components.Controls.FlatButton BtnChange;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstCategory;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstOption;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblCurrentValue;
        private System.Windows.Forms.Label label5;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstNewValue;
        private Syncfusion.Windows.Forms.Tools.ToggleButton TglBtnNewValue;
    }
}
