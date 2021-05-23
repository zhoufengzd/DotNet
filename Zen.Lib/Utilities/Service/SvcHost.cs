using System;
using System.Collections.Generic;
using System.ServiceProcess;

using Zen.Utilities.Threading;

namespace Zen.Utilities.Service
{
    /// Service
    ///   = Executable invoked by service management console
    ///     + Keep running (Service Item) after it started
    ///       (Monitor status + Dispatch task [Optional:]) = RunWorkItem

    /// <summary>
    /// Service Host. 
    ///   Plug in any controllable object and it becomes a service
    ///   Default handling of shutdonw / power suspend events ==> 'Stop()'
    /// </summary>
    public class SvcHost : ServiceBase
    {
        public SvcHost(string svcName, IControllable worker)
        {
            InitializeComponent();
            base.ServiceName = svcName;

            _worker = worker;
        }

        #region Service Commands
        protected override void OnStart(string[] args)
        {
            _worker.Start();
        }

        protected override void OnStop()
        {
            _worker.Stop();
        }

        protected override void OnPause()
        {
            _worker.Pause();
        }

        /// <summary> default handling of shutdonw event ==> 'Stop' </summary>
        protected override void OnShutdown()
        {
            _worker.Stop();
        }

        /// <summary> default handling of power suspend event ==> 'Stop' </summary>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            if (powerStatus == PowerBroadcastStatus.Suspend)
                _worker.Stop();

            return base.OnPowerEvent(powerStatus);
        }
        #endregion

        #region Components handling functions
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.CanStop = true;
            this.CanHandlePowerEvent = true;
            this.CanShutdown = true;
            this.CanPauseAndContinue = true; // default: we support pause
        }
        #endregion

        #region protected & private data
        private IControllable _worker;


        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer _components = null;
        #endregion
    }
}
