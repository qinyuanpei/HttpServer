using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public class HttpService : HttpServer
    {
        private List<ServiceModule> m_modules;

        public HttpService(string ipAddress, int port)
            : base(ipAddress, port)
        {
            m_modules = new List<ServiceModule>();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="module">ServiceModule</param>
        public void RegisterModule(ServiceModule module)
        {
            this.m_modules.Add(module);
        }

        /// <summary>
        /// 卸载模块
        /// </summary>
        /// <param name="module"></param>
        public void RemoveModule(ServiceModule module)
        {
            this.m_modules.Remove(module);
        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {
            base.OnDefault(request, response);
        }

        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            ServiceRoute route = ServiceRoute.Parse(request);
            ServiceModule module = m_modules.FirstOrDefault(m => m.SearchRoute(route));
            if (module != null){
                var result = module.ExecuteRoute(route);
            }
        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            base.OnPost(request, response);
        }
    }
}
