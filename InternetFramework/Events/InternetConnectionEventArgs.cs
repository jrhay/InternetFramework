using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    public class InternetConnectionEventArgs : InternetServerEventArgs
    {
        /// <summary>
        /// Address of remote side of connection
        /// </summary>
        public INetworkNode Remote { get; set; }
    }
}
