using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace HttpServerLib
{
    public class HttpServer:IServer
    {
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP { get; private set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; private set; }

        /// <summary>
        /// 服务器目录
        /// </summary>
        public string ServerRoot { get; private set; }

        /// <summary>
        /// 服务端Socet
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// 是否运行
        /// </summary>
        private bool isRunning = false;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        private HttpServer(IPAddress ipAddress, int port, string root)
        {
            this.ServerIP = ipAddress.ToString();
            this.ServerPort = port;

            //如果指定目录不存在则采用默认目录
            if(!Directory.Exists(root))
                this.ServerRoot = AppDomain.CurrentDomain.BaseDirectory;

            this.ServerRoot = root;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        public HttpServer(string ipAddress, int port, string root) :
            this(IPAddress.Parse(ipAddress), port, root) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public HttpServer(string ipAddress, int port) :
            this(IPAddress.Parse(ipAddress), port, AppDomain.CurrentDomain.BaseDirectory) { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        public HttpServer(int port, string root) :
            this(IPAddress.Loopback, port, root) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        public HttpServer(int port) :
            this(IPAddress.Loopback, port, AppDomain.CurrentDomain.BaseDirectory) { }

        #region 公开方法 

        /// <summary>
        /// 开启服务器
        /// </summary>
        public void Start()
        {
            if(isRunning)
                return;

            //创建服务端Socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort));
            serverSocket.Listen(10);
            isRunning = true;

            //输出服务器状态
            Console.WriteLine("Sever is running at http://{0}:{1}/.", ServerIP, ServerPort);

            //连接客户端
            while(isRunning)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread requestThread = new Thread(() =>{ ProcessRequest(clientSocket);});
                requestThread.Start();
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            serverSocket.Close();
        }

        /// <summary>
        /// 设置服务器目录
        /// </summary>
        /// <param name="root"></param>
        public void SetServerRoot(string root)
        {
            if(!Directory.Exists(root))
                this.ServerRoot = AppDomain.CurrentDomain.BaseDirectory;

            this.ServerRoot = root;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 处理客户端请求
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        private void ProcessRequest(Socket handler)
        {
            //构造请求报文
            HttpRequest request = new HttpRequest(handler);

           //根据请求类型进行处理
            if(request.Method == "GET"){
                OnGet(request);
            }else if(request.Method == "POST"){
                OnPost(request);
            }else{
                OnDefault();
            }
        }

        /// <summary>
        /// 处理服务端响应
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        /// <param name="response">响应报文</param>
        protected void ProcessResponse(Socket handler, HttpResponse response)
        {
            //构建响应头
            byte[] header = response.Encoding.GetBytes(response.BuildHeader());

            //发送响应头
            handler.Send(header);

            //发送空行
            handler.Send(response.Encoding.GetBytes(System.Environment.NewLine));

            //发送消息体
            handler.Send(response.Content);

            //结束会话
            handler.Close();
        }

        /// <summary>
        /// 根据文件扩展名获取内容类型
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <returns></returns>
        protected string GetContentType(string extension)
        {
            string reval = string.Empty;

            if (string.IsNullOrEmpty(extension))
                return null;

            switch (extension)
            {
                case ".htm":
                    reval = "text/html";
                    break;
                case ".html":
                    reval = "text/html";
                    break;
                case ".txt":
                    reval = "text/plain";
                    break;
                case ".css":
                    reval = "text/css";
                    break;
                case ".png":
                    reval = "image/png";
                    break;
                case ".gif":
                    reval = "image/gif";
                    break;
                case ".jpg":
                    reval = "image/jpg";
                    break;
                case ".jpeg":
                    reval = "image/jgeg";
                    break;
                case ".zip":
                    reval = "application/zip";
                    break;
            }
            return reval;
        }

        #endregion

        #region 虚方法

        /// <summary>
        /// 响应Get请求
        /// </summary>
        /// <param name="request">请求报文</param>
        public virtual void OnGet(HttpRequest request)
        {
           
        }

        /// <summary>
        /// 响应Post请求
        /// </summary>
        /// <param name="request"></param>
        public virtual void OnPost(HttpRequest request)
        {
            
        }

        /// <summary>
        /// 响应默认请求
        /// </summary>
        
        public virtual void OnDefault()
        {
            
        }

        /// <summary>
        /// 列取目录
        /// </summary>
        public virtual void OnListFiles()
        {

        }

        #endregion


    }
}
