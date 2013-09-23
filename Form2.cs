using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using TRAWebServer.Classes;

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
            host.Text = Server.Host;
            serverIp.Text = Server.ServerIp;
            serverPort.Text = Server.ServerPort.ToString(CultureInfo.InvariantCulture);
            startDate.Text = Server.StartDate;
            secretKey.Text = Server.SecretKey;
            docServer.Text = Server.DocServer;
            docDirectory.Text = Server.DocDirectory;

            var conn = new EntitiesModel();
            var grplist = conn.ScanGroups.ToList();

            // New table.
            var table = new DataTable();
            table.Columns.Add("group_code");
            table.Columns.Add("group_description");
            // Add rows.
            foreach (var g in grplist) table.Rows.Add(g.groupcode, g.groupdescription);
            
            patientImgCategory.DataSource = new DataView(table);
            insuranceImgCategory.DataSource = new DataView(table);
            documentsCategory.DataSource = new DataView(table);

            patientImgCategory.SelectedValue = Server.PatientImgCategory;
            insuranceImgCategory.SelectedValue = Server.InsuranceImgCategory;
            documentsCategory.SelectedValue = Server.DocumentsCategory;


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
            startDate.Text = ConfigurationManager.AppSettings["startDate"];
            secretKey.Text = ConfigurationManager.AppSettings["secretKey"];
            docDirectory.Text = ConfigurationManager.AppSettings["docDirectory"];
            docServer.Text = ConfigurationManager.AppSettings["docServer"];
            patientImgCategory.SelectedValue = ConfigurationManager.AppSettings["patientImgCategory"];
            insuranceImgCategory.SelectedValue = ConfigurationManager.AppSettings["insuranceImgCategory"];
            documentsCategory.SelectedValue = ConfigurationManager.AppSettings["documentsCategory"];
        }

        private void serverIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void patientImgCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
    }
}
