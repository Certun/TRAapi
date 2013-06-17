using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
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
            ip.Text = ConfigurationManager.AppSettings["ip"];
            port.Text = ConfigurationManager.AppSettings["port"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];

        }

        private void saveSettings_Click(object sender, EventArgs e)
        {

            Console.WriteLine("saveSettings_Click");
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["ip"].Value = ip.Text;
            config.AppSettings.Settings["port"].Value = port.Text;
            config.AppSettings.Settings["secretKey"].Value = secretKey.Text;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            Server.loadAppConfigSetting();
            Server.reStartServer();
        }

        private void enableDebug_CheckedChanged(object sender, EventArgs e)
        {
            Server.debug = enableDebug.Checked;
        }

        private void requestTest_Click(object sender, EventArgs e)
        {

        }
        
    }
}
