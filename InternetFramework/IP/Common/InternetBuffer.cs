using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Class to build up "packets" of data received over the network.  Bytes making up the packet may be
    /// received in multiple butsts of sending, or multiple packets may be received in a single burst.
    /// In either case, the the MessageReceived event is invoked for every packet received.
    /// </summary>
    /// <typeparam name="T">Type of data packet to receive</typeparam>
    public class InternetBuffer<T> : IInternetBuffer<T> where T:IInternetPacket
    {
        T DataType;

        private INetworkNode Parent = null;

        public event EventHandler<InternetCommunicationEventArgs> MessageReceived;

        internal Dictionary<INetworkNode, List<byte>> IncomingMessages = new Dictionary<INetworkNode, List<byte>>();

        public InternetBuffer(T BufferType, INetworkNode owner)
        {
            DataType = BufferType;
            Parent = owner;
        }

        /// <summary>
        /// Add data to the message being formed. Calls OnNewMessage() when a line has been assembled.
        /// </summary>
        /// <param name="From">Node message was received from</param>
        /// <param name="NewMessage">New data to add to message</param>
        public void AddBytes(INetworkNode From, byte[] NewMessage)
        {
            // Add incoming message to buffered message
            if (IncomingMessages.ContainsKey(From))
                IncomingMessages[From].AddRange(NewMessage);
            else
                IncomingMessages.Add(From, new List<byte>(NewMessage));

            // If buffered message has an end-of-line terminator, send the line(s) to any listeners
            byte[] IncomingMessage = IncomingMessages[From].ToArray();
            int MessageStart = 0;
            IEnumerable<int> LinePositions = DataType.FindPackets(IncomingMessage, (uint)NewMessage.Length);
            if ((LinePositions != null) && (LinePositions.Count() > 0))
            {
                foreach (int LineIndex in LinePositions)
                {
                    int MessageLength = LineIndex - MessageStart;
                    byte[] Message = new byte[MessageLength];

                    Buffer.BlockCopy(IncomingMessage, MessageStart, Message, 0, MessageLength);
                    OnMessageReceived(From, Message);

                    MessageStart += MessageLength - 1;
                }

                // Preserve any remaining incoming message we may have
                if ((MessageStart > 0) && (MessageStart < IncomingMessage.Length - 1))
                {
                    byte[] Remaining = new byte[IncomingMessage.Length - MessageStart - 1];
                    Buffer.BlockCopy(IncomingMessage, MessageStart+1, Remaining, 0, Remaining.Length);
                    IncomingMessages[From] = new List<byte>(Remaining);
                }
                else
                    IncomingMessages.Remove(From);
            }
        }

        internal virtual void OnMessageReceived(INetworkNode From, byte[] NewMessage)
        {
            MessageReceived?.Invoke(this, new InternetCommunicationEventArgs()
            {
                Remote = From,
                Local = Parent,
                Direction = CommunicationDirection.Inbound,
                Message = NewMessage
            });
        }

    }
}
