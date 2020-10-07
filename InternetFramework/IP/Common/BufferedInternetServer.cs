using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Base TCP server that blocks data sent 
    /// </summary>
    public class BufferedInternetServer<T> : InternetServer where T : IInternetPacket
    {
        internal InternetBuffer<T> Buffer;
        public T PacketType { get; internal set; }

        #region Lifecycle

        /// <summary>
        /// Create a new TCP server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="BufferType">Instance to indicate the buffer type</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedInternetServer(T BufferType, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base()
        {
            PacketType = BufferType;
            InitBuffer();
            CreateServer(IPAddressExtensions.LocalIPAddress(), Port, Protocol);
        }

        /// <summary>
        /// Create a new TCP server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="BufferType">Instance to indicate the buffer type</param>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedInternetServer(T BufferType, IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base()
        {
            PacketType = BufferType;
            InitBuffer();
            CreateServer(IPAddress, Port, Protocol);
        }

        private void InitBuffer()
        {
            Buffer = new InternetBuffer<T>(PacketType, this);
            Buffer.MessageReceived += Buffer_MessageReceived;
        }

        #endregion

        #region Message Receiving 

        internal override void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            Buffer.AddBytes(From, NewMessage);
        }

        private void Buffer_MessageReceived(object sender, Events.InternetCommunicationEventArgs e)
        {
            base.OnIncomingMessage(e.Remote, e.Message);
        }

        #endregion

    }
}
