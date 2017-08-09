using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:4050/");
            request.Method = "POST";
            var requestStream = request.GetRequestStream();
            var data = Encoding.UTF8.GetBytes("a=10&b=15");
            requestStream.Write(data, 0, data.Length);
            var response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var content = reader.ReadToEnd();
            }
        }
    }
}
