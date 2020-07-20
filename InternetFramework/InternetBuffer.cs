using InternetFramework.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace InternetFramework
{
    public class InternetBuffer
    {
        private INetworkNode Parent = null;

        public event EventHandler<InternetCommunicationEventArgs> MessageReceived;

        internal Dictionary<INetworkNode, List<byte>> IncomingMessages = new Dictionary<INetworkNode, List<byte>>();

        /// <summary>
        /// End of line indicator (default to CRLF)
        /// </summary>
        public byte[] EndOfLine { get; set; } = { 0x0D, 0x0A };

        public InternetBuffer(INetworkNode owner)
        {
            Parent = owner;
        }

        /// <summary>
        /// Remove any existing EndOfLine indicator bytes from a message buffer
        /// </summary>
        /// <param name="Message">Message to trim</param>
        /// <returns>If Message ended with EndOfLine, returns a new byte array without the EndOfLine bytes.  Otherwise, return Message</returns>
        public byte[] Trim(byte[] Message)
        {
            int Index = Message.Length - 1;
            int EOLIndex = EndOfLine.Length - 1;
            while ((Index >= 0) && (EOLIndex >= 0) && (Message[Index] == EndOfLine[EOLIndex]))
            {
                Index--;
                EOLIndex--;
            }

            if ((EOLIndex < 0) && (Index >= 0))
            {
                byte[] NewMessage = new byte[Index + 1];
                Buffer.BlockCopy(Message, 0, NewMessage, 0, Index + 1);
                return NewMessage;
            }
            else
                return Message;
        }

        private IEnumerable<int> FindEndsOfLine(byte[] bytes, byte[] EndOfLine)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes.Skip(i).Take(EndOfLine.Length).SequenceEqual(EndOfLine))
                    yield return i;
            }
        }

        /// <summary>
        /// Add data to the message being formed. Calls OnNewMessage() when a line has been assembled.
        /// </summary>
        /// <param name="From">Node message was received from</param>
        /// <param name="NewMessage">New data to add to message</param>
        public void AddMessage(INetworkNode From, byte[] NewMessage)
        {
            // Add incoming message to buffered message
            if (IncomingMessages.ContainsKey(From))
                IncomingMessages[From].AddRange(NewMessage);
            else
                IncomingMessages.Add(From, new List<byte>(NewMessage));

            // If buffered message has an end-of-line terminator, send the line(s) to any listeners
            byte[] IncomingMessage = IncomingMessages[From].ToArray();
            int MessageStart = 0;
            IEnumerable<int> LinePositions = FindEndsOfLine(IncomingMessage, EndOfLine);
            if ((LinePositions != null) && (LinePositions.Count() > 0))
            {
                foreach (int LineIndex in LinePositions)
                {
                    int MessageLength = LineIndex + EndOfLine.Length;
                    byte[] Message = new byte[MessageLength];

                    Buffer.BlockCopy(IncomingMessage, MessageStart, Message, 0, MessageLength);
                    MessageReceived?.Invoke(this, new InternetCommunicationEventArgs()
                    {
                        Remote = From,
                        Local = Parent,
                        Direction = CommunicationDirection.Inbound,
                        Message = IncomingMessage
                    });

                    MessageStart += MessageLength;
                }

                // Preserve any remaining incoming message we may have
                if ((MessageStart > 0) && (MessageStart < IncomingMessage.Length-1))
                {
                    byte[] Remaining = new byte[IncomingMessage.Length - MessageStart + 1];
                    Buffer.BlockCopy(IncomingMessage, MessageStart, Remaining, 0, Remaining.Length);
                    IncomingMessages[From] = new List<byte>(Remaining);
                }
                else
                    IncomingMessages.Remove(From);
            }
        }
    }
}
