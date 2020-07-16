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
        BufferedTCPClient Client;

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
                rdoCRLFEOL.Enabled = !isConnected;
                rdoPromptEOL.Enabled = !isConnected;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (Client != null)
                await Client.DisconnectAsync();
            else
            {
                using (Client = new BufferedTCPClient(23, RFCProtocol.Telnet))
                {
                    Client.NewConnection += Client_NewConnection;

                    // Set prompt Telnet server prompt as EOL
                    if (rdoPromptEOL.Checked)
                        Client.EndOfLine = UTF8Encoding.UTF8.GetBytes(txtPrompt.Text);
                    else
                        Client.EndOfLine = TelnetServer.CRLF;

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
                string Message = UTF8Encoding.UTF8.GetString(Client.Trim(e.Message));
                //string Message = UTF8Encoding.UTF8.GetString(e.Message);
                txtConsole.Text += "Rcv: \"" + Message.Trim() + "\"";
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
                    await Client.SendAsync(TelnetServer.CRLF);
                    txtSend.Text = null;
                }
                btnSend.Enabled = true;
            }
        }

        private void rdoPromptEOL_CheckedChanged(object sender, EventArgs e)
        {
            txtPrompt.Enabled = rdoPromptEOL.Checked;
        }
    }
}
