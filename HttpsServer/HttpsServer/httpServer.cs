using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HttpsServer
{
    public class httpServer
    {
        public const String Msg_Dir = "/root/msg/";
        public const String Web_Dir = "/root/web/";
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "HTTPcity HTTP server v0.1";

        private bool running = false;

        private TcpListener listener;

        public httpServer(int port) 
        {

            listener = new TcpListener(IPAddress.Any, port);  

        }

        public void Start()
        {
            Thread serverThread = new Thread( new ThreadStart(Run));
            serverThread.Start(); 
        }

        private void Run()
        {
            running = true;
            listener.Start();
            while (running)
            {
                Console.WriteLine("waiting for connection .... ");

                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client Connected!");

                HandleClient(client);

                client.Close();

            }

            running = false;

            listener.Stop();
        }
        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            String msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
            }

            Console.WriteLine("Request: \n" + msg);

            Request req = Request.GetRequest(msg);
            Response resp = Response.From(req);
            resp.Post(client.GetStream());

        } 

    }
}
