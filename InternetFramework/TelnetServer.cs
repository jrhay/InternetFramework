using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework
{
    public class TelnetServer : BufferedTCPServer
    {
        /// <summary>
        /// CRLF end-of-line control character for Telnet
        /// </summary>
        static byte[] CRLF = { 0x0D, 0x0A };

        #region Lifecycle

        /// <summary>
        /// Create a new Telnet server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public TelnetServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        {
            EndOfLine = CRLF;
        }

        /// <summary>
        /// Create a new Telnet server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public TelnetServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddress, Port, Protocol)
        {
            this.EndOfLine = CRLF;
        }

        #endregion

    }
}
