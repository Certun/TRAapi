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
        public TcpListener listener;
        public bool is_active = true;
        public Thread thread;
        private HttpProcessor processor;


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
                try
                {
                    TcpClient s = listener.AcceptTcpClient();
                    processor = new HttpProcessor(s, this);
                    thread = new Thread(new ThreadStart(processor.process));
                    thread.Start();
                    Thread.Sleep(1);
                }
                catch (SocketException) // or whatever the exception is that you're getting
                {

                }

            }
        }

        public void stop()
        {
            is_active = false;
            listener.Stop();
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);

    }
}
