using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework
{
    /// <summary>
    /// Definition of a packet of data for an internet protocol (line, block, datagarm, etc)
    /// </summary>
    public interface IInternetPacket
    {
        /// <summary>
        /// Determine if a sequence of bytes contains one or more complete packets of information,
        /// and if so return the 0-based index into the byte array of the end of each packet
        /// </summary>
        /// <param name="bytes">Sequence of bytes to check for packets</param>
        /// <returns>0-based index of end of each complete packet found</returns>
        IEnumerable<int> FindPackets(byte[] bytes);
    }
}
