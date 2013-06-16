using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRAWebServer
{
    public class HttpRest
    {

        public string secretKey;
        public string action;

        public Method post = Method.POST;
        public Method get = Method.GET;


        RestClient client;
        RestRequest request;

        public HttpRest(string url)
        {
            // TODO: Complete member initialization
            client = new RestClient(url);
            this.setSecretKey();
        }

        private void setSecretKey()
        {
            //TODO get secretKey form App.config
            this.secretKey = "carli";
        }


        public string Send(string action, dynamic data)
        {
            // convert object or int to string (JSON)
            data = data.ToString();

            if (!isJson(data))
            {
                // if data is not a json send a GET
                if (Server.debug)
                {
                    Server.WriteDisplay("Method: GET");
                    Server.WriteDisplay("Action: " + action);
                    Server.WriteDisplay("Request: " + data);
                }
                request = new RestRequest(data, Method.GET);
            }
            else
            {
                // if data is JOSN then send a POST
                if (Server.debug)
                {
                    Server.WriteDisplay("Method: POST");
                    Server.WriteDisplay("Action: " + action);
                    Server.WriteDisplay("Request Data: " + data);
                }
                request = new RestRequest("", Method.POST);
                request.AddParameter("application/json", data.ToString(), ParameterType.RequestBody);
            }

            request.AddHeader("Action", action);
            request.AddHeader("Secret-Key", secretKey);
            
            RestResponse response = (RestResponse) client.Execute(request);
            var content = response.Content; // raw content as string

            return content;
        }

        private bool isJson(string str)
        {
            str = str.Trim();
            return ((str.StartsWith("{") && str.EndsWith("}")) || (str.StartsWith("[") && str.EndsWith("]")));
        }


    }
}
