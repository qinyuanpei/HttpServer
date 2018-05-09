using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public interface IServer
    {
        /// <summary>
        /// 响应GET方法
        /// </summary>
        /// <param name="request">Http请求</param>
        void OnGet(HttpRequest request, HttpResponse response);


        /// <summary>
        /// 响应Post方法
        /// </summary>
        /// <param name="request">Http请求</param>
        void OnPost(HttpRequest request, HttpResponse response);


        /// <summary>
        /// 响应默认请求
        /// </summary>
        /// <param name="request">Http请求</param>
        void OnDefault(HttpRequest request, HttpResponse response);
    }
}
