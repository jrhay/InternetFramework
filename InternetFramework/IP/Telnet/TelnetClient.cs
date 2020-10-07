using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    public class TelnetClient : LineBasedTCPClient
    {
        #region Lifecycle

        /// <summary>
        /// Create a new Telnet client instance 
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public TelnetClient(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(Port, Protocol)
        {
        }

        #endregion

    }
}
