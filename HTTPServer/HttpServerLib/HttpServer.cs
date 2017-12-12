using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Runtime.InteropServices;

namespace HTTPServerLib
{

    public class HttpServer : IServer
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
        /// 是否运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 服务器协议
        /// </summary>
        public Protocols Protocol { get; private set; }

        /// <summary>
        /// 服务端Socet
        /// </summary>
        private TcpListener serverListener;

        /// <summary>
        /// 日志接口
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// SSL证书
        /// </summary>
        private X509Certificate serverCertificate = null;

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
            if (!Directory.Exists(root))
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
            this(IPAddress.Parse(ipAddress), port, root)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public HttpServer(string ipAddress, int port) :
            this(IPAddress.Parse(ipAddress), port, AppDomain.CurrentDomain.BaseDirectory)
        { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        public HttpServer(int port, string root) :
            this(IPAddress.Loopback, port, root)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        public HttpServer(int port) :
            this(IPAddress.Loopback, port, AppDomain.CurrentDomain.BaseDirectory)
        { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip"></param>
        public HttpServer(string ip) :
            this(IPAddress.Parse(ip), 80, AppDomain.CurrentDomain.BaseDirectory)
        { }

        #region 公开方法

        /// <summary>
        /// 开启服务器
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;

            //创建服务端Socket
            this.serverListener = new TcpListener(IPAddress.Parse(ServerIP), ServerPort);
            this.Protocol = serverCertificate == null ? Protocols.Http : Protocols.Https;
            this.IsRunning = true;
            this.serverListener.Start();
            this.Log(string.Format("Sever is running at {0}://{1}:{2}", Protocol.ToString().ToLower(), ServerIP, ServerPort));

            try
            {
                while (IsRunning)
                {
                    TcpClient client = serverListener.AcceptTcpClient();
                    Thread requestThread = new Thread(() => { ProcessRequest(client); });
                    requestThread.Start();
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }


        public HttpServer SetSSL(string certificate)
        {
            return SetSSL(X509Certificate.CreateFromCertFile(certificate));
        }


        public HttpServer SetSSL(X509Certificate certifiate)
        {
            this.serverCertificate = certifiate;
            return this;
        }

        public void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;
            serverListener.Stop();
        }

        /// <summary>
        /// 设置服务器目录
        /// </summary>
        /// <param name="root"></param>
        public HttpServer SetRoot(string root)
        {
            if (!Directory.Exists(root))
                this.ServerRoot = AppDomain.CurrentDomain.BaseDirectory;

            this.ServerRoot = root;
            return this;
        }
        /// <summary>
        /// 获取服务器目录
        /// </summary>
        public string GetRoot()
        {
            return this.ServerRoot;
        }

        /// <summary>
        /// 设置端口
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public HttpServer SetPort(int port)
        {
            this.ServerPort = port;
            return this;
        }


        #endregion

        #region 内部方法

        /// <summary>
        /// 处理客户端请求
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        private void ProcessRequest(TcpClient handler)
        {
            //处理请求
            Stream clientStream = handler.GetStream();

            //处理SSL
            if (serverCertificate != null) clientStream = ProcessSSL(clientStream);
            if (clientStream == null) return;

            //构造HTTP请求
            HttpRequest request = new HttpRequest(clientStream);
            request.Logger = Logger;

            //构造HTTP响应
            HttpResponse response = new HttpResponse(clientStream);
            response.Logger = Logger;

            //处理请求类型
            switch (request.Method)
            {
                case "GET":
                    OnGet(request, response);
                    break;
                case "POST":
                    OnPost(request, response);
                    break;
                default:
                    OnDefault(request, response);
                    break;
            }
        }


        /// <summary>
        /// 处理ssl加密请求
        /// </summary>
        /// <param name="clientStream"></param>
        /// <returns></returns>
        private Stream ProcessSSL(Stream clientStream)
        {
            try
            {
                SslStream sslStream = new SslStream(clientStream);
                sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                sslStream.ReadTimeout = 10000;
                sslStream.WriteTimeout = 10000;
                return sslStream;
            }
            catch (Exception e)
            {
                Log(e.Message);
                clientStream.Close();
            }

            return null;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected void Log(object message)
        {
            if (Logger != null) Logger.Log(message);
        }

        #endregion

        #region 虚方法

        /// <summary>
        /// 响应Get请求
        /// </summary>
        /// <param name="request">请求报文</param>
        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应Post请求
        /// </summary>
        /// <param name="request"></param>
        public virtual void OnPost(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应默认请求
        /// </summary>

        public virtual void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        #endregion
    }
}
