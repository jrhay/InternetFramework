namespace WindowsTelnetClient
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
            this.grpConsole = new System.Windows.Forms.GroupBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.rdoPromptEOL = new System.Windows.Forms.RadioButton();
            this.rdoCRLFEOL = new System.Windows.Forms.RadioButton();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.grpConsole.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConsole
            // 
            this.grpConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConsole.Controls.Add(this.txtConsole);
            this.grpConsole.Location = new System.Drawing.Point(13, 194);
            this.grpConsole.Name = "grpConsole";
            this.grpConsole.Size = new System.Drawing.Size(1519, 834);
            this.grpConsole.TabIndex = 0;
            this.grpConsole.TabStop = false;
            this.grpConsole.Text = "Console";
            // 
            // txtConsole
            // 
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Location = new System.Drawing.Point(3, 27);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(1513, 804);
            this.txtConsole.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(21, 65);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(244, 31);
            this.txtHost.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(21, 103);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(163, 43);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.Location = new System.Drawing.Point(431, 150);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(900, 31);
            this.txtSend.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(426, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Command";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(1366, 138);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(163, 43);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rdoPromptEOL
            // 
            this.rdoPromptEOL.AutoSize = true;
            this.rdoPromptEOL.Location = new System.Drawing.Point(449, 31);
            this.rdoPromptEOL.Name = "rdoPromptEOL";
            this.rdoPromptEOL.Size = new System.Drawing.Size(190, 29);
            this.rdoPromptEOL.TabIndex = 7;
            this.rdoPromptEOL.Text = "Prompt As EOL";
            this.rdoPromptEOL.UseVisualStyleBackColor = true;
            this.rdoPromptEOL.CheckedChanged += new System.EventHandler(this.rdoPromptEOL_CheckedChanged);
            // 
            // rdoCRLFEOL
            // 
            this.rdoCRLFEOL.AutoSize = true;
            this.rdoCRLFEOL.Checked = true;
            this.rdoCRLFEOL.Location = new System.Drawing.Point(449, 66);
            this.rdoCRLFEOL.Name = "rdoCRLFEOL";
            this.rdoCRLFEOL.Size = new System.Drawing.Size(177, 29);
            this.rdoCRLFEOL.TabIndex = 8;
            this.rdoCRLFEOL.TabStop = true;
            this.rdoCRLFEOL.Text = "CRLF As EOL";
            this.rdoCRLFEOL.UseVisualStyleBackColor = true;
            // 
            // txtPrompt
            // 
            this.txtPrompt.Location = new System.Drawing.Point(645, 29);
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(244, 31);
            this.txtPrompt.TabIndex = 9;
            this.txtPrompt.Text = "> ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1544, 1058);
            this.Controls.Add(this.txtPrompt);
            this.Controls.Add(this.rdoCRLFEOL);
            this.Controls.Add(this.rdoPromptEOL);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpConsole);
            this.Name = "Form1";
            this.Text = "Windows Telnet Client";
            this.grpConsole.ResumeLayout(false);
            this.grpConsole.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConsole;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RadioButton rdoPromptEOL;
        private System.Windows.Forms.RadioButton rdoCRLFEOL;
        private System.Windows.Forms.TextBox txtPrompt;
    }
}

