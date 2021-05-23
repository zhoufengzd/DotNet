using System;
using System.Collections.Generic;

namespace Zen.Utilities.Defs
{
    /// <summary>
    /// Please keep enums listed here brief and generic
    /// </summary>
    [Serializable]
    public enum ErrorLevel
    {
        None = 0,
        Warning = 1,
        Error,
        Fatal,
    }

    [Serializable]
    public enum FileErrorType
    {
        Unspecified = 0,
        FileNotFound = 1,
        AccessDenied,
        InvalidFormat,
        OverwriteExistingFile,
    }

    [Serializable]
    public class FileError
    {
        public FileError(string filePath)
            : this(filePath, ErrorLevel.Warning)
        {
        }
        public FileError(string filePath, ErrorLevel level)
        {
            _filePath = filePath;
            _level = level;
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public ErrorLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public List<FileErrorType> Errors
        {
            get { LoadErrors(); return _errors; }
        }

        #region private function
        private void LoadErrors()
        {
            if (_errors != null)
                return;
            _errors = new List<FileErrorType>();
        }
        #endregion

        #region private data
        private string _filePath;
        private ErrorLevel _level = ErrorLevel.None;
        private List<FileErrorType> _errors;
        #endregion
    }
}
