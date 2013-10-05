using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp.Contrib;

namespace WebPortal.Classes
{

    public class PortalHttpServer : HttpServer
    {
        private readonly EntitiesModel _conn;

        public PortalHttpServer(int port) : base(port)
        {
            _conn = new EntitiesModel();
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
                // store header action for easiest access
                var action = (string) p.HttpHeaders["Action"];
                // request if a single data for example and ID of a patient or an appoointment book
                var request = p.HttpUrl.Substring(1);

                // DO STUFF HERE!
                switch (action)
                {

                    case "getPatientData":
                        // DO STUFF HERE!

                        response.results = new {};

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="inputData"></param>
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
                        /////////////////////////////////////////
                        // Here is where the request is handle //
                        /////////////////////////////////////////
                        response.results = new JObject();
                        // appointments
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
                        // patients demographics
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
                        // insurance
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
                                    ins[i]["piorden"] = isGood.Rows[0]["piorden"].ToString();
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
                        // documents
                        var doc = (JArray)data["documents"];
                        if (doc.Any())
                        {
                            response.results.documents = new JObject();
                            response.results.documents.successes = new JArray();
                            response.results.documents.failures = new JArray();

                            for (var i = 0; i < doc.Count(); i++)
                            {
                                if (WorkDocuments(doc[i]))
                                {
                                    doc[i]["document"] = null; // do not return the document back
                                    response.results.documents.successes.Add(doc[i]);
                                }
                                else
                                {
                                    doc[i]["document"] = null; // do not return the document back
                                    response.results.documents.failures.Add(doc[i]);
                                }
                            }
                        }
                        // signatures are handle very similar to documents
                        var sig = (JArray)data["signatures"];
                        if (sig.Any())
                        {
                            response.results.signatures = new JObject();
                            response.results.signatures.successes = new JArray();
                            response.results.signatures.failures = new JArray();

                            for (var i = 0; i < sig.Count(); i++)
                            {
                                if (WorkSignatures(sig[i]))
                                {
                                    sig[i]["pdf"] = null; // do not return the pdf back
                                    response.results.signatures.successes.Add(sig[i]);
                                }
                                else
                                {
                                    sig[i]["pdf"] = null; // do not return the pdf back
                                    response.results.signatures.failures.Add(sig[i]);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool WorkAppointment(JToken data)
        {
            try
            {
                var app = _conn.Apoints.Single(a => a.apnum == data["apnum"].ToString());
                app.apstatus = data["apstatus"].ToString();
                _conn.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool WorkPaitnet(JToken data)
        {
            try
            {
                var dat2000 = _conn.DAT2000.Single(p =>
                    p.ptrectype == ToChar(data["ptrectype"].ToString()) &&
                    p.ptrecno == data["ptrecno"].ToString() &&
                    p.ptrecsuffx == data["ptrecsuffx"].ToString()
                    );

                dat2000.ptlastname = data["ptlastname"].ToString();
                dat2000.ptfirstname = data["ptfirstname"].ToString();
                dat2000.ptinitname = data["ptinitname"].ToString();
                dat2000.ptsex = ToChar(data["ptsex"].ToString());
                dat2000.ptcivilstatus = ToChar(data["ptcivilstatus"].ToString());
                dat2000.ptbirthdate = Convert.ToDateTime(data["ptbirthdate"].ToString());
                dat2000.ptpaddress1 = data["ptpaddress1"].ToString();
                dat2000.ptpaddress2 = data["ptpaddress2"].ToString();
                dat2000.ptpcity = data["ptpcity"].ToString();
                dat2000.ptpstate = data["ptpstate"].ToString();
                dat2000.ptpzip = data["ptpzip"].ToString();
                dat2000.ptraddress1 = data["ptraddress1"].ToString();
                dat2000.ptraddress2 = data["ptraddress2"].ToString();
                dat2000.ptrcity = data["ptrcity"].ToString();
                dat2000.ptrstate = data["ptrstate"].ToString();
                dat2000.ptrzip = data["ptrzip"].ToString();
                dat2000.pthomephone = data["pthomephone"].ToString();
                dat2000.ptworkphone = data["ptworkphone"].ToString();
                dat2000.ptemail = data["ptemail"].ToString();
                dat2000.ptcelphone = data["ptcelphone"].ToString();
                dat2000.ptlanguage = ToChar(data["ptlanguage"].ToString());
                dat2000.ptrace = ToChar(data["ptrace"].ToString());
                _conn.SaveChanges();

                // photo handler
                SaveDocument(
                    data["ptphotoid"].ToString(),
                    "pateint-photo",
                    Server.PatientImgCategory,
                    ToChar(data["ptrectype"].ToString()),
                    data["ptrecno"].ToString(),
                    data["ptrecsuffx"].ToString()
                    );
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ins"></param>
        /// <returns></returns>
        private dynamic WorkInsurence(JToken ins)
        {
            try
            {
                var pipattype = ToChar(ins["pipattype"].ToString());
                var piorden = ToByte(ins["piorden"].ToString());
                // try to fin the insurance
                var d8000 = _conn.DAT8000.SingleOrDefault(d =>
                    d.pipattype == pipattype &&
                    d.pipatno == ins["pipatno"].ToString() &&
                    d.pipatsufx == ins["pipatsufx"].ToString() &&
                    d.piorden == piorden
                    );
                
                // if not found
                if (d8000 == null)
                {
                    // get next orden number
                    var maxO = (from d in _conn.DAT8000
                                where d.pipattype == ToChar(ins["pipattype"].ToString()) &&
                                     d.pipatno == ins["pipatno"].ToString() &&
                                     d.pipatsufx == ins["pipatsufx"].ToString()
                              select d.piorden).Max();

                    var maxD = (from d in _conn.DAT8000
                                where d.pipattype == ToChar(ins["pipattype"].ToString()) &&
                                     d.pipatno == ins["pipatno"].ToString() &&
                                     d.pipatsufx == ins["pipatsufx"].ToString()
                              select d.pidisplayorder).Max();

                    var nextOrden = Convert.ToInt32(maxO + 1); // maxO is never Null
                    var nextDisplay = maxD != null ? Convert.ToInt32(maxD + 1) : 1;

                    // Insert new Insurance to the database
                    d8000 = new DAT8000
                        {
                            pipattype = ToChar(ins["pipattype"].ToString()),
                            pipatno = ins["pipatno"].ToString(),
                            pipatsufx = ins["pipatsufx"].ToString(),
                            piorden = Convert.ToByte(nextOrden),
                            pidisplayorder = Convert.ToByte(nextDisplay),
                            pilastname = ins["pilastname"].ToString(),
                            pifirstname = ins["pifirstname"].ToString(),
                            piinitname = ins["piinitname"].ToString(),
                            pitype = ToChar(ins["pitype"].ToString()),
                            piinscode = ins["piinscode"].ToString(),
                            pigroup = ins["pigroup"].ToString(),
                            piexpdate = Convert.ToDateTime(ins["piexpdate"]),
                            pisubscriberlastname = ins["pisubscriberlastname"].ToString(),
                            pisubscriberfirstname = ins["pisubscriberfirstname"].ToString(),
                            pisubscriberinit = ToChar(ins["pisubscriberinit"].ToString()),
                            pisex = ToChar(ins["pisex"].ToString()),
                            piworkplace = ins["piworkplace"].ToString(),
                            pibirthdate = Convert.ToDateTime(ins["pibirthdate"].ToString()),
                            piaddress1 = ins["piaddress1"].ToString(),
                            piaddress2 = ins["piaddress2"].ToString(),
                            picity = ins["picity"].ToString(),
                            pistate = ins["pistate"].ToString(),
                            pizip = ins["pizip"].ToString(),
                            pirelation = ToChar(ins["pirelation"].ToString()),
                            piidsubscriber = ins["piidsubscriber"].ToString()
                        };
                    _conn.Add(d8000);
                    _conn.SaveChanges();


                    SaveDocument(
                        ins["piimage"].ToString(),
                        "insurance-" + nextOrden.ToString(CultureInfo.InvariantCulture),
                        Server.InsuranceImgCategory,
                        ToChar(ins["pipattype"].ToString()),
                        ins["pipatno"].ToString(),
                        ins["pipatsufx"].ToString()
                        );


                    return true;
                }

                // update insurance...
                d8000 = _conn.DAT8000.Single(d=>
                    d.pipattype == ToChar(ins["pipattype"].ToString()) &&
                    d.pipatno == ins["pipattype"].ToString() &&
                    d.pipatsufx == ins["pipattype"].ToString()
                    );

                d8000.piexpdate = Convert.ToDateTime(ins["piexpdate"].ToString());
                d8000.piworkplace = ins["piworkplace"].ToString();
                d8000.piaddress1 = ins["piaddress1"].ToString();
                d8000.piaddress2 = ins["piaddress2"].ToString();
                d8000.picity = ins["picity"].ToString();
                d8000.pistate = ins["pistate"].ToString();
                d8000.pizip = ins["pizip"].ToString();
                d8000.pirelation = ToChar(ins["pirelation"].ToString());
                _conn.SaveChanges();

                SaveDocument(
                    ins["piimage"].ToString(),
                    "insurance-" + ins["piorden"],
                    Server.InsuranceImgCategory,
                    ToChar(ins["pipattype"].ToString()),
                    ins["pipatno"].ToString(),
                    ins["pipatsufx"].ToString()
                );
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool WorkDocuments(JToken data)
        {
            return SaveDocument(
                data["document"].ToString(),
                data["documentName"].ToString(),
                Server.DocumentsCategory,
                ToChar(data["scanrectype"].ToString()),
                data["scanrecno"].ToString(),
                data["scanrecsufix"].ToString()
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool WorkSignatures(JToken data)
        {
            return SaveDocument(
                data["pdf"].ToString(),
                data["title"] + ".pdf",  // ad .pdf to the letter litle
                Server.DocumentsCategory,
                ToChar(data["recnumtype"].ToString()),
                data["recnumno"].ToString(),
                data["recnumsuffx"].ToString()
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        /// <param name="cat"></param>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        private bool SaveDocument(string document, string name, string cat, char type, string no, string suffix )
        {
            try
            {
                if (document == "") return false;
                var parsename = new StringBuilder(name);
                var path = GetDocPathByCategory(cat);
                var bytes = Convert.FromBase64String(document.Substring(document.IndexOf(',') + 1));
                var newName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + parsename.Replace(" ", "_");
                var stream = new FileStream(String.Format("\\\\" + Server.DocServer + "\\{0}\\{1}", path, newName), FileMode.CreateNew);
                var writer = new BinaryWriter(stream);
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();
                return InsertDocumentDataToSql(type, no, suffix, Server.DocumentsCategory, newName, path);
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private static string GetDocPathByCategory(string category)
        {
            var dt = DateTime.Now;
            var year = dt.ToString("yyyy");
            var month = dt.ToString("MM");
            var path = String.Format("{0}\\000\\{1}\\{2}\\{3}\\", Server.DocDirectory, year, year + "-" + month, category);
            Directory.CreateDirectory("\\\\" + Server.DocServer + "\\" + path);
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="no"></param>
        /// <param name="suffix"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool InsertDocumentDataToSql(char type, string no, string suffix, string category, string name, string path)
        {
            try
            {
                _conn.Add(new Scanned
                    {
                        scanrectype = type,
                        scanrecno = no,
                        scanrecsufx = suffix,
                        scangroupcode = category,
                        scanfilename = name,
                        scanpath = path,
                        scancreateddate = DateTime.Now,
                        scanupdateddate = DateTime.Now,
                        scanuserid = "web"
                    });
                _conn.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                return false;
            }
        }

        private static char ToChar(string str)
        {
            try
            {
                return Convert.ToChar(str);
            }
            catch (Exception)
            {
                return Convert.ToChar(" ");
            }
        }

        private static byte ToByte(string str)
        {
            try
            {
                return Convert.ToByte(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion
    }
}
