using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerLib
{
    public class HttpResponse : BaseHeader
    {
        /// <summary>
        /// Age字段
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// Sever字段
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Accept-Ranges字段
        /// </summary>
        public string Accept_Ranges { get; set; }

        /// <summary>
        /// Vary字段
        /// </summary>
        public string Vary { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">响应内容</param>
        public HttpResponse(byte[] content,Encoding encoding)
        {
            //初始化内容
            this.Content = content;
            this.Encoding = encoding;

            //初始化响应头部信息
            this.Age = "";
            this.Server = "";
            this.Accept_Ranges = "";
            this.Vary = "";
            this.StatusCode = "";

            //设置HTTP通用头信息
            this.Cache_Control = "";
            this.Pragma = "";
            this.Connection = "";
            this.Date = "";
            this.Transfer_Encoding = "";
            this.Upgrade = "";
            this.Via = "";

            //设置HTTP实体头部信息
            this.Allow = "";
            this.Location = "";
            this.Content_Base = "";
            this.Content_Encoding = "";
            this.Content_Language = "";
            this.Content_Length = Content.Length.ToString();
            this.Content_Location = "";
            this.Content_MD5 = "";
            this.Content_Range = "";
            this.Content_Type = "";
            this.Etag = "";
            this.Expires = "";
            this.Last_Modified = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">响应内容</param>
        /// <param name="encoding">内容编码</param>
        public HttpResponse(string content,Encoding encoding)
        {
            //初始化内容
            this.Content = encoding.GetBytes(content);
            this.Encoding = encoding;

            //初始化响应头部信息
            this.Age = "";
            this.Server = "";
            this.Accept_Ranges = "";
            this.Vary = "";
            this.StatusCode = "";

            //设置HTTP通用头信息
            this.Cache_Control = "";
            this.Pragma = "";
            this.Connection = "";
            this.Date = "";
            this.Transfer_Encoding = "";
            this.Upgrade = "";
            this.Via = "";

            //设置HTTP实体头部信息
            this.Allow = "";
            this.Location = "";
            this.Content_Base = "";
            this.Content_Encoding = "";
            this.Content_Language = "";
            this.Content_Length = Content.Length.ToString();
            this.Content_Location = "";
            this.Content_MD5 = "";
            this.Content_Range = "";
            this.Content_Type = "";
            this.Etag = "";
            this.Expires = "";
            this.Last_Modified = "";
        }

        /// <summary>
        /// 构建响应头部
        /// </summary>
        /// <returns></returns>
        public string BuildHeader()
        {
            StringBuilder builder = new StringBuilder();

            if(!string.IsNullOrEmpty(StatusCode))
                builder.Append("HTTP/1.1 " + StatusCode + "\r\n");

            if(!string.IsNullOrEmpty(Age))
                builder.Append("Age:" + Age + "\r\n");

            if(!string.IsNullOrEmpty(Server))
                builder.Append("Sever:" + Server + "\r\n");

            if(!string.IsNullOrEmpty(Accept_Ranges))
                builder.Append("Accept-Ranges:" + Accept_Ranges + "\r\n");

            if(!string.IsNullOrEmpty(Vary))
                builder.Append("Vary:" + Vary + "\r\n");

            if(!string.IsNullOrEmpty(Vary))
                builder.Append("Vary:" + Vary + "\r\n");

            if(!string.IsNullOrEmpty(Cache_Control))
                builder.Append("Cache-Control:" + Cache_Control + "\r\n");

            if(!string.IsNullOrEmpty(Pragma))
                builder.Append("Pragma:" + Pragma + "\r\n");

            if(!string.IsNullOrEmpty(Connection))
                builder.Append("Connection:" + Connection + "\r\n");

            if(!string.IsNullOrEmpty(Date))
                builder.Append("Date:" + Date + "\r\n");

            if(!string.IsNullOrEmpty(Transfer_Encoding))
                builder.Append("Transfer-Encoding:" + Transfer_Encoding + "\r\n");

            if(!string.IsNullOrEmpty(Upgrade))
                builder.Append("Upgrade:" + Upgrade + "\r\n");

            if(!string.IsNullOrEmpty(Via))
                builder.Append("Via:" + Via + "\r\n");

            if(!string.IsNullOrEmpty(Allow))
                builder.Append("Allow:" + Allow + "\r\n");

            if(!string.IsNullOrEmpty(Location))
                builder.Append("Location:" + Location + "\r\n");

            if(!string.IsNullOrEmpty(Content_Base))
                builder.Append("Content-Base:" + Content_Base + "\r\n");

            if(!string.IsNullOrEmpty(Content_Encoding))
                builder.Append("Content-Encoding:" + Content_Encoding + "\r\n");

            if(!string.IsNullOrEmpty(Content_Length))
                builder.Append("Content-Length:" + Content_Length + "\r\n");

            if(!string.IsNullOrEmpty(Content_Location))
                builder.Append("Content-Location:" + Content_Location + "\r\n");

            if(!string.IsNullOrEmpty(Content_MD5))
                builder.Append("Content-MD5:" + Content_MD5 + "\r\n");

            if(!string.IsNullOrEmpty(Content_Range))
                builder.Append("Content_Range:" + Content_Range + "\r\n");

            if(!string.IsNullOrEmpty(Content_Type))
                builder.Append("Content-Type:" + Content_Type + "\r\n");

            if(!string.IsNullOrEmpty(Etag))
                builder.Append("Etag:" + Etag + "\r\n");

            if(!string.IsNullOrEmpty(Expires))
                builder.Append("Expires:" + Expires + "\r\n");

            if(!string.IsNullOrEmpty(Last_Modified))
                builder.Append("Last-Modified :" + Last_Modified + "\r\n");

            return builder.ToString();
        }
    }
}
