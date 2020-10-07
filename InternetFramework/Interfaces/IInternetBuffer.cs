using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Provide for buffered reading/writing of packets of information over an internet protocol
    /// May assemble packets from multiple endpoints simultaneously
    /// </summary>
    public interface IInternetBuffer<T> where T: IInternetPacket
    {
        /// <summary>
        /// Event invoked when an entire packet has been assembled and pulled from the buffer. 
        /// The bytes forming the packet are in the Message property of the event arguments. 
        /// </summary>
        event EventHandler<InternetCommunicationEventArgs> MessageReceived;

        /// <summary>
        /// Add data to the packet being formed. Invokens MessageReceived when a line has been assembled.
        /// </summary>
        /// <param name="From">Node message was received from</param>
        /// <param name="NewMessage">New data to add to message</param>
        void AddBytes(INetworkNode From, byte[] NewMessage);
    }
}
