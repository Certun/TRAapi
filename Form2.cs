using System;
using System.Windows.Forms;
using System.Configuration;

namespace TRAWebServer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            host.Text = ConfigurationManager.AppSettings["host"];
            port.Text = ConfigurationManager.AppSettings["port"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];

        }

        private void saveSettings_Click(object sender, EventArgs e)
        {

        }

        private void enableDebug_CheckedChanged(object sender, EventArgs e)
        {
            Server.Debug = enableDebug.Checked;
        }

        private void requestTest_Click(object sender, EventArgs e)
        {

        }

        private void StartStop_Click(object sender, EventArgs e)
        {

        }

        private void generateKey_Click(object sender, EventArgs e)
        {
            secretKey.Text = Guid.NewGuid().ToString();
        }

        private void configCancel_Click(object sender, EventArgs e)
        {
            host.Text = ConfigurationManager.AppSettings["host"];
            port.Text = ConfigurationManager.AppSettings["port"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];
        }
        
    }
}
