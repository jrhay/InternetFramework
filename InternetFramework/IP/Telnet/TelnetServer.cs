using InternetFramework.Extensions;
using InternetFramework.IP.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InternetFramework
{
    public class TelnetServer : LineBasedTCPServer
    {
        #region Lifecycle

        /// <summary>
        /// Create a new Telnet server instance bound to the first found IP address for the local host
        /// </summary>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public TelnetServer(UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddressExtensions.LocalIPAddress(), Port, Protocol)
        {
        }

        /// <summary>
        /// Create a new Telnet server instance bound to a specific IP address for the local host
        /// </summary>
        /// <param name="IPAddress">IP Address to bind server to, must be an IP address on a currently-connected interface</param>
        /// <param name="Port">Port to use for server, default: 23 (Telnet)</param>
        /// <param name="Protocol">Protocol to use for the server, default: Telnet (RFC854)</param>
        public TelnetServer(IPAddress IPAddress, UInt16 Port = (UInt16)DefaultPorts.Telnet, RFCProtocol Protocol = RFCProtocol.Telnet) : base(IPAddress, Port, Protocol)
        {
        }

        #endregion

        #region NVT State Handling

        /// <summary>
        /// Dictionary to keep track of the per-connection NVT state
        /// </summary>
        internal Dictionary<INetworkNode, NVTState> RemoteState = new Dictionary<INetworkNode, NVTState>();

        internal Dictionary<INetworkNode, List<byte>> RemoteCommands = new Dictionary<INetworkNode, List<byte>>();

        internal override void OnNewConnection(INetworkNode Remote)
        {
            RemoteState[Remote] = NVTState.Normal;
            base.OnNewConnection(Remote);
        }

        internal override void OnRemoteDisconnected(INetworkNode Remote)
        {
            if (RemoteState.Keys.Contains(Remote))
                RemoteState.Remove(Remote);

            if (RemoteCommands.Keys.Contains(Remote))
                RemoteCommands.Remove(Remote);

            base.OnRemoteDisconnected(Remote);
        }

        #endregion

        #region Telnet Command Handling

        internal override void OnIncomingMessage(INetworkNode From, byte[] NewMessage)
        {
            List<byte> Message = new List<byte>(NewMessage.Length);
            for (int i = 0; i < NewMessage.Length; i++)
            {
                switch (RemoteState[From])
                {
                    case NVTState.AcceptingCommand:
                        switch (NewMessage[i])
                        {
                            case (byte)TelnetCommand.DO:
                                RemoteState[From] = NVTState.DoOption; break;
                            case (byte)TelnetCommand.DONT:
                                RemoteState[From] = NVTState.DontOption; break;
                            case (byte)TelnetCommand.WILL:
                                RemoteState[From] = NVTState.WillOption; break;
                            case (byte)TelnetCommand.WONT:
                                RemoteState[From] = NVTState.WontOption; break;

                            case (byte)TelnetCommand.IAC:
                                Message.Add(NewMessage[i]);
                                break;
                        }
                        break;

                    // Process Telnet "DO, DON'T, WILL, WON'T" option negotiation
                    case NVTState.DoOption:
                        TelnetOption Option = (TelnetOption)NewMessage[i];
                        if (!Get(Option))
                            Send(From, new byte[] { (byte)TelnetCommand.IAC, (byte)(Set(Option, true) ? TelnetCommand.WILL : TelnetCommand.WONT), (byte)Option });
                        RemoteState[From] = NVTState.Normal;
                        break;

                    case NVTState.DontOption:
                        Option = (TelnetOption)NewMessage[i];
                        if (Get(Option))
                            Send(From, new byte[] { (byte)TelnetCommand.IAC, (byte)(Set(Option, false) ? TelnetCommand.WONT : TelnetCommand.WILL), (byte)Option });
                        RemoteState[From] = NVTState.Normal;
                        break;

                    case NVTState.WillOption:
                        Option = (TelnetOption)NewMessage[i];
                        Send(From, "[TCP WILL " + Option.ToString() + "]");
                        if (!Get(Option))
                            Send(From, new byte[] { (byte)TelnetCommand.IAC, (byte)(Set(Option, true) ? TelnetCommand.DO : TelnetCommand.DONT), (byte)Option });
                        RemoteState[From] = NVTState.Normal;
                        break;

                    case NVTState.WontOption:
                        Option = (TelnetOption)NewMessage[i];
                        if (Get(Option))
                            Send(From, new byte[] { (byte)TelnetCommand.IAC, (byte)(Set(Option, false) ? TelnetCommand.DO : TelnetCommand.DONT), (byte)Option });
                        RemoteState[From] = NVTState.Normal;
                        break;

                    default:
                        if (NewMessage[i] == (byte)TelnetCommand.IAC)
                            RemoteState[From] = NVTState.AcceptingCommand;
                        else
                            Message.Add(NewMessage[i]);
                        break;
                }
            }

            if (Message.Count() > 0)
                base.OnIncomingMessage(From, Message.ToArray());

            if (DoEcho)
                _ = this.SendAsync(Message.ToArray());
        }

        #endregion

        #region Telnet Option Handling

        private bool DoEcho = false;

        /// <summary>
        /// Process a remote connection attempt to enable or disabled the specified option.
        /// If the option is known, it should be immediately set to the specified state (if Enabled is true, option subnegotiation may proceed later)
        /// </summary>
        /// <param name="Option">Option to enable</param>
        /// <param name="Enabled">Requested state of the option</param>
        /// <returns>TRUE if option is enabled, FALSE if option is not enabled or is not known/understood</returns>
        virtual public bool Set(TelnetOption Option, bool Enabled)
        {
            switch (Option)
            {
                case TelnetOption.ECHO:
                    DoEcho = Enabled;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Process a remote query if a specified option is currently enabled
        /// </summary>
        /// <param name="Option">Option to query</param>
        /// <returns>TRUE if option is currently enabled, FALSE if option is currently disabled or not known/understood</returns>
        virtual public bool Get(TelnetOption Option)
        {
            switch (Option)
            {
                case TelnetOption.ECHO:
                    return DoEcho;
            }
            return false;
        }

        #endregion


    }

}
