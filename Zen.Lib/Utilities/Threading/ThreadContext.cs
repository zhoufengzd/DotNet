using System;

namespace Zen.Utilities.Threading
{
    /// Controller Thread: like UI thread
    /// Worker Thread: 

    /// <summary>
    /// Simple shared context by reference. No lock defined.
    ///   Implicitly assuming: 1 writer <--> 1+ reader
    /// </summary>
    public class ThreadContext
    {
        public ThreadContext()
        {
        }

        public virtual void Reset()
        {
            _stopRequested = false;
            _pauseRequested = false;
            _percentage = 0;
            _error = null;
        }

        public virtual bool StopRequested
        {
            get { return _stopRequested; }
            set { _stopRequested = value; }
        }
        public virtual bool PauseRequested
        {
            get { return _pauseRequested; }
            set { _pauseRequested = value; }
        }
        public virtual int Percentage
        {
            get { return _percentage; }
            set { _percentage = value < 0 ? 0 : value; }
        }
        public virtual Exception Error
        {
            get { return _error; }
            set { _error = value; }
        }

        #region private data
        protected bool _stopRequested;
        protected bool _pauseRequested;
        protected int _percentage;
        protected Exception _error;
        #endregion
    }

    public abstract class RunItem : IRunable
    {
        public RunItem(ThreadContext threadContext)
        {
            _threadContext = threadContext;
        }

        public ThreadContext ThreadContext
        {
            get { return _threadContext; }
        }

        public abstract bool Run();

        #region protected data
        protected ThreadContext _threadContext;
        #endregion
    }

}
