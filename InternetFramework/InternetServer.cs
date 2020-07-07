using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Base class for a generic internet server implementation
    /// </summary>
    public class InternetServer : IInternetServer
    {
        #region Instance Variables

        /// <summary>
        /// Socket instance used by this server
        /// </summary>
        public Socket Socket { get; internal set; } = null;

        /// <summary>
        /// List of remote sockets currently connected
        /// </summary>
        internal List<INetworkNode> Remotes { get; } = new List<INetworkNode>();

        #endregion

        #region Lifecycle

        /// <summary>
        /// Create a new server instance
        /// </summary>
        /// <param name="Address">Local IP Address to bind the server to</param>
        /// <param name="Port">Local port number to listen for connections</param>
        /// <param name="Protocol">Protocol to be used by this server</param>
        public InternetServer(IPAddress Address, UInt16 Port, RFCProtocol Protocol)
        {
            this.Address = Address;
            this.Port = Port;
            this.Protocol = Protocol;
            CreateServer();
        }


        /// <summary>
        /// Disconnect all remote nodes
        /// </summary>
        internal void CloseAllRemotes()
        {
            List<INetworkNode> AllRemotes = new List<INetworkNode>(Remotes);
            foreach (INetworkNode Remote in AllRemotes)
                this.Disconnect(Remote);
        }

        ~InternetServer()
        {
            Dispose(false);
        }

        #endregion

        #region IInternetServer Interface

        private Boolean _IsRunning = false;
        public Boolean IsRunning { 
            get { return _IsRunning; }
            internal set
            {
                _IsRunning = value;
                if (value == true)
                    OnServerStarted();
                else
                    OnServerStopped();
            }
        }

        public uint ReceiveBufferSize { get; set; } = 1024;

        public IPAddress Address { get; private set; }

        public RFCProtocol Protocol { get; private set; }

        public UInt16 Port { get; private set; }

        public event EventHandler<InternetServerEventArgs> ServerStarted;

        public event EventHandler<InternetServerEventArgs> ServerStopping;

        public event EventHandler<InternetServerEventArgs> ServerStopped;

        public event EventHandler<InternetServerEventArgs> ServerShuttingDown;

        public event EventHandler<InternetCommunicationEventArgs> OutgoingMessage;

        public event EventHandler<InternetCommunicationEventArgs> IncomingMessage;

        public event EventHandler<InternetConnectionEventArgs> NewConnection;

        public event EventHandler<InternetConnectionEventArgs> RemoteDisconnecting;

        public event EventHandler<InternetConnectionEventArgs> RemoteDisconnected;

        public event EventHandler<InternetBytesTransferredEventArgs> BytesSent;

        public event EventHandler<InternetBytesTransferredEventArgs> BytesReceived;

        public async Task SendAsync(INetworkNode Remote, byte[] Message)
        {
            await Task.Run(() => {
                Send(Remote, Message);
            });
        }

        public virtual void Send(INetworkNode Remote, byte[] Message)
        {
            this.OnOutgoingMessage(Remote, Message);
            int bytesSent = Remote.Socket.Send(Message);
            if (bytesSent > 0)
                this.OnBytesSent(Remote, bytesSent);
        }

        public async Task SendAsync(INetworkNode Remote, string Message)
        {
            await SendAsync(Remote, UTF8Encoding.UTF8.GetBytes(Message));
        }

        public void Send(INetworkNode Remote, string Message)
        {
            SendAsync(Remote, Message).Wait();
        }

        public void Send(byte[] Message)
        {
            foreach (INetworkNode Remote in Remotes)
                Send(Remote, Message);
        }

        public async Task SendAsync(byte[] Message)
        {
            foreach (INetworkNode Remote in Remotes)
                await SendAsync(Remote, Message);
        }

        public void Send(string Message)
        {
            foreach (INetworkNode Remote in Remotes)
                Send(Remote, Message);
        }

        public async Task SendAsync(string Message)
        {
            foreach (INetworkNode Remote in Remotes)
                await SendAsync(Remote, Message);
        }

        public async Task ListenForConnectionsAsync()
        {
            await Task.Run(() => {
                ListenForConnections();
            });
        }

        public void ListenForConnections()
        {
            while (IsRunning)
            {
                try
                {
                    Socket newSocket = this.Socket.Accept();
                    if (newSocket != null)
                    {
                        IPEndPoint remoteEndpoint = newSocket.RemoteEndPoint as IPEndPoint;
                        NetworkNode Remote = new NetworkNode(remoteEndpoint.Address, this.Protocol, (ushort)remoteEndpoint.Port, newSocket);

                        this.OnNewConnection(Remote);
                        _ = ListenForMessagesAsync(Remote);
                    }
                } 
                catch (SocketException)
                {
                    IsRunning = false;
                }
            }
        }

        public async Task ListenForMessagesAsync(INetworkNode Remote)
        {
            await Task.Run(() => {
                ListenForMessages(Remote);
            });
        }

        public void ListenForMessages(INetworkNode Remote)
        {
            byte[] RecvBuffer = new byte[ReceiveBufferSize];
            int bytesRead = 1;
            while (bytesRead > 0)
            {
                try
                {
                    bytesRead = Remote.Socket.Receive(RecvBuffer, SocketFlags.None);
                    if (bytesRead <= 0)
                    {
                        CloseRemote(Remote);
                    }
                    else
                    {
                        this.OnBytesReceived(Remote, bytesRead);

                        byte[] Data = new byte[bytesRead];
                        Buffer.BlockCopy(RecvBuffer, 0, Data, 0, bytesRead);

                        this.OnIncomingMessage(Remote, Data);
                    }
                }
                catch (SocketException)
                {
                    // If we catch a socket exception, it's probably a fatal error or the socket is disconnected; abort the connection
                    bytesRead = 0;
                    CloseRemote(Remote);
                }
            }
        }

        public async Task DisconnectAsync(INetworkNode Remote)
        {
            await Task.Run(() => { 
                Disconnect(Remote); 
            });
        }

        public void Disconnect(INetworkNode Remote)
        {
            this.OnRemoteDisconnecting(Remote);

            Remote.Socket.Shutdown(SocketShutdown.Both);
            Remote.Socket.Disconnect(false);
            Remote.Socket.Close();
        }

        #endregion

        #region Event Handling

        private void OnServerStarted()
        {
            ServerStarted?.Invoke(this, new InternetServerEventArgs { Local = this });
        }

        private void OnServerStopping()
        {
            ServerStopping?.Invoke(this, new InternetServerEventArgs { Local = this });
        }

        private void OnServerStopped()
        {
            ServerStopped?.Invoke(this, new InternetServerEventArgs { Local = this });
        }

        private void OnServerShuttingDown()
        {
            ServerShuttingDown?.Invoke(this, new InternetServerEventArgs { Local = this });
        }

        internal virtual void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            if ((From == null) || (NewMessage == null) || (NewMessage.Length == 0))
                return;

            IncomingMessage?.Invoke(this, new InternetCommunicationEventArgs
            {
                Remote = From,
                Local = this,
                Direction = CommunicationDirection.Inbound,
                Message = NewMessage
            });
        }

        private void OnOutgoingMessage(INetworkNode To, byte[] NewMessage)
        {
            if ((To == null) || (NewMessage == null) || (NewMessage.Length == 0))
                return;

            OutgoingMessage?.Invoke(this, new InternetCommunicationEventArgs
            {
                Remote = To,
                Local = this,
                Direction = CommunicationDirection.Outbound,
                Message = NewMessage
            });
        }

        private void OnBytesSent(INetworkNode To, int NumberOfBytes)
        {
            if ((To == null) || (NumberOfBytes <= 0))
                return;

            BytesSent?.Invoke(this, new InternetBytesTransferredEventArgs
            {
                Remote = To,
                Local = this,
                Direction = CommunicationDirection.Outbound,
                NumBytes = NumberOfBytes
            });
        }

        internal virtual void OnBytesReceived(INetworkNode From, int NumberOfBytes)
        {
            if ((From == null) || (NumberOfBytes <= 0))
                return;

            BytesReceived?.Invoke(this, new InternetBytesTransferredEventArgs
            {
                Remote = From,
                Local = this,
                Direction = CommunicationDirection.Inbound,
                NumBytes = NumberOfBytes
            });
        }

        private void OnNewConnection(INetworkNode Remote)
        {
            if (Remote == null)
                return;

            if (!Remotes.Contains(Remote))
                Remotes.Add(Remote);

            NewConnection?.Invoke(this, new InternetConnectionEventArgs
            {
                Local = this,
                Remote = Remote
            });
        }

        private void OnRemoteDisconnected(INetworkNode Remote)
        {
            if (Remote == null)
                return;

            RemoteDisconnected?.Invoke(this, new InternetConnectionEventArgs
            {
                Local = this,
                Remote = Remote
            });

            if (Remotes.Contains(Remote))
                Remotes.Remove(Remote);
        }

        private void OnRemoteDisconnecting(INetworkNode Remote)
        {
            if (Remote == null)
                return;

            RemoteDisconnecting?.Invoke(this, new InternetConnectionEventArgs
            {
                Local = this,
                Remote = Remote
            });
        }

        #endregion

        #region Abstract Methods to Override

        /// <summary>
        /// Create a new socket for this server.  Base definition does nothing.
        /// </summary>
        public virtual void CreateServer()
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

        /// <summary>
        /// Start the server.  Base definition only binds the ServerSocket to the address.
        /// </summary>
        public virtual void Start()
        {
            this.Socket.Bind(new IPEndPoint(Address, Port));
            IsRunning = true;
        }

        /// <summary>
        /// Start the server.  Base definition calls Start() in an async thread.
        /// </summary>
        public virtual async Task StartAsync()
        {
            await Task.Run(() => { Start(); });
        }

        /// <summary>
        /// Stop the server. Base definition shutdowns all connected remote nodes and then
        /// closes the server socket.
        /// </summary>
        public virtual void Stop()
        {
            if (IsRunning)
            {
                OnServerStopping();
                CloseAllRemotes();

                OnServerShuttingDown();
                if ((this.Socket != null) && (this.Socket.IsBound))
                    this.Socket.Close();
                this.Socket = null;
            }
        }

        /// <summary>
        /// End the server and close the server socket.  After this call, server can not be Start()ed again.
        /// Base definition just calls Stop()
        /// </summary>
        public virtual void ShutDownServer()
        {
            Stop();
        }

        /// <summary>
        /// Stop the server.  Base definition calls Stop() in an async thread.
        /// </summary>
        public virtual async Task StopAsync()
        {
            await Task.Run(() => { Stop(); });
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
                    ShutDownServer();
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
