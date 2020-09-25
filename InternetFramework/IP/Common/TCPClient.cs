using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Client that connects to a TCP server on a given IP interface and port in the background
    /// and raises events on client actions
    /// </summary>
    public class TCPClient : InternetClient
    {
        #region Lifecycle

        /// <summary>
        /// Create a new TCP client instance
        /// </summary>
        /// <param name="port">Local port number to listen for connections</param>
        /// <param name="protocol">Protocol to be used by this server</param>
        public TCPClient(UInt16 port = (UInt16)DefaultPorts.Telnet, RFCProtocol protocol = RFCProtocol.TCP) : base()
        {
            CreateClient(port, protocol);
        }

        #endregion

        #region Base server overrides

        public override void CreateClient(UInt16 port, RFCProtocol protocol)
        {
            base.CreateClient(port, protocol);
            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Connect(string host)
        {
            base.Connect(host);
            ListenForMessages();
        }

        #endregion

    }
}
