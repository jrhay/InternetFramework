using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    /// <summary>
    /// Arguments for events generated when data transfer is detected
    /// </summary>
    public class InternetBytesTransferredEventArgs : InternetEventArgs
    {
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
