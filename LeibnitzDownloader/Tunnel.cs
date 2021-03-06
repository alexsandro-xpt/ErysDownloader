﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace ErysDownloader
{
    public class Tunnel : ProxyHandle, IChannel
    {
        public Uri Url { get; set; }
        public ITransport Transport { get; set; }

        public virtual Response SendRequest(Request request)
        {

            IPAddress ip = Dns.GetHostEntry(request.Url.Host).AddressList[0];
            //var ip = request.Url;

            var myOutgoingCredencial = Credencial;

            if (base.Sucessor != null && !request.IsConnected)
            {
                // Estória:
                // Você está na minha casa, você ou eu so sairemos apenas com minha chave.
                // Agora estamos na sua casa, eu so sairei apenas com sua chave.

                //var yourOutgoingCredencial = base.Sucessor.Credencial;

                //base.Sucessor.Credencial = myOutgoingCredencial;

                base.Sucessor.HandleConnect(request.Session, request.RequestHeader["User-Agent"], myOutgoingCredencial);

                var yourOutgoingCredencial = base.Sucessor.Credencial;

                this.Credencial = yourOutgoingCredencial;

            }


            IEnumerable<string> requestTunnelWebServer = new[]
                {
                    string.Format("CONNECT {0} HTTP/1.1", string.Format("{0}:{1}", ip, request.Url.Port))
                    , string.Format("Host: {0}", string.Format("{0}:{1}", ip, request.Url.Port))
                    , "Proxy-Connection: Keep-Alive"
                    , string.Format("User-Agent: {0}", request.RequestHeader["User-Agent"])
                };


            if (myOutgoingCredencial != null)
            {
                requestTunnelWebServer = requestTunnelWebServer.Concat(new[]
                    {
                        "Proxy-Authorization: Basic " +
                        Convert.ToBase64String(
                            ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}"
                                                                       , myOutgoingCredencial.UserName,
                                                                       myOutgoingCredencial.Password)))
                    });
            }

            if (!request.IsConnected)
            {
                Connect(request.Session, requestTunnelWebServer);
            }

            request.RequestHeader["Proxy-Connection"] = "Keep-Alive";

            request.IsConnected = true;

            return Transport.ExecuteRequest(request);
        }

        public Tunnel() { }

        public Tunnel(Tunnel chain)
        {
            Sucessor = chain;
        }

    }
}
