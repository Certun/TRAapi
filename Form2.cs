using System;
using System.Data;
using System.Data.SqlClient;
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
            serverIp.Text = ConfigurationManager.AppSettings["serverIp"];
            serverPort.Text = ConfigurationManager.AppSettings["serverPort"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];
            traDirectory.Text = ConfigurationManager.AppSettings["traDirectory"];

            var db = new DbManager("Data Source=desktop-pc\\SQLDATABASE; Initial Catalog=TraData; Trusted_Connection=True");
            const string query = @"SELECT * FROM scan_groups";
            var cmd = new SqlCommand(query, db.Connection);
            var categories = db.GetDataTableResults(cmd);

            patientImgCategory.DataSource = new DataView(categories);
            insuranceImgCategory.DataSource = new DataView(categories);
            documentsCategory.DataSource = new DataView(categories);
            patientImgCategory.Text = ConfigurationManager.AppSettings["patientImgCategory"];
            insuranceImgCategory.Text = ConfigurationManager.AppSettings["insuranceImgCategory"];
            documentsCategory.Text = ConfigurationManager.AppSettings["documentsCategory"];


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
            serverIp.Text = ConfigurationManager.AppSettings["serverIp"];
            serverPort.Text = ConfigurationManager.AppSettings["serverPort"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];
            traDirectory.Text = ConfigurationManager.AppSettings["traDirectory"];
            patientImgCategory.Text = ConfigurationManager.AppSettings["patientImgCategory"];
            insuranceImgCategory.Text = ConfigurationManager.AppSettings["insuranceImgCategory"];
            documentsCategory.Text = ConfigurationManager.AppSettings["documentsCategory"];
        }

        private void serverIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void patientImgCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
