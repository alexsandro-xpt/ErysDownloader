ErysDownloader
==============

After a lot of years, since of .net 2.0, I'm using WebClient .net class, it's is stack HTTP library but sometime I need more server HTTP control flow and it's sometime sounds bad.

In 2012 I decide plan and project a new HTTP library to my projects and I call it as ErysDownloader, Erys is my old female friend called Eryane, she was a frenetic's movie downloader.

ErysDownloader, is a **pure C# lightweight HTTP text/html download library**. No dependece.

Usage:
-------------
      //Direct internet connection
      var sd = new SocketDownloader();

      //or classic proxy
      var proxy = new ClassicProxy();
      proxy.Host = "10.2.0.1";
      proxy.Port = 3128;
      proxy.Credencial = new Credencial()
      {
          UserName = "myusername", Password = Encoding.ASCII.GetString(Convert.FromBase64String("RDSa4OP=="))
      };
      sd = new SocketDownloader(proxy);

      //or proxy tunneling
      sd = new SocketDownloader(new Tunnel() { Host = "myproxy", Port = 8080 });

      //or chain proxy
      sd = new SocketDownloader(new Tunnel(
        new Tunnel()
        {
            Host = "myproxy1", Port = 8080
        })
      {
          Host = "myproxy2", Port = 8080
      });

      var uri = new Uri("http://www.microsoft.com");
      IEnumerable<string> requestHeaders = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
      var response = sd.Execute(uri, requestHeaders);
      //response.Headers
      //response.Content


Feature:
-------------
* Full HTTP GET verb stack protocol (SSL, chucked content, gzip and deflate)
* Direct Connection
* Classic Proxy
* Tunnel Proxy
* Chain Proxy
* KeepAlive




To do:
-------------
* HEAD Verb before GET to persist tcp connection
* SOCKS 4 Proxy
* SOCKS 5 Proxy
* HTTP 2.0 implementation
