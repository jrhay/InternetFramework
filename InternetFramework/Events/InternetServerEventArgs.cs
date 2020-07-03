using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InternetFramework.Events
{
    /// <summary>
    /// General EventArgs for IIneternetServer-generated events
    /// </summary>
    public class InternetServerEventArgs : EventArgs
    {
        /// <summary>
        /// Address of local side of connection
        /// </summary>
        public Socket Local { get; set; }

    }
}
