using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpServerLib;
using HttpServer;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleServer server = new ExampleServer("0.0.0.0",4050);
            server.SetServerRoot("./");
            server.Start();
        }
    }
}
