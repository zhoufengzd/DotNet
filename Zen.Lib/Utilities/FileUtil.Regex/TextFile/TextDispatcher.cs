using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Zen.Utilities.FileUtil
{
    using ITextStreamConsumer = Zen.Utilities.Algorithm.IDataStreamConsumer<string>;
    using ITextStreamProvider = Zen.Utilities.Algorithm.IDataStreamProvider<string>;

    /// <summary>
    /// Dispatch text: ITextStreamProvider(1) --> (n)ITextStreamConsumer
    /// </summary>
    public sealed class TextDispatcher : Zen.Utilities.Threading.IRunable
    {
        public TextDispatcher(string filePath, ITextStreamConsumer txtConsumer)
            : this(new TxtFileStreamProvider(filePath), txtConsumer)
        {
        }
        public TextDispatcher(ITextStreamProvider provider, ITextStreamConsumer txtConsumer)
        {
            Debug.Assert(provider != null);
            _provider = provider;

            _consumers = new List<ITextStreamConsumer>();
            AddConsumer(txtConsumer);
        }

        public void AddConsumer(ITextStreamConsumer txtConsumer)
        {
            _consumers.Add(txtConsumer);
        }

        public int BatchSize
        {
            set { Debug.Assert(value > 0); _batchSize = value; }
        }

        public long FetchCount
        {
            get { return _fetchCount; }
        }

        public bool Run()
        {
            Debug.Assert(_consumers != null && _consumers.Count > 0);

            _fetchCount = 0;
            while (!_provider.EndOfData)
            {
                foreach (ITextStreamConsumer textConsumer in _consumers)
                    textConsumer.Feed(_provider.Fetch());

                _fetchCount++;
                if (_batchSize != null && _fetchCount % _batchSize == 0)
                {
                    foreach (ITextStreamConsumer textConsumer in _consumers)
                        textConsumer.Flush();
                }
            }

            _provider.Close();
            foreach (ITextStreamConsumer textConsumer in _consumers)
                textConsumer.Close();

            return true;
        }

        #region private data
        private ITextStreamProvider _provider;
        private List<ITextStreamConsumer> _consumers;
        private int? _batchSize = null;
        private long _fetchCount;
        #endregion
    }

    public sealed class TxtFileStreamProvider : ITextStreamProvider
    {
        /// <param name="lineFeed">If true, read file line by line; otherwise, read to the end</param>
        public TxtFileStreamProvider(string filePath)
            : this(filePath, true)
        {
        }

        /// <param name="lineFeed">If true, read file line by line; otherwise, read to the end</param>
        public TxtFileStreamProvider(string filePath, bool lineFeed)
            : this(filePath, lineFeed, System.Text.Encoding.Default)
        {
        }

        /// <param name="lineFeed">If true, read file line by line; otherwise, read to the end</param>
        public TxtFileStreamProvider(string filePath, bool lineFeed, System.Text.Encoding encoding)
        {
            _reader = new StreamReader(filePath, encoding);
            _lineFeed = lineFeed;
        }

        ~TxtFileStreamProvider()
        {
            Close();
        }

        public string Fetch()
        {
            if (_reader != null && !_reader.EndOfStream)
                return _lineFeed ? _reader.ReadLine() : _reader.ReadToEnd();

            return null;
        }

        public bool EndOfData
        {
            get { return _reader.EndOfStream; }
        }

        public void Close()
        {
            if (_reader != null)
                _reader.Close();

            _reader = null;
        }

        public void Dispose()
        {
            Close();
        }

        #region private data
        private StreamReader _reader;
        private bool _lineFeed = true;
        #endregion
    }

    public sealed class StrStreamProvider : ITextStreamProvider
    {
        static readonly string[] LineBreak = new string[] { "\r\n" };

        /// <param name="lineFeed">If true, read file line by line; otherwise, read to the end</param>
        public StrStreamProvider(string rawBuffer)
        {
            _rawBuffer = rawBuffer;
        }

        ~StrStreamProvider()
        {
            Close();
        }

        public string Fetch()
        {
            return _textStack.Dequeue();
        }

        public bool EndOfData
        {
            get { LoadControllers(); return (_textStack.Count < 1); }
        }

        public void Close()
        {
            _rawBuffer = null;
            _textStack = null;
        }

        public void Dispose()
        {
            Close();
        }

        #region private functions
        private void LoadControllers()
        {
            if (_textStack != null)
                return;

            if (_lineFeed)
            {
                _textStack = new Queue<string>(_rawBuffer.Split(LineBreak, StringSplitOptions.None));
            }
            else
            {
                _textStack = new Queue<string>();
                _textStack.Enqueue(_rawBuffer);
            }
        }
        #endregion

        #region private data
        private string _rawBuffer;
        private Queue<string> _textStack;

        private bool _lineFeed = true;
        #endregion
    }
}
