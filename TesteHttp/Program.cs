using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ErysDownloader;

namespace TesteHttp
{
    class Program
    {
        public static string ReadLineText(System.IO.Stream session)
        {
            var line = ReadLineBinary(session);
            return line == null ? null : Encoding.ASCII.GetString(line);
        }

        public static byte[] ReadLineBinary(System.IO.Stream session)
        {
            var lineBuffer = new List<byte>();

            while (true)
            {
                int b = session.ReadByte();
                if (b == -1) return null;
                if (b == 10) break;
                if (b != 13) lineBuffer.Add((byte)b);
            }

            return lineBuffer.ToArray();
        }
        static void Main(string[] args)
        {

            //using (var wc = new WebClient())
            //{
            //    //wc.Proxy = new WebProxy("127.0.0.1", 8888);//192.168.0.74   8889
            //    wc.Proxy.Credentials = new NetworkCredential("alex", "123456");
            //    Console.WriteLine(wc.DownloadString("http://meuip.datahouse.com.br/").Length);
            //    Console.WriteLine("cabo.");
            //    Console.ReadKey();
            //    return;
            //}


            //var socket = new TcpClient("192.168.0.74", 8889);
            //var stream = socket.GetStream();

            //var lineTerminator = new byte[] { 13, 10 };


            //IEnumerable<string> commandLines = new[]
            //    {
            //        "GET http://meuip.datahouse.com.br/ HTTP/1.1"
            //        ,"Proxy-Authorization: Basic YWxleDoxMjM0NTY="
            //        , "Host: meuip.datahouse.com.br"
            //        , "Proxy-Connection: Keep-Alive"
            //    };

            //commandLines = new[] { "GET http://meuip.datahouse.com.br/ HTTP/1.1\r\nProxy-Authorization: Basic YWxleDoxMjM0NTY=\r\nHost: meuip.datahouse.com.br\r\nProxy-Connection: Keep-Alive\r\n\r\n" };
            ///*stream.Write(lineTerminator, 0, lineTerminator.Length);
            //for (int i = 0; i < 15; i++)
            //{
            //    Console.WriteLine(ReadLineText(stream));
            //}*/

            //foreach (string r in commandLines)
            //{
            //    var data = Encoding.ASCII.GetBytes(r);
            //    stream.Write(data, 0, data.Length);
            //    //stream.Write(lineTerminator, 0, lineTerminator.Length);
            //}
            ////stream.Write(lineTerminator, 0, lineTerminator.Length);


            //for (int i = 0; i < 20; i++)
            //{
            //    Console.WriteLine(ReadLineText(stream));
            //}

            //Console.ReadKey();


            //return;

            var tunnel0 = new Tunnel();
            tunnel0.Host = "82.99.209.34";
            tunnel0.Port = 80;

            var tunnel = new Tunnel();
            tunnel.Host = "187.72.145.54";
            tunnel.Port = 8080;

            var proxy = new ClassicProxy();
            proxy.Host = "10.2.0.1";
            proxy.Port = 3128;

            //proxy.Host = tunnel0.Host;
            //proxy.Port = tunnel0.Port;
            //tunnel.Host = proxy.Host;
            //tunnel.Port = proxy.Port;

            //proxy.SendRequest(new Request() {});

            proxy.Credencial = new Credencial() { UserName = "A06130887", Password = Encoding.ASCII.GetString(Convert.FromBase64String("TDc4OA==")) };
            //tunnel.Credencial = proxy.Credencial;

            HttpRequest target = new SocketDownloader();

            var uri = new Uri("http://www.tecmundo.com.br/");
            //var uri = new Uri("http://meuip.datahouse.com.br/");
            //var uri = new Uri("http://seraqueganho.com.br/");


            IEnumerable<string> request = new[] { "Connection: Keep-Alive", "Accept-Encoding: gzip, deflate" };
            const string expected = "/html";
            Response actual = null;
            //try {

                actual = target.Execute(uri, request);
                //target.Request.IsConnected = false;

            //}
            //catch (Exception ex) {

            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine(ex.StackTrace);
            //    Console.ReadKey();
            //    return;
            //}
                if(!string.IsNullOrWhiteSpace(actual.Content))Console.WriteLine(actual.Content.Contains(tunnel.Host));
                    //Console.WriteLine(actual.Content);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }

            uri = new Uri("http://www.tecmundo.com.br/seguranca");
            actual = target.Execute(uri, request);
            //target.Request.IsConnected = false;

            Console.WriteLine();
            Console.WriteLine(actual.Content.Contains(tunnel.Host));
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }

            uri = new Uri("http://www.tecmundo.com.br/memoria/30536-jedec-anuncia-as-especificacoes-definitivas-do-padrao-ddr4.htm");
            actual = target.Execute(uri, request);


            Console.WriteLine();
            Console.WriteLine(actual.Content.Contains(tunnel.Host));
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }

            Console.ReadKey();
        }
    }
}
