using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Generic interface of a network node, used for indicating remote side of connections from server instances
    /// Does no management of resources
    /// </summary>
    class NetworkNode : INetworkNode
    {
        public IPAddress Address { get; internal set; }

        public RFCProtocol Protocol { get; internal set; }

        public ushort Port { get; internal set; }

        public Socket Socket { get; internal set; }

        public NetworkNode()
        {
            Address = IPAddress.None;
            Protocol = RFCProtocol.Unknown;
            Port = 0;
            Socket = null;
        }

        public NetworkNode(IPAddress address, RFCProtocol protocol = RFCProtocol.Unknown, ushort port = 0, Socket socket = null)
        {
            Address = address;
            Protocol = protocol;
            Port = port;
            Socket = socket;
        }

        public override string ToString()
        {
            return Address.ToString() + ":" + Port + " (" + Protocol + ")";
        }

        #region IDisposable Interface

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    // Note: We assume here that the socket instance will be managed/shutdown by other code
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
