using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zen.Utilities.FileUtil;
using Zen.Search.Util.DTS;
using Zen.Utilities.Threading;
using dtSearch.Engine;

namespace Zen.Search.Util
{
    public sealed class DTFileIndexOptions
    {
        public string DTConfigureDirectory
        {
            get { return _dtConfigureDir; }
            set { _dtConfigureDir = value; }
        }
        public BatchInOutOption InOutOpt
        {
            get { return _inOutOpt; }
            set { _inOutOpt = value; }
        }

        private string _dtConfigureDir;
        private BatchInOutOption _inOutOpt = new BatchInOutOption();
    }

    public sealed class FTFileIndexer : ControllerBase
    {
        public FTFileIndexer(DTFileIndexOptions options)
        {
            _options = options;
        }

        public int DocCount
        {
            get { return _fileSource.RecordProcessed; }
        }

        #region Overriden base functions
        protected override State RequestAction(ActionRequest request)
        {
            if (!IsActionAllowed(request))
                return _currentState;

            _latestRequest = request;
            if (_latestRequest == ActionRequest.Start)
            {
                _currentState = State.Running;
                if (_indexJob == null)
                {
                    if (_runThreadPool)
                        ThreadRunner.Run(DoRun);
                    else
                        DoRun();
                }
                else
                {
                    _fileSource.Start();
                }
            }
            else if (_latestRequest == ActionRequest.Stop)
            {
                _currentState = State.Stopped;
                _fileSource.Stop();
            }
            else
            {
                _currentState = State.Paused;
                _fileSource.Pause();

            }
            return _currentState;
        }

        protected override void DoRun()
        {
            _currentState = State.Running;

            List<string> files = FileDirLister.GetFiles(_options.InOutOpt.InputOptions);
            _fileSource = new DTFileSource(files);

            DTFactory factory = new DTFactory(_options.DTConfigureDirectory);
            _indexJob = factory.GetCreateIndexJob(_options.InOutOpt.OutputOptions.OutputDirectory, _fileSource);

            _indexJob.Execute();
            _currentState = State.Stopped;
        }
        #endregion

        #region private data
        private DTFileSource _fileSource;
        private IndexJob _indexJob;
        private DTFileIndexOptions _options;
        #endregion
    }

}
