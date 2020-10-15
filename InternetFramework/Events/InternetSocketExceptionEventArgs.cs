using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    /// <summary>
    /// Arguments for events generated when a socket exception is captured.
    /// </summary>
    public class InternetSocketExceptionEventArgs : InternetEventArgs
    {
        /// <summary>
        /// The socket exception that triggered this event
        /// </summary>
        public SocketException SocketException { get; set; }
    }
}
