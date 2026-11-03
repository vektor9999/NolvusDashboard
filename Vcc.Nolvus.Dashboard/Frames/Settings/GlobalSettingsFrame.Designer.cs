namespace Vcc.Nolvus.Dashboard.Frames.Settings
{
    partial class GlobalSettingsFrame
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
            Syncfusion.Windows.Forms.Tools.ActiveStateCollection activeStateCollection3 = new Syncfusion.Windows.Forms.Tools.ActiveStateCollection();
            Syncfusion.Windows.Forms.Tools.InactiveStateCollection inactiveStateCollection3 = new Syncfusion.Windows.Forms.Tools.InactiveStateCollection();
            Syncfusion.Windows.Forms.Tools.SliderCollection sliderCollection3 = new Syncfusion.Windows.Forms.Tools.SliderCollection();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.LblStepText = new System.Windows.Forms.Label();
            this.BtnBack = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PnlMessage = new System.Windows.Forms.Panel();
            this.LblMessage = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.BtnSaveMegaInfo = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.LblPassword = new System.Windows.Forms.Label();
            this.TxtBxPassword = new System.Windows.Forms.TextBox();
            this.TxtBxUserName = new System.Windows.Forms.TextBox();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblMegaMessage = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TglBtnAnonymous = new Syncfusion.Windows.Forms.Tools.ToggleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.LnkLblInfo = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.DrpDwnLstProcessCount = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnCLearLogs = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnClearCache = new Vcc.Nolvus.Components.Controls.FlatButton();
            this.panel1.SuspendLayout();
            this.PnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnAnonymous)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstProcessCount)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblStepText
            // 
            this.LblStepText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStepText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.LblStepText.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStepText.ForeColor = System.Drawing.Color.White;
            this.LblStepText.Location = new System.Drawing.Point(3, 1);
            this.LblStepText.Name = "LblStepText";
            this.LblStepText.Size = new System.Drawing.Size(1101, 37);
            this.LblStepText.TabIndex = 20;
            this.LblStepText.Text = "Global Settings";
            this.LblStepText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnBack
            // 
            this.BtnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnBack.BorderColor = System.Drawing.Color.Gray;
            this.BtnBack.ForeColor = System.Drawing.Color.White;
            this.BtnBack.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnBack.Location = new System.Drawing.Point(994, 660);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnBack.Size = new System.Drawing.Size(94, 42);
            this.BtnBack.TabIndex = 8;
            this.BtnBack.Text = "Back";
            this.BtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.PnlMessage);
            this.panel1.Controls.Add(this.BtnSaveMegaInfo);
            this.panel1.Controls.Add(this.LblPassword);
            this.panel1.Controls.Add(this.TxtBxPassword);
            this.panel1.Controls.Add(this.TxtBxUserName);
            this.panel1.Controls.Add(this.LblEmail);
            this.panel1.Controls.Add(this.LblMegaMessage);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.TglBtnAnonymous);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LnkLblInfo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Location = new System.Drawing.Point(7, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 345);
            this.panel1.TabIndex = 22;
            // 
            // PnlMessage
            // 
            this.PnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlMessage.BackColor = System.Drawing.Color.Orange;
            this.PnlMessage.Controls.Add(this.LblMessage);
            this.PnlMessage.Controls.Add(this.PicBox);
            this.PnlMessage.Location = new System.Drawing.Point(13, 220);
            this.PnlMessage.Name = "PnlMessage";
            this.PnlMessage.Size = new System.Drawing.Size(1054, 56);
            this.PnlMessage.TabIndex = 85;
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(44, 7);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(1003, 46);
            this.LblMessage.TabIndex = 1;
            this.LblMessage.Text = "Your credentials will be stored and encrypted in your local Dashboard config file" +
    ". Note that if you have a mega premium account, you will be able to download meg" +
    "a files faster.";
            // 
            // PicBox
            // 
            this.PicBox.Image = global::Vcc.Nolvus.Dashboard.Properties.Resources.Warning_Message;
            this.PicBox.Location = new System.Drawing.Point(6, 7);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(32, 32);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 0;
            this.PicBox.TabStop = false;
            // 
            // BtnSaveMegaInfo
            // 
            this.BtnSaveMegaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSaveMegaInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnSaveMegaInfo.BorderColor = System.Drawing.Color.Gray;
            this.BtnSaveMegaInfo.ForeColor = System.Drawing.Color.White;
            this.BtnSaveMegaInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnSaveMegaInfo.Location = new System.Drawing.Point(973, 289);
            this.BtnSaveMegaInfo.Name = "BtnSaveMegaInfo";
            this.BtnSaveMegaInfo.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnSaveMegaInfo.Size = new System.Drawing.Size(94, 42);
            this.BtnSaveMegaInfo.TabIndex = 84;
            this.BtnSaveMegaInfo.Text = "Save";
            this.BtnSaveMegaInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSaveMegaInfo.UseVisualStyleBackColor = false;
            this.BtnSaveMegaInfo.Click += new System.EventHandler(this.BtnSaveMegaInfo_Click);
            // 
            // LblPassword
            // 
            this.LblPassword.AutoSize = true;
            this.LblPassword.ForeColor = System.Drawing.Color.White;
            this.LblPassword.Location = new System.Drawing.Point(10, 188);
            this.LblPassword.Name = "LblPassword";
            this.LblPassword.Size = new System.Drawing.Size(53, 13);
            this.LblPassword.TabIndex = 83;
            this.LblPassword.Text = "Password";
            // 
            // TxtBxPassword
            // 
            this.TxtBxPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxPassword.ForeColor = System.Drawing.Color.White;
            this.TxtBxPassword.Location = new System.Drawing.Point(79, 186);
            this.TxtBxPassword.Name = "TxtBxPassword";
            this.TxtBxPassword.PasswordChar = '*';
            this.TxtBxPassword.Size = new System.Drawing.Size(259, 20);
            this.TxtBxPassword.TabIndex = 80;
            // 
            // TxtBxUserName
            // 
            this.TxtBxUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.TxtBxUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBxUserName.ForeColor = System.Drawing.Color.White;
            this.TxtBxUserName.Location = new System.Drawing.Point(79, 152);
            this.TxtBxUserName.Name = "TxtBxUserName";
            this.TxtBxUserName.Size = new System.Drawing.Size(259, 20);
            this.TxtBxUserName.TabIndex = 79;
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.ForeColor = System.Drawing.Color.White;
            this.LblEmail.Location = new System.Drawing.Point(10, 154);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(32, 13);
            this.LblEmail.TabIndex = 82;
            this.LblEmail.Text = "Email";
            // 
            // LblMegaMessage
            // 
            this.LblMegaMessage.AutoSize = true;
            this.LblMegaMessage.ForeColor = System.Drawing.Color.White;
            this.LblMegaMessage.Location = new System.Drawing.Point(8, 113);
            this.LblMegaMessage.Name = "LblMegaMessage";
            this.LblMegaMessage.Size = new System.Drawing.Size(484, 13);
            this.LblMegaMessage.TabIndex = 81;
            this.LblMegaMessage.Text = " When created, please enter the email address and password you used to create you" +
    "r mega account.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(10, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 13);
            this.label6.TabIndex = 78;
            this.label6.Text = "Anonymous Connection";
            // 
            // TglBtnAnonymous
            // 
            activeStateCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            activeStateCollection3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            activeStateCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(176)))));
            this.TglBtnAnonymous.ActiveState = activeStateCollection3;
            this.TglBtnAnonymous.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TglBtnAnonymous.ForeColor = System.Drawing.Color.Black;
            inactiveStateCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            inactiveStateCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            inactiveStateCollection3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            inactiveStateCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.TglBtnAnonymous.InactiveState = inactiveStateCollection3;
            this.TglBtnAnonymous.Location = new System.Drawing.Point(135, 73);
            this.TglBtnAnonymous.MinimumSize = new System.Drawing.Size(52, 20);
            this.TglBtnAnonymous.Name = "TglBtnAnonymous";
            this.TglBtnAnonymous.Size = new System.Drawing.Size(63, 24);
            sliderCollection3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            sliderCollection3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            sliderCollection3.InactiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            sliderCollection3.InactiveHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TglBtnAnonymous.Slider = sliderCollection3;
            this.TglBtnAnonymous.TabIndex = 77;
            this.TglBtnAnonymous.Text = "toggleButton1";
            this.TglBtnAnonymous.ThemeName = "Office2016Black";
            this.TglBtnAnonymous.ToggleStateChanged += new Syncfusion.Windows.Forms.Tools.ToggleStateChangedEventHandler(this.TglBtnAnonymous_ToggleStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(62, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "to create your free mega.nz account.";
            // 
            // LnkLblInfo
            // 
            this.LnkLblInfo.AutoSize = true;
            this.LnkLblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LnkLblInfo.ForeColor = System.Drawing.Color.Orange;
            this.LnkLblInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LnkLblInfo.LinkColor = System.Drawing.Color.Orange;
            this.LnkLblInfo.Location = new System.Drawing.Point(37, 45);
            this.LnkLblInfo.Name = "LnkLblInfo";
            this.LnkLblInfo.Size = new System.Drawing.Size(28, 13);
            this.LnkLblInfo.TabIndex = 75;
            this.LnkLblInfo.TabStop = true;
            this.LnkLblInfo.Text = "here";
            this.LnkLblInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLblInfo_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 74;
            this.label1.Text = "Click";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Orange;
            this.label20.Location = new System.Drawing.Point(5, 5);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(120, 21);
            this.label20.TabIndex = 73;
            this.label20.Text = "Mega Account";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.DrpDwnLstProcessCount);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(8, 398);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1080, 102);
            this.panel2.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(194, 13);
            this.label7.TabIndex = 76;
            this.label7.Text = "Number of simultaneus mods installation";
            // 
            // DrpDwnLstProcessCount
            // 
            this.DrpDwnLstProcessCount.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center;
            this.DrpDwnLstProcessCount.DropDownStyle = Syncfusion.WinForms.ListView.Enums.DropDownStyle.DropDownList;
            this.DrpDwnLstProcessCount.Location = new System.Drawing.Point(210, 37);
            this.DrpDwnLstProcessCount.Name = "DrpDwnLstProcessCount";
            this.DrpDwnLstProcessCount.Size = new System.Drawing.Size(189, 28);
            this.DrpDwnLstProcessCount.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DrpDwnLstProcessCount.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrpDwnLstProcessCount.TabIndex = 75;
            this.DrpDwnLstProcessCount.ThemeName = "Office2016Black";
            this.DrpDwnLstProcessCount.SelectedIndexChanged += new System.EventHandler(this.DrpDwnLstProcessCount_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Orange;
            this.label8.Location = new System.Drawing.Point(5, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 21);
            this.label8.TabIndex = 74;
            this.label8.Text = "Process";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.BtnClearCache);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.BtnCLearLogs);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(8, 506);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1080, 148);
            this.panel3.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Orange;
            this.label3.Location = new System.Drawing.Point(5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 21);
            this.label3.TabIndex = 75;
            this.label3.Text = "Actions";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 76;
            this.label4.Text = "Logs";
            // 
            // BtnCLearLogs
            // 
            this.BtnCLearLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnCLearLogs.BorderColor = System.Drawing.Color.Gray;
            this.BtnCLearLogs.ForeColor = System.Drawing.Color.White;
            this.BtnCLearLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnCLearLogs.Location = new System.Drawing.Point(78, 37);
            this.BtnCLearLogs.Name = "BtnCLearLogs";
            this.BtnCLearLogs.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnCLearLogs.Size = new System.Drawing.Size(61, 30);
            this.BtnCLearLogs.TabIndex = 85;
            this.BtnCLearLogs.Text = "Clear";
            this.BtnCLearLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCLearLogs.UseVisualStyleBackColor = false;
            this.BtnCLearLogs.Click += new System.EventHandler(this.BtnCLearLogs_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(9, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 86;
            this.label5.Text = "Cache";
            // 
            // BtnClearCache
            // 
            this.BtnClearCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BtnClearCache.BorderColor = System.Drawing.Color.Gray;
            this.BtnClearCache.ForeColor = System.Drawing.Color.White;
            this.BtnClearCache.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BtnClearCache.Location = new System.Drawing.Point(78, 78);
            this.BtnClearCache.Name = "BtnClearCache";
            this.BtnClearCache.OnHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BtnClearCache.Size = new System.Drawing.Size(61, 30);
            this.BtnClearCache.TabIndex = 87;
            this.BtnClearCache.Text = "Clear";
            this.BtnClearCache.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnClearCache.UseVisualStyleBackColor = false;
            this.BtnClearCache.Click += new System.EventHandler(this.BtnClearCache_Click);
            // 
            // GlobalSettingsFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblStepText);
            this.Controls.Add(this.BtnBack);
            this.Name = "GlobalSettingsFrame";
            this.Size = new System.Drawing.Size(1107, 720);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PnlMessage.ResumeLayout(false);
            this.PnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TglBtnAnonymous)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrpDwnLstProcessCount)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Components.Controls.FlatButton BtnBack;
        private System.Windows.Forms.Label LblStepText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel LnkLblInfo;
        private System.Windows.Forms.Label label2;
        private Syncfusion.Windows.Forms.Tools.ToggleButton TglBtnAnonymous;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LblMegaMessage;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.TextBox TxtBxUserName;
        private System.Windows.Forms.TextBox TxtBxPassword;
        private System.Windows.Forms.Label LblPassword;
        private Components.Controls.FlatButton BtnSaveMegaInfo;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.Panel PnlMessage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private Syncfusion.WinForms.ListView.SfComboBox DrpDwnLstProcessCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Components.Controls.FlatButton BtnCLearLogs;
        private Components.Controls.FlatButton BtnClearCache;
        private System.Windows.Forms.Label label5;
    }
}
