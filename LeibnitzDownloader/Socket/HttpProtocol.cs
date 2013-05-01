﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.IO;
using System.Net.Security;
using System.Linq;
using System.Security.Authentication;

namespace ErysDownloader.Socket
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	internal class HttpProtocol : ITransport
	{
		public virtual Response ExecuteRequest(Request request) {
		    var basicRequest = new List<string>
                                   {
                                       "GET " + request.Url/*.PathAndQuery*/ + " HTTP/1.1"
                                       , "Host: " + request.Url.DnsSafeHost
                                   };

		    
            if(request.RequestHeader.Keys.Contains("get"))
            {
                basicRequest[0] = request.RequestHeader["get"];
                request.RequestHeader.Remove("get");
            }

            foreach (var header in request.RequestHeader)
            {
                var reqHeader = string.Format("{0}: {1}", header.Key, header.Value);
                basicRequest.Add(reqHeader);
            }

            if (request.Url.Scheme == "https")
            {
                //http://stackoverflow.com/questions/7216390/weird-tcpclient-https-post-response
                var sslStream = new SslStream(request.Session); //, true
                sslStream.AuthenticateAsClient(request.Url.Host, null, SslProtocols.Tls, false);
                request.Session = sslStream;
            }


            // Escreve no stream a requisição HTTP GET.
		    StreamHandle.WriteLine(request.Session, basicRequest);

		    ResponseHeaders responseHeaders = GetResponseHeader(request.Session);

		    var response = new Response();
		    response.Headers = responseHeaders;


            if (response.Headers.IsValid())
            {

                //byte[] buffer = new byte[0];// StreamHandle.GetBuffer(request.Session, response.Headers.TransferEncoding);

                //StreamHandle.LoadBuffer(request.Session, buffer);

                var responseHtml = GetResponseHtml(request.Session, response.Headers.TransferEncoding, response.Headers.ContentLength);

                MemoryStream outputStream = new MemoryStream();

                var outputDecorator = StreamHandle.OutputDecorator(outputStream, response.Headers.ContentEncoding, response.Headers.CharSetEncoder, responseHtml.ToArray());

                outputStream.Position = 0;

                response.Content = outputDecorator.ReadToEnd();

            }

            if(response.Headers.Keys.Contains("set-cookie"))
            {
                request.RequestHeader["cookie"] = response.Headers["set-cookie"];
            }
            if (response.Headers.Keys.Contains("connection") && response.Headers["connection"].ToLower().Contains("close"))
            {
                request.IsConnected = false;
            }


		    return response;
		}

	    private ResponseHeaders GetResponseHeader(System.IO.Stream session)
		{
            var headers = new ResponseHeaders();
            var line = StreamHandle.ReadLineText(session);

            if (string.IsNullOrEmpty(line)) line = StreamHandle.ReadLineText(session);

            for (; !string.IsNullOrEmpty(line); line = StreamHandle.ReadLineText(session))
            {
                if (line.Contains(':'))
                {
                    var split = line.Split(new[] { ':' }, 2);
                    var k = split[0].Trim().ToLower();

                    if (!headers.Keys.Contains(k))
                    {
                        headers.Add(k, split[1].Trim());
                    }
                    else if (k != "status")
                    {
                        headers[k] += "," + split[1].Trim();
                    }
                }
                else if (line.Length >= 12)
                {
                    headers.Add("status", line.Substring(9, 3));
                }
            }

            return headers;
		}


        private List<byte> GetResponseHtml(System.IO.Stream session, bool hasTransferEncoding, int resourceLength)
        {
            var dataLoaded = new List<byte>();
            var bytesRead = 0;
            int bytesToLoad = resourceLength;

            do
            {

                byte[] buffer = StreamHandle.GetBuffer(session, hasTransferEncoding, ref bytesToLoad);
                
                bytesRead = StreamHandle.LoadBuffer(session, buffer);
                dataLoaded.AddRange(buffer);

                if(!hasTransferEncoding) bytesToLoad = resourceLength - dataLoaded.Count;

                //Console.WriteLine(bytesToLoad);

            } while (bytesRead > 0 && bytesToLoad > 0);


            //dataLoaded.AddRange(buffer);
            //Console.WriteLine(dataLoaded.Count);

            /*if (bytesRead > 0)
            {
                dataLoaded.AddRange(GetResponseHtml(session, hasTransferEncoding, resourceLength - buffer.Length));
            }*/

            return dataLoaded;
        }

	}
}

