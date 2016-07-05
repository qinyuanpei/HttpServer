using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerLib
{
     /// <summary>
     /// HTTP公共头部定义
     /// </summary>
     public class BaseHeader
    {
        /*-------------------------------------------------------------*/
        /*------------------以下通用头部参数定义 --------------------*/
        /*-------------------------------------------------------------*/

         /// <summary>
         /// Cache-Control字段
         /// </summary>
         public string Cache_Control { get; set; }

         /// <summary>
         /// Pragma字段
         /// </summary>
         public string Pragma { get; set; }

         /// <summary>
         /// Connection字段
         /// </summary>
         public string Connection { get; set; }

         /// <summary>
         /// Date字段
         /// </summary>
         public string Date { get; set; }

         /// <summary>
         /// Transfer-Encoding字段
         /// </summary>
         public string Transfer_Encoding { get; set; }

         /// <summary>
         /// Upgrade字段
         /// </summary>
         public string Upgrade { get; set; }

         /// <summary>
         /// Via字段
         /// </summary>
         public string Via { get; set; }

         /*-------------------------------------------------------------*/
         /*------------------以下为实体头部参数定义 --------------------*/
         /*-------------------------------------------------------------*/

         /// <summary>
         /// Allow字段
         /// </summary>
         public string Allow { get; set; }

         /// <summary>
         /// Location字段
         /// </summary>
         public string Location { get; set; }

         /// <summary>
         /// Content_Base字段
         /// </summary>
         public string Content_Base { get; set; }

         /// <summary>
         /// Content_Encoding字段 
         /// </summary>
         public string Content_Encoding {get;set;}

         /// <summary>
         /// Content_Language字段
         /// </summary>
         public string Content_Language { get; set; }

         /// <summary>
         /// Content_Length字段
         /// </summary>
         public string Content_Length { get; set; }

         /// <summary>
         /// Content_Location字段
         /// </summary>
         public string Content_Location { get; set; }

         /// <summary>
         /// Content_MD5字段
         /// </summary>
         public string Content_MD5 { get; set; }

         /// <summary>
         /// Content_Range字段
         /// </summary>
         public string Content_Range { get; set; }

         /// <summary>
         /// Content_Type字段
         /// </summary>
         public string Content_Type { get; set; }

         /// <summary>
         /// Etag字段
         /// </summary>
         public string Etag { get; set; }

         /// <summary>
         /// Expires字段
         /// </summary>
         public string Expires { get; set; }

         /// <summary>
         /// Last_Modified字段
         /// </summary>
         public string Last_Modified { get; set; }


         /*-------------------------------------------------------------*/
         /*------------------以下为报文解析公共方法 --------------------*/
         /*-------------------------------------------------------------*/


        /// <summary>
        /// 根据键名从键值对中获取值
        /// </summary>
        /// <param name="content">报文内容</param>
        /// <param name="key">键名</param>
        /// <returns>键名存在则返回值否则返回空字符</returns>
        protected string GetKeyValueByKey(string content, string key)
        {
            //防御编程
            if(string.IsNullOrEmpty(content) || string.IsNullOrEmpty(key))
                return "";

            //按照换行符对报文内容进行分割
            string[] AllLines = content.Split('\n');

            //根据键名来匹配指定行报文
            var line = AllLines.Where(item => item.Split(':')[0] == key);

            //如果键名不存在则返回null
            if (line == null || line.Count() <= 0)
                return null;

            //如果值不存在则返回null
            string[] reval = line.First<string>().Split(':');
            if (reval.Length <= 1)
                return null;

            return reval[1];
        }

        /// <summary>
        /// 根据键名从键值对中获取值数组
        /// </summary>
        /// <param name="content">报文内容</param>
        /// <param name="key">键名</param>
        /// <returns>键名存在则返回值否则返回null</returns>
        protected string[] GetKeyValueArrayByKey(string content, string key)
        {
            //防御编程
            if(string.IsNullOrEmpty(content) || string.IsNullOrEmpty(key))
                return null;

            //按照换行符对报文内容进行分割
            string[] AllLines = content.Split('\n');

            //根据键名来匹配指定行报文
            var line = AllLines.Where(item => item.Split(':')[0] == key);

            //如果键名不存在则返回null
            if (line == null || line.Count() <= 0)
                return null;

            //如果值不存在则返回null
            string[] reval = line.First<string>().Split(':');
            if (reval.Length <= 1)
                return null;

            return reval[1].Split(',');
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
