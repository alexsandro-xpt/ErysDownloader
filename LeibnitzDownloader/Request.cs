﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ErysDownloader.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErysDownloader
{
    public class Request
    {
        public virtual System.IO.Stream Session
        {
            get;
            set;
        }

        public virtual Uri Url
        {
            get;
            set;
        }

        public virtual Dictionary<string, string> RequestHeader
        {
            get;
            set;
        }

        /// <summary>
        /// Flag evitar reconectar em proxy e demais chains de proxys uma vez que ja foi conectado.
        /// </summary>
        public virtual bool IsConnected
        {
            get;
            set;
        }

    }

}