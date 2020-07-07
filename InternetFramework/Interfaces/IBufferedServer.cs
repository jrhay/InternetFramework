using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework.Interfaces
{
    /// <summary>
    /// Interface for a server that buffers sent and received data into packets/lines/etc.
    /// </summary>
    public interface IBufferedServer
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
    }
}
