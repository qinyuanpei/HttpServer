using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPServerLib;
using HttpServer;

namespace HTTPServerLib
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleServer server = new ExampleServer("0.0.0.0", 4050);
            server.SetRoot(@"D:\Hexo\public");
            server.Logger = new ConsoleLogger();
            server.Start();
        }
    }
}
