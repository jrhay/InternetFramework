namespace WindowsTelnetServer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtToSend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSendToSelected = new System.Windows.Forms.Button();
            this.btnSendToAll = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbIPAddress = new System.Windows.Forms.ComboBox();
            this.btnDisconnectClient = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Number";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(198, 58);
            this.numPort.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(209, 31);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(458, 13);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(179, 61);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Run Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connected Clients";
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 25;
            this.lstClients.Location = new System.Drawing.Point(28, 176);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(229, 204);
            this.lstClients.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(278, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Communication Log";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(283, 176);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1080, 594);
            this.txtLog.TabIndex = 7;
            // 
            // txtToSend
            // 
            this.txtToSend.Location = new System.Drawing.Point(28, 524);
            this.txtToSend.Name = "txtToSend";
            this.txtToSend.Size = new System.Drawing.Size(229, 31);
            this.txtToSend.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 496);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "Send Message";
            // 
            // btnSendToSelected
            // 
            this.btnSendToSelected.Location = new System.Drawing.Point(28, 579);
            this.btnSendToSelected.Name = "btnSendToSelected";
            this.btnSendToSelected.Size = new System.Drawing.Size(179, 61);
            this.btnSendToSelected.TabIndex = 10;
            this.btnSendToSelected.Text = "Send To Selected Client";
            this.btnSendToSelected.UseVisualStyleBackColor = true;
            this.btnSendToSelected.Click += new System.EventHandler(this.btnSendToSelected_Click);
            // 
            // btnSendToAll
            // 
            this.btnSendToAll.Location = new System.Drawing.Point(28, 665);
            this.btnSendToAll.Name = "btnSendToAll";
            this.btnSendToAll.Size = new System.Drawing.Size(179, 61);
            this.btnSendToAll.TabIndex = 11;
            this.btnSendToAll.Text = "Send To All Clients";
            this.btnSendToAll.UseVisualStyleBackColor = true;
            this.btnSendToAll.Click += new System.EventHandler(this.btnSendToAll_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "IP Address";
            // 
            // cmbIPAddress
            // 
            this.cmbIPAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIPAddress.FormattingEnabled = true;
            this.cmbIPAddress.Location = new System.Drawing.Point(198, 13);
            this.cmbIPAddress.Name = "cmbIPAddress";
            this.cmbIPAddress.Size = new System.Drawing.Size(209, 33);
            this.cmbIPAddress.TabIndex = 13;
            // 
            // btnDisconnectClient
            // 
            this.btnDisconnectClient.Location = new System.Drawing.Point(38, 386);
            this.btnDisconnectClient.Name = "btnDisconnectClient";
            this.btnDisconnectClient.Size = new System.Drawing.Size(219, 61);
            this.btnDisconnectClient.TabIndex = 14;
            this.btnDisconnectClient.Text = "Disconnect Client";
            this.btnDisconnectClient.UseVisualStyleBackColor = true;
            this.btnDisconnectClient.Click += new System.EventHandler(this.btnDisconnectClient_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(666, 12);
            this.btnStopServer.Name = "btnShutdownServer";
            this.btnStopServer.Size = new System.Drawing.Size(179, 61);
            this.btnStopServer.TabIndex = 15;
            this.btnStopServer.Text = "Shut Down Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 782);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnDisconnectClient);
            this.Controls.Add(this.cmbIPAddress);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSendToAll);
            this.Controls.Add(this.btnSendToSelected);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtToSend);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Windows Telnet Server";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtToSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSendToSelected;
        private System.Windows.Forms.Button btnSendToAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbIPAddress;
        private System.Windows.Forms.Button btnDisconnectClient;
        private System.Windows.Forms.Button btnStopServer;
    }
}

