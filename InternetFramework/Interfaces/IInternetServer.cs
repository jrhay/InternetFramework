using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// General interface for an Internet protocol server
    /// </summary>
    public interface IInternetServer : INetworkNode
    {
        /// <summary>
        /// Is the server currently running? (i.e., actively waiting for connections?)
        /// </summary>
        Boolean IsRunning { get; }

        /// <summary>
        /// Size of receive buffer, in bytes.  Default is 1024.
        /// Changes will affect new connections only.
        /// </summary>
        uint ReceiveBufferSize { get; set; }

        /// <summary>
        /// Event invoked when a server is started and waiting for remote connections.
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetEventArgs> ServerStarted;

        /// <summary>
        /// Event invoked when a server is stopping and disconnecting any remote connections.
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetEventArgs> ServerStopping;

        /// <summary>
        /// Event invoked when a server has stopped and is no longer accepting any connections
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetEventArgs> ServerStopped;

        /// <summary>
        /// Event invoked when a server is shutting down completely (and will never be restarted).
        /// All event handlers should be removed from the server and all other resources associated with the server cleaned up in this event.
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetEventArgs> ServerShuttingDown;

        /// <summary>
        /// Event invoked when a new client connects to the server 
        /// Will be called after the socket has been opened
        /// (note that this may not be called for connectionless protocols)
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetConnectionEventArgs> NewConnection;

        /// <summary>
        /// Event invoked when a connected client disconnects from the server
        /// Will be called before the socket is closed
        /// (note that this may not be called for connectionless protocols)
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetConnectionEventArgs> RemoteDisconnected;

        /// <summary>
        /// Event invoked when a connected client will be disconnected
        /// This is only called when a client connection is being closed from the server side,
        /// and will be called before the socket is shut down.
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetConnectionEventArgs> RemoteDisconnecting;

        /// <summary>
        /// Event invoked upon receiving a new message from any remote node
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetCommunicationEventArgs> IncomingMessage;

        /// <summary>
        /// Event invoked when starting to send a message to any remote node
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetCommunicationEventArgs> OutgoingMessage;

        /// <summary>
        /// Event invoked after sending a message to any remote node
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetBytesTransferredEventArgs> BytesSent;

        /// <summary>
        /// Event invoked immediately upon receiving data from any remote node
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetBytesTransferredEventArgs> BytesReceived;

        /// <summary>
        /// Event invoked when a system socket exception is captured. 
        /// If no handler is assigned, the exception will be rethrown.
        /// </summary>
        event EventHandler<InternetSocketExceptionEventArgs> SocketExceptionOccured;

        /// <summary>
        /// Start the server
        /// </summary>
        void Start();

        /// <summary>
        /// Start the server
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Stop the server: disconnect any connected remote nodes, stop listening for new connections, close the server socket
        /// </summary>
        void Stop();

        /// <summary>
        /// Stop the server: disconnect any connected remote nodes, stop listening for new connections, close the server socket
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Start synchronously listenting for connections from remote nodes
        /// </summary>
        void ListenForConnections();

        /// <summary>
        /// Start asyncronously listenting for connections from remote nodes
        /// </summary>
        Task ListenForConnectionsAsync();

        /// <summary>
        /// Start asynchronously listening for messages from a remote connection
        /// </summary>
        void ListenForMessages(INetworkNode Remote);

        /// <summary>
        /// Start asynchronously listening for messages from a remote connection
        /// </summary>
        Task ListenForMessagesAsync(INetworkNode Remote);

        /// <summary>
        /// Send a message to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        void Send(INetworkNode Remote, byte[] Message);

        /// <summary>
        /// Send a message to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        Task SendAsync(INetworkNode Remote, byte[] Message);

        /// <summary>
        /// Send a UTF8 message to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        void Send(INetworkNode Remote, string Message);

        /// <summary>
        /// Send a UTF8 message to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        Task SendAsync(INetworkNode Remote, string Message);

        /// <summary>
        /// Send a message to all remote nodes
        /// </summary>
        /// <param name="Message">Message to send</param>
        void Send(byte[] Message);

        /// <summary>
        /// Send a message to all remote nodes
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendAsync(byte[] Message);

        /// <summary>
        /// Send a UTF8 message to all remote nodes
        /// </summary>
        /// <param name="Message">Message to send</param>
        void Send(string Message);

        /// <summary>
        /// Send a UTF8 message to all remote nodes
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendAsync(string Message);

        /// <summary>
        /// Forcibly disconnect a remote node, flushing all transmit and receive buffers
        /// </summary>
        /// <param name="Remote">Node to disconnect</param>
        void Disconnect(INetworkNode Remote);

        /// <summary>
        /// Forcibly disconnect a remote node, flushing all transmit and receive buffers
        /// </summary>
        /// <param name="Remote">Node to disconnect</param>
        Task DisconnectAsync(INetworkNode Remote);
    }
}
