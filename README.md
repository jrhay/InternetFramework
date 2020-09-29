# InternetFramework
.NET Standard Library to implement basic Internet RFC-defined protocols (Telnet, etc) in fully native .NET event-driven async models

The idea of this lirbary is to carefully impelment the core RFCs, implementing all the basic boilerplate code necessary in one place, and allowing developers to focus on implementing custom functionality from a modern high-level view.  For instance, all that is needed to implement a basic TCP-based server accepting multiple simultaneous clients is to implement a few event handlers and then an async call wrapped in a using statement:

            using (Server = new TCPServer((IPAddress)cmbIPAddress.SelectedItem, (ushort)numPort.Value))
            {
                Server.ServerStarted += Server_ServerStarted;
                Server.ServerStopping += Server_ServerStopping;
                Server.ServerStopped += Server_ServerStopped;
                Server.ServerShuttingDown += Server_ServerShuttingDown;
                Server.IncomingMessage += Server_MessageTransmitting;
                Server.OutgoingMessage += Server_MessageTransmitting;
                Server.NewConnection += Server_NewConnection;
                Server.RemoteDisconnected += Server_RemoteDisconnected;
                Server.RemoteDisconnecting += Server_RemoteDisconnecting;

                await Server.StartAsync();
            }
            
### Static Application Security Testing (SAST)
![PVS-Studio Logo](https://www.viva64.com/media/img/logo.png)
Code in the primary repo for [InternetFramework](https://github.com/jrhay/InternetFramework.git) is analyzed for typos, bugs, and security errors per the [Common Weakness Enumeration](https://cwe.mitre.org/), [SEI CERT](https://wiki.sei.cmu.edu/confluence/display/seccode/SEI+CERT+Coding+Standards), and [MISRA](https://www.misra.org.uk/) coding standards using the [PVS-Studio](https://www.viva64.com/en/pvs-studio/) SAST tool before being pushed to github.
