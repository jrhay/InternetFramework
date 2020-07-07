
namespace InternetFramework
{
    /// <summary>
    /// Protocol used for communication
    /// </summary>
    public enum RFCProtocol
    {
        /// <summary>
        /// Unknown or unspecified protocol
        /// </summary>
        Unknown = 0,

        #region UDP

        /// <summary>
        /// User Datagram Protocol, RFC 768
        /// </summary>
        RFC768,

        /// <summary>
        /// User Datagram Protocol, RFC 768
        /// </summary>
        UDP = RFC768,

        #endregion

        #region TCP

        /// <summary>
        /// Transmission Control Protocol, RFC 793
        /// </summary>
        RFC793,

        /// <summary>
        /// Terminal Control Protocol, RFC 793
        /// </summary>
        TCP = RFC793,

        #endregion

        #region Telnet

        /// <summary>
        /// Basic Telnet Protocol Definition, RFC 854
        /// </summary>
        RFC854,

        Telnet = RFC854,

        #endregion

        /// <summary>
        /// Other/undefined protocol
        /// </summary>
        Other
    }
}
