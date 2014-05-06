using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Rlc.Cron;

namespace WebPortal.Classes
{
    public class Server
    {
        public static HttpServer HttpServer;

        public static Form2 MainForm;
        public static Thread MainThread;
        public static bool Debug = false;

        public static string Host;
        public static string ServerIp;
        public static int ServerPort;
        public static string TestUser;
        public static string TestArea;
        public static string SecretKey;
        public static string DocServer;
        public static string DocDirectory;

        public static string DataConnString;

        public static string PatientImgCategory;
        public static string InsuranceImgCategory;
        public static string DocumentsCategory;
        public static string SyncBuffer;

        public static CronObject Cron;
//        public static Flags Flag;

        public static void Main(String[] args)
        {
            LoadAppConfigSetting();
            MainForm = new Form2 {Text = @"Web Portal Server - v1.2.285"};
//            Flag = new Flags();
            MainForm.FormClosing += mainForm_FormClosing;
            MainForm.resetBtn.Click += resetBtn_Click;
            MainForm.forceSync.Click += forceSync_Click;
            MainForm.StartStop.Click +=StartStop_Click;
            MainForm.configSave.Click +=configSave_Click;
            Application.Run(MainForm);
        }

        public static void ReStartServer()
        {
            if (HttpServer == null) return;
            WriteDisplay("Shutting Down Server");
            HttpServer.Stop();
            WriteDisplay("Restarting Local Server on Port: " + ServerPort);
            HttpServer = new PortalHttpServer(ServerPort);
            MainThread = new Thread(HttpServer.Listen);
            MainThread.Start();
        }

        public static void LoadAppConfigSetting()
        {
            var config   = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Host         = config.AppSettings.Settings["host"].Value;
            ServerIp     = config.AppSettings.Settings["serverIp"].Value;
            ServerPort   = Convert.ToInt32(config.AppSettings.Settings["serverPort"].Value);
            SecretKey    = config.AppSettings.Settings["secretKey"].Value;
            TestUser     = config.AppSettings.Settings["testUser"].Value;
            TestArea     = config.AppSettings.Settings["testArea"].Value;
            DocServer    = config.AppSettings.Settings["docServer"].Value;
            DocDirectory = config.AppSettings.Settings["docDirectory"].Value;
            PatientImgCategory      = config.AppSettings.Settings["patientImgCategory"].Value;
            InsuranceImgCategory    = config.AppSettings.Settings["insuranceImgCategory"].Value;
            DocumentsCategory       = config.AppSettings.Settings["documentsCategory"].Value;
            SyncBuffer              = config.AppSettings.Settings["syncBuffer"].Value;
            DataConnString = ConfigurationManager.ConnectionStrings["TraDataConnection"].ConnectionString;
        }

        static void forceSync_Click(object sender, EventArgs e)
        {
            if (Debug) WriteDisplay("Forcing Sync...");
            new Thread(SendRequest).Start();
        }

        public static void SendRequest()
        {
            if (Debug) WriteDisplay("************* Sending Request *************");
            Syncer.SyncData();
            Syncer.SyncApps();
            Syncer.SyncDeleted();  
        }

        static void StartStop_Click(object sender, EventArgs e)
        {
            if (MainForm.StartStop.Text == @"Start")
            {
                StartServer();
            }
            else
            {
                StopServer();
            }
        }

        public static void StartServer()
        {
            try
            {
                // Startting HTTP server
                HttpServer = new PortalHttpServer(ServerPort);
                MainThread = new Thread(HttpServer.Listen);
                MainThread.Start();
                // Starting Cron Job
                var now = DateTime.Now;
                var cronSchedule = CronSchedule.Parse("* * * * *");
                var cronSchedules = new List<CronSchedule> { cronSchedule };
                var dc = new CronObjectDataContext
                {
                    Object = DateTime.Now,
                    CronSchedules = cronSchedules,
                    LastTrigger = now
                };
                Cron = new CronObject(dc);
                Cron.OnCronTrigger += Cron_OnCronTrigger;
                Cron.Start();
                // Set button text
                SetTextBtn(@"Stop");
                SetColorDisplay(Color.Aquamarine);

                WriteDisplay("Server Started on port: " + ServerPort);
            }
            catch (Exception)
            {
                WriteDisplay("Unable to start server");
                SetColorDisplay(Color.Tomato);
            }
        }
        public static void StopServer()
        {
            try
            {
                // Stopping server 
                HttpServer.Stop();
                // Stoping Cron Job
                Cron.Stop();
                WriteDisplay("Server Stopped");
                // Set button text
                SetTextBtn(@"Start");
                SetColorDisplay(Color.White);
            }
            catch (Exception)
            {
                WriteDisplay("Unable to stop server");
                SetColorDisplay(Color.Tomato);
            }

        }

        private static void configSave_Click(object sender, EventArgs e)
        {
            Console.WriteLine(@"saveSettings_Click");
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["host"].Value           = MainForm.host.Text.Trim();
            config.AppSettings.Settings["serverIp"].Value       = MainForm.serverIp.Text.Trim();
            config.AppSettings.Settings["serverPort"].Value     = MainForm.serverPort.Text.Trim();
            config.AppSettings.Settings["testUser"].Value       = MainForm.testUser.Text.Trim();
            config.AppSettings.Settings["testArea"].Value       = MainForm.testArea.Text.Trim();
            config.AppSettings.Settings["secretKey"].Value      = MainForm.secretKey.Text.Trim();
            config.AppSettings.Settings["docDirectory"].Value   = MainForm.docDirectory.Text.Trim();
            config.AppSettings.Settings["docServer"].Value      = MainForm.docServer.Text.Trim();
            config.AppSettings.Settings["patientImgCategory"].Value     = MainForm.patientImgCategory.SelectedValue.ToString();
            config.AppSettings.Settings["insuranceImgCategory"].Value   = MainForm.insuranceImgCategory.SelectedValue.ToString();
            config.AppSettings.Settings["documentsCategory"].Value      = MainForm.documentsCategory.SelectedValue.ToString();
            config.AppSettings.Settings["syncbuffer"].Value = MainForm.syncBuffer.Text;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            LoadAppConfigSetting();
            ReStartServer();
            MessageBox.Show(@"Configuration Saved!");
        }

        private static void Cron_OnCronTrigger(CronObject cronObject)
        {
            SendRequest();         
        }

        static void resetBtn_Click(object sender, EventArgs e)
        {
            MainForm.display.ResetText();
        }

        static void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        delegate void WriteDisplayCallabck(string text);
        public static void WriteDisplay(dynamic msg)
        {
            string text;
            if (msg is Exception)
            {
                text = msg.Message;
            }
            else
            {
                text = msg != null ? msg.ToString() : "";
            }

            var dtime = "[ "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ] ";
            var textBox = MainForm.display;
            if (textBox.InvokeRequired)
            {
                var d = new WriteDisplayCallabck(WriteDisplay);
                textBox.Invoke(d, new object[] { text });
            }
            else
            {
                textBox.Text = dtime + text + Environment.NewLine + textBox.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        delegate void SetColorDisplayCallabck(Color color);
        public static void SetColorDisplay(Color color)
        {
            var display = MainForm.display;
            if (display.InvokeRequired)
            {
                var d = new SetColorDisplayCallabck(SetColorDisplay);
                display.Invoke(d, new object[] { color });
            }
            else
            {
                display.BackColor = color;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextBtnCallabck(string text);
        public static void SetTextBtn(string text)
        {
            var btn = MainForm.StartStop;
            if (btn.InvokeRequired)
            {
                var d = new SetTextBtnCallabck(SetTextBtn);
                btn.Invoke(d, new object[] { text });
            }
            else
            {
                btn.Text = text;
            }
        }

    }
}
