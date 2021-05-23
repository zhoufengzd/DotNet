using System;
using System.Collections.Generic;

namespace Zen.DBMS
{
    using ErrorLevel = Zen.Utilities.Defs.ErrorLevel;

    /// <summary>
    /// Light weight, open schema error output
    /// </summary>
    [Serializable]
    public enum ErrorType
    {
        Unspecified = 0,
        ColumnNotFound = 1,

        ExceedsColumnSize,
        InvalidDataType,
        NonNullValueExpected,

        LoadTimeOut,
        InvalidKeyValue,
    }

    [Serializable]
    public class FieldError
    {
        public FieldError()
        {
        }

        public ErrorLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public List<ErrorType> Errors
        {
            get { LoadErrors(); return _errors; }
        }

        #region private function
        private void LoadErrors()
        {
            if (_errors != null)
                return;
            _errors = new List<ErrorType>();
        }
        #endregion

        #region private data
        private ErrorLevel _level = ErrorLevel.None;
        private List<ErrorType> _errors = null;
        #endregion
    }
}
