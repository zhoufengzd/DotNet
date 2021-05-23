using Zen.Utilities.Threading;

namespace Zen.Utilities.Service
{
    /// <summary>
    /// Provide default implementation of service.
    ///     Support routinely check stop/ pause request
    ///     Expected service work item: small amount of work + one item at a time.
    /// </summary>
    public class SvcWorkerBase : ControllerBase
    {
        static readonly int DefaultPauseInterval = Zen.Common.Def.TimeUnit.HalfSecond;

        #region protected functions
        protected override void DoRun()
        {
            _currentState = State.Running;

            while (true)
            {
                if (_latestRequest == ActionRequest.Stop)
                    break;

                // Pauses (sleeps) if user requests
                while (_latestRequest == ActionRequest.Pause)
                {
                    _currentState = State.Paused;
                    System.Threading.Thread.Sleep(DefaultPauseInterval);
                }
                _currentState = State.Running;

                //Check 'Stop' request one more time (Senario: Pause -> Stop)
                if (_latestRequest == ActionRequest.Stop)
                    break;

                RunWorkItem();
            }

            _currentState = State.Stopped;
        }

        protected virtual void RunWorkItem() { }
        #endregion
    }
}