﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Rlc.Cron;

namespace TRAWebServer
{
    public class Server
    {
        public static HttpServer HttpServer;

        public static Form2 MainForm = new Form2();
        public static Thread MainThread;
        public static bool Debug = false;

        public static string Host;
        public static int Port;
        public static string SecretKey;

        public static CronObject Cron;


        public static int Main(String[] args)
        {

            LoadAppConfigSetting();

            MainForm.FormClosing += mainForm_FormClosing;
            MainForm.resetBtn.Click += resetBtn_Click;
            MainForm.requestTest.Click += requestTest_Click;
            MainForm.StartStop.Click +=StartStop_Click;
            MainForm.configSave.Click +=configSave_Click;

            Application.Run(MainForm);
            return 0;
        }

        public static void ReStartServer()
        {
            if (HttpServer != null)
            {
                WriteDisplay("Shutting Down Server");
                HttpServer.stop();
                WriteDisplay("Restarting Local Server on Port: " + Port);
                HttpServer = new PortalHttpServer(Port);
                MainThread = new Thread(HttpServer.listen);
                MainThread.Start();
            }

        }

        public static void LoadAppConfigSetting()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Host = config.AppSettings.Settings["host"].Value;
            Port = Convert.ToInt32(config.AppSettings.Settings["port"].Value);
            SecretKey = config.AppSettings.Settings["secretKey"].Value;
        }

        static void requestTest_Click(object sender, EventArgs e)
        {
            var requestThread = new Thread(SendRequest);
            requestThread.Start();
        }

        public static void SendRequest()
        {

            Scheduler.Run();

//            mainForm.enableDebug.Checked = false;
//            const string url = "http://certun.com/salus/dataProvider/Api.php";
//            const url = "http://localhost/salus/app/dataProvider/Api.php";


//            CsPortalWeb.ProcessNewAppointmentByApointmentNumber("A0000000000001150020130619051731910000");

//            var rest = new HttpRest(url);
//            // Test GET request
//            if (debug) Server.WriteDisplay("************* Sending GET Request *************");
//            string response = rest.Send("getPatientData", "16");
//            if (debug)
//            {
//                WriteDisplay("URL: " + url);
//                WriteDisplay("Response: " + response);
//            }
//
//            // Test POST request
//            if (debug) Server.WriteDisplay("************* Sending POST Request *************");
//            response = rest.Send("setPatientData", "{\"id\":19,\"fname\":\"API TEst\"}");
//            if (debug)
//            {
//                WriteDisplay("URL: " + url);
//                WriteDisplay("Response: " + response);
//            }
            
        }

        static void StartStop_Click(object sender, EventArgs e)
        {
            if (MainForm.StartStop.Text == @"Start")
            {
//                DatabaseScheduler.Run();
                try
                {
                    // Startting HTTP server
                    HttpServer = new PortalHttpServer(Port);
                    MainThread = new Thread(HttpServer.listen);
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

                    WriteDisplay("Server Started on port: " + Port);
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
                HttpServer.stop();
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
            config.AppSettings.Settings["host"].Value = MainForm.host.Text;
            config.AppSettings.Settings["port"].Value = MainForm.port.Text;
            config.AppSettings.Settings["secretKey"].Value = MainForm.secretKey.Text;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            LoadAppConfigSetting();
            ReStartServer();
        }

        private static void Cron_OnCronTrigger(CronObject cronObject)
        {
            Scheduler.Run();
          
        }

        static void resetBtn_Click(object sender, EventArgs e)
        {
            MainForm.display.ResetText();
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
