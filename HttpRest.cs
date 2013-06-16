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
        private string url;

        public HttpRest(string url)
        {
            // TODO: Complete member initialization
            this.url = url;
            this.setSecretKey();
        }

        private void buildHeaders()
        {
            request.AddHeader("action", action);
            request.AddHeader("secretKey", secretKey);
        }

        private void setSecretKey()
        {
            this.secretKey = "carli";
        }

        public void BuildRequest(string action, object data, Method method)
        {
            client = new RestClient(url);
            this.action = action;
            
            
            bool dataType = data.GetType() == typeof(Object);
            if (dataType)
            {

            }

            request = new RestRequest("", method);
            buildHeaders();
            
            


        }

        public string Send()
        {

            RestResponse response = (RestResponse) client.Execute(request);
            var content = response.Content; // raw content as string

            return content;
        }


    }
}
