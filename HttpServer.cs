using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TRAWebServer
{
    public abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;


        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            System.Net.IPAddress localIP = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(localIP, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
}
