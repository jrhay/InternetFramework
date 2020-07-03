using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    public class InternetBytesTransferredEventArgs : InternetServerEventArgs
    {
        /// <summary>
        /// Address of remote side of communication
        /// </summary>
        public Socket Remote { get; set; }

        /// <summary>
        /// Direction of communication
        /// </summary>
        public CommunicationDirection Direction { get; set; }

        /// <summary>
        /// Number of bytes sent to or received from the socket
        /// </summary>
        public int NumBytes { get; set; }
    }
}
