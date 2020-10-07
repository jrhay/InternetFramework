using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Server process that accepts TCP connections on a given IP interface and port in the background
    /// and raises events on client actions, sending a receiving lines of text deliminated by CRLF 
    /// (or, optionally other end-of-line byte sequences)
    /// </summary>

    /// </summary>
    public class LineBasedTCPServer : TCPServer<LineBasedProtocol>, ILineBasedServer
    {
        #region Lifecycle

        /// <summary>
        /// Create a new server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public LineBasedTCPServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        {
        }

        /// <summary>
        /// Create a new server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public LineBasedTCPServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddress, Port, Protocol)
        {
        }

        #endregion

        #region Base server overrides

        public override void Send(INetworkNode Remote, string Message)
        {
            base.Send(Remote, PacketType.StringToMessage(Message));
        }

        #endregion

        #region ILineBasedServer Interface

        public void SendLine(INetworkNode Remote, byte[] DataLine)
        {
            byte[] Line = new byte[DataLine.Length + PacketType.EndOfLine.Length];
            System.Buffer.BlockCopy(DataLine, 0, Line, 0, DataLine.Length);
            System.Buffer.BlockCopy(PacketType.EndOfLine, 0, Line, DataLine.Length, PacketType.EndOfLine.Length);
            this.Send(Remote, Line);
        }

        public async Task SendLineAsync(INetworkNode Remote, byte[] DataLine)
        {
            await Task.Run(() => SendLine(Remote, DataLine));
        }

        public void SendLine(INetworkNode Remote, string DataLine)
        {
            this.SendLine(Remote, PacketType.StringToMessage(DataLine));
        }

        public async Task SendLineAsync(INetworkNode Remote, string DataLine)
        {
            await Task.Run(() => SendLine(Remote, DataLine));
        }

        public void SendLine(byte[] Message)
        {
            foreach (INetworkNode Remote in Remotes)
                SendLine(Remote, Message);
        }

        public async Task SendLineAsync(byte[] Message)
        {
            foreach (INetworkNode Remote in Remotes)
                await SendLineAsync(Remote, Message);
        }

        public void SendLine(string Message)
        {
            foreach (INetworkNode Remote in Remotes)
                SendLine(Remote, Message);
        }

        public async Task SendLineAsync(string Message)
        {
            foreach (INetworkNode Remote in Remotes)
                await SendLineAsync(Remote, Message);
        }

        #endregion

    }
}
