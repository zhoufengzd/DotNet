using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Zen.Utilities.FileUtil
{
    using ITextStreamConsumer = Zen.Utilities.Algorithm.IDataStreamConsumer<string>;
    using ReplaceBatchOption = BatchOptionBase<ReplaceOption>;

    public sealed class TextReplacer : ITextStreamConsumer
    {
        public TextReplacer(StreamWriter writer, ReplaceOption replaceOpt)
        {
            _writer = writer;

            Debug.Assert(replaceOpt != null && replaceOpt.ActionType != ReplaceAction.Unspecified);
            _replaceOpt = replaceOpt;

            LoadControllers();
        }

        ~TextReplacer()
        {
            Close();
        }
        public void Dispose()
        {
            Close();
        }

        public void Flush()
        {
            if (_writer != null)
                _writer.Flush();
        }

        public void Close()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
            }

            _writer = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        public void Feed(string rawText)
        {
            if (!IsMatch(rawText))
            {
                _writer.WriteLine(rawText);
                return;
            }

            switch (_replaceOpt.ActionType)
            {
                case ReplaceAction.ReplaceMatched:
                    Replace(rawText); break;
                case ReplaceAction.AppendAfter:
                    _writer.WriteLine(rawText + _replaceOpt.NewValue); break;
                case ReplaceAction.AppendBefore:
                    _writer.WriteLine(_replaceOpt.NewValue + rawText); break;
                case ReplaceAction.MergeLine:
                    _writer.Write(rawText); break;
                case ReplaceAction.RemoveMatchedLine:
                    break; // do not write this line
                case ReplaceAction.KeepMatchedWord:
                    Match match = _regexIncludeFilter.Match(rawText);
                    _writer.WriteLine(match.Groups[1].Value);
                    break;
                case ReplaceAction.KeepMatchedLine:
                    _writer.WriteLine(rawText);
                    break;
                default:
                    break;
            }
        }

        #region private functions
        private void LoadControllers()
        {
            SearchOpt searchOpt = _replaceOpt.SearchOption;
            if (searchOpt.RegexSettings != null)
            {
                _regexIncludeFilter = new Regex(searchOpt.IncludeFilter, (RegexOptions)searchOpt.RegexSettings);

                if (!string.IsNullOrEmpty(searchOpt.ExcludeFilter))
                    _regexExcludeFilter = new Regex(searchOpt.ExcludeFilter, (RegexOptions)searchOpt.RegexSettings);
            }
        }

        private bool IsMatch(string rawText)
        {
            // null text skipped
            if (rawText == null || _replaceOpt.ActionType == ReplaceAction.Unspecified)
                return false;

            if (_regexIncludeFilter != null)
            {
                return (_regexIncludeFilter.IsMatch(rawText) &&
                    (_regexExcludeFilter == null || !_regexExcludeFilter.IsMatch(rawText)));
            }
            else
            {
                SearchOpt searchOpt = _replaceOpt.SearchOption;
                return (rawText.IndexOf(searchOpt.IncludeFilter) != -1 &&
                    (string.IsNullOrEmpty(searchOpt.ExcludeFilter) || rawText.IndexOf(searchOpt.ExcludeFilter) == -1));
            }
        }

        //private void UpdateText(string rawText)
        //{
        //    if (!IsMatch(rawText))
        //    {
        //        _writer.WriteLine(rawText);
        //        return;
        //    }

        //    switch (_replaceOpt.ActionType)
        //    {
        //        case ReplaceAction.ReplaceMatched:
        //            Replace(rawText); break;
        //        case ReplaceAction.AppendAfter:
        //            _writer.WriteLine(rawText + _replaceOpt.NewValue); break;
        //        case ReplaceAction.AppendBefore:
        //            _writer.WriteLine(_replaceOpt.NewValue + rawText); break;
        //        case ReplaceAction.MergeLine:
        //            _writer.Write(rawText); break;
        //        case ReplaceAction.RemoveMatchedLine:
        //            break; // do not write this line
        //        case ReplaceAction.KeepMatchedWord:
        //            Match match = _regexIncludeFilter.Match(rawText);
        //            _writer.WriteLine(match.Groups[1].Value);
        //            break; 
        //        case ReplaceAction.KeepMatchedLine:
        //            _writer.WriteLine(rawText); break;
        //            break; 
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// Only call this when rawText is matched
        /// </summary>
        private void Replace(string rawText)
        {
            string lineProcessed = null;
            if (rawText == string.Empty)
            {
                lineProcessed = _replaceOpt.NewValue;
            }
            else
            {
                if (_regexIncludeFilter != null)
                    lineProcessed = _regexIncludeFilter.Replace(rawText, _replaceOpt.NewValue);
                else
                    lineProcessed = rawText.Replace(_replaceOpt.SearchOption.IncludeFilter, _replaceOpt.NewValue);
            }

            _writer.WriteLine(lineProcessed);
        }


        #endregion

        #region private data
        private StreamWriter _writer;
        private ReplaceOption _replaceOpt;

        private Regex _regexIncludeFilter;
        private Regex _regexExcludeFilter;
        #endregion
    }

    public sealed class BatchTextReplacer : BatchOperatorBase
    {
        public BatchTextReplacer(ReplaceBatchOption batchOpt)
            : base(batchOpt.FileOptions)
        {
            Debug.Assert(batchOpt != null);
            _batchOpt = batchOpt;
        }

        #region Overriden base functions
        protected override void ProcessFile(string inputFile)
        {
            string outputFile = BuildOutPath(_inOutOpt.OutputOptions.OutputDirectory, _inOutOpt.InputOptions.QueryDirectory, inputFile);

            TxtFileStreamProvider provider = new TxtFileStreamProvider(inputFile, _batchOpt.ProcessOption.LineFeed);
            TextReplacer processor = new TextReplacer((new StreamWriter(outputFile, false)), _batchOpt.ProcessOption);
            TextDispatcher dispatcher = new TextDispatcher(provider, processor);

            dispatcher.Run();
        }
        #endregion

        #region private data
        private ReplaceBatchOption _batchOpt;
        #endregion
    }

}
