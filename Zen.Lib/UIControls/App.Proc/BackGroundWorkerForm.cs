using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Zen.UIControls.Forms
{
    using Zen.Utilities.Threading;
    /// <summary>
    /// Standardize running background worker thread in Form
    /// </summary>
    public class BackGroundWorkerForm : Form
    {
        public BackGroundWorkerForm()
        {
        }

        #region Override region. Callback from background worker.
        /// <summary> Construct new work item here</summary>
        protected virtual IRunable LoadWorkItem(BGWThreadContext ctxt) 
        {
            Debug.Assert(false);
            return null;
        }
        /// <summary> Configure work item before run if needed</summary>
        protected virtual void ResetWorkItem() { }

        protected virtual void OnProgress(int percentage) { }

        protected virtual void OnCompleted() { }

        protected virtual void OnCanceled() { }

        protected virtual void OnError(Exception ex) { }
        /// <summary>
        /// Reset / Enable controls to the intial state, like enable 'Run' button
        /// Override this if needed
        /// </summary>
        protected virtual void ResetControls() { }
        /// <summary>
        /// Disable controls when background thread is running, like disable 'Run' button
        /// Override this if needed
        /// </summary>
        protected virtual void DisableControls() { }
        #endregion

        #region Exposed worker call.
        /// <summary>
        /// Handle UI event here, like 'Run' button clicked.
        /// </summary>
        protected void RunBackground()
        {
            DisableControls();

            LoadControllers();
            ResetWorkerContext();
            ResetWorkItem();

            _backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Handle UI event here, like 'Stop' or 'Cancel' button clicked.
        /// </summary>
        protected void StopWork()
        {
            if (_workerContext != null)
                _workerContext.StopRequested = true;
        }
        #endregion

        #region Implementation detail

        #region private interactions with the background thread.
        private void LoadControllers()
        {
            if (_backgroundWorker != null)
                return;

            // Setup background worker, worker context, and work item
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;

            _backgroundWorker.DoWork += new DoWorkEventHandler(DoBackgroundWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgress);

            _workerContext = new BGWThreadContext(_backgroundWorker);
            _workItem = LoadWorkItem(_workerContext);
        }

        private void ResetWorkerContext()
        {
            _workerContext.Reset();
        }

        private void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetControls();

            if (_workerContext.Error != null)
                OnError(_workerContext.Error);
            else if (_workerContext.StopRequested)
                OnCanceled();
            else
                OnCompleted();
        }

        private void DoBackgroundWork(object sender, DoWorkEventArgs e)
        {
            Debug.Assert(_workItem != null);
            if (_workItem != null)
                _workItem.Run();
        }

        private void OnProgress(object sender, ProgressChangedEventArgs e)
        {
            OnProgress(e.ProgressPercentage);
        }
        #endregion

        #region protected controller
        protected BackgroundWorker _backgroundWorker;
        protected IRunable _workItem;
        #endregion

        #region private data
        private BGWThreadContext _workerContext;
        #endregion

        #endregion //Implementation detail
    }

    #region WorkerContext

    /// <summary>
    /// Shared context between background thread and UI thread. 
    ///   Cancel request + Percentage Completed + Error
    /// </summary>
    public sealed class BGWThreadContext: ThreadContext
    {
        public BGWThreadContext(BackgroundWorker runningThread)
        {
            _runningThread = runningThread;
        }

        public override int Percentage
        {
            get { return _percentage; }
            set
            {
                base.Percentage = value;
                if (_runningThread != null && _runningThread.IsBusy)
                    _runningThread.ReportProgress(_percentage);
            }
        }

        #region private data
        private BackgroundWorker _runningThread;
        #endregion
    }

    #endregion

    #region demo code
    #endregion

}
