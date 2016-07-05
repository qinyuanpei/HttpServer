using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpServerLib;
using System.IO;

namespace HttpServer
{
    public class ExampleServer : HttpServerLib.HttpServer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public ExampleServer(string ipAddress, int port)
            : base(ipAddress, port)
        {

        }

        public override void OnPost(HttpRequest request)
        {
            //获取客户端传递的参数
            int num1 = int.Parse(request.Params["num1"]);
            int num2 = int.Parse(request.Params["num2"]);

            //设置返回信息
            string content = string.Format("这是通过Post方式返回的数据:num1={0},num2={1}", num1, num2);

            //构造响应报文
            HttpResponse response = new HttpResponse(content, Encoding.UTF8);
            response.StatusCode = "200";
            response.Content_Type = "text/html; charset=UTF-8";
            response.Server = "ExampleServer";

            //发送响应
            ProcessResponse(request.Handler, response);
        }

        public override void OnGet(HttpRequest request)
        {
            if(request.Method == "GET")
            {
                ///链接形式1:"http://localhost:4050/assets/styles/style.css"表示访问指定文件资源，
                ///此时读取服务器目录下的/assets/styles/style.css文件。

                ///链接形式1:"http://localhost:4050/assets/styles/"表示访问指定页面资源，
                ///此时读取服务器目录下的/assets/styles/style.index文件。

                //当文件不存在时应返回404状态码
                string requestURL = request.URL;
                requestURL = requestURL.Replace("/", @"\").Replace("\\..", "");

                //判断地址中是否存在扩展名
                string extension = Path.GetExtension(requestURL);

                //根据有无扩展名按照两种不同链接进行处
                string requestFile = string.Empty;
                if(extension != ""){
                    requestFile = ServerRoot + requestURL;
                }else{
                    requestFile = ServerRoot + requestURL + "index.html";
                }

                //构造HTTP响应
                HttpResponse response = ResponseWithFile(requestFile);

                //发送响应
                ProcessResponse(request.Handler, response);
            }
        }

        public override void OnListFiles()
        {

        }

        /// <summary>
        /// 使用文件来提供HTTP响应
        /// </summary>
        /// <param name="fileName">文件名</param>
        private HttpResponse ResponseWithFile(string fileName)
        {
            //准备HTTP响应报文
            HttpResponse response;

            //获取文件扩展名以判断内容类型
            string extension  = Path.GetExtension(fileName);

            //获取当前内容类型
            string contentType = GetContentType(extension);

            //如果文件不存在则返回404否则读取文件内容
            if(!File.Exists(fileName)){
                response = new HttpResponse("<html><body><h1>404 - Not Found</h1></body></html>", Encoding.UTF8);
                response.StatusCode = "404";
                response.Content_Type = "text/html";
                response.Server = "ExampleServer";
            }else{
                response = new HttpResponse(File.ReadAllBytes(fileName), Encoding.UTF8);
                response.StatusCode = "200";
                response.Content_Type = contentType;
                response.Server = "ExampleServer";
            }

            //返回数据
            return response;
        }
    }
}
