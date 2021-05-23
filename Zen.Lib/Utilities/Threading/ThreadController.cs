using System.Threading;

namespace Zen.Utilities.Threading
{
    /// <summary>
    /// Help to execute a runnable object in thread pool
    /// </summary>
    public sealed class ThreadRunner
    {
        /// <summary> Start runnable object in ThreadPool </summary>
        public static void Run(ThreadStart RunIt)
        {
            WaitCallback callback = new WaitCallback(delegate(object runObj) { RunIt(); });
            ThreadPool.QueueUserWorkItem(callback, null);
        }

        /// <summary> Start runnable object in ThreadPool </summary>
        public static void Run(IRunable runnable)
        {
            WaitCallback callback = new WaitCallback(delegate(object runObj) { ((IRunable)runObj).Run(); });
            ThreadPool.QueueUserWorkItem(callback, runnable);
        }

        /// <summary> Advanced: start runnable object in local thread, and return thread instance </summary>
        public static void Run(ThreadStart RunIt, out Thread worker)
        {
            worker = new Thread(delegate(object runObj) { RunIt(); });
            worker.Start();
        }

        /// <summary> Advanced: start runnable object in local thread, and return thread instance </summary>
        public static void Run(IRunable runnable, out Thread worker)
        {
            worker = new Thread(delegate(object runObj) { ((IRunable)runObj).Run(); });
            worker.Start();
        }
    }

}
