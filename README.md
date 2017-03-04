# HttpServer
一个使用C#编写的简易Web服务器, 目前支持：
* 静态页面渲染
* GET/POST请求

# 快速开始

## HTTP服务器示例
```
class Program
{
    static void Main(string[] args)
    {
        ExampleServer server = new ExampleServer("0.0.0.0",4050);
        server.SetServerRoot("./");
        server.Start();
    }
}
```

## GET/POST请求示例
```
public override void OnGet(HttpRequest request)
{
    if(request.Method == "GET")
    {
        ///链接形式1:"http://localhost:4050/assets/styles/style.css"表示访问指定文件资源，
        ///此时读取服务器目录下的/assets/styles/style.css文件。

        ///链接形式1:"http://localhost:4050/assets/styles/"表示访问指定页面资源，
        ///此时读取服务器目录下的/assets/styles/style.index文件。

        //当文件不存在时应返回404状态码
        string requestURL = request.URL;
        requestURL = requestURL.Replace("/", @"\").Replace("\\..", "");

        //判断地址中是否存在扩展名
        string extension = Path.GetExtension(requestURL);

        //根据有无扩展名按照两种不同链接进行处
        string requestFile = string.Empty;
        if(extension != ""){
            requestFile = ServerRoot + requestURL;
        }else{
            requestFile = ServerRoot + requestURL + "index.html";
        }

        //构造HTTP响应
        HttpResponse response = ResponseWithFile(requestFile);

        //发送响应
        ProcessResponse(request.Handler, response);
    }
}

```

```
public override void OnPost(HttpRequest request)
{
    //获取客户端传递的参数
    int num1 = int.Parse(request.Params["num1"]);
    int num2 = int.Parse(request.Params["num2"]);

    //设置返回信息
    string content = string.Format("这是通过Post方式返回的数据:num1={0},num2={1}", num1, num2);

    //构造响应报文
    HttpResponse response = new HttpResponse(content, Encoding.UTF8);
    response.StatusCode = "200";
    response.Content_Type = "text/html; charset=UTF-8";
    response.Server = "ExampleServer";

    //发送响应
    ProcessResponse(request.Handler, response);
}
```




