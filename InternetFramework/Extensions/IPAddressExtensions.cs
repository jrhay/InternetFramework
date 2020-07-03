using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace InternetFramework.Extensions
{
    public static class IPAddressExtensions
    {
        /// <summary>
        /// Is there any network interface currently connected to the internet?
        /// </summary>
        /// <returns>TRUE if a connection is avaliable, FALSE otherwise</returns>
        public static Boolean IsConnected()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// Get a list of all local IP addresses currently avaliable
        /// </summary>
        /// <returns>All avaliable IP addreses for the local host, may be empty if no interfaces currently connected</returns>
        public static IEnumerable<IPAddress> LocalIPAddresses()
        {
            List<IPAddress> Addresses = new List<IPAddress>();
            if (IPAddressExtensions.IsConnected())
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                Addresses.AddRange(host.AddressList.Where(ip => 
                                ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || 
                                ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6));
            }
            return Addresses;
        }

        /// <summary>
        /// Return the first connected IP address for the local host
        /// </summary>
        /// <returns>IP address on first found Internet connection on the local host</returns>
        public static IPAddress LocalIPAddress()
        {
            return IPAddressExtensions.LocalIPAddresses().FirstOrDefault();
        }
    }
}
