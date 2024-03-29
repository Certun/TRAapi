﻿using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebPortal.Classes
{
    public abstract class HttpServer
    {

        protected int Port;
        public TcpListener Listener;
        public bool IsActive = true;
        public Thread Thread;
        private HttpProcessor _processor;


        protected HttpServer(int port)
        {
            Port = port;
        }

        public void Listen()
        {
            try
            {
                var localIp = IPAddress.Parse(Server.ServerIp);
                Listener = new TcpListener(localIp, Port);
                Listener.Start();
            }
            catch (Exception e)
            {
                Server.WriteDisplay(e);
                Server.SetTextBtn(@"Start");
                Server.SetColorDisplay(Color.White);
                return;
            }

            while (IsActive)
            {
                try
                {
                    var s = Listener.AcceptTcpClient();
                    _processor = new HttpProcessor(s, this);
                    Thread = new Thread(_processor.Process);
                    Thread.Start();
                    Thread.Sleep(1);
                }
                catch (SocketException) // or whatever the exception is that you're getting
                {

                }
            }
        }

        public void Stop()
        {
            IsActive = false;
            Listener.Stop();
        }

        public abstract void HandleGetRequest(HttpProcessor p);
        public abstract void HandlePostRequest(HttpProcessor p, StreamReader inputData);

    }
}
