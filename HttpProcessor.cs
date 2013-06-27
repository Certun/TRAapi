using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TRAWebServer
{
    public class HttpProcessor
    {
        public TcpClient TcpSocket;
        public HttpServer Srv;

        private Stream _inputStream;
        public StreamWriter OutputStream;

        public String HttpMethod;
        public String HttpUrl;
        public String HttpProtocolVersionstring;
        public Hashtable HttpHeaders = new Hashtable();

        private const int MaxPostSize = 10*1024*1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            TcpSocket = s;
            Srv = srv;
        }

        private static string StreamReadLine(Stream inputStream)
        {
            var data = "";
            while (true)
            {
                int nextChar = inputStream.ReadByte();
                if (nextChar == '\n') { break; }
                if (nextChar == '\r') { continue; }
                if (nextChar == -1) { Thread.Sleep(1); continue; }
                data += Convert.ToChar(nextChar);
            }
            return data;
        }


        public void Process()
        {

            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            _inputStream = new BufferedStream(TcpSocket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            OutputStream = new StreamWriter(new BufferedStream(TcpSocket.GetStream()));
            try
            {
                ParseRequest();

                if (Server.Debug) Server.WriteDisplay("************* Receiving " + HttpMethod + " Request *************");
                
                ReadHeaders();

                if (HttpMethod.Equals("GET"))
                {
                    HandleGetRequest();
                }
                else if (HttpMethod.Equals("POST"))
                {
                    HandlePostRequest();
                }
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                WriteFailure();
            }
            OutputStream.Flush();
            // bs.Flush(); // flush any remaining output
            _inputStream = null; OutputStream = null; // bs = null;            
            TcpSocket.Close();
        }

        public void ParseRequest()
        {
            var request = StreamReadLine(_inputStream);
            var tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            HttpMethod = tokens[0].ToUpper();
            HttpUrl = tokens[1];
            HttpProtocolVersionstring = tokens[2];

            Console.WriteLine(@"starting: " + request);
        }

        public void ReadHeaders()
        {
            Console.WriteLine(@"readHeaders()");
            String line;
            while ((line = StreamReadLine(_inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine(@"got headers");
                    return;
                }

                var separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                var name = line.Substring(0, separator);
                var pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                var value = line.Substring(pos, line.Length - pos);
                
                if (Server.Debug)
                {
                    Server.WriteDisplay("header: " + name + ":" + value);
                }
                
                HttpHeaders[name] = value;
            }
        }

        public void HandleGetRequest()
        {
            Srv.HandleGetRequest(this);
        }

        private const int BufSize = 4096;

        public void HandlePostRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            Console.WriteLine(@"get post data start");
            var ms = new MemoryStream();
            if (HttpHeaders.ContainsKey("Content-Length"))
            {
                int contentLen = Convert.ToInt32(HttpHeaders["Content-Length"]);
                if (contentLen > MaxPostSize)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          contentLen));
                }
                var buf = new byte[BufSize];
                var toRead = contentLen;
                while (toRead > 0)
                {
                    Console.WriteLine(@"starting Read, to_read={0}", toRead);

                    int numread = _inputStream.Read(buf, 0, Math.Min(BufSize, toRead));
                    Console.WriteLine(@"read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (toRead == 0)
                        {
                            break;
                        }
                        throw new Exception("client disconnected during post");
                    }
                    toRead -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine(@"get post data end");
            Srv.HandlePostRequest(this, new StreamReader(ms));

        }

        public void WriteSuccess(string contentType = "text/json")
        {
            OutputStream.WriteLine("HTTP/1.0 200 OK");
            OutputStream.WriteLine("Content-Type: " + contentType);
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
        }

        public void WriteFailure()
        {
            OutputStream.WriteLine("HTTP/1.0 404 File not found");
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
        }
    }
}
