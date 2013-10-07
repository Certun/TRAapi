using RestSharp;

namespace WebPortal.Classes
{
    public class HttpRest
    {

        public static string SecretKey = string.Empty;
        public static string Host = string.Empty;

        readonly RestClient _client;
        RestRequest _request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public HttpRest(string url)
        {
            // TODO: Complete member initialization
            _client = new RestClient(url);
            SetSecretKey();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetSecretKey()
        {
            //TODO get secretKey form App.config
            SecretKey = Server.SecretKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Send(string action, dynamic data)
        {
            // convert object or int to string (JSON)
            data = data.ToString();

            if (!IsJson(data))
            {
                // if data is not a json send a GET
//                if (Server.Debug)
//                {
//                    Server.WriteDisplay("Method: GET");
//                    Server.WriteDisplay("Action: " + action);
//                    Server.WriteDisplay("Request: " + data);
//                }
                _request = new RestRequest(data, Method.GET);
            }
            else
            {
                // if data is JOSN then send a POST
//                if (Server.Debug)
//                {
//                    Server.WriteDisplay("Method: POST");
//                    Server.WriteDisplay("Action: " + action);
//                    Server.WriteDisplay("Request Data: " + data);
//                }
                _request = new RestRequest("", Method.POST);
                _request.AddParameter("application/json", data.ToString(), ParameterType.RequestBody);
            }

            _request.AddHeader("Action", action);
            _request.AddHeader("Secret-Key", SecretKey);
            
            var response = (RestResponse) _client.Execute(_request);
            var content = response.Content; // raw content as string

            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsJson(string str)
        {
            str = str.Trim();
            return ((str.StartsWith("{") && str.EndsWith("}")) || (str.StartsWith("[") && str.EndsWith("]")));
        }


    }
}
