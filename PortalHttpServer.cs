using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using RestSharp.Contrib;

namespace TRAWebServer
{

    public class PortalHttpServer : HttpServer
    {

        public PortalHttpServer(int port)
            : base(port)
        {

        }

        // allow get actions array
        private readonly string[] _getActions = new[] {"*"};

        // allow set action array
        private readonly string[] _setActions = new[] {
            "setPatientData",
            "setAppointmentStatus"
        };

        #region GET Request
        public override void handleGETRequest(HttpProcessor p)
        {
            // write request success headers (THis is required for every reuqest)
            p.WriteSuccess();

            // create  new response object
            var response = new Response();

            // verify server secret key
            if ((string) p.HttpHeaders["Secret-Key"] != Server.SecretKey)
            {
                // handle wrong secretkey error
                response.error = "Access denied";
            }
            else if (String.IsNullOrEmpty((string) p.HttpHeaders["Action"]))
            {
                // handle no action error
                response.error = "No action provided";
            }
            else if (!_getActions.Contains((string) p.HttpHeaders["Action"]))
            {
                // handle action definition error
                response.error = "Action not defined as a get actions";
            }
            else
            {
                // start valid reqiest --------------------------------------------------------------->>>>
                // Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
                var dbMngr = new DbManager("Data Source=ernesto-THINK\\TRA; Initial Catalog=TraData; Trusted_Connection=True");
                var data = new DataSet();
                // store header action for easiest access
                var action = (string) p.HttpHeaders["Action"];
                // request if a single data for example and ID of a patient or an appoointment book
                var request = p.HttpUrl.Substring(1);

                // DO STUFF HERE!
                switch (action)
                {

                    case "getPatientData":
                        // DO STUFF HERE!
                        var query = @"SELECT * FROM DAT2000 WHERE pt_portal_id = @portalId";
                        var cmd = new SqlCommand(query, dbMngr.Connection);
                        cmd.Parameters.AddWithValue("@portalId", request);
                        var results = dbMngr.GetDataTableResults(cmd);

                        results.TableName = "patient";
                        data.Tables.Add(results);

                        query = @"SELECT * FROM Apoint WHERE book_code = '1'";
                        cmd = new SqlCommand(query, dbMngr.Connection);

                        results = dbMngr.GetDataTableResults(cmd);
                        results.TableName = "appointment";
                        data.Tables.Add(results);

                        response.data = data;


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    case "getInsuranceData":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    case "getBooks":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    default:
                        response.error = "Nothing to do";
                        break;

                }


                // write this stuff on display for de bugging
                if (Server.Debug)
                {
                    Server.WriteDisplay("secretKey: " + p.HttpHeaders["Secret-Key"]);
                    Server.WriteDisplay("action: " + p.HttpHeaders["Action"]);
                    Server.WriteDisplay("request: " + request);
                }

                // end valid reqiest <<<<---------------------------------------------------------------
            }

            // JOSN convert response
            var jresponse = JsonConvert.SerializeObject(response);
            // send back response as json
            p.OutputStream.WriteLine(jresponse);
        }
        #endregion

        #region POST Request
        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            var dbMngr = new DbManager("Data Source=ernesto-THINK\\TRA; Initial Catalog=TraData; Trusted_Connection=True");
            var rawData = inputData.ReadToEnd();
            // write request success headers (THis is required for every reuqest)
            p.WriteSuccess();
            // create  new response object
            var response = new Response();
            // verify secret key
            if ((string) p.HttpHeaders["Secret-Key"] != Server.SecretKey)
            {
                // handle wrong secretkey error
                response.error = "Access denied";
            }
            else if (String.IsNullOrEmpty((string) p.HttpHeaders["Action"]))
            {
                // handle no action error
                response.error = "No action provided";
            }
            else if (!_setActions.Contains((string) p.HttpHeaders["Action"]))
            {
                // handle action definition error
                response.error = "Action not defined as a set actions";
            }
            else
            {
                // start valid reqiest --------------------------------------------------------------->>>>
                // store header action for easiest access
                var action = (string) p.HttpHeaders["Action"];
                dynamic data;
                // parse the data into an object
                var unscapedData = HttpUtility.UrlDecode(rawData);
                if (unscapedData.StartsWith("{"))
                {
                    data = JObject.Parse(unscapedData);
                }
                else
                {
                    data = JArray.Parse(unscapedData);
                }


                SqlCommand cmd;
                string query;
                switch (action)
                {
                    case "setPatientData":
                        query = @"UPDATE DAT2000
                                     SET pt_last_name      = @pt_last_name,
                                         pt_first_name     = @pt_first_name,
                                         pt_init_name      = @pt_init_name,
                                         pt_soc_sec_sufx   = @pt_soc_sec_sufx,
                                         pt_sex            = @pt_sex,
                                         pt_civil_status   = @pt_civil_status,
                                         pt_birth_date     = @pt_birth_date,
                                         pt_p_address_1    = @pt_p_address_1,
                                         pt_p_address_2    = @pt_p_address_2,
                                         pt_p_city         = @pt_p_city,
                                         pt_p_state        = @pt_p_state,
                                         pt_p_zip          = @pt_p_zip,
                                         pt_r_address_1    = @pt_r_address_1,
                                         pt_r_address_2    = @pt_r_address_2,
                                         pt_r_city         = @pt_r_city,
                                         pt_r_state        = @pt_r_state,
                                         pt_r_zip          = @pt_r_zip,
                                         pt_home_phone     = @pt_home_phone,
                                         pt_work_phone     = @pt_work_phone,
                                         pt_religion       = @pt_religion,
                                         pt_email          = @pt_email,
                                         pt_cel_phone      = @pt_cel_phone,
                                         pt_language       = @pt_language,
                                         pt_race           = @pt_race
                                   WHERE pt_rec_type       = @pt_rec_type
                                     AND pt_rec_no         = @pt_rec_no
                                     AND pt_rec_suffx      = @pt_rec_suffx";
                        cmd = new SqlCommand(query, dbMngr.Connection);
                        cmd.Parameters.AddWithValue("@pt_last_name",    data["pt_last_name"].ToString());
                        cmd.Parameters.AddWithValue("@pt_first_name",   data["pt_first_name"].ToString());
                        cmd.Parameters.AddWithValue("@pt_init_name",    data["pt_init_name"].ToString());
                        cmd.Parameters.AddWithValue("@pt_soc_sec_sufx", data["pt_soc_sec_sufx"].ToString());
                        cmd.Parameters.AddWithValue("@pt_sex",          data["pt_sex"].ToString());
                        cmd.Parameters.AddWithValue("@pt_civil_status", data["pt_civil_status"].ToString());
                        cmd.Parameters.AddWithValue("@pt_birth_date",   data["pt_birth_date"].ToString());
                        cmd.Parameters.AddWithValue("@pt_p_address_1",  data["pt_p_address_1"].ToString());
                        cmd.Parameters.AddWithValue("@pt_p_address_2",  data["pt_p_address_2"].ToString());
                        cmd.Parameters.AddWithValue("@pt_p_city",       data["pt_p_city"].ToString());
                        cmd.Parameters.AddWithValue("@pt_p_state",      data["pt_p_state"].ToString());
                        cmd.Parameters.AddWithValue("@pt_p_zip",        data["pt_p_zip"].ToString());
                        cmd.Parameters.AddWithValue("@pt_r_address_1",  data["pt_r_address_1"].ToString());
                        cmd.Parameters.AddWithValue("@pt_r_address_2",  data["pt_r_address_2"].ToString());
                        cmd.Parameters.AddWithValue("@pt_r_city",       data["pt_r_city"].ToString());
                        cmd.Parameters.AddWithValue("@pt_r_state",      data["pt_r_state"].ToString());
                        cmd.Parameters.AddWithValue("@pt_r_zip",        data["pt_r_zip"].ToString());
                        cmd.Parameters.AddWithValue("@pt_home_phone",   data["pt_home_phone"].ToString());
                        cmd.Parameters.AddWithValue("@pt_work_phone",   data["pt_work_phone"].ToString());
                        cmd.Parameters.AddWithValue("@pt_religion",     data["pt_religion"].ToString());
                        cmd.Parameters.AddWithValue("@pt_email",        data["pt_email"].ToString());
                        cmd.Parameters.AddWithValue("@pt_cel_phone",    data["pt_cel_phone"].ToString());
                        cmd.Parameters.AddWithValue("@pt_language",     data["pt_language"].ToString());
                        cmd.Parameters.AddWithValue("@pt_race",         data["pt_race"].ToString());
                        cmd.Parameters.AddWithValue("@pt_rec_type",     data["pt_rec_type"].ToString());
                        cmd.Parameters.AddWithValue("@pt_rec_no",       data["pt_rec_no"].ToString());
                        cmd.Parameters.AddWithValue("@pt_rec_suffx",    data["pt_rec_suffx"].ToString());
                        dbMngr.ExecuteNonQuery(cmd);

                        // photo handler
                        if (data["pt_photo_id"].ToString() != "")
                        {
                            var photoBase64 = data["pt_photo_id"].ToString();
                            byte[] photo = Convert.FromBase64String(photoBase64.Substring(photoBase64.IndexOf(',') + 1));
                            using (var stream = new MemoryStream(photo, 0, photo.Length))
                            {
                                var img = Image.FromStream(stream);
                                var imgfrmt = img.RawFormat;
                                img.Save("C:\\Users\\hello.png", imgfrmt);
                                img.Dispose();
                            }
                        }


                        response.success = true;
                        break;

                    case "setInsuranceData":
                        // DO STUFF HERE!




                        response.success = true;
                        break;
                    case "setAppointmentStatus":
                        // DO STUFF HERE!
                        query = @"UPDATE DAT2000 
                                     SET web_portal_status  = @web_portal_status
                                   WHERE pt_rec_type        = @Ap_num";
                        cmd = new SqlCommand(query, dbMngr.Connection);
                        cmd.Parameters.AddWithValue("@web_portal_status", data["web_portal_status"].ToString());
                        cmd.Parameters.AddWithValue("@Ap_num",            data["Ap_num"].ToString());
                        dbMngr.ExecuteNonQuery(cmd);
                        response.success = true;
                        break;

                    default:
                        // define default case
                        response.success = false;
                        response.error = "Nothing to do";
                        break;
                }

                // write this stuff on display for de bugging
                if (Server.Debug)
                {
                    Server.WriteDisplay("secretKey: " + p.HttpHeaders["Secret-Key"]);
                    Server.WriteDisplay("action: " + p.HttpHeaders["Action"]);
                    Server.WriteDisplay("request: " + Uri.UnescapeDataString(rawData));
                }

                // end valid reqiest <<<<---------------------------------------------------------------
            }
            #endregion


            // JOSN convert
            var jresponse = JsonConvert.SerializeObject(response);
            // Send back response
            p.OutputStream.WriteLine(jresponse);
        }
    }
}
