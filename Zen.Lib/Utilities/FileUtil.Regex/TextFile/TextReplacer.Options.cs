using System;

namespace Zen.Utilities.FileUtil
{
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
    using Macros = Zen.Utilities.Generics.Macros;

    public enum ReplaceAction
    {
        Unspecified = 0,
        ReplaceMatched = 1,     // replace old value with new value
        AppendBefore,           // append new value at the begining of matched tokens
        AppendAfter,            // append new value at the end of matched tokens
        RemoveMatchedLine,      // remove the whole line if matched
        MergeLine,              // merge lines (remove line break)
        KeepMatchedWord,        // keep matched word
        KeepMatchedLine,        // keep matched whole line
    }

    [Serializable]
    public sealed class ReplaceOption
    {
        [Description("Replace action type. ")]
        public ReplaceAction ActionType
        {
            get { return _actionType; }
            set { _actionType = value; }
        }

        [Description("Find what? Text search options for process action. ")]
        public SearchOpt SearchOption
        {
            get { return Macros.SafeGet(ref _searchOption); }
            set { _searchOption = value; }
        }

        [Description("New value to replace or append to matched tokens.")]
        public string NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }

        [Description("Process text line by line. It's always true when merge lines.")]
        public bool LineFeed
        {
            get
            {
                if (_actionType == ReplaceAction.MergeLine) // always true when merge lines
                    _lineFeed = true;
                return _lineFeed;
            }
            set { _lineFeed = value; }
        }

        #region private data
        private ReplaceAction _actionType = ReplaceAction.Unspecified;
        private SearchOpt _searchOption = null;
        private string _newValue = null;
        private bool _lineFeed = true;
        #endregion
    }
}
