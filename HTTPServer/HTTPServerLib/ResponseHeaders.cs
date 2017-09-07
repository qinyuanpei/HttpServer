using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public enum ResponseHeaders
    {
        
        //Cache-Control 标头，指定请求/响应链上所有缓存机制必须服从的缓存指令。
        CacheControl = 0,
        //
        
        //Connection 标头，指定特定连接需要的选项。
        Connection = 1,
        //
        
        //Date 标头，指定响应产生的日期和时间。
        Date = 2,
        //
        
        //Keep-Alive 标头，指定用于维护持久连接的参数。
        KeepAlive = 3,
        //
        
        //Pragma 标头，指定可应用于请求/响应链上的任何代理的特定于实现的指令。
        Pragma = 4,
        //
        
        //Trailer 标头，指定指示的标头字段在消息（使用分块传输编码方法进行编码）的尾部显示。
        Trailer = 5,
        //
        
        //Transfer-Encoding 标头，指定对消息正文应用哪种类型的转换（如果有）。
        TransferEncoding = 6,
        //
        
        //Upgrade 标头，指定客户端支持的附加通信协议。
        Upgrade = 7,
        //
        
        //Via 标头，指定网关和代理程序要使用的中间协议。
        Via = 8,
        //
        
        //Warning 标头，指定关于可能未在消息中反映的消息的状态或转换的附加信息。
        Warning = 9,
        //
        
        //Allow 标头，指定支持的 HTTP 方法集。
        Allow = 10,
        //
        
        //Content-Length 标头，指定伴随正文数据的长度（以字节为单位）。
        ContentLength = 11,
        //
        
        //Content-Type 标头，指定伴随正文数据的 MIME 类型。
        ContentType = 12,
        //
        
        //Content-Encoding 标头，指定已应用于伴随正文数据的编码。
        ContentEncoding = 13,
        //
        
        //Content-Langauge 标头，指定自然语言或伴随正文数据的语言。
        ContentLanguage = 14,
        //
        
        //Content-Location 标头，指定可以从中获取伴随正文的 URI。
        ContentLocation = 15,
        //
        
        //Content-MD5 标头，指定伴随正文数据的 MD5 摘要，用于提供端到端消息完整性检查。
        ContentMd5 = 16,
        //
        
        //Range 标头，指定客户端请求返回的响应的单个或多个子范围来代替整个响应。
        ContentRange = 17,
        //
        
        //Expires 标头，指定日期和时间，在此之后伴随的正文数据应视为陈旧的。
        Expires = 18,
        //
        
        //Last-Modified 标头，指定上次修改伴随的正文数据的日期和时间。
        LastModified = 19,
        //
        
        //Accept-Ranges 标头，指定服务器接受的范围。
        AcceptRanges = 20,
        //
        
        //Age 标头，指定自起始服务器生成响应以来的时间长度（以秒为单位）。
        Age = 21,
        //
        
        //Etag 标头，指定请求的变量的当前值。
        ETag = 22,
        //
        
        //Location 标头，指定为获取请求的资源而将客户端重定向到的 URI。
        Location = 23,
        //
        
        //Proxy-Authenticate 标头，指定客户端必须对代理验证其自身。
        ProxyAuthenticate = 24,
        //
        
        //Retry-After 标头，指定某个时间（以秒为单位）或日期和时间，在此时间之后客户端可以重试其请求。
        RetryAfter = 25,
        //
        
        //Server 标头，指定关于起始服务器代理的信息。
        Server = 26,
        //
        
        //Set-Cookie 标头，指定提供给客户端的 Cookie 数据。
        SetCookie = 27,
        //
        
        //Vary 标头，指定用于确定缓存的响应是否为新响应的请求标头。
        Vary = 28,
        //
        
        //WWW-Authenticate 标头，指定客户端必须对服务器验证其自身。
        WwwAuthenticate = 29,
    }
}
