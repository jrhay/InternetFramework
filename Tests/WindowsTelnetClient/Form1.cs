using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InternetFramework;

namespace WindowsTelnetClient
{
    public partial class Form1 : Form
    {
        TCPClient Client;

        public Form1()
        {
            InitializeComponent();
            SetConnected(false);
        }

        private void SetConnected(bool isConnected)
        {
            if (grpConsole.InvokeRequired)
            {
                grpConsole.BeginInvoke(new MethodInvoker(delegate () { SetConnected(isConnected); }));
            }
            else
            {
                grpConsole.Enabled = isConnected;
                btnSend.Enabled = isConnected;
                btnConnect.Text = isConnected ? "Disconnect" : "Connect";
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (Client != null)
                await Client.DisconnectAsync();
            else
            {
                using (Client = new TCPClient(23, RFCProtocol.Telnet))
                {
                    Client.NewConnection += Client_NewConnection;
                    Client.RemoteDisconnected += Client_RemoteDisconnected;
                    Client.IncomingMessage += Client_IncomingMessage;

                    txtConsole.Text = null;
                    SetConnected(false);
                    await Client.ConnectAsync(txtHost.Text);
                }
            }
        }

        private void Client_IncomingMessage(object sender, InternetFramework.Events.InternetCommunicationEventArgs e)
        {
            if (grpConsole.InvokeRequired)
            {
                grpConsole.BeginInvoke(new MethodInvoker(delegate () { Client_IncomingMessage(sender, e); }));
            }
            else
            {
                txtConsole.Text += UTF8Encoding.UTF8.GetString(e.Message);
            }
        }

        private void Client_RemoteDisconnected(object sender, InternetFramework.Events.InternetConnectionEventArgs e)
        {
            if (grpConsole.InvokeRequired)
            {
                grpConsole.BeginInvoke(new MethodInvoker(delegate () { Client_RemoteDisconnected(sender, e); }));
            }
            else
            {
                txtConsole.Text += "Remote Disconnected.";
                SetConnected(false);
                Client = null;
            }
        }

        private void Client_NewConnection(object sender, InternetFramework.Events.InternetConnectionEventArgs e)
        {
            if (grpConsole.InvokeRequired)
            {
                grpConsole.BeginInvoke(new MethodInvoker(delegate () { Client_NewConnection(sender, e); }));
            }
            else
            {
                txtConsole.Text = "Connected to " + e.Remote.ToString() + "\r\n\r\n";
                SetConnected(true);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (grpConsole.InvokeRequired)
            {
                grpConsole.BeginInvoke(new MethodInvoker(delegate () { btnSend_Click(sender, e); }));
            }
            else
            {
                btnSend.Enabled = false;
                if (Client != null)
                {
                    await Client.SendAsync(txtSend.Text);
                    txtSend.Text = null;
                }
                btnSend.Enabled = true;
            }
        }
    }
}
