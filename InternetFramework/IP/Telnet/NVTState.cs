using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework.IP.Telnet
{
    /// <summary>
    /// State of the Telnet "Network Virtual Terminal" for a particular connection
    /// </summary>
    public enum NVTState
    {
        Normal,

        AcceptingCommand,

        DoOption,

        DontOption,

        WillOption,

        WontOption
    }

}
