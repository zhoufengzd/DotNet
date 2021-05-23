using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Zen.Utilities.Proc;

namespace Zen.DirectoryMonitor
{
    /// <summary>
	/// Summary description for DirectoryMonitor.
	/// </summary>
	public partial class MainDlg : Form
	{
        public MainDlg()
		{
            _fileMonitor = new FileMonitor();
			InitializeComponent();
		}

        private void OnLoad(object sender, EventArgs e)
        {
            _configMgr = new ConfigurationMgr();

            _configMgr.LoadProfile(out _option);
            _fileMonitor.Option = _option;
            _optionsCtrl.SetObject(_option);

            _refreshTimer = new Timer();
            _refreshTimer.Tick += new EventHandler(OnTimer);
            _refreshTimer.Enabled = false;
            _refreshTimer.Interval = 1500;
        }

		private void OnStart(object sender, EventArgs e)
		{
            if (!_optionsCtrl.OnOk())
                return;

            _fileMonitor.Option = _option;
            _configMgr.SaveProfile(_option);

            _fileMonitor.Start();
            _refreshTimer.Start();	
		}

        private void OnStop(object sender, EventArgs e)
        {
            _fileMonitor.Stop();
            _refreshTimer.Stop();
        }

        private void OnClearLog(object sender, EventArgs e)
        {
            _textBoxOut.Text = string.Empty;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            StringBuilder logBuffer = new StringBuilder();
            string queryDir = _option.QueryDirectory + "\\";

            //logBuffer.AppendLine("---------------------------------");
            //logBuffer.AppendLine("The following files are created: ");
            //logBuffer.AppendLine("---------------------------------");
            //foreach (KeyValuePair<string, int> kv in _fileMonitor.CreatedLog)
            //    logBuffer.AppendFormat("{0} created.\r\n", kv.Key);

            logBuffer.AppendLine("---------------------------------");
            logBuffer.AppendLine("The following files are changed: ");
            logBuffer.AppendLine("---------------------------------");
            foreach (KeyValuePair<string, int> kv in _fileMonitor.ChangedLog)
            {
                logBuffer.AppendFormat("{0} changed {1} time(s).\r\n",
                        kv.Key.Replace(queryDir, string.Empty), kv.Value);
            }

            //logBuffer.AppendLine("---------------------------------");
            //logBuffer.AppendLine("The following files are deleted: ");
            //logBuffer.AppendLine("---------------------------------");
            //foreach (KeyValuePair<string, int> kv in _fileMonitor.DeletedLog)
            //    logBuffer.AppendFormat("{0} deleted.\r\n", kv.Key);

            _textBoxOut.Text = logBuffer.ToString();
        }

        private FileMonitor _fileMonitor;
        private MonitorOpt _option;
        private ConfigurationMgr _configMgr;
        private Timer _refreshTimer;
    }
}

