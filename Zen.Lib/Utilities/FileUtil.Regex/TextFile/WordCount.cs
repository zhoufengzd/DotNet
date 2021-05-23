using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Zen.Utilities.FileUtil
{
    using ITextStreamConsumer = Zen.Utilities.Algorithm.IDataStreamConsumer<string>;

    /// <summary>
    /// Under research
    /// </summary>
    public static class TextCounter
    {
        public static Dictionary<string, int> CountWord(string txt, ref Dictionary<string, int> wordList)
        {
            if (wordList == null)
                wordList = new Dictionary<string,int>();

            // Split on all non-word characters.
            // Returns an array of all the words.
            string[] splitted = Regex.Split(txt, @"\W+");
            foreach (string word in splitted)
            {
                if (word.Length < 1)
                    continue;

                if (wordList.ContainsKey(word))
                    wordList[word]++;
                else
                    wordList.Add(word, 1);
            }

            return wordList;
        }

        /// <summary>
        /// Count word with loop and character tests.
        /// </summary>
        //public static int CountWord(string txt)
        //{
        //    int c = 0;
        //    for (int i = 1; i < txt.Length; i++)
        //    {
        //        if (char.IsWhiteSpace(txt[i - 1]) == true)
        //        {
        //            if (char.IsLetterOrDigit(txt[i]) == true ||
        //                char.IsPunctuation(txt[i]))
        //            {
        //                c++;
        //            }
        //        }
        //    }
        //    if (txt.Length > 2)
        //    {
        //        c++;
        //    }
        //    return c;
        //}

    }

    public sealed class WordCounter : ITextStreamConsumer
    {
        public WordCounter(Dictionary<string, int> result)
        {
            _result = result;
        }

        ~WordCounter()
        {
        }

        public void Feed(string rawText)
        {
            TextCounter.CountWord(rawText, ref _result);
        }


        public void Flush() { }
        public void Close() { }
        public void Dispose() { }

        #region private functions
        #endregion

        #region private data
        private Dictionary<string, int> _result;
        #endregion
    }

    public sealed class BatchTextCounter : BatchOperatorBase
    {
        public BatchTextCounter(BatchInOutOption inOutOpt)
            : base(inOutOpt)
        {
        }

        public Dictionary<string, int> Result
        {
            get { return _result; }
        }

        #region Overriden base functions
        protected override void ProcessFile(string inputFile)
        {
            WordCounter processor = new WordCounter(_result);
            TextDispatcher dispatcher = new TextDispatcher(inputFile, processor);

            dispatcher.Run();
        }
        #endregion

        #region private data
        private Dictionary<string, int> _result = new Dictionary<string,int>();
        #endregion
    }
}
