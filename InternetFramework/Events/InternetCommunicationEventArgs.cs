using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    public class InternetCommunicationEventArgs : InternetServerEventArgs
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
        /// Transmitted message
        /// </summary>
        public byte[] Message { get; set; }
    }
}
