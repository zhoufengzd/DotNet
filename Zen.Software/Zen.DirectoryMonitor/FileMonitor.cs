using System.Collections.Generic;
using System.IO;
using Zen.Common.Def;
using Zen.Common.Def.Layout;
using Zen.Utilities.Generics;
using Zen.Utilities.Threading;

namespace Zen.DirectoryMonitor
{
    public sealed class MonitorOpt
    {
        public MonitorOpt()
        {
        }

        [DirBrowserHint]
        public string QueryDirectory
        {
            get { return _queryDirectory; }
            set { _queryDirectory = value; }
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public bool IncludeSubdirectories
        {
            get { return _includeSubdirectories; }
            set { _includeSubdirectories = value; }
        }

        public List<NotifyFilters> NofityFilterList
        {
            get { return _nofityFilterList; }
            set { _nofityFilterList = value; }
        }

        #region private data
        private string _queryDirectory;
        private string _filter = "*.*";
        private bool _includeSubdirectories;
        private List<NotifyFilters> _nofityFilterList = new List<NotifyFilters>(new NotifyFilters[] { NotifyFilters.FileName, NotifyFilters.Security });
        #endregion
    }

    /// <summary>
    /// File monitor are callbacks for FileSystemWatcher thread. 
    ///   By inherit from ControllerBase, it could be called from standard controller UI.
    /// </summary>
    public sealed class FileMonitor : ControllerBase
    {
        public FileMonitor()
        {
            LoadControllers();
        }

        public MonitorOpt Option
        {
            get { return _option; }
            set { _option = value; }
        }

        public Dictionary<string, int> CreatedLog
        {
            get { return _createdLog; }
        }
        public Dictionary<string, int> ChangedLog
        {
            get { return _changedLog; }
        }
        public Dictionary<string, int> DeletedLog
        {
            get { return _deletedLog; }
        }
        public Dictionary<string, string> RenamedLog
        {
            get { return _renamedLog; }
        }

        #region protected functions
        protected override State RequestAction(ActionRequest request)
        {
            if (!IsActionAllowed(request))
                return _currentState;

            _latestRequest = request;
            switch (request)
            {
                case ActionRequest.Start:
                    Reset();
                    _currentState = State.Running;
                    _fileWatcher.EnableRaisingEvents = true;
                    break;

                case ActionRequest.Pause:
                    _currentState = State.Paused;
                    _fileWatcher.EnableRaisingEvents = false;
                    break;

                case ActionRequest.Stop:
                    _currentState = State.Stopped;
                    _fileWatcher.EnableRaisingEvents = false;
                    break;

                default:
                    break;
            }
            return _currentState;
        }
        #endregion

        #region private functions
        private void LoadControllers()
        {
            if (_fileWatcher != null)
                return;

            _createdLog = new Dictionary<string, int>();
            _changedLog = new Dictionary<string, int>();
            _deletedLog = new Dictionary<string, int>();
            _renamedLog = new Dictionary<string, string>();

            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
            _fileWatcher.Deleted += new FileSystemEventHandler(OnFileDeleted);
            _fileWatcher.Created += new FileSystemEventHandler(OnFileCreated);
            _fileWatcher.Renamed += new RenamedEventHandler(OnFileRenamed);
        }

        private void Reset()
        {
            _fileWatcher.Path = _option.QueryDirectory;
            _fileWatcher.Filter = _option.Filter;
            _fileWatcher.IncludeSubdirectories = _option.IncludeSubdirectories;
            _fileWatcher.NotifyFilter = (NotifyFilters)EnumConverter.ToBitFlag(_option.NofityFilterList);

            ClearLog();
        }

        private void ClearLog()
        {
            _createdLog.Clear();
            _changedLog.Clear();
            _deletedLog.Clear();
            _renamedLog.Clear();
        }

        private void OnFileRenamed(object source, RenamedEventArgs e)
        {
            _renamedLog.Add(e.OldFullPath, e.FullPath);
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            if (!_changedLog.ContainsKey(fullPath))
                _changedLog.Add(fullPath, 1);
            else
                _changedLog[fullPath] += 1;
        }

        private void OnFileDeleted(object source, FileSystemEventArgs e)
        {
            _deletedLog.Add(e.FullPath, 1);
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            _createdLog.Add(e.FullPath, 1);
        }
        #endregion

        #region private data
        private MonitorOpt _option;
        private FileSystemWatcher _fileWatcher;

        private Dictionary<string, int> _createdLog;
        private Dictionary<string, int> _changedLog;
        private Dictionary<string, int> _deletedLog;
        private Dictionary<string, string> _renamedLog;
        #endregion
    }
}
