using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// General interface for an internet protocol client
    /// </summary>
    public interface IInternetClient : INetworkNode
    {
        /// <summary>
        /// Is the client currently connected to a server?
        /// </summary>
        Boolean IsConnected { get; }

        /// <summary>
        /// Size of receive buffer, in bytes.  Default is 1024.
        /// Changes will affect new connections only.
        /// </summary>
        uint ReceiveBufferSize { get; set; }

        /// <summary>
        /// Event invoked when a client connections to a new server 
        /// Will be called after the socket has been opened
        /// (note that this may not be called for connectionless protocols)
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetConnectionEventArgs> NewConnection;

        /// <summary>
        /// Event invoked when a connected client disconnects from a server
        /// Will be called before the socket is closed
        /// (note that this may not be called for connectionless protocols)
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetConnectionEventArgs> RemoteDisconnected;

        /// <summary>
        /// Event invoked when a connected client starts to disconnect from the server
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
        /// Event invoked after sending a message to the server
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetBytesTransferredEventArgs> BytesSent;

        /// <summary>
        /// Event invoked immediately upon receiving data from the server
        /// Handlers will be invoked on a threadpool thread
        /// </summary>
        event EventHandler<InternetBytesTransferredEventArgs> BytesReceived;


        /// <summary>
        /// Attempt to connect to a server at the specified address
        /// </summary>
        /// <param name="host">DNS hostname to connect to or IPv4/IPv6 address string</param>
        void Connect(string host);

        /// <summary>
        /// Attempt to connect to a server at the specified address
        /// </summary>
        /// <param name="host">DNS hostname to connect to or IPv4/IPv6 address string</param>
        Task ConnectAsync(string host);

        /// <summary>
        /// Disconnect from the server, flushing all transmit and receive buffers
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Disconnect from the server, flushing all transmit and receive buffers
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="Message">Message to send</param>
        void Send(byte[] Message);

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendAsync(byte[] Message);

        /// <summary>
        /// Send a UTF8 message to the server
        /// </summary>
        /// <param name="Message">Message to send</param>
        void Send(string Message);

        /// <summary>
        /// Send a UTF8 message to the server
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendAsync(string Message);

    }
}
