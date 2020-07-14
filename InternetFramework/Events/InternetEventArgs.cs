using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    /// <summary>
    /// General EventArgs for INetworkNode-generated events
    /// </summary>
    public class InternetEventArgs : EventArgs
    {
        /// <summary>
        /// Address of local side of connection. 
        /// For events generated from an IInternetServer-conforming class, this will always be the server class.
        /// For events generated from an IInternetClient-conforming class, this will always be the client class.
        /// </summary>
        public INetworkNode Local { get; set; }

        /// <summary>
        /// Address of remote side of connection
        /// </summary>
        public INetworkNode Remote { get; set; }
    }
}
