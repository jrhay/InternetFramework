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
        /// <param name="Port">Local port number to listen for connections</param>
        /// <param name="Protocol">Protocol to be used by this server</param>
        public TCPClient(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(Port, Protocol)
        {
        }

        #endregion

        #region Base server overrides

        public override void CreateClient()
        {
            base.CreateClient();
            this.Socket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        internal override void OnNewConnection(INetworkNode Remote)
        {
            base.OnNewConnection(Remote);
            _ = ListenForMessagesAsync();
        }

        #endregion
    }
}
