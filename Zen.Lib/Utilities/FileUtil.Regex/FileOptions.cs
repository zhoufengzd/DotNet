using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Zen.Utilities.FileUtil
{
    using RegexOptions = System.Text.RegularExpressions.RegexOptions;
    using Macros = Zen.Utilities.Generics.Macros;

    [Serializable]
    public sealed class FileOutOption
    {
        public static readonly string DefaultOutDir = "output";

        [Description("Output file directory. Default is 'output'.")]
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set { _outputDirectory = value; }
        }

        #region private data
        private string _outputDirectory = DefaultOutDir;
        #endregion
    }

    [Serializable]
    [Description("File process batch input / out options")]
    public sealed class BatchInOutOption
    {
        [Description("Input file options.")]
        public FileDirSearchOpt InputOptions
        {
            get { return Macros.SafeGet(ref _inputOptions); }
            set { _inputOptions = value; }
        }

        [Description("Process output options.")]
        public FileOutOption OutputOptions
        {
            get { return Macros.SafeGet(ref _outputOptions); }
            set { _outputOptions = value; }
        }

        #region private data
        private FileDirSearchOpt _inputOptions;
        private FileOutOption _outputOptions;
        #endregion
    }


    /// <summary> For mark a field in a text line </summary>
    [Serializable]
    public sealed class FieldMark
    {
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public int Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #region private data
        private string _name;
        private int _start;
        private int _length;
        private string _value;
        #endregion
    }

    [Serializable]
    public sealed class LineMark
    {
        public LineMark(string filePath)
        {
            _filePath = filePath;
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public List<int> LineNumbers
        {
            get { return _lineNumbers; }
            set { _lineNumbers = value; }
        }

        #region private data
        private string _filePath;
        private List<int> _lineNumbers;
        #endregion
    }
}
