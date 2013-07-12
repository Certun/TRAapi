using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TRAWebServer.Classes
{
    static class Syncer
    {

        private static EntitiesModel _conn;
        private const string AppToSyncStatus = "0";

        /// <summary>
        /// 
        /// </summary>
        public static void SyncApps()
        {
            _conn = new EntitiesModel();
            // get all data
            var data = new { appointments = GetNewAppointments() };
            // process appointments
            var results = Send(
                "syncApps",
                JsonConvert.SerializeObject(
                    data,
                    Formatting.None,
                    new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
                    )
                );
            // process results
            ProcessResults(results);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SyncData()
        {
            _conn = new EntitiesModel();
            // get all data
            var data = new
                {
                    books = GetBooks(), 
                    insuranceCombo = GetInsuranceComboData(),
                    facultyCombo = GetFacultyComboData()
                };
            // process appointments
            var results = Send(
                "syncData",
                JsonConvert.SerializeObject(
                    data,
                    Formatting.None,
                    new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
                    )
                );
            // process results
            ProcessResults(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Apoint> GetNewAppointments()
        {
            return _conn.Apoints.Where(a => a.apstatus == AppToSyncStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appNum"></param>
        /// <param name="success"></param>
        public static void UpdateAppointmentByAppointmentNumber(string appNum, bool success)
        {
            var app = _conn.Apoints.Single(a => a.apnum == appNum);
            app.apstatus = "1";
            _conn.SaveChanges();
        }

        /// <summary>
        /// This will return an array of all books available where book_code is not 0
        /// </summary>
        /// <returns></returns>
        private static IQueryable<BookTable> GetBooks()
        {
            return  _conn.BookTables.Where(b => b.bookcode != 0);
        }

        /// <summary>
        /// The 
        /// </summary>
        /// <returns></returns>
        private static List<DAT3000> GetInsuranceComboData()
        {
            return  _conn.DAT3000.ToList();
        }

        /// <summary>
        /// The 
        /// </summary>
        /// <returns></returns>
        private static List<DAT9397F> GetFacultyComboData()
        {
            return _conn.DAT9397Fs.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="jdata"></param>
        /// <returns></returns>
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
    }
}
