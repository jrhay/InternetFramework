using System;
using System.Collections.Generic;
using System.Text;

namespace InternetFramework.IP.Telnet
{
    public enum TelnetCommand : byte
    {
        IAC = 255,  // Interpret as Command special key value
        DONT = 254,
        DO = 253,
        WONT = 252,
        WILL = 251,
        SB = 250,  // Start of option subnegotiation
        GOAHEAD = 249,  // "Go Ahead" signal
        ERASELINE = 248,  // Erase previous line
        ERASECHAR = 247,  // Erase previous character
        AREYOUTHERE = 246,  // "Are you there?" signal
        ABORT = 245,  // "Abort Output" signal
        INTERRUPT = 244,  // "Interrupt Process" signal
        BREAK = 243,  // Break character 
        DATAMARK = 242,  // Data stream of Synch
        NOP = 241,  // No operation
        SE = 240,  // End of option subnegotiation 
    }
}
