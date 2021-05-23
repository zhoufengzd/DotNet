using System;
using System.Collections.Generic;
using System.Threading;

namespace Zen.Utilities.Threading
{
    /// <summary>
    /// Synchronize main & backend thread when exists the application.
    /// </summary>
    public sealed class ThreadCoordinator
    {
        public ThreadCoordinator()
        {
            _threadMap = new Dictionary<int, Thread>();
        }

        public void RegisterThread(Thread thread)
        {
            int threadId = thread.ManagedThreadId;
            if (!_threadMap.ContainsKey(threadId))
            {
                _threadMap.Add(threadId, thread);
            }
        }

        public bool TryQuit()
        {
            foreach (KeyValuePair<int, Thread> kvp in _threadMap)
            {
                if (kvp.Value.ThreadState == ThreadState.Running)
                {
                    return false;
                }
            }

            return true;
        }

        private Dictionary<int, Thread> _threadMap;
    }

}
