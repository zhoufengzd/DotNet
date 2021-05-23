using System;
using System.Drawing;
using System.Drawing.Printing;

using System.Diagnostics;

namespace Zen.Utilities.Printing
{
    using WorkItem = Zen.Utilities.Threading.RunItem;
    using ThreadContext = Zen.Utilities.Threading.ThreadContext;

    /// <summary>
    /// Service provided:
    ///   1. Batch printing
    ///   2. Page count
    ///   3. Thread aware.    
    /// </summary>
    public sealed class PrintController : WorkItem
    {
        public PrintController(ThreadContext threadContext)
            :base(threadContext)
        {
            _worker = new PrintWorker(threadContext);
        }

        public PrintDocument PrintDoc
        {
            set 
            {
                _printDoc = value;
                _printDoc.PrintController = _worker;
            }
        }

        public int PageCount
        {
            get { return _worker.PageCount; }
        }

        /// <summary> Returns total number of pages printed </summary>
        public override bool Run()
        {
            _printDoc.Print();
            return true;
        }

        #region private functions
        #endregion

        #region private data
        private PrintWorker _worker;
        private PrintDocument _printDoc;

        #endregion
    }

    #region internal helper
    /// <summary>
    /// Provided additional services (besides what's availabe from StandardPrintController):
    ///   1. PageCount      2. Cancel event.
    /// </summary>
    internal sealed class PrintWorker : StandardPrintController
    {
        public PrintWorker(ThreadContext threadContext)
        {
            _threadContext = threadContext;
            _pageCount = 0;
        }

        public int PageCount
        {
            get { return _pageCount; }
        }

        public override void OnStartPrint(PrintDocument printDoc, PrintEventArgs e)
        {
            _pageCount = 1;
            base.OnStartPrint(printDoc, e);
        }

        public override Graphics OnStartPage(PrintDocument printDoc, PrintPageEventArgs e)
        {
            if (_threadContext.StopRequested)
                e.Cancel = true;

            return base.OnStartPage(printDoc, e);
        }

        public override void OnEndPage(PrintDocument printDoc, PrintPageEventArgs e)
        {
            _pageCount++;
            if (_threadContext.StopRequested)
                e.Cancel = true;

            base.OnEndPage(printDoc, e);
        }

        public override void OnEndPrint(PrintDocument printDoc, PrintEventArgs e)
        {
            if (_threadContext.StopRequested)
                e.Cancel = true;

            base.OnEndPrint(printDoc, e);
        }

        #region private functions
        #endregion

        #region private data
        private ThreadContext _threadContext;
        private int _pageCount;
        #endregion
    }
    #endregion
}
