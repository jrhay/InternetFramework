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
    public class BufferedTCPServer : TCPServer, IBufferedServer
    {
        private InternetBuffer Buffer = null;

        public byte[] EndOfLine { get { return Buffer.EndOfLine; } set { Buffer.EndOfLine = value; } }

        public byte[] Trim(byte[] Message) { return Buffer.Trim(Message); }

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


        #region Lifecycle

        /// <summary>
        /// Create a new TCP server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedTCPServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        {
            InitBuffer();
        }

        /// <summary>
        /// Create a new TCP server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: TCP (RFC793)</param>
        public BufferedTCPServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.TCP) : base(IPAddress, Port, Protocol)
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
            base.OnIncomingMessage(e.Remote, e.Message);
        }

        #endregion
    }
}
