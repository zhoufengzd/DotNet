
namespace Zen.WinSvcController
{
    using SvcWorkerBase = Zen.Utilities.Service.SvcWorkerBase;

    /// <summary>
    /// Task Dispatcher
    /// </summary>
    public class TaskDispatcherT : SvcWorkerBase
    {
        public TaskDispatcherT()
        {
        }


        #region private functions
        /// <summary>
        /// Check task request, run tasks
        /// </summary>
        protected override void RunWorkItem()
        {
            System.Threading.Thread.Sleep(1000);
        }

        private void DispatchTasks()
        {
            //// Pause if user requests 'Pause'
            //while (true)
            //{
            //    if (_latestRequest == ActionRequest.Pause || _latestRequest == ActionRequest.Stop)
            //        break;
            //    System.Threading.Thread.Sleep(DefaultInterval);
            //}
        }
        #endregion

        #region private data
        //private System.Timers.Timer _timer;
        #endregion
    }
}