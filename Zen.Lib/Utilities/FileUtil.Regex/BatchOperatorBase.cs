using System;
using System.Collections.Generic;
using System.IO;

using Zen.Utilities.Threading;

namespace Zen.Utilities.FileUtil
{
    using Macros = Zen.Utilities.Generics.Macros;

    [Serializable]
    public sealed class BatchOptionBase<ProcOptionT> where ProcOptionT : new()
    {
        public ProcOptionT ProcessOption
        {
            get { return Macros.SafeGet(ref _procOption); }
            set { _procOption = value; }
        }

        [System.ComponentModel.Description("Define input file search options and output directory and file extension, etc.")]
        public BatchInOutOption FileOptions
        {
            get { return Macros.SafeGet(ref _fileOptions); }
            set { _fileOptions = value; }
        }

        #region private data
        private ProcOptionT _procOption;
        private BatchInOutOption _fileOptions;
        #endregion
    }

    public class BatchOperatorBase : ControllerBase
    {
        static readonly char[] PathDelimiter = new char[] { '\\', '/', ' ' };

        public BatchOperatorBase(BatchInOutOption inOutOpt)
        {
            _inOutOpt = inOutOpt;
        }

        #region protected functions
        protected override void DoRun()
        {
            _currentState = State.Running;
            DoPreRun();

            List<string> targetFiles = FileDirLister.GetFiles(_inOutOpt.InputOptions);
            foreach (string inFile in targetFiles)
            {
                // Break if user requests 'Stop'
                if (_latestRequest == ActionRequest.Stop)
                    break;

                // Pause if user requests 'Pause'
                while (_latestRequest == ActionRequest.Pause)
                {
                    _currentState = State.Paused;
                    System.Threading.Thread.Sleep(250);
                }
                _currentState = State.Running;

                //Check 'Stop' request one more time (Senario: Pause -> Stop)
                if (_latestRequest == ActionRequest.Stop)
                    break;

                ProcessFile(inFile);
            }

            DoPostRun();
            _currentState = State.Stopped;
        }

        protected virtual void DoPreRun()
        {
            string outDir = _inOutOpt.OutputOptions.OutputDirectory;
            if (string.IsNullOrEmpty(outDir))
                outDir = FileOutOption.DefaultOutDir;

            if (!Path.IsPathRooted(outDir))
                outDir = Path.Combine(Path.GetDirectoryName(_inOutOpt.InputOptions.QueryDirectory), outDir);

            Directory.CreateDirectory(outDir);
            _inOutOpt.OutputOptions.OutputDirectory = outDir;
        }

        protected virtual void DoPostRun()
        {
        }

        /// <summary>
        /// Build output directory / path similarly to input directory / path
        /// </summary>
        protected string BuildOutPath(string outputRootDir, string inputRootDir, string inFile)
        {
            string outFileDir = outputRootDir;
            string fileName = Path.GetFileName(inFile);

            string relativeDir = Path.GetDirectoryName(inFile).Replace(inputRootDir, string.Empty);
            relativeDir = relativeDir.Trim(PathDelimiter);
            if (relativeDir.Length > 0)
            {
                outFileDir = Path.Combine(outputRootDir, relativeDir);
                Directory.CreateDirectory(outFileDir);
            }

            return Path.Combine(outFileDir, fileName);
        }

        protected virtual void ProcessFile(string inputFile)
        {
        }
        #endregion

        #region protected data
        protected BatchInOutOption _inOutOpt;
        #endregion
    }
}
