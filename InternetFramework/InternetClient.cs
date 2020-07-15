using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    public class InternetClient : IInternetClient
    {
        #region Instance Variables

        /// <summary>
        /// Socket instance used by this client
        /// </summary>
        public Socket Socket { get; internal set; } = null;

        /// <summary>
        /// Server that the client last connected to
        /// </summary>
        public INetworkNode Server { get; internal set; } = null;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Create a new client instance
        /// </summary>
        /// <param name="Port">Local port number to listen for connections</param>
        /// <param name="Protocol">Protocol to be used by this server</param>
        public InternetClient(UInt16 Port, RFCProtocol Protocol)
        {
            this.Port = Port;
            this.Protocol = Protocol;
            CreateClient();
        }

        ~InternetClient()
        {
            Dispose(false);
        }

        #endregion

        #region IInternetServer Interface

        public IPAddress Address { get; internal set; } = null;

        public RFCProtocol Protocol { get; internal set; } = RFCProtocol.Unknown;

        public ushort Port { get; internal set; } = 0;

        public bool IsConnected { get { return Socket == null ? false : Socket.Connected; } }

        public uint ReceiveBufferSize { get; set; } = 1024;


        public event EventHandler<InternetConnectionEventArgs> NewConnection;
        public event EventHandler<InternetConnectionEventArgs> RemoteDisconnected;
        public event EventHandler<InternetConnectionEventArgs> RemoteDisconnecting;
        public event EventHandler<InternetCommunicationEventArgs> IncomingMessage;
        public event EventHandler<InternetCommunicationEventArgs> OutgoingMessage;
        public event EventHandler<InternetBytesTransferredEventArgs> BytesSent;
        public event EventHandler<InternetBytesTransferredEventArgs> BytesReceived;

        /// <summary>
        /// Attempt to connect to a server at the specified address.
        /// Base method calls Socket's Connect(str, int) method using the configured port number and invokes OnNewConnection()
        /// </summary>
        /// <param name="host">DNS hostname to connect to or IPv4/IPv6 address string</param>
        public virtual void Connect(string host)
        {
            this.Socket.Connect(host, this.Port);
            if (this.Socket.RemoteEndPoint != null)
            {
                IPEndPoint remoteEndpoint = this.Socket.RemoteEndPoint as IPEndPoint;
                Server = new NetworkNode(remoteEndpoint.Address, this.Protocol, (ushort)remoteEndpoint.Port, this.Socket);
                OnNewConnection(Server);
            }
        }

        /// <summary>
        /// Attempt to connect to a server at the specified address.
        /// Base method calls Socket's Connect(str, int) method using the configured port number and invokes OnNewConnection()
        /// </summary>
        /// <param name="host">DNS hostname to connect to or IPv4/IPv6 address string</param>
        public virtual async Task ConnectAsync(string host)
        {
            await Task.Run(() => { Connect(host); });
        }

        /// <summary>
        /// Disconnect from the server, flushing all transmit and receive buffers
        /// </summary>
        public virtual void Disconnect()
        {
            try
            {
                if (IsConnected)
                {
                    OnRemoteDisconnecting(Server);
                    if (Server != null)
                        Server.Socket.Shutdown(SocketShutdown.Both);
                    this.Socket.Shutdown(SocketShutdown.Both);

                    if (Server != null)
                        Server.Socket.Disconnect(false);

                    this.Socket.Close();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Disconnect from the server, flushing all transmit and receive buffers
        /// </summary>
        public virtual async Task DisconnectAsync()
        {
            await Task.Run(() => { Disconnect(); });
        }

        public virtual void Send(byte[] Message)
        {
            this.OnOutgoingMessage((Server != null) ? Server : this, Message);
            Socket outSocket = (Server == null) ? Socket : Server.Socket;
            int bytesSent = outSocket.Send(Message);
            if (bytesSent > 0)
                this.OnBytesSent((Server != null) ? Server : this, bytesSent);
        }

        public async Task SendAsync(byte[] Message)
        {
            await Task.Run(() => { Send(Message); });
        }

        public void Send(string Message)
        {
            Send(UTF8Encoding.UTF8.GetBytes(Message));
        }

        public async Task SendAsync(string Message)
        {
            await Task.Run(() => { Send(Message); });
        }

        public async Task ListenForMessagesAsync()
        {
            await Task.Run(() => {
                ListenForMessages();
            });
        }

        public void ListenForMessages()
        {
            byte[] RecvBuffer = new byte[ReceiveBufferSize];
            while (IsConnected)
            {
                try
                {
                    int bytesRead = Socket.Receive(RecvBuffer, SocketFlags.None);
                    if (bytesRead <= 0)
                    {
                        CloseRemote(Server);
                    }
                    else
                    {
                        this.OnBytesReceived(Server, bytesRead);

                        byte[] Data = new byte[bytesRead];
                        Buffer.BlockCopy(RecvBuffer, 0, Data, 0, bytesRead);

                        this.OnIncomingMessage(Server, Data);
                    }
                }
                catch (SocketException)
                {
                    // If we catch a socket exception, it's probably a fatal error or the socket is disconnected; abort the connection
                    CloseRemote(Server);
                }
            }
        }



        #endregion

        #region Abstract Methods to Override

        /// <summary>
        /// Create a new socket for this client.  Base definition does nothing.
        /// </summary>
        public virtual void CreateClient()
        {
        }

        /// <summary>
        /// Close an existing connected socket.
        /// Derived methods should call the base method at the end of their operation
        /// to invoke the RemoteDisconnected event and perform final cleanup.
        /// </summary>
        public virtual void CloseRemote(INetworkNode Remote)
        {
            this.OnRemoteDisconnected(Remote);
        }


        #endregion

        #region Event Handling

        virtual internal void OnNewConnection(INetworkNode Remote)
        {
            if (NewConnection != null)
                Task.Run(() =>
                {
                    NewConnection.Invoke(this, new InternetConnectionEventArgs
                    {
                        Local = this,
                        Remote = Remote
                    });
                });
        }

        virtual internal void OnRemoteDisconnecting(INetworkNode Remote)
        {
            if ((RemoteDisconnecting == null) || (Remote == null))
                return;

            Task.Run(() =>
            {
                RemoteDisconnecting?.Invoke(this, new InternetConnectionEventArgs
                {
                    Local = this,
                    Remote = Remote
                });
            });
        }

        virtual internal void OnRemoteDisconnected(INetworkNode Remote)
        {
            if (Remote == null)
                return;

            if (RemoteDisconnected != null)
                Task.Run(() =>
                {
                    RemoteDisconnected.Invoke(this, new InternetConnectionEventArgs
                    {
                        Local = this,
                        Remote = Remote
                    });
                });

            if ((Server != null) && (Remote.Socket == Server.Socket))
            {
                Server = null;
            }
        }

        private void OnOutgoingMessage(INetworkNode To, byte[] NewMessage)
        {
            if ((OutgoingMessage == null) || (To == null) || (NewMessage == null) || (NewMessage.Length == 0))
                return;

            Task.Run(() =>
            {
                OutgoingMessage.Invoke(this, new InternetCommunicationEventArgs
                {
                    Remote = To,
                    Local = this,
                    Direction = CommunicationDirection.Outbound,
                    Message = NewMessage
                });
            });
        }

        private void OnBytesSent(INetworkNode To, int NumberOfBytes)
        {
            if ((BytesSent == null) || (To == null) || (NumberOfBytes <= 0))
                return;

            Task.Run(() =>
            {
                BytesSent.Invoke(this, new InternetBytesTransferredEventArgs
                {
                    Remote = To,
                    Local = this,
                    Direction = CommunicationDirection.Outbound,
                    NumBytes = NumberOfBytes
                });
            });
        }

        internal virtual void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            if ((IncomingMessage == null) || (From == null) || (NewMessage == null) || (NewMessage.Length == 0))
                return;

            Task.Run(() =>
            {
                IncomingMessage.Invoke(this, new InternetCommunicationEventArgs
                {
                    Remote = From,
                    Local = this,
                    Direction = CommunicationDirection.Inbound,
                    Message = NewMessage
                });
            });
        }

        internal virtual void OnBytesReceived(INetworkNode From, int NumberOfBytes)
        {
            if ((BytesReceived == null) || (From == null) || (NumberOfBytes <= 0))
                return;

            Task.Run(() =>
            {
                BytesReceived.Invoke(this, new InternetBytesTransferredEventArgs
                {
                    Remote = From,
                    Local = this,
                    Direction = CommunicationDirection.Inbound,
                    NumBytes = NumberOfBytes
                });
            });
        }

        #endregion

        #region IDisposable Interface

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Disconnect();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~InternetServer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
