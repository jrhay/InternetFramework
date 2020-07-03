using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Indication of flow of communication
    /// </summary>
    public enum CommunicationDirection
    {
        /// <summary>
        /// Remote -> Local
        /// </summary>
        Inbound, 

        /// <summary>
        /// Local -> Remote
        /// </summary>
        Outbound
    }
}
