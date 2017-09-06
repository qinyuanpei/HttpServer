using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public class HttpResponse : BaseHeader
    {
        public string Server { get; set; }
        public byte[] Content { get; private set; }

        public Encoding Encoding { get; private set; }

        private Stream handler;

        public ILogger Logger { get; set; }

        public HttpResponse(Stream stream)
        {
            this.handler = stream;
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="content">响应内容</param>
        /// <param name="encoding">内容编码</param>
        public HttpResponse SetContent(byte[] content, Encoding encoding = null)
        {
            //初始化内容
            this.Content = content;
            this.Encoding = encoding != null ? encoding : Encoding.UTF8;
            this.ContentLength = long.Parse(content.Length.ToString());
            return this;
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="content">响应内容</param>
        /// <param name="encoding">内容编码</param>
        public HttpResponse SetContent(string content, Encoding encoding = null)
        {
            //初始化内容
            encoding = encoding != null ? encoding : Encoding.UTF8;
            return SetContent(encoding.GetBytes(content), encoding);
        }

        public Stream GetResponseStream()
        {
            return this.handler;
        }

        /// <summary>
        /// 构建响应头部
        /// </summary>
        /// <returns></returns>
        protected string BuildHeader()
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(StatusCode))
                builder.Append("HTTP/1.1 " + StatusCode + "\r\n");

            if (!string.IsNullOrEmpty(this.ContentType))
                builder.AppendLine("Content-Type:" + this.ContentType);
            if (this.ContentLength > 0)
                builder.AppendLine("ContentLength:" + this.ContentLength);
            return builder.ToString();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void Send()
        {
            if (!handler.CanWrite) return;

            try
            {
                //发送响应头
                var header = BuildHeader();
                byte[] headerBytes = this.Encoding.GetBytes(header);
                handler.Write(headerBytes, 0, headerBytes.Length);

                //发送空行
                byte[] lineBytes = this.Encoding.GetBytes(System.Environment.NewLine);
                handler.Write(lineBytes, 0, lineBytes.Length);

                //发送内容
                handler.Write(Content, 0, Content.Length);
            }
            catch (Exception e)
            {

            }
            finally
            {
                handler.Close();
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void Log(object message)
        {
            if (Logger != null)
                Logger.Log(message);
        }
    }
}
