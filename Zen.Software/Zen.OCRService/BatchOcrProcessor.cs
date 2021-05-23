using System.Collections.Generic;
using System.IO;

using ModiLib;
using Zen.Utilities.FileUtil;

namespace OcrExecutor
{
    using OcrBatchOption = BatchOptionBase<OcrOption>;

    public sealed class BatchOcrProcessor : BatchOperatorBase
    {
        public BatchOcrProcessor(OcrBatchOption batchOpt)
            : base(batchOpt.FileOptions)
        {
            _batchOpt = batchOpt;

            _ocrEngine = new OcrEngine();
            _ocrEngine.OcrOption = _batchOpt.ProcessOption;
        }

        public int PageCount
        {
            get { return _pageCount; }
        }
        public int DocCount
        {
            get { return _docCount; }
        }
        public List<string> ErrorFiles
        {
            get { return _ocrEngine.ErrorFiles; }
        }

        #region Overriden base functions
        protected override void DoPreRun()
        {
            base.DoPreRun();
            _docCount = 0;
            _pageCount = 0;
            _ocrEngine.ErrorFiles.Clear();
        }

        protected override void ProcessFile(string inputFile)
        {
            string outputFile = BuildOutPath(_inOutOpt.OutputOptions.OutputDirectory, _inOutOpt.InputOptions.QueryDirectory, inputFile);
            _pageCount += _ocrEngine.Process(inputFile, Path.ChangeExtension(outputFile, ".txt"));
            _docCount++;
        }
        #endregion

        #region private data
        private OcrBatchOption _batchOpt;
        private OcrEngine _ocrEngine;
        private int _docCount;
        private int _pageCount;
        #endregion
    }

}
