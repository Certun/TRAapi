﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            "getInsuranceData",
            "getBooks"
        };

        // allow set action array
        private string[] setActions = new string[] {
            "setPatientData",
            "setInsuranceData",
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
            if ((string)p.httpHeaders["Secret-Key"] != "carli")
            {   
                // handle wrong secretkey error
                response.error = "Access denied";
            }else if(String.IsNullOrEmpty((string)p.httpHeaders["Action"])){
                // handle no action error
                response.error = "No action provided";   
            }else if(!getActions.Contains((string)p.httpHeaders["Action"])){
                // handle action definition error
                response.error = "Action not defined as a get actions"; 
            }else{
                // start valid reqiest --------------------------------------------------------------->>>>
                
                // store header action for easiest access
                string action = (string)p.httpHeaders["Action"];
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
                if (Server.debug)
                {
                    Server.WriteDisplay("secretKey: " + p.httpHeaders["Secret-Key"]);
                    Server.WriteDisplay("action: " + p.httpHeaders["Action"]);
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
            if ((string)p.httpHeaders["Secret-Key"] != "carli")
            {
                // handle wrong secretkey error
                response.error = "Access denied";
            }
            else if (String.IsNullOrEmpty((string) p.httpHeaders["Action"]))
            {
                // handle no action error
                response.error = "No action provided";
            }
            else if (!setActions.Contains((string) p.httpHeaders["Action"]))
            {
                // handle action definition error
                response.error = "Action not defined as a set actions";
            }
            else
            {
                // start valid reqiest --------------------------------------------------------------->>>>

                // store header action for easiest access
                var action = (string) p.httpHeaders["Action"];


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
                if (Server.debug)
                {
                    Server.WriteDisplay("secretKey: " + p.httpHeaders["Secret-Key"]);
                    Server.WriteDisplay("action: " + p.httpHeaders["Action"]);
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
