# HttpServer
一个使用C#编写的简易Web服务器, 目前支持：
* 静态页面处理 :smile:
* GET/POST请求 :smile:
* 支持HTTPS协议 :smile:
* 支持返回JSON :worried:
* 支持路由方法 :worried:

# 快速开始

## HTTP服务器示例
```
class Program
{
    static void Main(string[] args)
    {
        ExampleServer server = new ExampleServer("0.0.0.0",4050);
        server.Start();
    }
}
```

## GET/POST请求示例
```
 public override void OnPost(HttpRequest request, HttpResponse response)
 {
    //获取客户端传递的参数
    string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());

    //设置返回信息
    string content = string.Format("这是通过Post方式返回的数据:{0}", data);

    //构造响应报文
    response.SetContent(content);
    response.Content_Encoding = "utf-8";
    response.StatusCode = "200";
    response.Content_Type = "text/html; charset=UTF-8";
    response.Server = "ExampleServer";

    //发送响应
    response.Send();
}

public override void OnGet(HttpRequest request, HttpResponse response)
{

    ///链接形式1:"http://localhost:4050/assets/styles/style.css"表示访问指定文件资源，
    ///此时读取服务器目录下的/assets/styles/style.css文件。

    ///链接形式1:"http://localhost:4050/assets/styles/"表示访问指定页面资源，
    ///此时读取服务器目录下的/assets/styles/style.index文件。

    //当文件不存在时应返回404状态码
    string requestURL = request.URL;
    requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
    string requestFile = Path.Combine(ServerRoot, requestURL);

    //判断地址中是否存在扩展名
    string extension = Path.GetExtension(requestFile);

    //根据有无扩展名按照两种不同链接进行处
    if (extension != "")
    {
        //从文件中返回HTTP响应
        response = LoadFromFile(response, requestFile);
    } 
    else
    {
        //目录存在且不存在index页面时时列举目录
        if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
        {
            requestFile = Path.Combine(ServerRoot, requestFile);
            var content = ListDirectory(requestFile, requestURL);
            response = response.SetContent(content, Encoding.UTF8);
            response.Content_Type = "text/html; charset=UTF-8";
        } 
        else
        {
            //加载静态HTML页面
            requestFile = Path.Combine(requestFile, "index.html");
            response = LoadFromFile(response, requestFile);
            response.Content_Type = "text/html; charset=UTF-8";
        }
    }

    //发送HTTP响应
    response.Send();
}
```




