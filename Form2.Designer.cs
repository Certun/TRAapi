﻿namespace WebPortal
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.display = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.StartStop = new System.Windows.Forms.Button();
            this.forceSync = new System.Windows.Forms.Button();
            this.enableDebug = new System.Windows.Forms.CheckBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.testUser = new System.Windows.Forms.TextBox();
            this.syncBuffer = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.docServer = new System.Windows.Forms.TextBox();
            this.documentsCategory = new System.Windows.Forms.ComboBox();
            this.insuranceImgCategory = new System.Windows.Forms.ComboBox();
            this.patientImgCategory = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.docDirectory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.serverIp = new System.Windows.Forms.TextBox();
            this.generateKey = new System.Windows.Forms.Button();
            this.configCancel = new System.Windows.Forms.Button();
            this.host = new System.Windows.Forms.TextBox();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.secretKey = new System.Windows.Forms.TextBox();
            this.configSave = new System.Windows.Forms.Button();
            this.testArea = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.syncBuffer)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.display.Location = new System.Drawing.Point(6, 6);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(492, 368);
            this.display.TabIndex = 0;
            this.display.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 130);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // resetBtn
            // 
            this.resetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetBtn.Location = new System.Drawing.Point(342, 380);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(75, 23);
            this.resetBtn.TabIndex = 2;
            this.resetBtn.Text = "Reset Log";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabDisplay);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 149);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(512, 499);
            this.tabControl1.TabIndex = 3;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.StartStop);
            this.tabDisplay.Controls.Add(this.forceSync);
            this.tabDisplay.Controls.Add(this.enableDebug);
            this.tabDisplay.Controls.Add(this.display);
            this.tabDisplay.Controls.Add(this.resetBtn);
            this.tabDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplay.Size = new System.Drawing.Size(504, 436);
            this.tabDisplay.TabIndex = 0;
            this.tabDisplay.Text = "Log";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // StartStop
            // 
            this.StartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StartStop.Location = new System.Drawing.Point(423, 380);
            this.StartStop.Name = "StartStop";
            this.StartStop.Size = new System.Drawing.Size(75, 23);
            this.StartStop.TabIndex = 8;
            this.StartStop.Text = "Start";
            this.StartStop.UseVisualStyleBackColor = true;
            this.StartStop.Click += new System.EventHandler(this.StartStop_Click);
            // 
            // forceSync
            // 
            this.forceSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSync.AutoSize = true;
            this.forceSync.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.forceSync.Location = new System.Drawing.Point(261, 380);
            this.forceSync.Name = "forceSync";
            this.forceSync.Size = new System.Drawing.Size(75, 23);
            this.forceSync.TabIndex = 7;
            this.forceSync.Text = "Force Sync";
            this.forceSync.UseVisualStyleBackColor = true;
            this.forceSync.Click += new System.EventHandler(this.requestTest_Click);
            // 
            // enableDebug
            // 
            this.enableDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.enableDebug.AutoSize = true;
            this.enableDebug.Location = new System.Drawing.Point(6, 384);
            this.enableDebug.Name = "enableDebug";
            this.enableDebug.Size = new System.Drawing.Size(136, 17);
            this.enableDebug.TabIndex = 7;
            this.enableDebug.Text = "Enable Debug Verbose";
            this.enableDebug.UseVisualStyleBackColor = true;
            this.enableDebug.CheckedChanged += new System.EventHandler(this.enableDebug_CheckedChanged);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.label12);
            this.tabSettings.Controls.Add(this.testArea);
            this.tabSettings.Controls.Add(this.label10);
            this.tabSettings.Controls.Add(this.testUser);
            this.tabSettings.Controls.Add(this.syncBuffer);
            this.tabSettings.Controls.Add(this.label11);
            this.tabSettings.Controls.Add(this.groupBox1);
            this.tabSettings.Controls.Add(this.label5);
            this.tabSettings.Controls.Add(this.serverIp);
            this.tabSettings.Controls.Add(this.generateKey);
            this.tabSettings.Controls.Add(this.configCancel);
            this.tabSettings.Controls.Add(this.host);
            this.tabSettings.Controls.Add(this.serverPort);
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.secretKey);
            this.tabSettings.Controls.Add(this.configSave);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(504, 473);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Test User(s):";
            // 
            // testUser
            // 
            this.testUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testUser.Location = new System.Drawing.Point(116, 175);
            this.testUser.Name = "testUser";
            this.testUser.Size = new System.Drawing.Size(365, 20);
            this.testUser.TabIndex = 15;
            // 
            // syncBuffer
            // 
            this.syncBuffer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.syncBuffer.Location = new System.Drawing.Point(116, 148);
            this.syncBuffer.Name = "syncBuffer";
            this.syncBuffer.Size = new System.Drawing.Size(120, 20);
            this.syncBuffer.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Sync Buffer (min):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.docServer);
            this.groupBox1.Controls.Add(this.documentsCategory);
            this.groupBox1.Controls.Add(this.insuranceImgCategory);
            this.groupBox1.Controls.Add(this.patientImgCategory);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.docDirectory);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(18, 242);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 178);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Categories";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label9.Location = new System.Drawing.Point(9, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Document Server";
            // 
            // docServer
            // 
            this.docServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.docServer.Location = new System.Drawing.Point(136, 109);
            this.docServer.Name = "docServer";
            this.docServer.Size = new System.Drawing.Size(293, 20);
            this.docServer.TabIndex = 11;
            // 
            // documentsCategory
            // 
            this.documentsCategory.DisplayMember = "group_description";
            this.documentsCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.documentsCategory.FormattingEnabled = true;
            this.documentsCategory.Location = new System.Drawing.Point(137, 81);
            this.documentsCategory.Name = "documentsCategory";
            this.documentsCategory.Size = new System.Drawing.Size(293, 21);
            this.documentsCategory.TabIndex = 5;
            this.documentsCategory.ValueMember = "group_code";
            // 
            // insuranceImgCategory
            // 
            this.insuranceImgCategory.DisplayMember = "group_description";
            this.insuranceImgCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.insuranceImgCategory.FormattingEnabled = true;
            this.insuranceImgCategory.Location = new System.Drawing.Point(137, 54);
            this.insuranceImgCategory.Name = "insuranceImgCategory";
            this.insuranceImgCategory.Size = new System.Drawing.Size(293, 21);
            this.insuranceImgCategory.TabIndex = 4;
            this.insuranceImgCategory.ValueMember = "group_code";
            // 
            // patientImgCategory
            // 
            this.patientImgCategory.DisplayMember = "group_description";
            this.patientImgCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.patientImgCategory.FormattingEnabled = true;
            this.patientImgCategory.Location = new System.Drawing.Point(137, 26);
            this.patientImgCategory.Name = "patientImgCategory";
            this.patientImgCategory.Size = new System.Drawing.Size(293, 21);
            this.patientImgCategory.TabIndex = 3;
            this.patientImgCategory.ValueMember = "group_code";
            this.patientImgCategory.SelectedIndexChanged += new System.EventHandler(this.patientImgCategory_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label4.Location = new System.Drawing.Point(10, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Documents Directory";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label8.Location = new System.Drawing.Point(10, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Uploaded Documents";
            // 
            // docDirectory
            // 
            this.docDirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.docDirectory.Location = new System.Drawing.Point(137, 136);
            this.docDirectory.Name = "docDirectory";
            this.docDirectory.Size = new System.Drawing.Size(293, 20);
            this.docDirectory.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label7.Location = new System.Drawing.Point(10, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Insurance Image";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label6.Location = new System.Drawing.Point(10, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Patient Image";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label5.Location = new System.Drawing.Point(15, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Server IP:";
            // 
            // serverIp
            // 
            this.serverIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverIp.Location = new System.Drawing.Point(116, 53);
            this.serverIp.Name = "serverIp";
            this.serverIp.Size = new System.Drawing.Size(365, 20);
            this.serverIp.TabIndex = 11;
            this.serverIp.TextChanged += new System.EventHandler(this.serverIp_TextChanged);
            // 
            // generateKey
            // 
            this.generateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateKey.AutoSize = true;
            this.generateKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.generateKey.Location = new System.Drawing.Point(374, 142);
            this.generateKey.Name = "generateKey";
            this.generateKey.Size = new System.Drawing.Size(107, 26);
            this.generateKey.TabIndex = 8;
            this.generateKey.Text = "Generate New Key";
            this.generateKey.UseVisualStyleBackColor = true;
            this.generateKey.Click += new System.EventHandler(this.generateKey_Click);
            // 
            // configCancel
            // 
            this.configCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.configCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.configCancel.Location = new System.Drawing.Point(342, 444);
            this.configCancel.Name = "configCancel";
            this.configCancel.Size = new System.Drawing.Size(75, 23);
            this.configCancel.TabIndex = 7;
            this.configCancel.Text = "Cancel";
            this.configCancel.UseVisualStyleBackColor = true;
            this.configCancel.Click += new System.EventHandler(this.configCancel_Click);
            // 
            // host
            // 
            this.host.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.host.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.host.Location = new System.Drawing.Point(116, 22);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(365, 20);
            this.host.TabIndex = 6;
            this.host.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // serverPort
            // 
            this.serverPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverPort.Location = new System.Drawing.Point(116, 84);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(365, 20);
            this.serverPort.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Location = new System.Drawing.Point(14, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Location = new System.Drawing.Point(15, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Web Portal Host:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Location = new System.Drawing.Point(15, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Secret Key:";
            // 
            // secretKey
            // 
            this.secretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secretKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.secretKey.Location = new System.Drawing.Point(116, 116);
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(365, 20);
            this.secretKey.TabIndex = 1;
            // 
            // configSave
            // 
            this.configSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.configSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.configSave.Location = new System.Drawing.Point(423, 444);
            this.configSave.Name = "configSave";
            this.configSave.Size = new System.Drawing.Size(75, 23);
            this.configSave.TabIndex = 0;
            this.configSave.Text = "Save";
            this.configSave.UseVisualStyleBackColor = true;
            this.configSave.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // testArea
            // 
            this.testArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testArea.Location = new System.Drawing.Point(116, 206);
            this.testArea.Name = "testArea";
            this.testArea.Size = new System.Drawing.Size(365, 20);
            this.testArea.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Test Area(s):";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 660);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(554, 634);
            this.Name = "Form2";
            this.Text = "Web Portal Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabDisplay.ResumeLayout(false);
            this.tabDisplay.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.syncBuffer)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox display;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button forceSync;
        public System.Windows.Forms.CheckBox enableDebug;
        public System.Windows.Forms.Button StartStop;
        public System.Windows.Forms.Button configSave;
        public System.Windows.Forms.TextBox host;
        public System.Windows.Forms.TextBox serverPort;
        public System.Windows.Forms.TextBox secretKey;
        private System.Windows.Forms.Button generateKey;
        private System.Windows.Forms.Button configCancel;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox docDirectory;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox serverIp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox documentsCategory;
        public System.Windows.Forms.ComboBox insuranceImgCategory;
        public System.Windows.Forms.ComboBox patientImgCategory;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox docServer;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.NumericUpDown syncBuffer;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox testUser;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox testArea;

    }
}