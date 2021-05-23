using System;
using System.Diagnostics;

namespace Zen.Common.Def.Layout
{
    public enum Section
    {
        Unspecified = 0,
        Header,
        Body,
        Footer,
    }

    [Flags]
    public enum DateTimeMode
    {
        Unknown = 0,
        DateOnly = 0x01,
        TimeOnly = 0x10,
        Both = DateOnly & TimeOnly,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LayoutHint : Attribute
    {
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        public Section Section
        {
            get { return _section; }
            set { _section = value; }
        }
        public int RowCount
        {
            get { return _rowCount; }
            set { _rowCount = (value > 0) ? value : 1; }
        }
        public int ColumnCount
        {
            get { return _columnCount; }
            set { _columnCount = (value > 0) ? value : 1; }
        }
        public bool IsAdvanced
        {
            get { return _isAdvanced; }
            set { _isAdvanced = value; }
        }
        public bool IsReadonly
        {
            get { return _isReadonly; }
            set { _isReadonly = value; }
        }

        #region private data
        private string _label;
        private Section _section = Section.Body;
        private int _rowCount = 1;
        private int _columnCount = 1;
        private bool _isAdvanced = false;
        private bool _isReadonly = false;
        #endregion
    }

    #region Layout hint by control type
    /// <summary>
    /// Very flexible with following options
    ///   1. Verify format
    ///   2. Accepted / Non-Accepted chars
    ///   3. File / Directory browser
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TextBoxHint : LayoutHint
    {
        public static readonly char[] Numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };

        public string RegexFormat
        {
            get { return _regexFormat; }
            set { _regexFormat = value; }
        }

        /// <summary>
        /// Accepted or not accepted chars are both exposed if one set is smaller
        ///   Don't set both of them (no need and will fire assertions.
        /// </summary>
        public char[] CharsAccepted
        {
            get { return _accepted; }
            set { Debug.Assert(_notAccepted == null); _accepted = value; }
        }
        public char[] CharsNotAccepted
        {
            get { return _notAccepted; }
            set { Debug.Assert(_accepted == null); _notAccepted = value; }
        }

        #region private data
        private char[] _accepted;
        private char[] _notAccepted;
        private string _regexFormat = null;
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BrowserHint : LayoutHint
    {
        public string InitialDirectory
        {
            get { return _initialDir; }
            set { if (!string.IsNullOrEmpty(value)) _initialDir = value; }
        }
        public string Filter
        {
            get { return _filter; }
            set { if (!string.IsNullOrEmpty(value)) _filter = value; }
        }
        public abstract BrowserOption BrowserOptions { get; }

        #region private data
        protected string _filter = FileTypeFilter.AllFile;
        protected string _initialDir = @".\";
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FileBrowserHint : BrowserHint
    {
        public override BrowserOption BrowserOptions
        {
            get { return new FileBrowserOpt(_initialDir, _filter); }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DirBrowserHint : BrowserHint
    {
        public override BrowserOption BrowserOptions
        {
            get { return new DirectoryBrowserOpt(_initialDir, _filter); }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimePickerHint : LayoutHint
    {
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
        public DateTimeMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        #region private data
        private string _format;
        private DateTimeMode _mode = DateTimeMode.DateOnly;
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NumericUpDownHint : LayoutHint
    {
        public decimal Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        public decimal Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }
        public decimal Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }

        #region private data
        private decimal _minimum = 0;
        private decimal _maximum = 100;
        private decimal _increment = 10;
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionHint : LayoutHint
    {
        public bool EnableDelete
        {
            get { return _enableDelete; }
            set { _enableDelete = value; }
        }
        public bool EnableAdd
        {
            get { return _enableAdd; }
            set { _enableAdd = value; }
        }

        #region private data
        private bool _enableDelete;
        private bool _enableAdd;
        #endregion
    }

    #endregion
}
