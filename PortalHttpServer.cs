using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Data.SqlClient;

namespace TRAWebServer
{

    public class PortalHttpServer : HttpServer
    {

        public PortalHttpServer(int port)
            : base(port)
        {

        }

        // allow get actions array
        private readonly string[] _getActions = new[] {
            "getPatientData",
            "getInsuranceData",
            "getBooks"
        };

        // allow set action array
        private readonly string[] _setActions = new[] {
            "setPatientData",
            "setInsuranceData",
            "setAppointmentStatus"
        };

        public override void handleGETRequest(HttpProcessor p)
        {           
            // write request success headers (THis is required for every reuqest)
            p.WriteSuccess();

            // create  new response object
            var response = new Response();

            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if ((string)p.HttpHeaders["Secret-Key"] != "carli")
            {   
                // handle wrong secretkey error
                response.error = "Access denied";
            }else if(String.IsNullOrEmpty((string)p.HttpHeaders["Action"])){
                // handle no action error
                response.error = "No action provided";   
            }else if(!_getActions.Contains((string)p.HttpHeaders["Action"])){
                // handle action definition error
                response.error = "Action not defined as a get actions"; 
            }else{
                // start valid reqiest --------------------------------------------------------------->>>>
                // Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
                var dbMngr = new DbManager("Data Source=ernestoTHINK\\TRA; Initial Catalog=dbProjectCita; Trusted_Connection=True");
                var data = new DataSet();
                // store header action for easiest access
                var action = (string)p.HttpHeaders["Action"];
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
                        DataTable results = dbMngr.GetDataTableResults(cmd);

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
            string jresponse = JsonConvert.SerializeObject(response);
            // send back response as json
            p.OutputStream.WriteLine(jresponse);
        }


        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            string rawData = inputData.ReadToEnd();
            // write request success headers (THis is required for every reuqest)
            p.WriteSuccess();
            // create  new response object
            var response = new Response();
            
            
            // EDGARDO!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if ((string)p.HttpHeaders["Secret-Key"] != "carli")
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


                // parse the data into an object
                // var data = JObject.Parse(Uri.UnescapeDataString(rawData)) as JObject;

                // do stuuf with the object data
                // Server.WriteDisplay(data.ToString());



                switch (action)
                {
                    case "setPatientData":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    case "setInsuranceData":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    case "setAppointmentStatus":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";
                        
                        
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

            // JOSN convert
            string jresponse = JsonConvert.SerializeObject(response);
            // Send back response
            p.OutputStream.WriteLine(jresponse);
        }
    }
}
