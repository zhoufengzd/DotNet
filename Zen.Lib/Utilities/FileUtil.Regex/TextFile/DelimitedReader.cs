using System;
using System.Collections.Generic;
using Zen.Utilities.Text;

namespace Zen.Utilities.FileUtil
{
    using Macros = Zen.Utilities.Generics.Macros;
    using ITextStreamConsumer = Zen.Utilities.Algorithm.IDataStreamConsumer<string>;

    public sealed class DelimiterInfo
    {
        public DelimiterInfo()
        {
        }

        public DelimiterInfo(string[] delimiters, char? textQualifier, Dictionary<string, string> markMap)
        {
            _delimiters = delimiters;
            _textQualifier = textQualifier;
            _markMap = markMap;
        }

        public string[] Delimiters
        {
            get { return _delimiters; }
            set { _delimiters = value; }
        }
        /// <summary>
        /// TextQualifier: like double quote in the following example
        ///    "94030","Frank Woolwine","Business Analyst"
        /// </summary>
        public char? TextQualifier
        {
            get { return _textQualifier; }
            set { _textQualifier = value; }
        }
        public Dictionary<string, string> MarkMap
        {
            get { return Macros.SafeGet<Dictionary<string, string>>(ref _markMap); }
            set { _markMap = value; }
        }

        #region private data
        private string[] _delimiters = null;
        private char? _textQualifier;
        private Dictionary<string, string> _markMap;
        #endregion
    }

    public sealed class DelimitedReader : ITextStreamConsumer, IDisposable
    {
        public delegate void FeedTextCallback(string[] fieldData);

        /// <summary>
        /// By default, assume it's simple csv file.
        ///   simple == no text qualifier + no mark map
        /// </summary>
        public DelimitedReader()
            : this(_csvDelimter)
        {
        }
        public DelimitedReader(DelimiterInfo delimterInfo)
        {
            _delimterInfo = delimterInfo;
        }
        ~DelimitedReader()
        {
            Close();
        }
        public void Dispose()
        {
            Close();
        }

        public FeedTextCallback Callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        public void Feed(string rawText)
        {
            string[] rawFieldData = rawText.Split(_delimterInfo.Delimiters, StringSplitOptions.None);
            string[] fieldData = rawFieldData;

            // Check char map / text qualifier
            Dictionary<string, string> charMap = _delimterInfo.MarkMap;
            if (_delimterInfo.TextQualifier != null)
            {
                charMap.Add(_delimterInfo.TextQualifier.ToString(), string.Empty);
            }

            if (charMap.Count > 0)
            {
                fieldData = new string[rawFieldData.Length];
                for (int ind = 0; ind < rawFieldData.Length; ind++)
                    fieldData[ind] = RegexPro.ReplaceMultiple(rawFieldData[ind], charMap);
            }

            if (_callback != null)
            {
                _callback(fieldData);
            }
        }

        public void Flush() { }
        public void Close() { }

        #region private functions
        private void LoadControllers()
        {
        }
        #endregion

        #region private data
        private DelimiterInfo _delimterInfo;
        private FeedTextCallback _callback;
        private static DelimiterInfo _csvDelimter = new DelimiterInfo(new string[] { "," }, null, null);
        #endregion
    }

}
