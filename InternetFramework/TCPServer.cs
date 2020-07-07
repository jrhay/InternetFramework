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
    public class TCPServer : InternetServer, ITCPServer
    {
        #region ITCPServer Interface

        #endregion

        #region Lifecycle

        /// <summary>
        /// Create a new TCP server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public TCPServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        { }

        /// <summary>
        /// Create a new TCP server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public TCPServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddress, Port, Protocol)
        { }

        #endregion

        #region Base server overrides

        public override void CreateServer()
        {
            base.CreateServer();
            this.Socket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
