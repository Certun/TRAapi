using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TRAWebServer
{
    class Resposnse
    {
        public bool success { get; set; }
        public string error { get; set; }
        public DataSet data { get; set; }
    }
}
