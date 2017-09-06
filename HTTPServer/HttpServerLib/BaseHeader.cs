using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
     public class BaseHeader
    {
         /// <summary>
         /// HTTP(S)地址
         /// </summary>
         public string URL { get; set; }

         /// <summary>
         /// 编码格式
         /// </summary>
         public Encoding Encoding { get; set; }

         /// <summary>
         /// HTTP状态码
         /// </summary>
         public string StatusCode { get; set; }

         /// <summary>
         /// HTTP协议版本
         /// </summary>
         public string ProtocolVersion { get; set; }

         /// <summary>
         /// HTTP头部字段
         /// </summary>
         public Dictionary<string, string> Headers { get; set; }

         /// <summary>
         /// HTTP消息体
         /// </summary>
         public string Body { get; set; }

         /// <summary>
         /// ContentType
         /// </summary>
         public string ContentType { get; set; }

         /// <summary>
         /// ContentLength
         /// </summary>
         public long ContentLength { get; set; }

         /// <summary>
         /// ContentEncoding
         /// </summary>
         public string ContentEncoding { get; set; }

         /// <summary>
         /// CharacterSet
         /// </summary>
         public string CharacterSet { get; set; }

         /// <summary>
         /// 设置HTTP头部字段
         /// </summary>
         /// <param name="field">字段名</param>
         /// <param name="value">字段值</param>
         public void SetHeaders(HeaderFields field, string value)
         {

         }

         /// <summary>
         /// 获取HTTP头部字段
         /// </summary>
         /// <param name="field">字段名</param>
         /// <returns></returns>
         public string GetHeaders(HeaderFields field)
         {
             return null;
         }



       

        /// <summary>
        /// 从内容中解析请求参数并返回一个字典
        /// </summary>
        /// <param name="content">使用&连接的参数字符串</param>
        /// <returns>如果存在参数则返回参数否则返回null</returns>
        protected Dictionary<string, string> GetRequestParams(string content)
        {
            //防御编程
            if(string.IsNullOrEmpty(content))
                return null;

            //按照&对字符进行分割
            string[] reval = content.Split('&');
            if(reval.Length <= 0)
                return null;

            //将结果添加至字典
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(string val in reval)
            {
                string[] kv = val.Split('=');
                if(kv.Length <= 1)
                    dict.Add(kv[0], "");
                dict.Add(kv[0],kv[1]);
            }

            //返回字典
            return dict;
        }
    }
}
