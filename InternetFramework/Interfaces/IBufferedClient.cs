using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Interface for a client that buffers sent and received data into packets/lines/etc.
    /// </summary>
    public interface IBufferedClient
    {
        /// <summary>
        /// Byte sequence used to denote "End of Data Section"
        /// </summary>
        byte[] EndOfLine { get; set; }

        /// <summary>
        /// Remove any existing EndOfLine indicator bytes from a message buffer
        /// </summary>
        /// <param name="Message">Message to trim</param>
        /// <returns>If Message ended with EndOfLine, returns a new byte array without the EndOfLine bytes.  Otherwise, return Message</returns>
        byte[] Trim(byte[] Message);

        /// <summary>
        /// Send a message followed by the EndOfLine sequence to the connected server
        /// </summary>
        /// <param name="Message">Message to send</param>
        void SendLine(byte[] DataLine);

        /// <summary>
        /// Send a message followed by the EndOfLine sequence to the connected server
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendLineAsync(byte[] DataLine);

        /// <summary>
        /// Send a UTF8 message followed by the EndOfLine sequence to the connected server
        /// </summary>
        /// <param name="Message">Message to send</param>
        void SendLine(string DataLine);

        /// <summary>
        /// Send a UTF8 message followed by the EndOfLine sequence to the connected server
        /// </summary>
        /// <param name="Message">Message to send</param>
        Task SendLineAsync(string DataLine);

    }
}
