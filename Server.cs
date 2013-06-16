using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TRAWebServer
{
    public class Server
    {
        public static Form2 mainForm = new Form2();
        public static Thread thread;
        public static bool debug = false;

        public static int Main(String[] args)
        {
            HttpServer httpServer;
            
            // create Portal Server
            httpServer = new PortalHttpServer(8080);
            thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();


            mainForm.FormClosing += new FormClosingEventHandler(mainForm_FormClosing);
            mainForm.resetBtn.Click += resetBtn_Click;
            mainForm.requestTest.Click += requestTest_Click;

            Application.Run(mainForm);
            return 0;
        }

        static void requestTest_Click(object sender, EventArgs e)
        {
            mainForm.enableDebug.Checked = false;
            HttpRest res = new HttpRest("http://127.0.0.1:8080");
            var data = new RequestData();
            res.BuildRequest("getPatientData", data, res.get);
            res.Send();
            WriteDisplay(res.Send());
            
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
