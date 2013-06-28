using System.Drawing;
using System.Globalization;
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
        readonly DbManager _dbMngr = new DbManager("Data Source=ernesto-THINK\\TRA; Initial Catalog=TraData; Trusted_Connection=True");

        public PortalHttpServer(int port)
            : base(port)
        {

        }

        // allow get actions array
        private readonly string[] _getActions = new[] {"*"};

        // allow set action array
        private readonly string[] _setActions = new[] {
            "setPatientData",
            "setAppointmentStatus",
            "setSync"
        };

        #region GET Request
        public override void HandleGetRequest(HttpProcessor p)
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

                        response.results = data;


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
        public override void HandlePostRequest(HttpProcessor p, StreamReader inputData)
        {
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


                switch (action)
                {
                    case "setSync":

                        response.results = new JObject();
                        
                        var app = (JArray) data["appointments"];
                        if (app.Any())
                        {
                            response.results.appointments = new JObject();
                            response.results.appointments.successes = new JArray();
                            response.results.appointments.failures = new JArray();
                            
                            for (var i = 0; i < app.Count(); i++)
                            {
                                if (WorkAppointment(app[i]))
                                {
                                    response.results.appointments.successes.Add(app[i]);
                                }
                                else
                                {
                                    response.results.appointments.failures.Add(app[i]);
                                }
                            }
                        }

                        var pat = (JArray)data["patients"];
                        if (pat.Any())
                        {
                            response.results.patients = new JObject();
                            response.results.patients.successes = new JArray();
                            response.results.patients.failures = new JArray();

                            for (var i = 0; i < pat.Count(); i++)
                            {
                                if (WorkPaitnet(pat[i]))
                                {
                                    response.results.patients.successes.Add(pat[i]);
                                }
                                else
                                {
                                    response.results.patients.failures.Add(pat[i]);
                                }
                            }
                        }

                        var ins = (JArray) data["insurance"];
                        if (ins.Any())
                        {
                            response.results.insurance = new JObject();
                            response.results.insurance.successes = new JArray();
                            response.results.insurance.failures = new JArray();

                            for (var i = 0; i < ins.Count(); i++)
                            {
                                var isGood = WorkInsurence(ins[i]);
                                if (isGood.GetType() == typeof (DataTable))
                                {
                                    ins[i]["pi_orden"] = isGood.Rows[0]["pi_orden"].ToString();

//                                    var returnData = (DataRow) isGood.Rows[0];
//                                    var insurance = new Insurance();
//                                    ConvertDataRowtoObject(returnData, insurance);
                                    response.results.insurance.successes.Add(ins[i]);
                                }
                                else if(isGood)
                                {
                                    response.results.insurance.successes.Add(ins[i]);
                                }
                                else
                                {
                                    response.results.insurance.failures.Add(ins[i]);
                                }
                            }
                        }

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


        #region  Workers

        private bool WorkAppointment(JToken data)
        {
            try
            {
                const string query = @"UPDATE Apoint 
                                          SET ap_status     = @ap_status
                                        WHERE Ap_num   = @Ap_num";
                var cmd = new SqlCommand(query, _dbMngr.Connection);
                cmd.Parameters.AddWithValue("@ap_status", data["ap_status"].ToString());
                cmd.Parameters.AddWithValue("@Ap_num", data["Ap_num"].ToString());
                _dbMngr.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;

            }

        }

        private bool WorkPaitnet(JToken data)
        {
            try
            {
                const string query = @"UPDATE DAT2000
                                     SET pt_last_name      = @pt_last_name,
                                         pt_first_name     = @pt_first_name,
                                         pt_init_name      = @pt_init_name,
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
                var cmd = new SqlCommand(query, _dbMngr.Connection);
                cmd.Parameters.AddWithValue("@pt_last_name", data["pt_last_name"].ToString());
                cmd.Parameters.AddWithValue("@pt_first_name", data["pt_first_name"].ToString());
                cmd.Parameters.AddWithValue("@pt_init_name", data["pt_init_name"].ToString());
                cmd.Parameters.AddWithValue("@pt_sex", data["pt_sex"].ToString());
                cmd.Parameters.AddWithValue("@pt_civil_status", data["pt_civil_status"].ToString());
                cmd.Parameters.AddWithValue("@pt_birth_date", data["pt_birth_date"].ToString());
                cmd.Parameters.AddWithValue("@pt_p_address_1", data["pt_p_address_1"].ToString());
                cmd.Parameters.AddWithValue("@pt_p_address_2", data["pt_p_address_2"].ToString());
                cmd.Parameters.AddWithValue("@pt_p_city", data["pt_p_city"].ToString());
                cmd.Parameters.AddWithValue("@pt_p_state", data["pt_p_state"].ToString());
                cmd.Parameters.AddWithValue("@pt_p_zip", data["pt_p_zip"].ToString());
                cmd.Parameters.AddWithValue("@pt_r_address_1", data["pt_r_address_1"].ToString());
                cmd.Parameters.AddWithValue("@pt_r_address_2", data["pt_r_address_2"].ToString());
                cmd.Parameters.AddWithValue("@pt_r_city", data["pt_r_city"].ToString());
                cmd.Parameters.AddWithValue("@pt_r_state", data["pt_r_state"].ToString());
                cmd.Parameters.AddWithValue("@pt_r_zip", data["pt_r_zip"].ToString());
                cmd.Parameters.AddWithValue("@pt_home_phone", data["pt_home_phone"].ToString());
                cmd.Parameters.AddWithValue("@pt_work_phone", data["pt_work_phone"].ToString());
                cmd.Parameters.AddWithValue("@pt_religion", data["pt_religion"].ToString());
                cmd.Parameters.AddWithValue("@pt_email", data["pt_email"].ToString());
                cmd.Parameters.AddWithValue("@pt_cel_phone", data["pt_cel_phone"].ToString());
                cmd.Parameters.AddWithValue("@pt_language", data["pt_language"].ToString());
                cmd.Parameters.AddWithValue("@pt_race", data["pt_race"].ToString());
                cmd.Parameters.AddWithValue("@pt_rec_type", data["pt_rec_type"].ToString());
                cmd.Parameters.AddWithValue("@pt_rec_no", data["pt_rec_no"].ToString());
                cmd.Parameters.AddWithValue("@pt_rec_suffx", data["pt_rec_suffx"].ToString());
                _dbMngr.ExecuteNonQuery(cmd);

                // photo handler


                ImageBase64ToImageDirectoryByRecTypeRecNoRecSuffixAndCategory(
                    data["pt_photo_id"].ToString(),
                    data["pt_rec_type"].ToString(),
                    data["pt_rec_no"].ToString(),
                    data["pt_rec_suffx"].ToString(),
                    "pateint-id"
                    );
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }

        }

        private static void ImageBase64ToImageDirectoryByRecTypeRecNoRecSuffixAndCategory(string base64Img, string type, string no, string sufix, string category)
        {
            if (base64Img == "") return;
            try
            {
                var photo = Convert.FromBase64String(base64Img.Substring(base64Img.IndexOf(',') + 1));
                using (var stream = new MemoryStream(photo, 0, photo.Length))
                {
                    var img = Image.FromStream(stream);
                    var imgfrmt = img.RawFormat;
                    img.Save(
                        Server.TraDirectory + "\\Patients\\" + category + "-" + type + "-" + no + "-" + sufix + ".png", imgfrmt);
                    img.Dispose();
                }
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
            }
        }

        private dynamic GetInsuranceByRecTypeMumberSuffixAndOrden(string type, string no, string suffix, string orden)
        {
            const string query = @"SELECT * 
                                         FROM DAT8000 
                                        WHERE pi_pat_type = @pi_pat_type
                                          AND pi_pat_no   = @pi_pat_no
                                          AND pi_pat_sufx = @pi_pat_sufx
                                          AND pi_orden    = @pi_orden";
            var cmd = new SqlCommand(query, _dbMngr.Connection);
            cmd.Parameters.AddWithValue("@pi_pat_type", type);
            cmd.Parameters.AddWithValue("@pi_pat_no", no);
            cmd.Parameters.AddWithValue("@pi_pat_sufx", suffix);
            cmd.Parameters.AddWithValue("@pi_orden", orden);
            return _dbMngr.GetDataTableResults(cmd);
        }

        private dynamic WorkInsurence(JToken ins)
        {
            try
            {
                // check is insurance exist
                var table = GetInsuranceByRecTypeMumberSuffixAndOrden(
                    (string) ins["pi_pat_type"],
                    (string) ins["pi_pat_no"],
                    (string) ins["pi_pat_sufx"],
                    (string) ins["pi_orden"]
                    );

                string query;
                SqlCommand cmd;

                // if a new insurance
                if (table.Rows.Count == 0)
                {
                    // get next orden number
                    query = @"SELECT MAX(pi_orden)          AS next_orden, 
                                     MAX(pi_display_order)  AS pi_display_order 
                                FROM DAT8000 
                               WHERE pi_pat_type    = @pi_pat_type 
                                 AND pi_pat_no      = @pi_pat_no  
                                 AND pi_pat_sufx    = @pi_pat_sufx";
                    cmd = new SqlCommand(query, _dbMngr.Connection);
                    cmd.Parameters.AddWithValue("@pi_pat_type", ins["pi_pat_type"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_no", ins["pi_pat_no"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_sufx", ins["pi_pat_sufx"].ToString());
                    table = _dbMngr.GetDataTableResults(cmd);
                    int nextOrden;
                    int nextDisplay;
                    if (table.Rows.Count == 0)
                    {
                        nextOrden = 1;
                        nextDisplay = 1;
                    }
                    else
                    {
                        nextOrden = Convert.ToInt32(table.Rows[0]["next_orden"].ToString()) + 1;
                        nextDisplay = Convert.ToInt32(table.Rows[0]["pi_display_order"].ToString()) + 1;
                    }
                    // build INSERT SQL
                    query = @"INSERT INTO DAT8000
                                      ( pi_pat_type,
                                        pi_pat_no,
                                        pi_pat_sufx,
                                        pi_orden,
                                        pi_display_order,
                                        pi_last_name,
                                        pi_first_name,
                                        pi_init_name,
                                        pi_type,
                                        pi_ins_code,
                                        pi_group,
                                        pi_exp_date,
                                        pi_subscriber_last_name,
                                        pi_subscriber_first_name,
                                        pi_subscriber_init,
                                        pi_sex,
                                        pi_work_place,
                                        pi_birth_date,
                                        pi_address_1,
                                        pi_address_2,
                                        pi_city,
                                        pi_state,
                                        pi_zip,
                                        pi_relation,
                                        pi_id_subscriber )
                                VALUES( @pi_pat_type,
                                        @pi_pat_no,
                                        @pi_pat_sufx,
                                        @pi_orden,
                                        @pi_display_order,
                                        @pi_last_name,
                                        @pi_first_name,
                                        @pi_init_name,
                                        @pi_type,
                                        @pi_ins_code,
                                        @pi_group,
                                        @pi_exp_date,
                                        @pi_subscriber_last_name,
                                        @pi_subscriber_first_name,
                                        @pi_subscriber_init,
                                        @pi_sex,
                                        @pi_work_place,
                                        @pi_birth_date,
                                        @pi_address_1,
                                        @pi_address_2,
                                        @pi_city,
                                        @pi_state,
                                        @pi_zip,
                                        @pi_relation,
                                        @pi_id_subscriber );
                                 SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(query, _dbMngr.Connection);
                    cmd.Parameters.AddWithValue("@pi_pat_type", ins["pi_pat_type"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_no", ins["pi_pat_no"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_sufx", ins["pi_pat_sufx"].ToString());
                    cmd.Parameters.AddWithValue("@pi_orden", nextOrden);
                    cmd.Parameters.AddWithValue("@pi_display_order", nextDisplay);
                    cmd.Parameters.AddWithValue("@pi_last_name", ins["pi_last_name"].ToString());
                    cmd.Parameters.AddWithValue("@pi_first_name", ins["pi_first_name"].ToString());
                    cmd.Parameters.AddWithValue("@pi_init_name", ins["pi_init_name"].ToString());
                    cmd.Parameters.AddWithValue("@pi_type", "T"); // defautlt "T" insert value
                    cmd.Parameters.AddWithValue("@pi_ins_code", ins["pi_ins_code"].ToString());
                        // defautlt "999" insert value
                    cmd.Parameters.AddWithValue("@pi_group", ins["pi_group"].ToString());
                    cmd.Parameters.AddWithValue("@pi_exp_date", ins["pi_exp_date"].ToString());
                    cmd.Parameters.AddWithValue("@pi_subscriber_last_name", ins["pi_subscriber_last_name"].ToString());
                    cmd.Parameters.AddWithValue("@pi_subscriber_first_name", ins["pi_subscriber_first_name"].ToString());
                    cmd.Parameters.AddWithValue("@pi_subscriber_init", ins["pi_subscriber_init"].ToString());
                    cmd.Parameters.AddWithValue("@pi_sex", ins["pi_sex"].ToString());
                    cmd.Parameters.AddWithValue("@pi_work_place", ins["pi_work_place"].ToString());
                    cmd.Parameters.AddWithValue("@pi_birth_date", ins["pi_birth_date"].ToString());
                    cmd.Parameters.AddWithValue("@pi_address_1", ins["pi_address_1"].ToString());
                    cmd.Parameters.AddWithValue("@pi_address_2", ins["pi_address_2"].ToString());
                    cmd.Parameters.AddWithValue("@pi_city", ins["pi_city"].ToString());
                    cmd.Parameters.AddWithValue("@pi_state", ins["pi_state"].ToString());
                    cmd.Parameters.AddWithValue("@pi_zip", ins["pi_zip"].ToString());
                    cmd.Parameters.AddWithValue("@pi_relation", ins["pi_relation"].ToString());
                    cmd.Parameters.AddWithValue("@pi_id_subscriber", ins["pi_id_subscriber"].ToString());
                    _dbMngr.ExecuteNonQuery(cmd);

                    query = @"SELECT * 
                                FROM DAT8000 
                               WHERE pi_pat_type    = @pi_pat_type 
                                 AND pi_pat_no      = @pi_pat_no  
                                 AND pi_pat_sufx    = @pi_pat_sufx";
                    cmd = new SqlCommand(query, _dbMngr.Connection);
                    cmd.Parameters.AddWithValue("@pi_pat_type", ins["pi_pat_type"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_no", ins["pi_pat_no"].ToString());
                    cmd.Parameters.AddWithValue("@pi_pat_sufx", ins["pi_pat_sufx"].ToString());
                    _dbMngr.GetDataTableResults(cmd);

                    ImageBase64ToImageDirectoryByRecTypeRecNoRecSuffixAndCategory(
                        ins["pi_image"].ToString(),
                        ins["pi_pat_type"].ToString(),
                        ins["pi_pat_no"].ToString(),
                        ins["pi_pat_sufx"].ToString(),
                        "insurance-" + nextOrden.ToString(CultureInfo.InvariantCulture)
                        );

                    return GetInsuranceByRecTypeMumberSuffixAndOrden(
                        (string) ins["pi_pat_type"],
                        (string) ins["pi_pat_no"],
                        (string) ins["pi_pat_sufx"],
                        nextOrden.ToString(CultureInfo.InvariantCulture)
                        );
                }

                query = @"UPDATE DAT8000
                                     SET pi_exp_date                = @pi_exp_date,
                                         pi_work_place              = @pi_work_place,
                                         pi_address_1               = @pi_address_1,
                                         pi_address_2               = @pi_address_2,
                                         pi_city                    = @pi_city,
                                         pi_state                   = @pi_state,
                                         pi_zip                     = @pi_zip,
                                         pi_relation                = @pi_relation

                                   WHERE pi_pat_type                = @pi_pat_type
                                     AND pi_pat_no                  = @pi_pat_no
                                     AND pi_pat_sufx                = @pi_pat_sufx
                                     AND pi_orden                   = @pi_orden";
                cmd = new SqlCommand(query, _dbMngr.Connection);
                cmd.Parameters.AddWithValue("@pi_exp_date", ins["pi_exp_date"].ToString());
                cmd.Parameters.AddWithValue("@pi_work_place", ins["pi_work_place"].ToString());
                cmd.Parameters.AddWithValue("@pi_address_1", ins["pi_address_1"].ToString());
                cmd.Parameters.AddWithValue("@pi_address_2", ins["pi_address_2"].ToString());
                cmd.Parameters.AddWithValue("@pi_city", ins["pi_city"].ToString());
                cmd.Parameters.AddWithValue("@pi_state", ins["pi_state"].ToString());
                cmd.Parameters.AddWithValue("@pi_zip", ins["pi_zip"].ToString());
                cmd.Parameters.AddWithValue("@pi_relation", ins["pi_relation"].ToString());

                cmd.Parameters.AddWithValue("@pi_pat_type", ins["pi_pat_type"].ToString());
                cmd.Parameters.AddWithValue("@pi_pat_no", ins["pi_pat_no"].ToString());
                cmd.Parameters.AddWithValue("@pi_pat_sufx", ins["pi_pat_sufx"].ToString());
                cmd.Parameters.AddWithValue("@pi_orden", ins["pi_orden"].ToString());
                _dbMngr.ExecuteNonQuery(cmd);

                ImageBase64ToImageDirectoryByRecTypeRecNoRecSuffixAndCategory(
                    ins["pi_image"].ToString(),
                    ins["pi_pat_type"].ToString(),
                    ins["pi_pat_no"].ToString(),
                    ins["pi_pat_sufx"].ToString(),
                    "insurance-" + ins["pi_orden"]
                    );

                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }


        }

        #endregion
    }
}
