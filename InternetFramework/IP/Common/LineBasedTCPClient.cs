using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Client that connects to a TCP server on a given IP interface and port in the background
    /// and raises events on server actions, sending a receiving lines of text deliminated by CRLF 
    /// (or, optionally other end-of-line byte sequences)
    /// </summary>
    public class LineBasedTCPClient : TCPClient<LineBasedProtocol>, ILineBasedClient
    {
        #region Lifecycle

        /// <summary>
        /// Create a new client instance 
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public LineBasedTCPClient(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(Port, Protocol)
        {
        }

        #endregion

        #region Base client overrides

        public override void Send(string Message)
        {
            base.Send(PacketType.StringToMessage(Message));
        }

        #endregion

        #region ILineBasedClient Protocol

        public void SendLine(byte[] DataLine)
        {
            byte[] Line = new byte[DataLine.Length + PacketType.EndOfLine.Length];
            System.Buffer.BlockCopy(DataLine, 0, Line, 0, DataLine.Length);
            System.Buffer.BlockCopy(PacketType.EndOfLine, 0, Line, DataLine.Length, PacketType.EndOfLine.Length);
            this.Send(Line);
        }

        public async Task SendLineAsync(byte[] DataLine)
        {
            await Task.Run(() => SendLine(DataLine));
        }

        public void SendLine(string DataLine)
        {
            this.Send(DataLine);
            this.Send(PacketType.EndOfLine);
        }

        public async Task SendLineAsync(string DataLine)
        {
            await Task.Run(() => SendLine(DataLine));
        }

        #endregion
    }
}
