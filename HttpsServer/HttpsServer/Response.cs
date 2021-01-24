using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HttpsServer
{
    public class Response
    {
        private Byte[] data = null;
        private String status;
        private String mime;
        private Response(String mime, String status, Byte[] data)
        {
            this.mime = mime;
            this.status = status;
            this.data = data;
        }

        public static Response From(Request request)
        {
            if (request == null)
                return MakeNullRequest();

            if (request.Type == "GET")
            {
                String file = Environment.CurrentDirectory + httpServer.Web_Dir + request.URL;
                FileInfo f = new FileInfo(file);
                if (f.Exists && f.Extension.Contains("."))
                { return MakeFromFile(f); }
                else 
                {
                    DirectoryInfo di = new DirectoryInfo(f + "/");

                    if (!di.Exists)
                    { return MakePageNotFound(); }
                    
                    FileInfo[] files = di.GetFiles();

                    foreach (FileInfo ff in files)
                    {
                        String n = ff.Name;
                        if (n.Contains("default.html") || n.Contains("default.htm") || n.Contains("index.html") || n.Contains("index.htm"))
                        return MakeFromFile(ff);
                    }

                }
               
            }
            else
                return MakeMethodNotAllowed();


            return MakePageNotFound();
        
        }

        private static Response MakeFromFile(FileInfo f)
        {
           
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("200 OK", "text/html", d);
        }


        private static Response MakeNullRequest()
        {
            String file = Environment.CurrentDirectory + httpServer.Msg_Dir + "400.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("400 Bad Request", "text/html", d);
        }
        private static Response MakePageNotFound()
        {
            String file = Environment.CurrentDirectory + httpServer.Msg_Dir + "404.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("404 Page Not Found", "text/html", d);
        }

        private static Response MakeMethodNotAllowed()
        {
            String file = Environment.CurrentDirectory + httpServer.Msg_Dir + "405.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("405 Method Not Allowed", "text/html", d);
        }


        public void Post(NetworkStream stream)
        {
             
            StreamWriter writer = new StreamWriter(stream);
           
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
                httpServer.VERSION, status, httpServer.NAME, mime, data.Length));
            writer.Flush();
            stream.Write(data, 0, data.Length);
        }
    }
}
