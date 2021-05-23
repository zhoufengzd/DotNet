using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Microsoft.Win32;

using Zen.Utilities.Threading;

namespace Zen.Utilities.Service
{
    /// <summary>
    /// Services Manager.
    ///   Provide standard controller API.
    ///   Also support 'Enable' & 'Disable' service by updating registry settings.
    /// </summary>
    public sealed class SvcsManager : IControllable
    {
        static readonly string SvcRegistryFmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{0}";
        static readonly string SvcRegistryStartUpType = "Start";
        static readonly TimeSpan DefaultTimeOut = TimeSpan.FromSeconds(5); // Default: 5 seconds
        static readonly SvcStateFacade Facade = new SvcStateFacade();

        public SvcsManager(string svcName)
        {
            _svcName = svcName;
            _svcController = new ServiceController(svcName);
            _regKeySvcStartup = string.Format(SvcRegistryFmt, svcName);

            _actionTimeOut = DefaultTimeOut;
        }

        public bool Start()
        {
            return RunTask(ServiceControllerStatus.Running);
        }
        public bool Stop()
        {
            return RunTask(ServiceControllerStatus.Stopped);
        }
        public bool Pause()
        {
            return RunTask(ServiceControllerStatus.Paused);
        }
        public State State 
        {
            get 
            {
                _svcController.Refresh();
                return Facade.ToState(_svcController.Status);
            }
        }

        public bool Enable()
        {
            return UpdateStartupMode(ServiceStartMode.Automatic);
        }
        public bool Disable()
        {
            return UpdateStartupMode(ServiceStartMode.Disabled);
        }
        public bool Enabled
        {
            get 
            {
                ServiceStartMode mode = 
                    (ServiceStartMode)Registry.GetValue(_regKeySvcStartup, SvcRegistryStartUpType, ServiceStartMode.Disabled);
                return (mode != ServiceStartMode.Disabled); 
            }
        }

        public int ActionTimeOut
        {
            set
            {
                if (value < 1)
                    _actionTimeOut = null;
                else
                    _actionTimeOut = TimeSpan.FromSeconds(value);
            }
        }
        public string LastError
        {
            get { return _lastError; }
        }

        public ServiceController Inner
        {
            get { return _svcController; }
        }
        #region private functions
        private bool RunTask(ServiceControllerStatus targetStatus)
        {
            _lastError = null;

            _svcController.Refresh();
            if (_svcController.Status == targetStatus)
                return true;

            if (!PreRunCheck(targetStatus))
            {
                _lastError = "Action is not supported.";
                return false;
            }

            try
            {
                // special case for 'Paused' 
                if (_svcController.Status == ServiceControllerStatus.Paused)
                {
                    if (targetStatus == ServiceControllerStatus.Running)
                        _svcController.Continue();
                }
                else
                {
                    ServiceController[] dependents = _svcController.DependentServices;
                    foreach (ServiceController dps in dependents)
                    {
                        if (dps.Status == targetStatus)
                            continue;

                        InvokeService(dps, targetStatus);
                    }

                    InvokeService(_svcController, targetStatus);
                }

                if (_actionTimeOut != null)
                    _svcController.WaitForStatus(targetStatus, (TimeSpan)_actionTimeOut);
                else
                    _svcController.WaitForStatus(targetStatus);
            }
            catch (Exception ex)
            {
                _lastError = ex.Message;
            }

            return string.IsNullOrEmpty(_lastError);
        }

        private bool PreRunCheck(ServiceControllerStatus targetStatus)
        {
            switch (targetStatus)
            {
                case ServiceControllerStatus.Stopped:
                    return _svcController.CanStop;
                case ServiceControllerStatus.Paused:
                    return _svcController.CanPauseAndContinue;
                case ServiceControllerStatus.Running: 
                default: 
                    return true;
            }
        }

        private void InvokeService(ServiceController controller, ServiceControllerStatus targetStatus)
        {
            switch (targetStatus)
            {
                case ServiceControllerStatus.Running: controller.Start(); break;
                case ServiceControllerStatus.Stopped: controller.Stop(); break;
                case ServiceControllerStatus.Paused: controller.Pause(); break;
                default: break;
            }
        }

        private bool UpdateStartupMode(ServiceStartMode mode)
        {
            _lastError = null;
            try
            {
                Registry.SetValue(_regKeySvcStartup, SvcRegistryStartUpType, mode, RegistryValueKind.DWord);
                _svcController.Close();
                _svcController = new ServiceController(_svcName);
            }
            catch (Exception ex)
            {
                _lastError = ex.Message;
            }

            return string.IsNullOrEmpty(_lastError);
        }
        #endregion

        #region private data
        private ServiceController _svcController;
        private string _svcName;
        private string _regKeySvcStartup;

        private TimeSpan? _actionTimeOut;
        private string _lastError;
        #endregion
    }

    internal sealed class SvcStateFacade
    {
        public SvcStateFacade()
        {
            LoadControllers();
        }

        public State ToState(ServiceControllerStatus svcStatus)
        {
            return _stateMap[svcStatus];
        }

        #region private functions
        private void LoadControllers()
        {
            if (_stateMap != null)
                return;

            _stateMap = new Dictionary<ServiceControllerStatus, State>();
            _stateMap.Add(ServiceControllerStatus.Paused, State.Paused);
            _stateMap.Add(ServiceControllerStatus.Running, State.Running);
            _stateMap.Add(ServiceControllerStatus.Stopped, State.Stopped);
            _stateMap.Add(ServiceControllerStatus.ContinuePending, State.Pending);
            _stateMap.Add(ServiceControllerStatus.PausePending, State.Pending);
            _stateMap.Add(ServiceControllerStatus.StartPending, State.Pending);
            _stateMap.Add(ServiceControllerStatus.StopPending, State.Pending);
        }

        #endregion

        #region private data
        private Dictionary<ServiceControllerStatus, State> _stateMap;
        #endregion


    }
}
