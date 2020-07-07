using System;
using System.Net;
using System.Net.Sockets;

namespace InternetFramework
{
    /// <summary>
    /// Interface for a general Internet Protocol server or client process
    /// </summary>
    public interface INetworkNode : IDisposable
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

        /// <summary>
        /// System.Net.Sockets socket instance for this node, if avaliable
        /// </summary>
        Socket Socket { get; }
    }
}
