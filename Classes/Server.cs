using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Rlc.Cron;

namespace TRAWebServer.Classes
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
        public static string SecretKey;
        public static string DocServer;
        public static string DocDirectory;

        public static string DataConnString;

        public static string PatientImgCategory;
        public static string InsuranceImgCategory;
        public static string DocumentsCategory;

        public static CronObject Cron;
        public static Flags Flag;

        public static void Main(String[] args)
        {

            LoadAppConfigSetting();
            MainForm = new Form2();
            Flag = new Flags();
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
            DocServer    = config.AppSettings.Settings["docServer"].Value;
            DocDirectory = config.AppSettings.Settings["docDirectory"].Value;
            PatientImgCategory      = config.AppSettings.Settings["patientImgCategory"].Value;
            InsuranceImgCategory    = config.AppSettings.Settings["insuranceImgCategory"].Value;
            DocumentsCategory       = config.AppSettings.Settings["documentsCategory"].Value;
            DataConnString = ConfigurationManager.ConnectionStrings["TraDataConnection"].ConnectionString;
        }

        static void forceSync_Click(object sender, EventArgs e)
        {
            var requestThread = new Thread(SendRequest);
            requestThread.Start();
        }

        public static void SendRequest()
        {
            Syncer.SyncApps();          
        }

        static void StartStop_Click(object sender, EventArgs e)
        {
            if (MainForm.StartStop.Text == @"Start")
            {
//                DatabaseScheduler.Run();
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
                    MainForm.StartStop.Text = @"Stop";
                    MainForm.display.BackColor = Color.Aquamarine;

                    WriteDisplay("Server Started on port: " + ServerPort);
                }
                catch (Exception)
                {
                    WriteDisplay("Unable to start server");
                    MainForm.display.BackColor = Color.Tomato;
                }

            }
            else
            {
                // Stopping server 
                HttpServer.Stop();
                // Stoping Cron Job
                Cron.Stop();
                WriteDisplay("Server Stopped");
                // Set button text
                MainForm.StartStop.Text = @"Start";
                MainForm.display.BackColor = Color.White;
            }
        }

        private static void configSave_Click(object sender, EventArgs e)
        {
            Console.WriteLine(@"saveSettings_Click");
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["host"].Value           = MainForm.host.Text.Trim();
            config.AppSettings.Settings["serverIp"].Value       = MainForm.serverIp.Text.Trim();
            config.AppSettings.Settings["serverPort"].Value     = MainForm.serverPort.Text.Trim();
            config.AppSettings.Settings["secretKey"].Value      = MainForm.secretKey.Text.Trim();
            config.AppSettings.Settings["docDirectory"].Value   = MainForm.docDirectory.Text.Trim();
            config.AppSettings.Settings["docServer"].Value      = MainForm.docServer.Text.Trim();
            config.AppSettings.Settings["patientImgCategory"].Value     = MainForm.patientImgCategory.SelectedValue.ToString();
            config.AppSettings.Settings["insuranceImgCategory"].Value   = MainForm.insuranceImgCategory.SelectedValue.ToString();
            config.AppSettings.Settings["documentsCategory"].Value      = MainForm.documentsCategory.SelectedValue.ToString();

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            LoadAppConfigSetting();
            ReStartServer();
            MessageBox.Show(@"Configuration Saved!");
        }

        private static void Cron_OnCronTrigger(CronObject cronObject)
        {
            Syncer.SyncApps();
          
        }

        static void resetBtn_Click(object sender, EventArgs e)
        {
            MainForm.display.ResetText();

            var flg = Flag.Program("frm2000").Type("RPT").Code(2).GetFlag();
            var val = Flag.Program("frm2000").Type("RPT").Code(4).GetValue();
            WriteDisplay(flg);
            WriteDisplay(val);


            var foo = Flag.Program("frm2000").Type("RPT").Code(6);
            WriteDisplay(foo.GetFlag());
            WriteDisplay(foo.GetValue());
        }

        static void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

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
                text = msg.ToString();
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
                textBox.Text += dtime + text + Environment.NewLine;
            }
        }

    }
}
