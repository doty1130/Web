using System;

namespace HttpsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server on port 8080");
            httpServer server = new httpServer(8080);
            server.Start();
        }
    }
}
