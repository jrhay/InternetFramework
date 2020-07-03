using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using InternetFramework;
using InternetFramework.Extensions;

namespace WindowsTelnetServer
{
    public partial class Form1 : Form
    {
        TCPServer Server;

        public Form1()
        {
            InitializeComponent();

            ReloadIPAddresses();

            SetServerRunning(false);
        }

        #region Helper Methods

        private void AddLog(String Message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new MethodInvoker(delegate () { AddLog(Message); }));
            }
            else
            {
                txtLog.Text += Message + "\r\n";
            }
        }

        private void SetServerRunning(bool isRunning)
        {
            if (btnStopServer.InvokeRequired)
            {
                btnStopServer.BeginInvoke(new MethodInvoker(delegate () { SetServerRunning(isRunning); }));
            }
            else
            {
                btnStartServer.Enabled = !isRunning;
                btnStopServer.Enabled = isRunning;
            }
        }

        private void ReloadIPAddresses()
        {
            cmbIPAddress.Items.Clear();
            cmbIPAddress.Items.AddRange(IPAddressExtensions.LocalIPAddresses().ToArray());
        }

        #endregion

        #region Server Events

        private void Server_ServerStopped(object sender, InternetFramework.Events.InternetServerEventArgs e)
        {
            AddLog("Server Stopped.");
        }

        private void Server_ServerShuttingDown(object sender, InternetFramework.Events.InternetServerEventArgs e)
        {
            AddLog("Server Shutting Down.");

            Server.ServerStarted -= Server_ServerStarted;
            Server.ServerStopping -= Server_ServerStopping;
            Server.ServerStopped -= Server_ServerStopped;
            Server.ServerShuttingDown -= Server_ServerShuttingDown;
            Server.IncomingMessage -= Server_MessageTransmitting;
            Server.OutgoingMessage -= Server_MessageTransmitting;
            Server.NewConnection -= Server_NewConnection;
            Server.RemoteDisconnected -= Server_RemoteDisconnected;
            Server.RemoteDisconnecting -= Server_RemoteDisconnecting;

            SetServerRunning(false);
        }

        private void Server_ServerStopping(object sender, InternetFramework.Events.InternetServerEventArgs e)
        {
            AddLog("Server Stopping");
        }

        private void Server_ServerStarted(object sender, InternetFramework.Events.InternetServerEventArgs e)
        {
            AddLog("Server Started.");
            SetServerRunning(true);
        }

        private void Server_RemoteDisconnected(object sender, InternetFramework.Events.InternetConnectionEventArgs e)
        {
            AddLog("Client " + e.Remote + " Disconnected");
            RemoveClient(e.Remote);
        }

        private void Server_RemoteDisconnecting(object sender, InternetFramework.Events.InternetConnectionEventArgs e)
        {
            Server.Send(e.Remote, "Goodbye!\r\n");
        }

        private void Server_NewConnection(object sender, InternetFramework.Events.InternetConnectionEventArgs e)
        {
            AddLog("Client " + e.Remote + " Connected");
            AddClient(e.Remote);
            Server.Send(e.Remote, UTF8Encoding.UTF8.GetBytes("Welcome to Windows Telnet Server, a test program for the .NET Standard 2.0 InternetFramework\r\n"));
        }

        private void Server_MessageTransmitting(object sender, InternetFramework.Events.InternetCommunicationEventArgs e)
        {
            if (e.Direction == CommunicationDirection.Outbound)
                AddLog("To Client " + e.Remote + " => \"" + UTF8Encoding.UTF8.GetString(e.Message) + "\"");
            else
                AddLog("From Client " + e.Remote + " => \"" + UTF8Encoding.UTF8.GetString(e.Message) + "\"");
        }

        #endregion

        #region UI Handling

        private async void btnStartServer_Click(object sender, EventArgs e)
        {
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
        }

        private async void btnStopServer_Click(object sender, EventArgs e)
        {
            if (Server != null)
                await Server.StopAsync();
        }

        private async void btnResumeServer_Click(object sender, EventArgs e)
        {
            if (Server != null)
                await Server.ListenForConnectionsAsync();
        }

        private async void btnSendToSelected_Click(object sender, EventArgs e)
        {
            Socket SelectedClient = GetSelectedClient();
            if ((Server != null) && (SelectedClient != null))
            {
                await Server.SendAsync(SelectedClient, UTF8Encoding.UTF8.GetBytes(txtToSend.Text));
            }
        }

        private async void btnSendToAll_Click(object sender, EventArgs e)
        {
            if (Server != null)
            {
                await Server.SendAsync(txtToSend.Text);
            }
        }

        private async void btnDisconnectClient_Click(object sender, EventArgs e)
        {
            Socket SelectedClient = GetSelectedClient();
            if ((Server != null) && (SelectedClient != null))
            {
                await Server.DisconnectAsync(SelectedClient);
            }
        }

        private Socket GetSelectedClient()
        {
            return (Socket)lstClients.SelectedItem;
        }

        private void AddClient(Socket Client)
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.BeginInvoke(new MethodInvoker(delegate () { AddClient(Client); }));
            }
            else
            {
                lstClients.Items.Add(Client);
            }
        }
        private void RemoveClient(Socket Client)
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.BeginInvoke(new MethodInvoker(delegate () { RemoveClient(Client); }));
            }
            else
            {
                lstClients.Items.Remove(Client);
            }
        }

        #endregion
    }
}
