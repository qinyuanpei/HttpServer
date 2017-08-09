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
            if (IsRunning)
            {
                return;
            }

            //创建服务端Socket
            this.serverListener = new TcpListener(IPAddress.Parse(ServerIP), ServerPort);
            this.Protocol = serverCertificate == null ? Protocols.Http : Protocols.Http;
            this.IsRunning = true;
            this.Log(string.Format("Sever is running at {0}://{1}:{2}", Protocol.ToString().ToLower(), ServerIP, ServerPort));
            this.serverListener.Start();

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
            catch (AuthenticationException e)
            {
                Log(e.Message);
                if (e.InnerException != null) Log(e.InnerException.Message);
                Log("Authentication failed - closing the connection: " + e.Message);
                clientStream.Close();
            }
            catch (Exception e)
            {
                Log(e.Message);
                clientStream.Close();
            }

            return null;
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

        /// <summary>
        /// 根据物理路径获取MIME文件二进制流,并填写文件类型
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        /// <param name="response">响应对象</param>
        /// <returns></returns>
        public byte[] GetFile(string filePath,HttpResponse response)
        {
            try
            {
                response.Content_Type = GetMimeFromFile(filePath);
                return System.IO.File.ReadAllBytes(filePath);
            }
            catch
            {
                Console.WriteLine("请求该物理地址错误:{0}", filePath);
                return System.Text.Encoding.UTF8.GetBytes("<html><head></head><body>服务器 <br/> 输出错误 <br/> :(</body></html>");
            }
        }

        /// <summary>
        /// 获取文件MIME类型，并检测文件是否存在
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        /// <returns></returns>
        public static string GetMimeFromFile(string filePath)
        {
            IntPtr mimeout;
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException(filePath + " 未找到");

            int MaxContent = (int)new FileInfo(filePath).Length;
            if (MaxContent > 4096) MaxContent = 4096;
            FileStream fs = File.OpenRead(filePath);

            byte[] buf = new byte[MaxContent];
            fs.Read(buf, 0, MaxContent);
            fs.Close();
            int result = FindMimeFromData(IntPtr.Zero, filePath, buf, MaxContent, null, 0, out mimeout, 0);

            if (result != 0)
                throw Marshal.GetExceptionForHR(result);
            string mime = Marshal.PtrToStringUni(mimeout);
            Marshal.FreeCoTaskMem(mimeout);
            
            return mime;
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
              [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
              [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] 
              byte[] pBuffer,
              int cbSize,
              [MarshalAs(UnmanagedType.LPWStr)]  
              string pwzMimeProposed,
              int dwMimeFlags,
              out IntPtr ppwzMimeOut,
              int dwReserved);

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
