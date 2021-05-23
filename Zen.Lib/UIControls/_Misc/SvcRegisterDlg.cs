using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Zen.UIControls
{
    partial class SvcRegisterDlg : Form
    {
        const string UserPasswdFmt = "/Username=\"{0}\" /Password=\"{1}\" ";
        static readonly string InstallUtilCmd = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "installutil.exe");
        static readonly string WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

        public SvcRegisterDlg(IEnumerable<string> serviceExes)
            : this(serviceExes, "Service Installation...")
        {
        }
        public SvcRegisterDlg(IEnumerable<string> serviceExes, string formTitle)
        {
            _serviceExecutables = serviceExes;
            this.Text = formTitle;

            InitializeComponent();
        }

        #region private functions
        private void OnFormLoad(object sender, EventArgs e)
        {
            _svcLogonCtrl.ErrorMsg = "Warning: User and Password required for service account!";
            _radioButtonWinUser.Checked = true;
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            _svcLogonCtrl.Enabled = _radioButtonWinUser.Checked;
        }

        private void OnInstall(object sender, EventArgs e)
        {
            try
            {
                string logOnString = null;
                if (_radioButtonWinUser.Checked)
                {
                    if (!_svcLogonCtrl.OnOk())
                        return;

                    logOnString = string.Format(UserPasswdFmt, _svcLogonCtrl.UserName, _svcLogonCtrl.PassWord);
                }

                StringBuilder errorMsg = new StringBuilder();
                errorMsg.Append("Service(s) installation failed.");
                if (_radioButtonWinUser.Checked)
                {
                    errorMsg.Append("  Please check to be sure you have entered a valid Windows username and password.");
                }
                errorMsg.Append("  Note that if these services are already installed, you must remove them before running this installation.");

                foreach (string exe in _serviceExecutables)
                {
                    if (!InstallSvc(exe, logOnString))
                    {
                        MessageBox.Show(errorMsg.ToString());
                        return;
                    }
                }

                this.Visible = false;
                MessageBox.Show("Successfully installed service(s).");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Service installing failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnUnInstall(object sender, EventArgs e)
        {

        }

        private bool InstallSvc(string svcName, string logOnString)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(InstallUtilCmd);
            startInfo.Arguments = (logOnString == null)? svcName : (logOnString + " " + svcName);
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = WorkingDirectory;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process installProc = Process.Start(startInfo);
            string output = installProc.StandardOutput.ReadToEnd();
            installProc.WaitForExit();

            return (installProc.ExitCode > -1);
        }

        #endregion

        #region private variables
        private IEnumerable<string> _serviceExecutables;
        #endregion
    }
}