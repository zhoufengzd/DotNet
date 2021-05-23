using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Zen.Utilities.FileUtil
{
    using Macros = Zen.Utilities.Generics.Macros;
    using RegexOptions = System.Text.RegularExpressions.RegexOptions;

    /// <summary>
    /// Applies to text search, file name search, etc.
    /// </summary>
    [Serializable]
    public sealed class SearchOpt
    {
        /// <summary>
        /// Included string patterns 
        /// </summary>
        [Description("Find what:")]
        public string IncludeFilter
        {
            get { return _inFilter; }
            set { _inFilter = value; }
        }

        /// <summary>
        /// Excluded string patterns 
        /// </summary>
        [Description("But not what (optional):")]
        public string ExcludeFilter
        {
            get { return _outFilter; }
            set { _outFilter = value; }
        }

        [Description("Reflection of RegexOptionList. "), Browsable(false)]
        public RegexOptions? RegexSettings
        {
            get
            {
                if (_regexOptionList == null || _regexOptionList.Count < 1)
                    return null;

                RegexOptions options = RegexOptions.None;
                foreach (RegexOptions rgx in _regexOptionList)
                    options = options | rgx;

                return options;
            }
        }

        [Description("Advanced .Net Regex option for constructing Regex object.")]
        public List<RegexOptions> RegexOptionList
        {
            get { return Macros.SafeGet(ref _regexOptionList); }
            set { _regexOptionList = value; }
        }

        #region private data
        private string _outFilter;
        private string _inFilter;
        private List<RegexOptions> _regexOptionList;
        #endregion
    }

    /// Wild cards like '*', '?' are allowed.
    [Serializable]
    public sealed class FileDirSearchOpt
    {
        [Description("Input file directory. ")]
        public string QueryDirectory
        {
            get { return _inputDirectory; }
            set { _inputDirectory = value; }
        }

        [Description("Find what? File search options. ")]
        public SearchOpt SearchOptions
        {
            get { return Macros.SafeGet(ref _searchOpt); }
            set { _searchOpt = value; }
        }

        [Description("Whether doing recursive search or not.")]
        public bool Recursive
        {
            get { return _recursive; }
            set { _recursive = value; }
        }

        #region private data
        private string _inputDirectory;
        private SearchOpt _searchOpt;
        private bool _recursive;
        #endregion
    }

}
