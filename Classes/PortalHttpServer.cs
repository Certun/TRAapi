using System.Globalization;
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
        private readonly string[] _getActions = {"*"};
        // allow set action array
        private readonly string[] _setActions =
        {
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
                         response.results = new {};
                        // define success
                        response.success = true;
                        // define error if any
                        response.error = "";
                        
                        break;

                    case "getBooks":
                        // DO STUFF HERE!
                        response.results = new { };
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
            if(Server.Debug) Server.WriteDisplay("Request Recived");
            var rawData = inputData.ReadToEnd();
            // write request success headers (THis is required for every reuqest)
            p.WriteSuccess();
            // create  new response object
            var response = new Response {success = true};
            // verify secret key
            if ((string) p.HttpHeaders["Secret-Key"] != Server.SecretKey)
            {
                // handle wrong secretkey error
                response.success = false;
                response.error = "Access denied";
                if (Server.Debug) Server.WriteDisplay(response.error);
            }
            else if (String.IsNullOrEmpty((string) p.HttpHeaders["Action"]))
            {
                // handle no action error
                response.success = false;
                response.error = "No Action Provided";
                if (Server.Debug) Server.WriteDisplay(response.error);
            }
            else if (!_setActions.Contains((string) p.HttpHeaders["Action"]))
            {
                // handle action definition error
                response.success = false;
                response.error = "Action Not Defined As a Set Actions";
                if (Server.Debug) Server.WriteDisplay(response.error);
            }
            else
            {
                // start valid reqiest --------------------------------------------------------------->>>>
                // store header action for easiest access
                var action = (string) p.HttpHeaders["Action"];
                dynamic data;
                // parse the data into an object
                if (Server.Debug) Server.WriteDisplay("UrlDecode, Decoding Data");
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

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Appointment(s)", app.Count()));
                            
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

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Patient(s)", pat.Count()));

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

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Insurance(s)", ins.Count()));

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

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Document(s)", doc.Count()));

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

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Signatures(s)", sig.Count()));

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

                        // clinical are handle very similar to documents
                        var cli = (JArray)data["clinical"];
                        if (cli.Any())
                        {
                            response.results.clinical = new JObject();
                            response.results.clinical.successes = new JArray();
                            response.results.clinical.failures = new JArray();

                            if (Server.Debug) Server.WriteDisplay(String.Format("Working {0} Clinical(s)", cli.Count()));

                            for (var i = 0; i < cli.Count(); i++)
                            {
                                if (WorkClinical(cli[i]))
                                {
                                    cli[i]["pdf"] = null; // do not return the pdf back
                                    response.results.clinical.successes.Add(cli[i]);
                                }
                                else
                                {
                                    cli[i]["pdf"] = null; // do not return the pdf back
                                    response.results.clinical.failures.Add(cli[i]);
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
                if (Server.Debug) Server.WriteDisplay("Raw String Data: " + Uri.UnescapeDataString(rawData));
                

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
                string status;
                switch (data["apstatus"].ToString())
                {
                    case  "0":
                        status = "Procesando";
                        break;
                    case  "1":
                        status = "Pendiente";
                        break;
                    case  "2":
                        status = "Completado";
                        break;
                    case  "3":
                        status = "Confirmado";
                        break;
                    case  "4":
                        status = "Cancelado";
                        break;
                    case  "8":
                        status = "Confirmacion";
                        break;
                    case  "9":
                        status = "Error";
                        break;  
                    default:
                        status = "N/A";
                        break;
                }

                app.apstatus = status;
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
                var dat2000 = _conn.DAT2000.SingleOrDefault(p =>
                    p.ptrectype == ToChar(data["ptrectype"].ToString()) &&
                    p.ptrecno == data["ptrecno"].ToString() &&
                    p.ptrecsuffx == data["ptrecsuffx"].ToString()
                    );

                if (dat2000 == null) return true;

                dat2000.ptlastname = data["ptlastname"].ToString().ToUpper();
                dat2000.ptfirstname = data["ptfirstname"].ToString().ToUpper();
                dat2000.ptinitname = data["ptinitname"].ToString().ToUpper();
                dat2000.ptsex = ToChar(data["ptsex"].ToString().ToUpper());
                dat2000.ptcivilstatus = ToChar(data["ptcivilstatus"].ToString().ToUpper());

                dat2000.ptspouselastname = data["ptspouselastname"].ToString().ToUpper();
                dat2000.ptspousefirstname = data["ptspousefirstname"].ToString().ToUpper();
                dat2000.ptspouseinit = ToChar(data["ptspouseinit"].ToString().ToUpper());

                dat2000.ptmotherlastname = data["ptmotherlastname"].ToString().ToUpper();
                dat2000.ptmotherfirstname = data["ptmotherfirstname"].ToString().ToUpper();
                dat2000.ptmotherinit = ToChar(data["ptmotherinit"].ToString().ToUpper());

                dat2000.ptfatherlastname = data["ptfatherlastname"].ToString().ToUpper();
                dat2000.ptfatherfirstname = data["ptfatherfirstname"].ToString().ToUpper();
                dat2000.ptfatherinit = ToChar(data["ptfatherinit"].ToString().ToUpper());

                dat2000.ptbirthdate = Convert.ToDateTime(data["ptbirthdate"].ToString());
                dat2000.ptpaddress1 = data["ptpaddress1"].ToString().ToUpper();
                dat2000.ptpaddress2 = data["ptpaddress2"].ToString().ToUpper();
                dat2000.ptpcity = data["ptpcity"].ToString().ToUpper();
                dat2000.ptpstate = data["ptpstate"].ToString().ToUpper();
                dat2000.ptpzip = data["ptpzip"].ToString().ToUpper();
                dat2000.ptraddress1 = data["ptraddress1"].ToString().ToUpper();
                dat2000.ptraddress2 = data["ptraddress2"].ToString().ToUpper();
                dat2000.ptrcity = data["ptrcity"].ToString().ToUpper();
                dat2000.ptrstate = data["ptrstate"].ToString().ToUpper();
                dat2000.ptrzip = data["ptrzip"].ToString().ToUpper();

                dat2000.pthomephone = data["pthomephone"].ToString().ToUpper();
                dat2000.ptworkphone = data["ptworkphone"].ToString().ToUpper();
                dat2000.ptworkext = data["ptworkext"].ToString().ToUpper();

                dat2000.ptworkplace = data["ptworkplace"].ToString().ToUpper();
                dat2000.ptworktitle = data["ptworktitle"].ToString().ToUpper();

                dat2000.ptresplastname = data["ptresplastname"].ToString().ToUpper();
                dat2000.ptrespfirstname = data["ptrespfirstname"].ToString().ToUpper();
                dat2000.ptrespinit = ToChar(data["ptrespinit"].ToString().ToUpper());

                dat2000.ptresponsiblerelation = data["ptresponsiblerelation"].ToString().ToUpper();
                dat2000.ptrespphone = data["ptrespphone"].ToString().ToUpper();
                dat2000.ptrefering = data["ptrefering"].ToString().ToUpper();

                dat2000.ptethnic = Convert.ToInt32(data["ptethnic"].ToString());
                dat2000.ptbirthplace = data["ptbirthplace"].ToString().ToUpper();

                dat2000.ptautlast1 = data["ptautlast1"].ToString().ToUpper();
                dat2000.ptautfirst1 = data["ptautfirst1"].ToString().ToUpper();
                dat2000.ptautinit1 = ToChar(data["ptautinit1"].ToString().ToUpper());
                dat2000.ptautlast2 = data["ptautlast2"].ToString().ToUpper();
                dat2000.ptautfirst2 = data["ptautfirst2"].ToString().ToUpper();
                dat2000.ptautinit2 = ToChar(data["ptautinit2"].ToString().ToUpper());

                dat2000.ptautdocmsg = ToChar(data["ptautdocmsg"].ToString().ToUpper());
                dat2000.ptautpatmsg = ToChar(data["ptautpatmsg"].ToString().ToUpper());

                dat2000.ptemail = data["ptemail"].ToString().ToUpper();
                dat2000.ptcelphone = data["ptcelphone"].ToString().ToUpper();
                dat2000.ptlanguage = Convert.ToInt32(data["ptlanguage"].ToString());
                dat2000.ptrace = Convert.ToInt32(data["ptrace"].ToString());

                dat2000.ptrespfirstname = data["ptrespfirstname"].ToString();
                _conn.SaveChanges();

                // photo handler
                var newImg = data["newImg"].ToString();
                if (newImg != "" && newImg != "0")
                {
                    SaveDocument(
                        data["ptphotoid"].ToString(),
                        "pateint-photo.jpg",
                        Server.PatientImgCategory,
                        ToChar(data["ptrectype"].ToString()),
                        data["ptrecno"].ToString(),
                        data["ptrecsuffx"].ToString()
                        );
                }
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
                string newImg;
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
                            pipattype = ToChar(ins["pipattype"].ToString().ToUpper()),
                            pipatno = ins["pipatno"].ToString().ToUpper(),
                            pipatsufx = ins["pipatsufx"].ToString().ToUpper(),
                            piorden = Convert.ToByte(nextOrden),
                            pidisplayorder = Convert.ToByte(nextDisplay),
                            pilastname = ins["pilastname"].ToString().ToUpper(),
                            pifirstname = ins["pifirstname"].ToString().ToUpper(),
                            piinitname = ins["piinitname"].ToString().ToUpper(),
                            pitype = ToChar(ins["pitype"].ToString().ToUpper()),
                            piinscode = ins["piinscode"].ToString().ToUpper(),
                            pigroup = ins["pigroup"].ToString().ToUpper(),
                            pisubscriberlastname = ins["pisubscriberlastname"].ToString().ToUpper(),
                            pisubscriberfirstname = ins["pisubscriberfirstname"].ToString().ToUpper(),
                            pisubscriberinit = ToChar(ins["pisubscriberinit"].ToString().ToUpper()),
                            pisex = ToChar(ins["pisex"].ToString().ToUpper()),
                            piworkplace = ins["piworkplace"].ToString(),
                            piaddress1 = ins["piaddress1"].ToString().ToUpper(),
                            piaddress2 = ins["piaddress2"].ToString().ToUpper(),
                            picity = ins["picity"].ToString().ToUpper(),
                            pistate = ins["pistate"].ToString().ToUpper(),
                            pizip = ins["pizip"].ToString().ToUpper(),
                            pirelation = ToChar(ins["pirelation"].ToString().ToUpper()),
                            piidsubscriber = ins["piidsubscriber"].ToString().ToUpper()
                        };

                    if (ins["piexpdate"].ToString() != "")
                    {
                        d8000.piexpdate = Convert.ToDateTime(ins["piexpdate"].ToString());
                    }

                    if (ins["pibirthdate"].ToString() != "")
                    {
                        d8000.pibirthdate = Convert.ToDateTime(ins["pibirthdate"].ToString());
                    }

                    _conn.Add(d8000);
                    _conn.SaveChanges();

                    newImg = ins["newImg"].ToString();
                    if (newImg != "" && newImg != "0")
                    {
                        SaveDocument(
                            ins["piimage"].ToString(),
                            "insurance-" + nextOrden.ToString(CultureInfo.InvariantCulture) + ".jpg",
                            Server.InsuranceImgCategory,
                            ToChar(ins["pipattype"].ToString()),
                            ins["pipatno"].ToString(),
                            ins["pipatsufx"].ToString()
                            );
                    }
                    return true;
                }

                // update insurance...
                if (ins["piexpdate"].ToString() != "")
                {
                    d8000.piexpdate = Convert.ToDateTime(ins["piexpdate"].ToString());
                }

                d8000.piworkplace = ins["piworkplace"].ToString().ToUpper();
                d8000.piaddress1 = ins["piaddress1"].ToString().ToUpper();
                d8000.piaddress2 = ins["piaddress2"].ToString().ToUpper();
                d8000.picity = ins["picity"].ToString().ToUpper();
                d8000.pistate = ins["pistate"].ToString().ToUpper();
                d8000.pizip = ins["pizip"].ToString().ToUpper();
                d8000.pirelation = ToChar(ins["pirelation"].ToString().ToUpper());
                _conn.SaveChanges();

                newImg = ins["newImg"].ToString();
                if (newImg != "" && newImg != "0")
                {
                    SaveDocument(
                        ins["piimage"].ToString(),
                        "insurance-" + ins["piorden"] + ".jpg",
                        Server.InsuranceImgCategory,
                        ToChar(ins["pipattype"].ToString()),
                        ins["pipatno"].ToString(),
                        ins["pipatsufx"].ToString()
                        );
                }
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
                data["letterType"] + ".pdf",  // ad .pdf to the letter litle
                Server.DocumentsCategory,
                ToChar(data["recnumtype"].ToString()),
                data["recnumno"].ToString(),
                data["recnumsuffx"].ToString()
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool WorkClinical(JToken data)
        {
            return SaveDocument(
                data["pdf"].ToString(),
                "clinicaldata" + ".pdf",  // ad .pdf to the letter litle
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
                var ext = name.Substring(name.LastIndexOf('.'));
                var path = GetDocPathByCategory(cat);
                var bytes = Convert.FromBase64String(document.Substring(document.IndexOf(',') + 1));
                var newName = type + no + suffix + DateTime.Now.Ticks + ext;
                var stream = new FileStream(String.Format("\\\\" + Server.DocServer + "\\{0}\\{1}", path, newName), FileMode.CreateNew);
                var writer = new BinaryWriter(stream);
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();
                return InsertDocumentDataToSql(type, no, suffix, cat, newName, path);
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
