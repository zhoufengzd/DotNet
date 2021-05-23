using System.Collections.Generic;
using System.IO;
using Zen.Utilities.Threading;

namespace Zen.Search.Util.DTS
{
    /// <summary>
    /// Must be thread safe
    /// </summary>
    public class DTFileSource : dtSearch.Engine.DataSource
    {
        public DTFileSource(IEnumerable<string> dataFiles)
        {
            _dataFiles = new Stack<string>(dataFiles);
        }

        public override bool Rewind()
        {
            return true;
        }
        public override bool GetNextDoc()
        {
            if (_latestRequest == ActionRequest.Stop || (_curDocCount >= _dataFiles.Count))
                return false;

            while (_latestRequest == ActionRequest.Pause)
            {
                System.Threading.Thread.Sleep(250);
            }

            SetDocProperties(_dataFiles.Pop());

            _curDocCount++;
            return true;
        }

        public bool Start()
        {
            _latestRequest = ActionRequest.Start;
            return true;
        }
        public bool Stop()
        {
            _latestRequest = ActionRequest.Stop;
            return true;
        }
        public bool Pause()
        {
            _latestRequest = ActionRequest.Pause;
            return true;
        }

        public int RecordProcessed
        {
            get { return _curDocCount; }
        }

        #region private functions
        private void SetDocProperties(string filePath)
        {
            DocName = filePath;
            DocIsFile = false; // If true dtSearch will try to open the file
            DocDisplayName = Path.GetFileName(filePath);

            FileInfo fi = new FileInfo(filePath);
            DocCreatedDate = fi.CreationTime;
            DocModifiedDate = fi.LastWriteTime;

            using (StreamReader reader = new StreamReader(filePath))
            {
                DocText = reader.ReadToEnd();
            }
        }

        #endregion

        #region private data
        private Stack<string> _dataFiles;
        private int _curDocCount = 0;
        protected ActionRequest _latestRequest = ActionRequest.Unspecified;
        #endregion
    }
}