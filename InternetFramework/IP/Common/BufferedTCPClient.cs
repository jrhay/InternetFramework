using InternetFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Base TCP server that blocks data sent 
    /// </summary>
    public class BufferedTCPClient : TCPClient, IBufferedClient
    {
        private InternetBuffer Buffer = null;

        public byte[] EndOfLine { get { return Buffer.EndOfLine; } set { Buffer.EndOfLine = value; } }

        public byte[] Trim(byte[] Message) { return Buffer.Trim(Message); }

        public void SendLine(byte[] DataLine)
        {
            this.Send(DataLine);
            this.Send(EndOfLine);
        }

        public async Task SendLineAsync(byte[] DataLine)
        {
            await Task.Run(() => SendLine(DataLine));
        }

        public void SendLine(string DataLine)
        {
            this.Send(DataLine);
            this.Send(EndOfLine);
        }

        public async Task SendLineAsync(string DataLine)
        {
            await Task.Run(() => SendLine(DataLine));
        }

        #region Lifecycle

        /// <summary>
        /// Create a new TCP client instance
        /// </summary>
        /// <param name="Port">Local port number to listen for connections</param>
        /// <param name="Protocol">Protocol to be used by this server</param>
        public BufferedTCPClient(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(Port, Protocol)
        {
            InitBuffer();
        }

        private void InitBuffer()
        {
            Buffer = new InternetBuffer(this);
            Buffer.MessageReceived += Buffer_MessageReceived;
        }

        #endregion

        #region Message Receiving 

        internal override void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            Buffer.AddMessage(From, NewMessage);
        }

        private void Buffer_MessageReceived(object sender, Events.InternetCommunicationEventArgs e)
        {
            base.OnIncomingMessage(this, e.Message);
        }

        #endregion
    }
}
