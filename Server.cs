using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TRAWebServer
{
    public class Server
    {
        public static HttpServer httpServer;

        public static Form2 mainForm = new Form2();
        public static Thread thread;
        public static bool debug = false;

        public static string ip;
        public static int port;
        public static string secretKey;

        public static int Main(String[] args)
        {          

            loadAppConfigSetting();

            // create Portal Server
            WriteDisplay("Starting Local Server on Port: " + port);
            httpServer = new PortalHttpServer(port);
            thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();

            mainForm.FormClosing += new FormClosingEventHandler(mainForm_FormClosing);
            mainForm.resetBtn.Click += resetBtn_Click;
            mainForm.requestTest.Click += requestTest_Click;

            Application.Run(mainForm);
            return 0;
        }

        public static void reStartServer()
        {
            WriteDisplay("Shutting Down Server");
            httpServer.stop();
            WriteDisplay("Restarting Local Server on Port: " + port);
            httpServer = new PortalHttpServer(port);
            thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }

        public static void loadAppConfigSetting()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ip = config.AppSettings.Settings["ip"].Value;
            port = Convert.ToInt32(config.AppSettings.Settings["port"].Value);
            secretKey = config.AppSettings.Settings["secretKey"].Value;
        }

        static void requestTest_Click(object sender, EventArgs e)
        {
            
            //mainForm.enableDebug.Checked = false;
            string url = "http://certun.com/salus/dataProvider/Api.php";
            //string url = "http://localhost/salus/app/dataProvider/Api.php";
            HttpRest rest = new HttpRest(url);
            
            // Test GET request
            if (debug) Server.WriteDisplay("************* Sending GET Request *************");
            string response = rest.Send("getPatientData", "16");
            if (debug)
            {
                WriteDisplay("URL: " + url);
                WriteDisplay("Response: " + response);
            }

            // Test POST request
            if (debug) Server.WriteDisplay("************* Sending POST Request *************");
            response = rest.Send("setPatientData", "{\"id\":19,\"fname\":\"API TEst\"}");
            if (debug)
            {
                WriteDisplay("URL: " + url);
                WriteDisplay("Response: " + response);
            }
            
        }

        static void resetBtn_Click(object sender, EventArgs e)
        {
            mainForm.display.ResetText();
        }

        static void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        delegate void WriteDisplayCallabck(string text);

        public static void WriteDisplay(string text)
        {

            RichTextBox textBox = mainForm.display;
            if (textBox.InvokeRequired)
            {
                WriteDisplayCallabck d = new WriteDisplayCallabck(WriteDisplay);
                textBox.Invoke(d, new object[] { text });
            }
            else
            {
                textBox.Text += text + Environment.NewLine;
            }
        }
    }
}
