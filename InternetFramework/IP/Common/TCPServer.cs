using InternetFramework;
using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Server process that accepts TCP connections on a given IP interface and port in the background
    /// and raises events on client actions
    /// </summary>
    public class TCPServer<P> : BufferedInternetServer<P> where P:IInternetPacket, new()
    {
        #region Lifecycle

        /// <summary>
        /// Create a new TCP server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public TCPServer(UInt16 port = (UInt16)DefaultPorts.Telnet, RFCProtocol protocol = RFCProtocol.TCP) : base(new P(), port, protocol)
        {
        }

        /// <summary>
        /// Create a new TCP server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="address">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public TCPServer(IPAddress address, UInt16 port = (UInt16)DefaultPorts.Telnet, RFCProtocol protocol = RFCProtocol.TCP) : base(new P(), address, port, protocol)
        {
        }

        #endregion

        #region Base server overrides

        public override void CreateServer(IPAddress address, UInt16 port, RFCProtocol protocol)
        {
            base.CreateServer(address, port, protocol);
            this.Socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            base.Start();
            this.Socket.Listen(100);
            ListenForConnections();
        }

        #endregion

    }
}
