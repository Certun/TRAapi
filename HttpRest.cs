using RestSharp;


namespace TRAWebServer
{
    public class HttpRest
    {

        public static string SecretKey = string.Empty;
        public static string Host = string.Empty;


        readonly RestClient _client;
        RestRequest _request;

        public HttpRest(string url)
        {
            // TODO: Complete member initialization
            _client = new RestClient(url);
            SetSecretKey();
        }

        private void SetSecretKey()
        {
            //TODO get secretKey form App.config
            SecretKey = "carli";
        }


        public string Send(string action, dynamic data)
        {
            // convert object or int to string (JSON)
            data = data.ToString();

            if (!isJson(data))
            {
                // if data is not a json send a GET
                if (Server.Debug)
                {
                    Server.WriteDisplay("Method: GET");
                    Server.WriteDisplay("Action: " + action);
                    Server.WriteDisplay("Request: " + data);
                }
                _request = new RestRequest(data, Method.GET);
            }
            else
            {
                // if data is JOSN then send a POST
                if (Server.Debug)
                {
                    Server.WriteDisplay("Method: POST");
                    Server.WriteDisplay("Action: " + action);
                    Server.WriteDisplay("Request Data: " + data);
                }
                _request = new RestRequest("", Method.POST);
                _request.AddParameter("application/json", data.ToString(), ParameterType.RequestBody);
            }

            _request.AddHeader("Action", action);
            _request.AddHeader("Secret-Key", SecretKey);
            
            var response = (RestResponse) _client.Execute(_request);
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
