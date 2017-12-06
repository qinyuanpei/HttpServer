using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public enum RequestHeaderFields
    {
        
        //Cache-Control 标头，指定请求/响应链上所有缓存控制机制必须服从的指令。
        CacheControl = 0,
        
        //Connection 标头，指定特定连接需要的选项。
        Connection = 1,
        
        //Date 标头，指定开始创建请求的日期和时间。
        Date = 2,
        
        //Keep-Alive 标头，指定用以维护持久性连接的参数。
        KeepAlive = 3,
        
        //Pragma 标头，指定可应用于请求/响应链上的任何代理的特定于实现的指令。
        Pragma = 4,
        
        //Trailer 标头，指定标头字段显示在以 chunked 传输编码方式编码的消息的尾部。
        Trailer = 5,
        
        //Transfer-Encoding 标头，指定对消息正文应用的转换的类型（如果有）。
        TransferEncoding = 6,
        
        //Upgrade 标头，指定客户端支持的附加通信协议。
        Upgrade = 7,
        
        //Via 标头，指定网关和代理程序要使用的中间协议。
        Via = 8,
        
        //Warning 标头，指定关于可能未在消息中反映的消息的状态或转换的附加信息。
        Warning = 9,
        
        //Allow 标头，指定支持的 HTTP 方法集。
        Allow = 10,
        
        //Content-Length 标头，指定伴随正文数据的长度（以字节为单位）。
        ContentLength = 11,
        
        //Content-Type 标头，指定伴随正文数据的 MIME 类型。
        ContentType = 12,
        
        //Content-Encoding 标头，指定已应用于伴随正文数据的编码。
        ContentEncoding = 13,
        
        //Content-Langauge 标头，指定伴随正文数据的自然语言。
        ContentLanguage = 14,
        
        //Content-Location 标头，指定可从其中获得伴随正文的 URI。
        ContentLocation = 15,
        
        //Content-MD5 标头，指定伴随正文数据的 MD5 摘要，用于提供端到端消息完整性检查。
        ContentMd5 = 16,
        
        //Content-Range 标头，指定在完整正文中应用伴随部分正文数据的位置。
        ContentRange = 17,
        
        //Expires 标头，指定日期和时间，在此之后伴随的正文数据应视为陈旧的。
        Expires = 18,
        
        //Last-Modified 标头，指定上次修改伴随的正文数据的日期和时间。
        LastModified = 19,
        
        //Accept 标头，指定响应可接受的 MIME 类型。
        Accept = 20,
        
        //Accept-Charset 标头，指定响应可接受的字符集。
        AcceptCharset = 21,
        
        //Accept-Encoding 标头，指定响应可接受的内容编码。
        AcceptEncoding = 22,
        
        //Accept-Langauge 标头，指定响应首选的自然语言。
        AcceptLanguage = 23,
        
        //Authorization 标头，指定客户端为向服务器验证自身身份而出示的凭据。
        Authorization = 24,
        
        //Cookie 标头，指定向服务器提供的 Cookie 数据。
        Cookie = 25,
        
        //Expect 标头，指定客户端要求的特定服务器行为。
        Expect = 26,
        
        //From 标头，指定控制请求用户代理的用户的 Internet 电子邮件地址。
        From = 27,
        
        //Host 标头，指定所请求资源的主机名和端口号。
        Host = 28,
        
        //If-Match 标头，指定仅当客户端的指示资源的缓存副本是最新的时，才执行请求的操作。
        IfMatch = 29,
        
        //If-Modified-Since 标头，指定仅当自指示的数据和时间之后修改了请求的资源时，才执行请求的操作。
        IfModifiedSince = 30,
        
        //If-None-Match 标头，指定仅当客户端的指示资源的缓存副本都不是最新的时，才执行请求的操作。
        IfNoneMatch = 31,
        
        //If-Range 标头，指定如果客户端的缓存副本是最新的，仅发送指定范围的请求资源。
        IfRange = 32,
        
        //If-Unmodified-Since 标头，指定仅当自指示的日期和时间之后修改了请求的资源时，才执行请求的操作。
        IfUnmodifiedSince = 33,
        
        //Max-Forwards 标头，指定一个整数，表示此请求还可转发的次数。
        MaxForwards = 34,
        
        //Proxy-Authorization 标头，指定客户端为向代理验证自身身份而出示的凭据。
        ProxyAuthorization = 35,
        
        //Referer 标头，指定从中获得请求 URI 的资源的 URI。
        Referer = 36,
        
        //Range 标头，指定代替整个响应返回的客户端请求的响应的子范围。
        Range = 37,
        
        //TE 标头，指定响应可接受的传输编码方式。
        Te = 38,
        
        //Translate 标头，与 WebDAV 功能一起使用的 HTTP 规范的 Microsoft 扩展。
        Translate = 39,
        
        //User-Agent 标头，指定有关客户端代理的信息。
        UserAgent = 40,
    }
}
