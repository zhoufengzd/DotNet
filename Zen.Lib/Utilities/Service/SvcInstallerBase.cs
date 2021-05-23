using System;
using System.Configuration.Install;
using System.ServiceProcess;

using Zen.Utilities.Proc;

namespace Zen.Utilities.Service
{
    using Path = System.IO.Path;

    /// <summary>
    /// Service Installer Base Settings.
    /// </summary>
    public class SvcInstallerBase : Installer
    {
        public SvcInstallerBase(string serviceName)
        {
            _svcSettings = new SvcInstallerSettings();
            _svcSettings.ServiceName = serviceName;

            InitializeComponent();
        }

        public SvcInstallerSettings SvcInstallerSettings
        {
            get { return _svcSettings; }
            set { _svcSettings = value; }
        }

        /// <summary>
        /// Default Installation Logic
        /// </summary>
        #region Installer Functions
        private void InitializeComponent()
        {
            _serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            _serviceInstaller = new System.ServiceProcess.ServiceInstaller();

            Installers.Add(_serviceProcessInstaller);
            Installers.Add(_serviceInstaller);
        }
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Override the 'OnBeforeInstall' method.
        /// </summary>
        /// <param name="savedState"></param> 
        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            UpdateSvcSettings();
            base.OnBeforeInstall(savedState);
        }

        /// <summary>
        /// Uninstall based on the service name
        /// </summary>
        /// <PARAM name="savedState"></PARAM>
        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            UpdateSvcSettings();
            base.OnBeforeUninstall(savedState);
        }

        /// <summary>
        /// Give user a change to pass username / password via command line.
        /// </summary>
        private void UpdateSvcSettings()
        {
            string userName = TryGetParameterValue("Username");
            string passWord = null;
            if (!string.IsNullOrEmpty(userName))
                passWord = TryGetParameterValue("Password");

            if (!string.IsNullOrEmpty(passWord))
            {
                _svcSettings.Username = userName;
                _svcSettings.Password = passWord;
                _svcSettings.AccountType = ServiceAccount.User;
            }

            if (string.IsNullOrEmpty(_svcSettings.Username))
            {
                _svcSettings.Password = string.Empty;
                _svcSettings.AccountType = ServiceAccount.LocalSystem;
            }

            // 
            // _serviceProcessInstaller
            // 
            _serviceProcessInstaller.Account = _svcSettings.AccountType;
            _serviceProcessInstaller.Username = _svcSettings.Username;
            _serviceProcessInstaller.Password = _svcSettings.Password;
            // 
            // _serviceInstaller
            // 
            _serviceInstaller.ServiceName = _svcSettings.ServiceName;
            _serviceInstaller.DisplayName = _svcSettings.DisplayName;
            _serviceInstaller.Description = _svcSettings.Description;
            _serviceInstaller.StartType = _svcSettings.StartType;
            if (_svcSettings.ServicesDependedOn != null)
                _serviceInstaller.ServicesDependedOn = _svcSettings.ServicesDependedOn;
        }

        private SvcInstallerBase()
        {
        }
        /// <summary> Return the parameter value </summary>
        /// <PARAM name="key">Context parameter key</PARAM>
        /// <returns>Context parameter specified by key</returns>
        private string TryGetParameterValue(string key)
        {
            string prmValue = null;
            try
            {
                System.Collections.Specialized.StringDictionary parameters = Context.Parameters;
                if (parameters.Count < 1)
                    return prmValue;

                prmValue = parameters[key].ToString();
                prmValue.Trim();
            }
            catch (Exception)
            {
            }

            return prmValue;
        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        #region Default Installation controllers
        private SvcInstallerSettings _svcSettings;
        private System.ComponentModel.IContainer _components = null;
        private System.ServiceProcess.ServiceProcessInstaller _serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller _serviceInstaller;
        #endregion
    }

    /// <summary>
    /// Service Settings used by the Installer
    /// </summary>
    public sealed class SvcInstallerSettings
    {
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public ServiceAccount AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }
        public ServiceStartMode StartType
        {
            get { return _startType; }
            set { _startType = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string[] ServicesDependedOn
        {
            get { return _servicesDependedOn; }
            set { _servicesDependedOn = value; }
        }

        #region private data
        private string _serviceName = null;
        private string _displayName = string.Empty;
        private string _description = string.Empty;
        private ServiceAccount _accountType = ServiceAccount.LocalSystem;
        private ServiceStartMode _startType = ServiceStartMode.Automatic;
        private string _password = string.Empty;
        private string _username = string.Empty;
        private string[] _servicesDependedOn = null;
        #endregion
    }

    public sealed class SvcInstallerCmd
    {
        static readonly string InstallUtilCmd = "installutil.exe";
        static readonly string ArgLongFmt = "{0} /LogFile=\"{1}\" /Username=\"{2}\" /Password=\"{3}\" \"{4}\" ";
        static readonly string ArgShortFmt = "{0} /LogFile=\"{1}\" \"{2}\" ";

        public SvcInstallerCmd()
        {
        }

        public string Output
        {
            get { return (_exec == null) ? null : _exec.Output; }
        }

        public bool Install(string assemblyPath)
        {
            return RunCommand(assemblyPath, true, null, null);
        }

        public bool Install(string assemblyPath, string userName, string password)
        {
            return RunCommand(assemblyPath, true, userName, password);
        }

        public bool UnInstall(string assemblyPath)
        {
            return RunCommand(assemblyPath, false, null, null);
        }

        #region private functions
        private void LoadControllers()
        {
            if (_exec != null)
                return;

            _exec = new Executor();
        }

        private bool RunCommand(string assemblyPath, bool installSvc, string userName, string password)
        {
            LoadControllers();

            string assemblyFullPath = FileUtil.PathBuilder.BuildFullPath(assemblyPath, _exec.ExePathList);
            string logFile = Path.Combine(_exec.WorkingDir, DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".log");

            string arguments = null;
            string unInstallOption = installSvc ? string.Empty : "/u";
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                arguments = string.Format(ArgShortFmt, unInstallOption, logFile, assemblyFullPath);
            else
                arguments = string.Format(ArgLongFmt, unInstallOption, logFile, userName, password, assemblyFullPath);

            return _exec.RunProcess(InstallUtilCmd, arguments);
        }
        #endregion

        #region private data
        private Executor _exec;
        #endregion

    }

}