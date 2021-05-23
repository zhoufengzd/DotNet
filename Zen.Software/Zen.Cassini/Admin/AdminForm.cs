using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Cassini.WebServer
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        public AdminForm(String[] args)
        {
            InitializeComponent();
            ParseArgs(args);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (_appPath.Length > 0)
            {
                Start();
            }
            else
            {
                _appDirTextBox.Focus();
            }
        }

        #region private methods
        private void OnBrowse(object sender, EventArgs e)
        {
            FolderBrowserDialog dirOpenDlg = new FolderBrowserDialog();
            if (dirOpenDlg.ShowDialog() != DialogResult.OK)
                return;

            _appDirTextBox.Text = dirOpenDlg.SelectedPath;
        }

        private void OnLinkClick(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            _browseLink.Links[_browseLink.Links.IndexOf(e.Link)].Visited = true;
            System.Diagnostics.Process.Start(_browseLink.Text);
        }

        private void OnStartButtonClick(object sender, System.EventArgs e)
        {
            Start();
        }

        private void OnStopButtonClick(object sender, System.EventArgs e)
        {
            Stop();
        }

        private void Start()
        {
            _appPath = _appDirTextBox.Text;
            if (_appPath.Length == 0 || !Directory.Exists(_appPath))
            {
                ShowError("Invalid Application Directory");
                _appDirTextBox.SelectAll();
                _appDirTextBox.Focus();
                return;
            }

            _portString = _portTextBox.Text;
            int portNumber = -1;
            try
            {
                portNumber = Int32.Parse(_portString);
            }
            catch
            {
            }
            if (portNumber <= 0)
            {
                ShowError("Invalid Port");
                _portTextBox.SelectAll();
                _portTextBox.Focus();
                return;
            }

            _virtRoot = _vrootTextBox.Text;
            if (_virtRoot.Length == 0 || !_virtRoot.StartsWith("/"))
            {
                ShowError("Invalid Virtual Root");
                _vrootTextBox.SelectAll();
                _vrootTextBox.Focus();
                return;
            }

            try
            {
                _server = new Cassini.Server(portNumber, _virtRoot, _appPath);
                _server.Start();
            }
            catch
            {
                ShowError(
                    "Cassini Managed Web Server failed to start listening on port " + portNumber + ".\r\n" +
                    "Possible conflict with another Web Server on the same port.");
                _portTextBox.SelectAll();
                _portTextBox.Focus();
                return;
            }

            _browseLink.Text = GetLinkText();
            EnableSettingCtrls(false);
        }

        private void Stop()
        {
            try
            {
                if (_server != null)
                {
                    _server.Stop();
                    _server = null;
                }
            }
            catch (Exception)
            {
                throw;
            }

            EnableSettingCtrls(true);
            //Close();
        }

        private void EnableSettingCtrls(bool enable)
        {
            if (enable)
            {
                _appDirTextBox.Enabled = true;
                _portTextBox.Enabled = true;
                _vrootTextBox.Enabled = true;

                _browseLabel.Visible = false;
                _browseLink.Visible = false;

                _startButton.Enabled = true;
                _stopButton.Enabled = false;
            }
            else
            {
                _appDirTextBox.Enabled = false;
                _portTextBox.Enabled = false;
                _vrootTextBox.Enabled = false;

                _browseLabel.Visible = true;                
                _browseLink.Visible = true;
                _browseLink.Focus();

                _startButton.Enabled = false;
                _stopButton.Enabled = true;
            }
        }

        private String GetLinkText()
        {
            String url = "http://localhost";
            if (_portString != "80")
                url += ":" + _portString;
            url += _virtRoot;
            if (!url.EndsWith("/"))
                url += "/";
            return url;
        }

        private void ParseArgs(String[] args)
        {
            try
            {
                if (args.Length >= 1)
                    _appPath = args[0];

                if (args.Length >= 2)
                    _portString = args[1];

                if (args.Length >= 3)
                    _virtRoot = args[2];
            }
            catch
            {
            }

            if (_portString == null)
                _portString = "80";

            if (_virtRoot == null)
                _virtRoot = "/";

            if (_appPath == null)
                _appPath = String.Empty;
        }

        private void ShowError(String err)
        {
            MessageBox.Show(err, "Cassini Personal Web Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region private data
        // web server settings
        private String _appPath;
        private String _portString;
        private String _virtRoot;

        // the web server
        private Cassini.Server _server;
        #endregion
    }
}