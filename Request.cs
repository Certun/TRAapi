using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TRAWebServer
{
    class Request
    {
        public string secretKey { get; set; }
        public string action { get; set; }
        public DataSet data { get; set; }
    }
}
