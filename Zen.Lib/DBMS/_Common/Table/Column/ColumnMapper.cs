using System;
using System.Collections.Generic;

namespace Zen.DBMS
{
    using ColumnMapper = Zen.DBMS.ColumnMapper;

    /// <summary>
    /// Map Field Name (Doc Property Name) 
    ///    -> Real Column Name -> Column Index, or
    ///    -> Meta Column Name -> Column Index
    /// </summary>
    class CtsFieldMapper
    {
        public CtsFieldMapper(CombinedFormInfo formInfo)
        {
            _formInfo = formInfo;
        }

        /// <summary>
        /// Map field name (could be meta column name) to column index. Return -1 if not found
        /// </summary>
        public int MapColumnIndex(string fieldName)
        {
            string colName = MapColumnName(fieldName);
            if (colName == null)
                return -1;

            return _columnMapper.MapColumnIndex(colName);
        }

        /// <summary>
        /// Map field name (could be meta column name) to FieldInfo. Return null if not found
        /// </summary>
        public FieldInfo MapFieldDef(string fieldName)
        {
            string colName = MapColumnName(fieldName);
            if (colName == null)
                return null;

            FieldInfoList fields = _formInfo.FieldDefs;
            return fields[fieldName];
        }

        /// <summary>
        /// Map field name (could be meta column name) to physical column name. Return null if not found
        /// </summary>
        public string MapColumnName(string fieldName)
        {
            LoadMapper();

            int colIndex = _columnMapper.MapColumnIndex(fieldName);
            if (colIndex != -1)
                return fieldName;

            return _formColumnMapper.MetaNameToColName(fieldName);
        }

        /// <summary>
        /// Map physical column Index to physical column name. Return null if not found
        /// </summary>
        public string MapColumnName(int columnIndex)
        {
            LoadMapper();
            return _columnMapper.MapColumnName(columnIndex);
        }

        public CombinedFormInfo CombinedFormInfo
        {
            get { return _formInfo; }
        }
        #region private functions
        private void LoadMapper()
        {
            if (_columnMapper != null)
                return;

            _columnMapper = new ColumnMapper(_formInfo.TableSchema);
            _formColumnMapper = new FormColumnMapper(_formInfo.FormTableInfo.Columns, _formInfo.FormTableInfo.MetaColumns);
        }
        #endregion

        #region private data & controller
        private CombinedFormInfo _formInfo;

        private ColumnMapper _columnMapper;
        private FormColumnMapper _formColumnMapper;
        #endregion
    }
}
