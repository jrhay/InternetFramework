using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    /// <summary>
    /// Arguments for events generated when data transfer is detected
    /// </summary>
    public class InternetCommunicationEventArgs : InternetEventArgs
    {
        /// <summary>
        /// Direction of communication
        /// </summary>
        public CommunicationDirection Direction { get; set; }

        /// <summary>
        /// Transmitted message
        /// </summary>
        public byte[] Message { get; set; }
    }
}
