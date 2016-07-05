using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace HttpServerLib
{
    /// <summary>
    /// HTTP请求定义
    /// </summary>
    public class HttpRequest : BaseHeader
    {
        /// <summary>
        /// 可接受的内容类型
        /// </summary>
        public string[] AcceptTypes { get; private set; }

        /// <summary>
        /// 可接受的内容编码
        /// </summary>
        public string[] AcceptEncoding { get; private set; }

        /// <summary>
        /// 可接受的内容字符集
        /// </summary>
        public string[] AcceptCharset { get; private set; }

        /// <summary>
        /// 可接受的内容语言
        /// </summary>
        public string[] AcceptLanguage { get; private set; }

        /// <summary>
        /// HTTP授权证书
        /// </summary>
        public string Authorization { get; private set; }

        /// <summary>
        /// If-Match字段
        /// </summary>
        public string If_Match { get; private set; }

        /// <summary>
        /// If_None_Match字段
        /// </summary>
        public string If_None_Match { get; private set; }

        /// <summary>
        /// If_Modified_Since字段
        /// </summary>
        public string If_Modified_Since { get; private set; }

        /// <summary>
        /// If_Unmodified_Since字段
        /// </summary>
        public string If_Unmodified_Since { get; private set; }

        /// <summary>
        /// If_Range字段
        /// </summary>
        public string If_Range { get; private set; }

        /// <summary>
        /// Range字段
        /// </summary>
        public string Range { get; private set; }

        /// <summary>
        /// Proxy-Authenticate字段
        /// </summary>
        public string Proxy_Authenticate { get; private set; }

        /// <summary>
        /// Proxy_Authorization字段
        /// </summary>
        public string Proxy_Authorization { get; private set; }

        /// <summary>
        /// 客户端指定的主机地址和端口号
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// 客户端指定从跳转到该网站的原始连接
        /// </summary>
        public string Referer { get; private set; }

        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string User_Agent { get; private set; }

        /// <summary>
        /// HTTP请求方式
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, string> Params { get; private set; }

        /// <summary>
        /// 请求Socket
        /// </summary>
        public Socket Handler { get; private set; }

        /// <summary>
        /// 定义缓冲区
        /// </summary>
        private byte[] bytes = new byte[1024 * 1024 * 2];

        /// <summary>
        /// 客户端请求报文
        /// </summary>
        private string content = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        public HttpRequest(Socket handler)
        {
            //解析客户端请求
            int length = handler.Receive(bytes);
            string content = Encoding.UTF8.GetString(bytes, 0, length);

            //缓存客户端请求报文
            this.content = content;

            //按行分割请求报文
            string[] lines = content.Split('\n');

            //获取请求方法
            this.Method =  lines[0] == "" ? "" : lines[0].Split(' ')[0];

            //获取请求地址
            this.URL = lines[0] == "" ? "" : lines[0].Split(' ')[1];

            //获取请求参数
            if(this.Method == "GET" && this.URL.Contains('?')){
                this.Params = GetRequestParams(lines[0].Split(' ')[1].Split('?')[1]);
            }else if(this.Method == "POST"){
                this.Params = GetRequestParams(lines[lines.Length-1]);
            }

            //获取请求Socket
            this.Handler = handler;

            //获取各种请求报文参数
            this.AcceptTypes = GetKeyValueArrayByKey(content, "Accept");
            this.AcceptCharset = GetKeyValueArrayByKey(content, "Accept-Charset");
            this.AcceptEncoding = GetKeyValueArrayByKey(content, "Accept-Encoding");
            this.AcceptLanguage = GetKeyValueArrayByKey(content, "Accept-Langauge");
            this.Authorization =  GetKeyValueByKey(content, "Authorization");
            this.If_Match = GetKeyValueByKey(content,"If-Match");
            this.If_None_Match = GetKeyValueByKey(content, "If-None-Match");
            this.If_Modified_Since = GetKeyValueByKey(content, "If-Modified-Since");
            this.If_Unmodified_Since = GetKeyValueByKey(content, "If-Unmodified-Since");
            this.If_Range = GetKeyValueByKey(content, "If-Range");
            this.Range = GetKeyValueByKey(content, "Range");
            this.Proxy_Authenticate = GetKeyValueByKey(content, "Proxy-Authenticate");
            this.Proxy_Authorization = GetKeyValueByKey(content, "Proxy-Authorization");
            this.Host = GetKeyValueByKey(content, "Host");
            this.Referer = GetKeyValueByKey(content, "Referer");
            this.User_Agent = GetKeyValueByKey(content, "User-Agent");

            //设置HTTP通用头信息
            this.Cache_Control = GetKeyValueByKey(content, "Cache-Control");
            this.Pragma = GetKeyValueByKey(content, "Pragma");
            this.Connection = GetKeyValueByKey(content, "Connection");
            this.Date = GetKeyValueByKey(content, "Date");
            this.Transfer_Encoding = GetKeyValueByKey(content, "Transfe-Encoding");
            this.Upgrade = GetKeyValueByKey(content, "Upgrade");
            this.Via = GetKeyValueByKey(content, "Via");

            //设置HTTP实体头部信息
            this.Allow = GetKeyValueByKey(content, "Allow");
            this.Location = GetKeyValueByKey(content, "Location");
            this.Content_Base = GetKeyValueByKey(content, "Content-Base");
            this.Content_Encoding = GetKeyValueByKey(content, "Content-Encoidng");
            this.Content_Language = GetKeyValueByKey(content, "Content-Language");
            this.Content_Length = GetKeyValueByKey(content, "Content-Length");
            this.Content_Location = GetKeyValueByKey(content, "Content-Location");
            this.Content_MD5 = GetKeyValueByKey(content, "Content-MD5");
            this.Content_Range = GetKeyValueByKey(content, "Content-Range");
            this.Content_Type = GetKeyValueByKey(content, "Content-Type");
            this.Etag = GetKeyValueByKey(content, "Etag");
            this.Expires = GetKeyValueByKey(content, "Expires");
            this.Last_Modified = GetKeyValueByKey(content, "Last-Modified");
        }

        /// <summary>
        /// 构建请求头部
        /// </summary>
        /// <returns></returns>
        public string BuildHeader()
        {
            return this.content;
        }
    }
}