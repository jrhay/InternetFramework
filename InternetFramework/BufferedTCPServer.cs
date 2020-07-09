using InternetFramework.Extensions;
using InternetFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Base TCP server that blocks data sent 
    /// </summary>
    public class BufferedTCPServer : TCPServer, IBufferedServer
    {

        #region IBufferedServer Interface

        /// <summary>
        /// End of line indicator (default to CRLF)
        /// </summary>
        public byte[] EndOfLine { get; set; } = { 0x0D, 0x0A };

        public byte[] Trim(byte[] Message)
        {
            int Index = Message.Length - 1;
            int EOLIndex = EndOfLine.Length - 1;
            while ((Index >= 0) && (EOLIndex >= 0) && (Message[Index] == EndOfLine[EOLIndex]))
            {
                Index--;
                EOLIndex--;
            }

            if (EOLIndex < 0)
            {
                byte[] NewMessage = new byte[Index+1];
                Buffer.BlockCopy(Message, 0, NewMessage, 0, Index+1);
                return NewMessage;
            }
            else
                return Message;
        }

        public void SendLine(INetworkNode Remote, byte[] DataLine)
        {
            this.Send(Remote, DataLine);
            this.Send(Remote, EndOfLine);
        }

        public async Task SendLineAsync(INetworkNode Remote, byte[] DataLine)
        {
            await Task.Run(() => SendLine(Remote, DataLine));
        }

        public void SendLine(INetworkNode Remote, string DataLine)
        {
            this.Send(Remote, DataLine);
            this.Send(Remote, EndOfLine);
        }

        public async Task SendLineAsync(INetworkNode Remote, string DataLine)
        {
            await Task.Run(() => SendLine(Remote, DataLine));
        }


        #endregion

        #region Lifecycle

        /// <summary>
        /// Create a new TCP server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedTCPServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        { }

        /// <summary>
        /// Create a new TCP server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedTCPServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddress, Port, Protocol)
        { }

        #endregion

        #region Instance Variables

        internal Dictionary<INetworkNode, List<byte>> IncomingMessages = new Dictionary<INetworkNode, List<byte>>();

        #endregion

        #region Overridden Event Handling

        private IEnumerable<int> FindEndsOfLine(byte[] bytes, byte[] EndOfLine)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes.Skip(i).Take(EndOfLine.Length).SequenceEqual(EndOfLine))
                    yield return i;
            }
        }

        internal override void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            // Add incoming message to buffered message
            if (IncomingMessages.ContainsKey(From))
                IncomingMessages[From].AddRange(NewMessage);
            else
                IncomingMessages.Add(From, new List<byte>(NewMessage));

            // If buffered message has an end-of-line terminator, send the line(s) to any listeners
            byte[] IncomingMessage = IncomingMessages[From].ToArray();
            int MessageStart = 0;
            IEnumerable<int> LinePositions = FindEndsOfLine(IncomingMessage, EndOfLine);
            if ((LinePositions != null) && (LinePositions.Count() > 0))
            {
                foreach (int LineIndex in LinePositions)
                {
                    int MessageLength = LineIndex + EndOfLine.Length;
                    byte[] Message = new byte[MessageLength];

                    Buffer.BlockCopy(IncomingMessage, MessageStart, Message, 0, MessageLength);
                    base.OnIncomingMessage(From, Message);

                    MessageStart += MessageLength;
                }

                // Preserve any remaining incoming message we may have
                if ((MessageStart > 0) && (MessageStart < IncomingMessage.Length))
                {
                    byte[] Remaining = new byte[IncomingMessage.Length - MessageStart + 1];
                    Buffer.BlockCopy(IncomingMessage, MessageStart, Remaining, 0, Remaining.Length);
                    IncomingMessages[From] = new List<byte>(Remaining);
                }
                else
                    IncomingMessages.Remove(From);
            }
        }

        #endregion
    }
}
