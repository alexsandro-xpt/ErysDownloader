using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErysDownloader.Test {
    /// <summary>
    ///This is a test class for HttpDownloaderTest and is intended
    ///to contain all HttpDownloaderTest Unit Tests
    ///</summary>
    [TestClass]
    public class SocketDownloaderTest {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion


        [TestMethod]
        public void given_binary_url_return_empty()
        {
            var target = new SocketDownloader();
            var uri = new Uri("http://python.org/ftp/python/2.7.3/python-2.7.3.msi");
            IEnumerable<string> request = new[] { "Connection: Keep-Alive", "Accept-Encoding: gzip, deflate" };
            string expected = string.Empty;
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers) {
                Debug.WriteLine(cabecalho);
            }
            Assert.IsTrue(string.IsNullOrWhiteSpace(actual.Content));
        }

        /// <summary>
        /// Transferencia de conteudo com Transfer-Encoding: Chunked usando GZIP deve funcionar
        ///</summary>
        [TestMethod]
        public void seraqueganho_te_chunked_gzip_keep_alive_return_html() {
            var target = new SocketDownloader();
            var uri = new Uri("http://seraqueganho.com.br/");
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected),"Não retornou um HTML esperado");
            Assert.IsTrue(actual.Headers.ContainsValue("chunked") || actual.Headers.ContainsValue("Chunked"), "Não obteve chunked.");
            Assert.IsTrue(actual.Headers.ContainsValue("gzip") || actual.Headers.ContainsValue("Gzip"), "Não obteve gzip.");
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
        }


        /// <summary>
        /// Transferencia de conteudo com Transfer-Encoding: Chunked usando DEFLATE deve funcionar
        ///</summary>
        [TestMethod]
        public void seraqueganho_te_chunked_deflate_keep_alive_return_html() {
            var uri = new Uri("http://seraqueganho.com.br/");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected));
            Assert.IsTrue(actual.Headers.ContainsValue("chunked") || actual.Headers.ContainsValue("Chunked"), "Não obteve chunked.");
            Assert.IsTrue(actual.Headers.ContainsValue("deflate") || actual.Headers.ContainsValue("Deflate"), "Não obteve deflate.");
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
        }

        /// <summary>
        /// Transferencia de conteudo com Transfer-Encoding: Chunked usando DEFLATE deve funcionar
        ///</summary>
        [TestMethod]
        public void stevesouders_te_chunked_deflate_keep_alive_return_html() {
            var uri = new Uri("http://stevesouders.com/");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected));
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
            //Assert.IsTrue(actual.Headers.ContainsValue("deflate") || actual.Headers.ContainsValue("Deflate"), "Não obteve deflate.");
        }

        [TestMethod]
        public void theopenalgorithm_gzip_keep_alive_return_html() {
            var uri = new Uri("http://www.theopenalgorithm.com/");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected));
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
            //Assert.IsTrue(actual.Headers.ContainsValue("gzip") || actual.Headers.ContainsValue("Gzip"), "Não obteve gzip.");
        }

        /// <summary>
        /// Dato um site https usando proxy https, deve retornar um HTML
        ///</summary>
        [TestCategory("Proxy"), TestMethod]
        public void given_https_site_with_htts_proxy_request_gzip_keep_alive_return_html() {
            var uri = new Uri("https://gist.github.com/");
            var target = new SocketDownloader(new Tunnel() { Host = "187.72.145.54", Port = 8080 });
            //target.Proxy = new ProxyServer { Uri = "https://187.72.145.54:8080" };
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);

            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();

            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected));
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
            Assert.IsTrue(actual.Headers.ContainsValue("gzip") || actual.Headers.ContainsValue("Gzip"), "Não obteve gzip.");
        }


        [TestCategory("Proxy"), TestMethod]
        public void proxy_request_confirm_intenet_identity_proxy_ip()
        {
            var uri = new Uri("http://meuip.datahouse.com.br/");
            var target = new SocketDownloader(new Tunnel() { Host = "187.72.145.54", Port = 8080 });
            //target.Proxy = new ProxyServer { Uri = "https://187.72.145.54:8080" };
            IEnumerable<string> request = new[] { "Connection: Keep-Alive", "Accept-Encoding: gzip, deflate" };
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Debug.WriteLine(cabecalho);
            }
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains("187.72.145.54"));
            Assert.IsTrue(actual.Content.Contains(expected));
        }


        [TestMethod]
        public void given_webclient_class_response_should_return_same_html()
        {
            //var _uri = new Uri("http://www.google.com.br/search?&tbm=shop&q=zoom g2 1nu");
            //var target = new SocketDownloader();
            //IEnumerable<string> request = new[] { "Connection: Keep-Alive" };

            //var expected = "";

            //using(var wc = new WebClient())
            //{
            //    expected = wc.DownloadString(_uri);
            //}

            //target.UserAgent = string.Empty;

            //var actual = target.Execute(_uri, request);


            //Assert.AreEqual(expected.Length, actual.Content.Length, "O conteudo transferido não é o mesmo.");
        }

        [TestMethod]
        public void given_iso_output_charset_should_have_char()
        {
            var _uri = new Uri("http://www.google.com.br/search?&tbm=shop&q=zoom g2 1nu");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] { "Connection: Keep-Alive" };

            var expected = "Á";

            target.UserAgent = string.Empty;

            var actual = target.Execute(_uri, request);

            var html = actual.Content;

            foreach (var header in actual.Headers)
            {
                Console.WriteLine(header);
            }

            //var buffer = Encoding.ASCII.GetBytes(html);
            //var msg = Encoding.UTF8.GetString(buffer);

            //Encoding iso = Encoding.Default;// Encoding.GetEncoding("ISO-8859-1");
            //Encoding utf8 = Encoding.GetEncoding("ISO-8859-1");// new UTF8Encoding();// Encoding.UTF8;
            //byte[] utfBytes = iso.GetBytes(html);
            //byte[] isoBytes = Encoding.Convert(iso, utf8, utfBytes);
            //string msg = iso.GetString(isoBytes);

            var msg = html;// Encoding.UTF8.GetString(isoBytes);

            Console.WriteLine(msg);


            Assert.IsTrue(msg.Contains(expected) || msg.Contains("á") || msg.Contains("ç"));
        }

        [TestMethod]
        public void given_https_site_request_gzip_keep_alive_return_html() {
            var _uri = new Uri("https://www.bcash.com.br/");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(_uri, request);

            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();

            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains(expected));
            Assert.IsTrue(actual.Headers.ContainsValue("keep-alive") || actual.Headers.ContainsValue("Keep-Alive"), "Não obteve keep-alive.");
            Assert.IsTrue(actual.Headers.ContainsValue("gzip") || actual.Headers.ContainsValue("Gzip"), "Não obteve gzip.");
        }

        [TestCategory("Chain"), TestCategory("Proxy"), TestMethod, Timeout(10 * 60 * 1000)]
        public void chain_proxy_request_return_html() {
            var uri = new Uri("http://meuip.datahouse.com.br/");
            var target = new SocketDownloader(new Tunnel(new Tunnel() { Host = "187.72.145.54", Port = 8080 }) { Host = "82.99.209.34", Port = 80 });
            //target.Proxy = new ProxyServer(new ProxyServer { Uri = "https://187.72.145.54:8080" }) { Uri = "https://152.92.137.2:3128" };
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            foreach (var cabecalho in actual.Headers)
            {
                Console.WriteLine(cabecalho);
            }
            Console.WriteLine();
            Console.WriteLine(actual.Content);
            Assert.IsTrue(actual.Content.Contains("187.72.145.54"));
            Assert.IsTrue(actual.Content.Contains(expected));
        }

        [TestMethod]
        public void keep_alive_request_should_persist_connection_return_html() {
            var uri = new Uri("http://seraqueganho.com.br/");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] {"Connection: Keep-Alive", "Accept-Encoding: gzip, deflate"};
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            Assert.IsTrue(actual.Content.Contains(expected));

            foreach (var url in (new[]
                                        {
                                            "http://seraqueganho.com.br/usuario/novo"
                                            , "http://seraqueganho.com.br/sobre-nos"
                                            , "http://seraqueganho.com.br/usuario/login"
                                            , "http://seraqueganho.com.br/numeros/mais-sorteados"
                                            , "http://seraqueganho.com.br/numeros/gerar-numeros-aleatorios"
                                        })) {
                actual = target.Execute(new Uri(url), request/*, actual.Server*/);
                Assert.IsTrue(actual.Content.Contains(expected));
            }
        }

        [TestMethod]
        public void keep_alive_request_should_persist_connection_return_html2()
        {
            var uri = new Uri("http://localhost/teste.html");
            var target = new SocketDownloader();
            IEnumerable<string> request = new[] { "Connection: Keep-Alive", "Accept-Encoding: gzip, deflate" };
            const string expected = "/html";
            var actual = target.Execute(uri, request);
            Assert.IsTrue(actual.Content.Contains(expected));

            foreach (var cabecalho in actual.Headers) {
                Debug.WriteLine(cabecalho);
            }

            foreach (var url in (new[]
                                        {
                                            "http://localhost/teste2.php"
                                            , "http://localhost/teste3.html"
                                            , "http://localhost/brModelo.exe" // Arquivo binario no meio da lista não pode persistir uma conexão. Por que tem que descontar da stream e pular todo o arquivo para prosseguir em outra stream.
                                            //Falta teste pra isto.
                                            
                                        })) {
                actual = target.Execute(new Uri(url), request/*, actual.Server*/);

                Debug.WriteLine("\r\n----------");
                foreach (var cabecalho in actual.Headers)
                {
                    Debug.WriteLine(cabecalho);
                }

                int status;
                Assert.IsTrue(Int32.TryParse(actual.Headers["status"], out status));
            }
        }
    }
}