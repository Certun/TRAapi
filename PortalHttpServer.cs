using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace TRAWebServer
{
    public class PortalHttpServer : HttpServer
    {
        public PortalHttpServer(int port)
            : base(port)
        {

        }

        // allow get actions array
        private string[] getActions = new string[] {
            "getPatientData",
            "getIsuranceData",
            "getBooks"
        };

        // allow set action array
        private string[] setActions = new string[] {
            "setPatientData",
            "setIsuranceData",
            "setAppointmentStatus"
        };

        public override void handleGETRequest(HttpProcessor p)
        {           
            // write request success headers (THis is required for every reuqest)
            p.writeSuccess();

            // create  new response object
            Response response = new Response();

            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if((string) p.httpHeaders["secretKey"] != "carli"){   
                // handle wrong secretkey error
                response.error = "Acceess denied";
            }else if(String.IsNullOrEmpty((string)p.httpHeaders["action"])){
                // handle no action error
                response.error = "No action provided";   
            }else if(!getActions.Contains((string)p.httpHeaders["action"])){
                // handle action definition error
                response.error = "Action not defined as a get actions"; 
            }else{
                // start valid reqiest --------------------------------------------------------------->>>>
                
                // store header action for easiest access
                string action = (string)p.httpHeaders["action"];
                // request if a single data for example and ID of a patient or an appoointment book
                string request = p.http_url.Substring(1);
                
                // DO STUFF HERE!
                switch (action)
                {
                    case "getPatientData":
                        // DO STUFF HERE!


                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";


                        break;

                    case "getIsuranceData":
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
                if (Server.debug)
                {
                    Server.WriteDisplay("secretKey: " + p.httpHeaders["secretKey"]);
                    Server.WriteDisplay("action: " + p.httpHeaders["action"]);
                    Server.WriteDisplay("request: " + request);
                }

                // end valid reqiest <<<<---------------------------------------------------------------
            }

            // JOSN convert response
            string jresponse = JsonConvert.SerializeObject(response);
            // send back response as json
            p.outputStream.WriteLine(jresponse);
        }


        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            string rawData = inputData.ReadToEnd();
            // write request success headers (THis is required for every reuqest)
            p.writeSuccess();
            // create  new response object
            Response response = new Response();
            
            
            // EDGARDO!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // TODO get secretKey from App.config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if ((string)p.httpHeaders["secretKey"] != "carli")
            {
                // handle wrong secretkey error
                response.error = "Acceess denied";
            }
            else if (String.IsNullOrEmpty((string) p.httpHeaders["action"]))
            {
                // handle no action error
                response.error = "No action provided";
            }
            else if (!setActions.Contains((string) p.httpHeaders["action"]))
            {
                // handle action definition error
                response.error = "Action not defined as a set actions";
            }
            else
            {
                // start valid reqiest --------------------------------------------------------------->>>>

                // store header action for easiest access
                var action = (string) p.httpHeaders["action"];


                // parse the data into an object
                JObject data = JObject.Parse(Uri.UnescapeDataString(rawData)) as JObject;

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

                    case "setIsuranceData":
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
                if (Server.debug)
                {
                    Server.WriteDisplay("secretKey: " + p.httpHeaders["secretKey"]);
                    Server.WriteDisplay("action: " + p.httpHeaders["action"]);
                    Server.WriteDisplay("request: " + Uri.UnescapeDataString(rawData));
                }

                // end valid reqiest <<<<---------------------------------------------------------------
            }

            // JOSN convert
            string jresponse = JsonConvert.SerializeObject(response);
            // Send back response
            p.outputStream.WriteLine(jresponse);
        }
    }
}
