using System;
using System.Net;

namespace InternetFramework
{
    /// <summary>
    /// Interface for a general Internet Protocol server or client process
    /// </summary>
    public interface INetworkNode
    {
        /// <summary>
        /// Node IP address (for IPv4 or IPv6 Addresses)
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// Protocol used for communication to the node
        /// </summary>
        RFCProtocol Protocol { get; }

        /// <summary>
        /// Port number
        /// </summary>
        UInt16 Port { get; }
    }
}
