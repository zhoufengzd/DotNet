namespace Cassini.WebServer
{
    partial class AdminForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.logoPanel = new System.Windows.Forms.Panel();
            this.logoLabel = new System.Windows.Forms.Label();
            this.appDirLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.vrootLabel = new System.Windows.Forms.Label();
            this._browseLabel = new System.Windows.Forms.Label();
            this._appDirTextBox = new System.Windows.Forms.TextBox();
            this._portTextBox = new System.Windows.Forms.TextBox();
            this._vrootTextBox = new System.Windows.Forms.TextBox();
            this._browseLink = new System.Windows.Forms.LinkLabel();
            this._startButton = new System.Windows.Forms.Button();
            this._stopButton = new System.Windows.Forms.Button();
            this._fileBrowserButton = new System.Windows.Forms.Button();
            this.logoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // logoPanel
            // 
            this.logoPanel.BackColor = System.Drawing.SystemColors.Info;
            this.logoPanel.Controls.Add(this.logoLabel);
            this.logoPanel.Location = new System.Drawing.Point(-1, 1);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(562, 100);
            this.logoPanel.TabIndex = 9;
            // 
            // logoLabel
            // 
            this.logoLabel.AutoSize = true;
            this.logoLabel.BackColor = System.Drawing.Color.Transparent;
            this.logoLabel.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoLabel.Location = new System.Drawing.Point(24, 24);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(342, 28);
            this.logoLabel.TabIndex = 0;
            this.logoLabel.Text = "Cassini Personal Web Server";
            this.logoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // appDirLabel
            // 
            this.appDirLabel.AutoSize = true;
            this.appDirLabel.BackColor = System.Drawing.Color.Transparent;
            this.appDirLabel.Location = new System.Drawing.Point(25, 116);
            this.appDirLabel.Name = "appDirLabel";
            this.appDirLabel.Size = new System.Drawing.Size(107, 13);
            this.appDirLabel.TabIndex = 10;
            this.appDirLabel.Text = "Application &Directory:";
            this.appDirLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.BackColor = System.Drawing.Color.Transparent;
            this.portLabel.Location = new System.Drawing.Point(25, 144);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(63, 13);
            this.portLabel.TabIndex = 8;
            this.portLabel.Text = "Server &Port:";
            this.portLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // vrootLabel
            // 
            this.vrootLabel.AutoSize = true;
            this.vrootLabel.BackColor = System.Drawing.Color.Transparent;
            this.vrootLabel.Location = new System.Drawing.Point(272, 144);
            this.vrootLabel.Name = "vrootLabel";
            this.vrootLabel.Size = new System.Drawing.Size(65, 13);
            this.vrootLabel.TabIndex = 7;
            this.vrootLabel.Text = "Virtual &Root:";
            this.vrootLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // browseLabel
            // 
            this._browseLabel.AutoSize = true;
            this._browseLabel.BackColor = System.Drawing.Color.Transparent;
            this._browseLabel.Location = new System.Drawing.Point(25, 169);
            this._browseLabel.Name = "browseLabel";
            this._browseLabel.Size = new System.Drawing.Size(83, 13);
            this._browseLabel.TabIndex = 6;
            this._browseLabel.Text = "Click to Browse:";
            this._browseLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _appDirTextBox
            // 
            this._appDirTextBox.Location = new System.Drawing.Point(138, 113);
            this._appDirTextBox.Name = "_appDirTextBox";
            this._appDirTextBox.Size = new System.Drawing.Size(354, 20);
            this._appDirTextBox.TabIndex = 0;
            // 
            // portTextBox
            // 
            this._portTextBox.Location = new System.Drawing.Point(138, 141);
            this._portTextBox.Name = "portTextBox";
            this._portTextBox.Size = new System.Drawing.Size(85, 20);
            this._portTextBox.TabIndex = 1;
            // 
            // vrootTextBox
            // 
            this._vrootTextBox.Location = new System.Drawing.Point(343, 141);
            this._vrootTextBox.Name = "vrootTextBox";
            this._vrootTextBox.Size = new System.Drawing.Size(185, 20);
            this._vrootTextBox.TabIndex = 2;
            // 
            // browseLink
            // 
            this._browseLink.AutoSize = true;
            this._browseLink.Location = new System.Drawing.Point(135, 169);
            this._browseLink.Name = "browseLink";
            this._browseLink.Size = new System.Drawing.Size(85, 13);
            this._browseLink.TabIndex = 3;
            this._browseLink.TabStop = true;
            this._browseLink.Text = "http://localhost/";
            this._browseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // startButton
            // 
            this._startButton.Location = new System.Drawing.Point(360, 213);
            this._startButton.Name = "startButton";
            this._startButton.Size = new System.Drawing.Size(75, 23);
            this._startButton.TabIndex = 4;
            this._startButton.Text = "&Start";
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this.OnStartButtonClick);
            // 
            // stopButton
            // 
            this._stopButton.Location = new System.Drawing.Point(451, 213);
            this._stopButton.Name = "stopButton";
            this._stopButton.Size = new System.Drawing.Size(75, 23);
            this._stopButton.TabIndex = 5;
            this._stopButton.Text = "Sto&p";
            this._stopButton.UseVisualStyleBackColor = true;
            this._stopButton.Click += new System.EventHandler(this.OnStopButtonClick);
            // 
            // browseButton
            // 
            this._fileBrowserButton.Location = new System.Drawing.Point(498, 113);
            this._fileBrowserButton.Name = "browseButton";
            this._fileBrowserButton.Size = new System.Drawing.Size(30, 23);
            this._fileBrowserButton.TabIndex = 11;
            this._fileBrowserButton.Text = "...";
            this._fileBrowserButton.UseVisualStyleBackColor = true;
            this._fileBrowserButton.Click += new System.EventHandler(this.OnBrowse);
            // 
            // AdminForm
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(560, 312);
            this.Controls.Add(this._fileBrowserButton);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._startButton);
            this.Controls.Add(this._browseLink);
            this.Controls.Add(this._vrootTextBox);
            this.Controls.Add(this._portTextBox);
            this.Controls.Add(this._appDirTextBox);
            this.Controls.Add(this._browseLabel);
            this.Controls.Add(this.vrootLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.logoPanel);
            this.Controls.Add(this.appDirLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AdminForm";
            this.Text = "Cassini Personal Web Server";
            this.Load += new System.EventHandler(this.OnLoad);
            this.logoPanel.ResumeLayout(false);
            this.logoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Label appDirLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label vrootLabel;
        private System.Windows.Forms.Label _browseLabel;
        private System.Windows.Forms.TextBox _appDirTextBox;
        private System.Windows.Forms.TextBox _portTextBox;
        private System.Windows.Forms.TextBox _vrootTextBox;
        private System.Windows.Forms.LinkLabel _browseLink;
        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.Button _stopButton;
        private System.Windows.Forms.Button _fileBrowserButton;

    }
}

