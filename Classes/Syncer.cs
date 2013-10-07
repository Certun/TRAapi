using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace WebPortal.Classes
{
    static class Syncer
    {

        private static EntitiesModel _conn;

        /// <summary>
        /// 
        /// </summary>
        public static void SyncApps()
        {
            if (Server.Debug) Server.WriteDisplay("Creating Data Connection");
            _conn = new EntitiesModel();
            
            if (Server.Debug) Server.WriteDisplay("Getting Appointments to Sync");
            var data = new { appointments = GetNewAppointments() };

            if (!data.appointments.Any())
            {
                if (Server.Debug) Server.WriteDisplay("No Appointments to Sync");
                _conn.Dispose();
                return;
            }

            if (Server.Debug) Server.WriteDisplay("JsonConvert Appointments");
            var jdata =  JsonConvert.SerializeObject(
                data,
                Formatting.None,
                new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
                );

            if (Server.Debug) Server.WriteDisplay("Sending Appointments");
            var results = Send("syncApps", jdata);

            if (Server.Debug) Server.WriteDisplay("Appointments Synced");
            if (Server.Debug) Server.WriteDisplay("Results:");
            if (Server.Debug) Server.WriteDisplay(results);

            ProcessResults(results);
            _conn.Dispose();

        }

        /// <summary>
        /// 
        /// </summary>
        public static void SyncData()
        {
            if (Server.Debug) Server.WriteDisplay("Creating Data Connection");
            _conn = new EntitiesModel();
            
            if (Server.Debug) Server.WriteDisplay("Getting Books and Insurances to Sync");
            var data = new
                {
                    books = GetBooks(), 
                    insuranceCombo = GetInsuranceComboData()
//                    facultyCombo = GetFacultyComboData()
                };

            if (Server.Debug) Server.WriteDisplay("JsonConvert Books and Insurances");
            var jdata = JsonConvert.SerializeObject(
                data,
                Formatting.None,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
                );

            if (Server.Debug) Server.WriteDisplay("Sending Books and Insurances");
            Send("syncData", jdata);
            if (Server.Debug) Server.WriteDisplay("Books and Insurances Synced");
            _conn.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        public static void ProcessResults(dynamic results)
        {
            try
            {
                if (results == null) return;
                if (results.GetType() != typeof(JObject)) return;

                if (!(bool)results["success"])
                {
                    if (Server.Debug) Server.WriteDisplay((string)results["error"]);
                    return;
                }

                var successes = (JArray)results["successes"];
                if (successes == null) return;
                foreach (var appNun in successes)
                {
                    UpdateAppointmentByAppointmentNumber((string)appNun, true);
                }

                var failueres = (JArray)results["failures"];
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
            var appointments = _conn.Apoints.Where(a =>
                // ******* Test Line ******** //
                // ******* Test Line ******** //
                // ******* Test Line ******** //
                
                a.apstatus == "test" &&
                //(a.apstatus != "Procesado" && a.apstatus != "Error") &&
                
                // ***** End Test Line ****** //
                // ***** End Test Line ****** //
                // ***** End Test Line ****** //
                a.entertime > Convert.ToDateTime(DateTime.Now.AddDays(1))
                ).Take(150);

            return appointments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appNum"></param>
        /// <param name="success"></param>
        public static void UpdateAppointmentByAppointmentNumber(string appNum, bool success)
        {
            var app = _conn.Apoints.Single(a => a.apnum == appNum);
            app.apstatus = (success ? "Procesado" : "Error");
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
//                ServicePointManager.ServerCertificateValidationCallback +=
//        (sender, certificate, chain, sslPolicyErrors) => true;

                if (Server.Debug) Server.WriteDisplay("new RestClient()");
                var client = new RestClient(Server.Host + "/dataProvider/Api.php");
                if (Server.Debug) Server.WriteDisplay("new RestRequest()");
                var request = new RestRequest("", Method.POST);

                request.AddParameter("application/json", jdata, ParameterType.RequestBody);
                request.AddHeader("Action", action);
                request.AddHeader("Secret-Key", Server.SecretKey);

                if (Server.Debug) Server.WriteDisplay("RestClient Execute()");
                var response = (RestResponse)client.Execute(request);
//
                if (response.ErrorMessage != "")
                {
                    if (Server.Debug) Server.WriteDisplay("RestRequest() ErrorMessage");
                    if (Server.Debug) Server.WriteDisplay(response.ErrorMessage);
                }

                if (response.Content != String.Empty) return JObject.Parse(response.Content);
                
                Server.WriteDisplay("Something went wront with the web portal");
                return null;
            }
            catch (Exception ex)
            {
                Server.WriteDisplay(ex);
                return null;
            }

        }
    }
}
