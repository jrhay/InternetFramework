using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Defines the "packet type" of line-based internet protocols
    /// Optionally will block lines received until a prompt sequence is received
    /// </summary>
    public class LineBasedProtocol : IInternetPacket
    {
        /// <summary>
        /// CRLF end-of-line control character for Internet RFC protocols
        /// </summary>
        public static byte[] CRLF = { 0x0D, 0x0A };

        /// <summary>
        /// End of line indicator (default to CRLF)
        /// </summary>
        public byte[] EndOfLine { get; set; } = CRLF;

        /// <summary>
        /// Wait for the server to display a prompt sequence when receiving new messages?
        /// </summary>
        public bool UsePrompt { get; set; } = false;

        /// <summary>
        /// If UsePrompt is true, wait for the server to send this byte sequence before deciding a full message has been received
        /// </summary>
        internal byte[] PromptSequence { get; set; }

        /// <summary>
        /// If UsePrompt is true, wait for the server to send the byte sequence for this string immediately after an EndOfLine sequence
        /// before deciding a full message has been received
        /// </summary>
        public string Prompt
        {
            get => MessageToString(Trim(PromptSequence));
            set => PromptSequence = EndOfLine.Concat(StringToMessage(value)).ToArray();
        }

        /// <summary>
        /// Determine if a sequence of bytes contains one or more lines, and if so return the 
        /// 0-based index into the byte array of the end of line
        /// </summary>
        /// <param name="bytes">Sequence of bytes to check for lines</param>
        /// <returns>0-based index of end of each complete line found</returns>
        public IEnumerable<int> FindPackets(byte[] bytes)
        {
            byte[] Delim = (UsePrompt && PromptSequence != null) ? PromptSequence : EndOfLine;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes.Skip(i).Take(Delim.Length).SequenceEqual(Delim))
                    yield return i + Delim.Length;
            }
        }

        /// <summary>
        /// Remove any existing EndOfLine byte sequence from the end a message buffer.
        /// If UsePrompt is true, removes any EndOfLine sequence immediately followed by the prompt string
        /// </summary>
        /// <param name="Message">Message to trim</param>
        /// <returns>If Message ended with EndOfLine, returns a new byte array without the EndOfLine bytes. If Message consists of exactly the EndOfLine, retrns empty byte array.  Otherwise, return Message</returns>
        public byte[] Trim(byte[] Message)
        {
            byte[] Delim = (UsePrompt && PromptSequence != null) ? PromptSequence : EndOfLine;
            int Index = Message.Length - 1;
            int EOLIndex = Delim.Length - 1;
            while ((Index >= 0) && (EOLIndex >= 0) && (Message[Index] == Delim[EOLIndex]))
            {
                Index--;
                EOLIndex--;
            }

            // If we have a prompt, make sure we don't have spurious prompt sequences before our message
            int MessageStart = -1;
            if (UsePrompt && PromptSequence != null)
            {
                byte[] PromptBytes = PromptSequence.Skip(EndOfLine.Length).ToArray();
                for (int i = 0; (MessageStart == -1) && (i < Index); i += PromptBytes.Length)
                {
                    if (!Message.Skip(i).Take(PromptBytes.Length).SequenceEqual(PromptBytes))
                        MessageStart = i;
                }
            }
            if (MessageStart == -1)
                MessageStart = 0;

            if ((EOLIndex < 0) && (Index >= 0))
            {
                byte[] NewMessage = new byte[Index + 1 - MessageStart];
                Buffer.BlockCopy(Message, MessageStart, NewMessage, 0, Index + 1 - MessageStart);
                return NewMessage;
            }
            else if ((Index < 0) && (EOLIndex < 0))
                return new byte[0];
            else
                return Message;
        }

        /// <summary>
        /// Convert a message line to a native .NET string
        /// </summary>
        /// <param name="Message">Message to make into a string</param>
        /// <returns>String version of the message</returns>
        public string MessageToString(byte[] Message)
        {
            return UTF8Encoding.UTF8.GetString(Trim(Message));
        }

        /// <summary>
        /// Convert a native .NET string to a message line
        /// </summary>
        /// <param name="Line">String to convert to message bytes</param>
        /// <returns>Byte array representing the string</returns>
        public byte[] StringToMessage(string Line)
        {
            return UTF8Encoding.UTF8.GetBytes(Line);
        }


    }
}
