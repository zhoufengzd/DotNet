using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.UIControls.Misc;
using Zen.Utilities.Service;

namespace Zen.WinSvcController
{

    public partial class SvcControllerDlg : Form
    {
        public SvcControllerDlg()
        {
            InitializeComponent();
        }

        #region Form Load
        private void OnFormLoad(object sender, EventArgs e)
        {
            this.SuspendLayout();

            LoadControllers();
            _comboBoxServiceNames.DataSource = _svcDisplayNameList;
            _fileDirBrowserCtrl.Options.Filter = FileTypeFilter.ExeFile;

            this.ResumeLayout(false);
        }
        #endregion

        #region Running service events
        private void OnSelectedSvcChanged(object sender, EventArgs e)
        {
            string svcName = _svcNameMap[_comboBoxServiceNames.SelectedItem.ToString()];
            _controller = new SvcsManager(svcName);
            SetButtonStatus();
        }

        private void OnStart(object sender, System.EventArgs e)
        {
            if (!_controller.Start())
            {
                MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            SetButtonStatus();
        }

        private void OnStop(object sender, System.EventArgs e)
        {
            if (!_controller.Stop())
            {
                MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            SetButtonStatus();
        }

        private void OnPause(object sender, System.EventArgs e)
        {
            if (!_controller.Pause())
            {
                MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            SetButtonStatus();
        }

        private void OnEnable(object sender, EventArgs e)
        {
            if (!_controller.Enable())
            {
                MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            SetButtonStatus();
        }

        private void OnDisable(object sender, EventArgs e)
        {
            if (!_controller.Disable())
            {
                MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            SetButtonStatus();
        }

        private void SetButtonStatus()
        {
            if (_controller.Enabled)
            {
                _buttonDisable.Enabled = true;
                _buttonEnable.Enabled = false;
            }
            else
            {
                _buttonDisable.Enabled = false;
                _buttonEnable.Enabled = true;
            }

            ServiceController inner = _controller.Inner;
            ServiceControllerStatus svrStatus = inner.Status;
            if (svrStatus == ServiceControllerStatus.Running)
            {
                _buttonPause.Enabled = inner.CanPauseAndContinue ? true : false;
                _buttonStop.Enabled = inner.CanStop ? true : false;
                _buttonStart.Enabled = false;
            }
            else if (svrStatus == ServiceControllerStatus.Paused)
            {
                _buttonStart.Enabled = true;
                _buttonPause.Enabled = false;
                _buttonStop.Enabled = inner.CanStop ? true : false;
            }
            else if (svrStatus == ServiceControllerStatus.Stopped)
            {
                _buttonStart.Enabled = true;
                _buttonPause.Enabled = false;
                _buttonStop.Enabled = false;
            }
        }

        private void LoadControllers()
        {
            if (_installer != null)
                return;

            LoadServiceList();
            _installer = new SvcInstallerCmd();
        }

        private void LoadServiceList()
        {
            _controller = null;

            ServiceController[] services = ServiceController.GetServices();
            _svcDisplayNameList = new List<string>(services.Length);
            _svcNameMap = new Dictionary<string, string>(services.Length);

            foreach (ServiceController svc in services)
            {
                _svcNameMap.Add(svc.DisplayName, svc.ServiceName);
                _svcDisplayNameList.Add(svc.DisplayName);
            }
        }

        private void RefreshServiceList()
        {
            LoadServiceList();
            _comboBoxServiceNames.DataSource = _svcDisplayNameList;
        }
        #endregion

        #region Install / UnInstall events
        private void OnInstall(object sender, EventArgs e)
        {
            if (!_fileDirBrowserCtrl.OnOk())
                return;

            if (_installer.Install(_fileDirBrowserCtrl.Text))
                RefreshServiceList();

            MessageBoxEx.ShowInfo(_installer.Output, this.Text);
        }
        private void OnUnInstall(object sender, EventArgs e)
        {
            if (!_fileDirBrowserCtrl.OnOk())
                return;

            if (_installer.UnInstall(_fileDirBrowserCtrl.Text))
                RefreshServiceList();

            MessageBoxEx.ShowInfo(_installer.Output, this.Text);
        }
        #endregion

        #region private data
        private Dictionary<string, string> _svcNameMap;
        private List<string> _svcDisplayNameList;
        private SvcsManager _controller;

        private SvcInstallerCmd _installer;
        #endregion

    }
}