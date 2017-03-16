using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public class HttpService : HttpServer
    {
        public HttpService(string ipAddress, int port)
            : base(ipAddress, port)
        {

        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {
            base.OnDefault(request, response);
        }

        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            base.OnGet(request, response);
        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            base.OnPost(request, response);
        }
    }
}
