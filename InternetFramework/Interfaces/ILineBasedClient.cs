using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Interface for a client that buffers sent and received data into logical lines of data
    /// Defines methods names to make line-based communication intent clear.
    /// </summary>
    public interface ILineBasedClient
    {
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
