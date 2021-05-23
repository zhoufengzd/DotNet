using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace Zen.Search.Util.DTS
{
    /// <summary>
    /// TableInfo: used by DTSearch
    /// </summary>
    public sealed class TableInfo
    {
        public TableInfo()
        {
        }
        public TableInfo(string tableName, string selectSql, string[] idColumns)
        {
            _tableName = tableName;
            _selectSql = selectSql;
            _idColumns = idColumns;
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public string SelectSql
        {
            get { return _selectSql; }
            set { _selectSql = value; }
        }
        public string[] IdColumns
        {
            get { return _idColumns; }
            set { _idColumns = value; }
        }

        #region private data
        private string _tableName;
        private string _selectSql;
        private string[] _idColumns = new string[] { };
        #endregion
    }

    // TO do: figure out primary key columns

    /// <summary>
    /// Must be thread safe
    /// </summary>
    public class DTTableSource : dtSearch.Engine.DataSource
    {
        /// <summary>
        /// Constructor for SampleDataSource
        /// </summary>
        public DTTableSource(DataTable dtTbl)
        {
            _dataTable = dtTbl;
        }

        public TableInfo TableInfo
        {
            get { return _tableInfo; }
            set { _tableInfo = value; }
        }

        /// <summary>
        /// Get the next document from the data source. The document
        /// information is stored in the properties.
        /// </summary>
        public override bool GetNextDoc()
        {
            if (_stopRequested || (_dataTable != null && _curRowIndex >= _dataTable.Rows.Count))
                return false; 

            if (_dataTable == null)
                Rewind();

            SetDocProperties();
            StringBuilder docNameBuffer = new StringBuilder();

            DataRow curRow = _dataTable.Rows[_curRowIndex];
            foreach (string idc in _tableInfo.IdColumns)
            {
                docNameBuffer.Append("#");
                docNameBuffer.Append(curRow[idc]);
            }
            DocName = docNameBuffer.ToString();

            // Set the DocText and DocFields properties by adding each element's
            // value to them during a loop over the columns of this row
            StringBuilder fieldValue = new StringBuilder();
            for (int i = 0; i < _dataTable.Columns.Count; i++)
            {
                string colName = _dataTable.Columns[i].ColumnName;
                if (IsSkippedColumn(colName))
                    continue;

                string colValue = CTS.Search.ColumnValueFormatter.GetFormattedString(_tableInfo.TableName, colName, curRow[i]);
                if (colValue != null)
                    fieldValue.AppendFormat("{0}\t{1}\t", colName, colValue);
            }
            DocFields = fieldValue.ToString();
            DocText = string.Empty;

            // Increment/Update the current row and table indices.  If this table is 
            // out of rows, look for the next table that has at least one row.
            _curRowIndex++;

            // Success - another document is ready
            return true;
        }

        /// <summary>
        /// Initialize the data source so that the next GetNextDoc call will
        /// return the first document.
        /// </summary>
        public override bool Rewind()
        {
            if (_dataTable != null)
                return true;

            try
            {
                OleDbConnection cn = new OleDbConnection(_connectionString);
                cn.Open();
                OleDbDataAdapter daSelect = new OleDbDataAdapter(_tableInfo.SelectSql, cn);

                daSelect.Fill(_dataTable);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// GetDocByName is only called from the Search Form and is not used 
        /// during indexing.   When a retrieved item is selected in the search
        /// results list, SearchForm calls GetDocByName to have the data source
        /// retrieve the item.
        /// </summary>
        /// <param name="name">Name of the row to get.  The name should be the same 
        /// as the DocName returned for this row.</param>
        /// <returns>true if the document was successfully retrieved.</returns>
        public bool GetDocByName(string name)
        {
            return false;
            // or prepare the content
        }

        public bool StopRequested
        {
            get { return _stopRequested; }
            set { _stopRequested = value; }
        }

        public int RecordProcessed
        {
            get { return _curRowIndex; }
        }

        #region private functions
        private void SetDocProperties()
        {
            // Constructor initializes all private members
            DocName = string.Empty;
            DocDisplayName = string.Empty;
            DocText = string.Empty;
            DocFields = string.Empty;
            DocIsFile = false;
            DocModifiedDate = System.DateTime.Now;
            DocCreatedDate = System.DateTime.Now;
        }

        private void GetExternalContent(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream reader = File.OpenRead(filename);
                if (reader.Length > 0)
                {
                    byte[] fileData = new byte[reader.Length];
                    reader.Read(fileData, 0, (int)reader.Length);
                    DocBytes = fileData;
                    HaveDocBytes = true;
                }
            }
        }

        private bool IsSkippedColumn(string columnName)
        {
            if (columnName == "__RowNumber")
                return true;

            foreach (string col in _tableInfo.IdColumns)
            {
                if (string.Compare(columnName, col, true) == 0)
                    return true;
            }

            return false;
        }

        #endregion

        #region private data
        private string _connectionString;
        private TableInfo _tableInfo;
        private DataTable _dataTable;
        private int _curRowIndex;
        private bool _stopRequested = false;
        #endregion
    }
}