namespace TRAWebServer
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
            this.requestTest = new System.Windows.Forms.Button();
            this.enableDebug = new System.Windows.Forms.CheckBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.traDirectory = new System.Windows.Forms.TextBox();
            this.generateKey = new System.Windows.Forms.Button();
            this.configCancel = new System.Windows.Forms.Button();
            this.host = new System.Windows.Forms.TextBox();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.secretKey = new System.Windows.Forms.TextBox();
            this.configSave = new System.Windows.Forms.Button();
            this.serverIp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tabControl1.Size = new System.Drawing.Size(512, 435);
            this.tabControl1.TabIndex = 3;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.StartStop);
            this.tabDisplay.Controls.Add(this.requestTest);
            this.tabDisplay.Controls.Add(this.enableDebug);
            this.tabDisplay.Controls.Add(this.display);
            this.tabDisplay.Controls.Add(this.resetBtn);
            this.tabDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplay.Size = new System.Drawing.Size(504, 409);
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
            // requestTest
            // 
            this.requestTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.requestTest.AutoSize = true;
            this.requestTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.requestTest.Location = new System.Drawing.Point(261, 380);
            this.requestTest.Name = "requestTest";
            this.requestTest.Size = new System.Drawing.Size(75, 23);
            this.requestTest.TabIndex = 7;
            this.requestTest.Text = "Send Test";
            this.requestTest.UseVisualStyleBackColor = true;
            this.requestTest.Click += new System.EventHandler(this.requestTest_Click);
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
            this.tabSettings.Controls.Add(this.label5);
            this.tabSettings.Controls.Add(this.serverIp);
            this.tabSettings.Controls.Add(this.label4);
            this.tabSettings.Controls.Add(this.traDirectory);
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
            this.tabSettings.Size = new System.Drawing.Size(504, 409);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "TRA Directory";
            // 
            // traDirectory
            // 
            this.traDirectory.Location = new System.Drawing.Point(116, 113);
            this.traDirectory.Name = "traDirectory";
            this.traDirectory.Size = new System.Drawing.Size(354, 20);
            this.traDirectory.TabIndex = 9;
            // 
            // generateKey
            // 
            this.generateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateKey.AutoSize = true;
            this.generateKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.generateKey.Location = new System.Drawing.Point(363, 170);
            this.generateKey.Name = "generateKey";
            this.generateKey.Size = new System.Drawing.Size(107, 23);
            this.generateKey.TabIndex = 8;
            this.generateKey.Text = "Generate New Key";
            this.generateKey.UseVisualStyleBackColor = true;
            this.generateKey.Click += new System.EventHandler(this.generateKey_Click);
            // 
            // configCancel
            // 
            this.configCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.configCancel.Location = new System.Drawing.Point(342, 380);
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
            this.host.Location = new System.Drawing.Point(116, 22);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(354, 20);
            this.host.TabIndex = 6;
            this.host.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // serverPort
            // 
            this.serverPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverPort.Location = new System.Drawing.Point(116, 84);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(354, 20);
            this.serverPort.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Web Portal Host:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Secret Key:";
            // 
            // secretKey
            // 
            this.secretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secretKey.Location = new System.Drawing.Point(116, 144);
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(354, 20);
            this.secretKey.TabIndex = 1;
            // 
            // configSave
            // 
            this.configSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.configSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.configSave.Location = new System.Drawing.Point(423, 380);
            this.configSave.Name = "configSave";
            this.configSave.Size = new System.Drawing.Size(75, 23);
            this.configSave.TabIndex = 0;
            this.configSave.Text = "Save";
            this.configSave.UseVisualStyleBackColor = true;
            this.configSave.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // serverIp
            // 
            this.serverIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverIp.Location = new System.Drawing.Point(116, 53);
            this.serverIp.Name = "serverIp";
            this.serverIp.Size = new System.Drawing.Size(354, 20);
            this.serverIp.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Server IP:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 596);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(554, 634);
            this.Name = "Form2";
            this.Text = "TRA Portal Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabDisplay.ResumeLayout(false);
            this.tabDisplay.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
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
        public System.Windows.Forms.Button requestTest;
        public System.Windows.Forms.CheckBox enableDebug;
        public System.Windows.Forms.Button StartStop;
        public System.Windows.Forms.Button configSave;
        public System.Windows.Forms.TextBox host;
        public System.Windows.Forms.TextBox serverPort;
        public System.Windows.Forms.TextBox secretKey;
        private System.Windows.Forms.Button generateKey;
        private System.Windows.Forms.Button configCancel;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox traDirectory;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox serverIp;

    }
}