using System;
using System.Collections.Generic;

namespace Zen.DBMS
{
    /// <summary>
    /// Transform raw field data into table column value with proper format
    /// </summary>
    class CtsFieldDataTransformer : IColumnDataProcessor<FieldError>
    {
        public CtsFieldDataTransformer(CtsFieldMapper fldMapper, IMeTblInterpreterMgr meMgr)
        {
            _fldMapper = fldMapper;
            _validator = new CtsFieldDataValidator(meMgr);
        }

        /// <summary>
        /// Return true if data is processed (transformed) properly
        /// </summary>
        public bool Process(int colIndex, ref object colValue, ref FieldError fldError)
        {
            return Process(_fldMapper.MapColumnName(colIndex), ref colValue, ref fldError);
        }
        public bool Process(string colName, ref object colValue, ref FieldError fldError)
        {
            if (fldError == null)
                fldError = new FieldError();

            if (string.IsNullOrEmpty(colName))
            {
                fldError.Errors.Add(ErrorType.ColumnNotFound);
                return false;
            }

            if (!_validator.Validate(_fldMapper.MapFieldDef(colName), colValue, out _colValueConverted, ref fldError))
                return false;

            colValue = _colValueConverted;
            return true;
        }

        public object ValueConverted
        {
            get { return _colValueConverted; }
        }

        #region private functions
        #endregion

        #region private data
        private CtsFieldMapper _fldMapper;

        private CtsFieldDataValidator _validator;
        private object _colValueConverted;
        #endregion

    }
}
