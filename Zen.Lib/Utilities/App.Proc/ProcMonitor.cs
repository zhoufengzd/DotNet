using System;
using System.Diagnostics;
using Zen.Common.Def;

namespace Zen.Utilities.Proc
{
    /// <summary>
    /// Monitor running process & try recovering?
    ///   1. Memory usage; 2. UI responding; 
    /// </summary>
    public sealed class ProcMonitor : IDisposable
    {
        public ProcMonitor()
        {
            _proc = Process.GetCurrentProcess();
        }
        ~ProcMonitor()
        {
            DoDispose();
        }
        public void Dispose()
        {
            DoDispose();
        }

        public string ProcessName
        {
            get { return _proc.ProcessName; }
        }
        public DateTime StartTime
        {
            get { return _proc.StartTime; }
        }
        public float PrivateMemoryKB
        {
            get { return _proc.PrivateMemorySize64 / (float)ByteUnit.KiloByte; }
        }
        public bool IsUIResponding 
        {
            get { return _proc.Responding; }
        }
        public int WorkerThreadsCount
        {
            get
            {
                int wkThreadsCount, ioThreadsCount;
                System.Threading.ThreadPool.GetAvailableThreads(out wkThreadsCount, out ioThreadsCount);
                return wkThreadsCount; 
            }
        }
        public int IOThreadsCount
        {
            get
            {
                int wkThreadsCount, ioThreadsCount;
                System.Threading.ThreadPool.GetAvailableThreads(out wkThreadsCount, out ioThreadsCount);
                return ioThreadsCount;
            }
        }
        
        #region private functions
        private void DoDispose()
        {
            if (_proc == null)
                return;
            
            _proc.Dispose();
            _proc = null;
        }
        #endregion

        #region private data
        private Process _proc;
        #endregion
    }
}
