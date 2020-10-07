using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Base internet client for packetized data
    /// </summary>
    public class BufferedInternetClient<T> : InternetClient where T: IInternetPacket 
    {
        internal InternetBuffer<T> Buffer;

        /// <summary>
        /// Type of packetized data being buffered
        /// </summary>
        public T PacketType { get; internal set; }

        #region Lifecycle

        /// <summary>
        /// Create a new client instance
        /// </summary>
        /// <param name="BufferType">Instance to indicate the buffer type</param>
        /// <param name="Port">Local port number to listen for connections</param>
        /// <param name="Protocol">Protocol to be used by this server</param>
        public BufferedInternetClient(T BufferType, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base()
        {
            PacketType = BufferType;
            InitBuffer();
            CreateClient(Port, Protocol);
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
            base.OnIncomingMessage(this, e.Message);
        }

        #endregion
    }
}
