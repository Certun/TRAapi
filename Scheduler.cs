using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using TRAWebServer.DataClasses;

namespace TRAWebServer
{
    static class Scheduler
    {

        private static readonly DbManager Db = new DbManager("Data Source=desktop-pc\\SQLDATABASE; Initial Catalog=TraData; Trusted_Connection=True");

        public static void Run()
        {
            // get all data
            var data = new
                {
                    appointments = NewApointments(), 
                    books = GetBooks(), 
                    insuranceCombo = GetInsuranceComboData(),
                    facultyCombo = GetFacultyComboData()
                };
            // process appointments
            var results = Send("syncData", JsonConvert.SerializeObject(data));
            // process results
            ProcessResults(results);

        }

        public static void ProcessResults(dynamic results)
        {
            
            try
            {
                if (results.GetType() != typeof(JObject)) return;

                if (!(bool)results["success"])
                {
                    Server.WriteDisplay((string) results["error"]);
                    return;
                }
                var successes = (JArray)results["successes"];
                if (successes == null) return;
                foreach (var appNun in successes)
                {
                    UpdateAppointmentByAppointmentNumber((string)appNun, true);
                }

                var failueres = (JArray)results["faillures"];
                if (failueres == null) return;
                foreach (var appNun in failueres)
                {
                    UpdateAppointmentByAppointmentNumber((string)appNun, false);
                }
            }
            catch (Exception ex)
            {
                Server.WriteDisplay(ex);
            }

        }

        private static ArrayList NewApointments()
        {
            var newAppointments = GetNewAppointments();
            var newAppointmentsArray = new ArrayList();
            for (var i = 0; i < newAppointments.Rows.Count; i++)
            {
                var app = newAppointments.Rows[i];
                var pat = GetPatientDemogrphicsByRecordNum((string)app["app_rec_type"],(string)app["app_rec_no"],(string)app["app_rec_suff"]);
                var appointment = new Appointment();
                ConvertDataRowtoObject(app, appointment);
                appointment.ap_Notes = appointment.ap_Notes.Trim();
                var patient = new Patient();
                ConvertDataRowtoObject(pat, patient);
                var insurances = GetPatientInsurancesByRecNum((string)app["app_rec_type"], (string)app["app_rec_no"], (string)app["app_rec_suff"]);
                newAppointmentsArray.Add(new { appointment, patient, insurances });
            }
            return newAppointmentsArray;
        }


        private static DataTable GetNewAppointments()
        {
            const string query = @"SELECT * FROM Apoint WHERE web_portal_status = 0";
            var cmd = new SqlCommand(query, Db.Connection);
            return Db.GetDataTableResults(cmd);
        }

        public static void UpdateAppointmentByAppointmentNumber(string appNum, bool success)
        {
            const string query = @"UPDATE Apoint SET web_portal_status = @Status WHERE Ap_num = @AppNum";
            var cmd = new SqlCommand(query, Db.Connection);
            var status = success ? "1" : "9";
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@AppNum", appNum);
            Db.ExecuteNonQuery(cmd);
        }

        private static DataRow GetPatientDemogrphicsByRecordNum(string type, string no, string suffix)
        {
            const string query = @"SELECT * FROM DAT2000 WHERE pt_rec_type= @AppRecType and pt_rec_no= @PtRecNo and pt_rec_suffx= @PtRecSuffx";
            var cmd = new SqlCommand(query, Db.Connection);
            cmd.Parameters.AddWithValue("@AppRecType", type);
            cmd.Parameters.AddWithValue("@PtRecNo", no);
            cmd.Parameters.AddWithValue("@PtRecSuffx", suffix);
            var patient = Db.GetDataTableResults(cmd);
            return patient.Rows[0];
        }

        private static Array GetPatientInsurancesByRecNum(string type, string no, string suffix)
        {
            const string query = @"SELECT * FROM DAT8000 WHERE pi_pat_type=@AppRecType and pi_pat_no=@PtRecNo and pi_pat_sufx=@PtRecSuffx";
            var cmd = new SqlCommand(query, Db.Connection);
            cmd.Parameters.AddWithValue("@AppRecType", type);
            cmd.Parameters.AddWithValue("@PtRecNo", no);
            cmd.Parameters.AddWithValue("@PtRecSuffx", suffix);
            var table = Db.GetDataTableResults(cmd);
            return ConvertTableToArray(table, "TRAWebServer.DataClasses.Insurance");
        }

        /// <summary>
        /// This will return an array of all books available where book_code is not 0
        /// </summary>
        /// <returns></returns>
        private static Array GetBooks()
        {
            const string query = @"SELECT * FROM Book_Table WHERE Book_Code != '0'";
            var cmd = new SqlCommand(query, Db.Connection);
            var table = Db.GetDataTableResults(cmd);
            return ConvertTableToArray(table, "TRAWebServer.DataClasses.Books");
        }

        /// <summary>
        /// The 
        /// </summary>
        /// <returns></returns>
        private static Array GetInsuranceComboData()
        {
            const string query = @"SELECT INS_CODE, INS_NAME FROM DAT3000";
            var cmd = new SqlCommand(query, Db.Connection);
            var table = Db.GetDataTableResults(cmd);
            return ConvertTableToArray(table, "TRAWebServer.DataClasses.InsuranceCombo");
        }

        /// <summary>
        /// The 
        /// </summary>
        /// <returns></returns>
        private static Array GetFacultyComboData()
        {
            const string query = @"SELECT fac_code, fac_last_name + ', ' + fac_first_name + ' ' + fac_init_name AS fac_fullname FROM DAT9397F";
            var cmd = new SqlCommand(query, Db.Connection);
            var table = Db.GetDataTableResults(cmd);
            return ConvertTableToArray(table, "TRAWebServer.DataClasses.FacultyCombo");
        }

        /// <summary>
        /// TODO: get image from???
        /// </summary>
        /// <param name="type">Record Type</param>
        /// <param name="no">Record Num</param>
        /// <param name="suffix">Record Suffix</param>
        /// <param name="imgType">photoId = Patient Photo ID, insImage = Insurance Image</param>
        /// <param name="identifier">Special identyfier for the image, like "pi_orden" in Insurance Table</param>
        /// <returns></returns>
        private static string GetBase64ImgageByRecNumAndType(string type, string no, string suffix, string imgType, string identifier)
        {
            var file = Assembly.GetEntryAssembly().GetManifestResourceStream("MyProject.Resources.myimage.png");
            if (file == null) return "";

            var img = new Bitmap(file);
            var ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            var imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
            
        }

 
        public static dynamic Send(string action, string jdata)
        {
            try
            {
                var client = new RestClient(Server.Host + "/dataProvider/Api.php");
                var request = new RestRequest("", Method.POST);
                request.AddParameter("application/json", jdata, ParameterType.RequestBody);
                request.AddHeader("Action", action);
                request.AddHeader("Secret-Key", Server.SecretKey);
                var response = (RestResponse)client.Execute(request);
                Server.WriteDisplay(response.Content);
                if (response.Content == String.Empty)
                {
                    Server.WriteDisplay("Something went wront with the web portal");
                    return "";
                }
                return JObject.Parse(response.Content);
            }
            catch (Exception ex)
            {

                Server.WriteDisplay(ex);
            }
            return null;
        }


        private static Array ConvertTableToArray(DataTable table, string newClass)
        {
            try
            {
                var tableArray = new object[table.Rows.Count];
                var type = Type.GetType(newClass);
                if (type != null)
                {
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        var newClassIns = Activator.CreateInstance(type);
                        var ins = table.Rows[i];
                        ConvertDataRowtoObject(ins, newClassIns);
                        tableArray[i] = newClassIns;
                    }
                }
                return tableArray;
            }
            catch (Exception ex)
            {

               // Server.WriteDisplay(ex);
            }

            return null;
        }

        /// Converts an DataRow object in a object type
        /// 
        /// <param name="dataRow">The datarow object to convert</param> 
        /// <param name="objectType">The object type to convert</param> 
        private static void ConvertDataRowtoObject(DataRow dataRow, Object objectType)
        {
            var t = objectType.GetType();
            var propertiesList = t.GetProperties();
            foreach (var properties in propertiesList)
            {
                try
                {
                    t.InvokeMember(
                        properties.Name,
                        BindingFlags.SetProperty,
                        null,
                        objectType,
                        new object[] { dataRow[properties.Name].ToString().Trim() 
                        }
                    );
                }
                catch (Exception ex)
                {
                    Server.WriteDisplay("ConvertDataRowtoObject");
                    Server.WriteDisplay(ex);
                }
            }
        }
    }
}
