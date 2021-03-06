﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    /// <summary>
    /// Interface for a client that buffers sent and received data into logical lines of data
    /// Defines methods names to make line-based communication intent clear.
    /// </summary>
    public interface ILineBasedServer
    {
        /// <summary>
        /// Send a message followed by the EndOfLine sequence to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        void SendLine(INetworkNode Remote, byte[] DataLine);

        /// <summary>
        /// Send a message followed by the EndOfLine sequence to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        Task SendLineAsync(INetworkNode Remote, byte[] DataLine);

        /// <summary>
        /// Send a UTF8 message followed by the EndOfLine sequence to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        void SendLine(INetworkNode Remote, string DataLine);

        /// <summary>
        /// Send a UTF8 message followed by the EndOfLine sequence to a remote node
        /// </summary>
        /// <param name="Remote">Address to send message to</param>
        /// <param name="Message">Message to send</param>
        Task SendLineAsync(INetworkNode Remote, string DataLine);

    }
}
