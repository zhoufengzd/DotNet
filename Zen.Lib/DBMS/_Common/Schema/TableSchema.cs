using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Zen.DBMS.Schema
{
    using Debug = System.Diagnostics.Debug;
    using System.Data.Common;

    /// <summary>
    ///   column location -> column name
    ///   column name -> column location
    /// </summary>
    public interface IColumnMapper
    {
        int MapColumnIndex(string columnName);
        string MapColumnName(int columnLocation);
    }

    #region internal
    internal sealed class ColumnMapper : IColumnMapper
    {
        public ColumnMapper(TableSchema tblSchema)
        {
            Debug.Assert(tblSchema != null);
            _tblSchema = tblSchema;
        }

        /// <summary>
        ///  Map from column name (dii tag) to table column location
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>-1: field not found</returns>
        public int MapColumnIndex(string columnName)
        {
            LoadNameIndexMap();
            if (string.IsNullOrEmpty(columnName))
                return -1;

            int colIndex = -1;
            if (!_columnNameIndexMap.TryGetValue(columnName, out colIndex))
                return -1;

            return colIndex;
        }

        /// <summary>
        ///  Map from location (csv field#) to table column name
        /// </summary>
        /// <param name="columnLocation"></param>
        /// <returns>null if map is not initialized</returns>
        public string MapColumnName(int columnLocation)
        {
            LoadNameIndexMap();
            if (columnLocation < 0)
                return null;

            string colName = null;
            if (!_columnIndexNameMap.TryGetValue(columnLocation, out colName))
                return null;

            return colName;
        }

        #region private functions
        private void LoadNameIndexMap()
        {
            if (_columnNameIndexMap != null)
                return;

            DataRowCollection colDefs = _tblSchema.ColumnDefs.Rows;
            _columnNameIndexMap = new Dictionary<string, int>(colDefs.Count, StringComparer.OrdinalIgnoreCase);
            _columnIndexNameMap = new Dictionary<int, string>(colDefs.Count);

            int colIndex = 0;
            string colName = null;
            foreach (DataRow dtRow in colDefs)
            {
                colName = dtRow["ColumnName"].ToString();
                _columnNameIndexMap.Add(colName, colIndex);
                _columnIndexNameMap.Add(colIndex, colName);
                colIndex++;
            }
        }
        #endregion

        #region private data
        private TableSchema _tblSchema;
        private Dictionary<string, int> _columnNameIndexMap;
        private Dictionary<int, string> _columnIndexNameMap;
        #endregion
    }
    #endregion

    /// <summary>
    /// The one place shop for table all schema related information
    /// </summary>
    public class TableSchema
    {
        static readonly string TableSelectFmt = "SELECT * FROM {0}"; //"SELECT TOP 1 * FROM {0}"

        public TableSchema(IDataSource dataSource, string tableName)
        {
            _dataSource = dataSource;
            _tableName = tableName;

            FetchTableSchema(); 
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public List<string> ColumnNameList
        {
            get { return _columnNameList; }
        }

        public DataRow ColumnDef(string columnName)
        {
            return _mappedSchema[columnName];
        }

        public DataRow ColumnDef(int columnIndex)
        {
            if (columnIndex > _rawSchema.Rows.Count - 1)
                return null;

            return _rawSchema.Rows[columnIndex];
        }

        public DataTable ColumnDefs
        {
            get { return _rawSchema; }
        }

        public IColumnMapper ColumnMapper
        {
            get { return _colMapper; }
        }

        public List<string> KeyColumnList
        {
            get { return _keyColumnList; }
        }
        public List<string> UniqueColumnList
        {
            get { return _uniqueColumnList; }
        }
        public List<int> ReadOnlyColumnIndexes
        {
            get { return _readOnlyColumnIndexes; }
        }
        public List<string> ReadOnlyColumnList
        {
            get { return _readOnlyColumnList; }
        }
        public List<string> LobColumnList
        {
            get { return _lobColumnList; }
        }
        public List<string> NonLobColumnList
        {
            get { return _nonLobColumnList; }
        }

        #region private functions
        private bool FetchTableSchema()
        {
            if (_rawSchema != null)
                return true;

            string strTargetTblTableSelect = string.Format(TableSelectFmt, _tableName);
            //DbCommand cmd = dataSource.
            //    new DbCommand(strTargetTblTableSelect, _conn);
            DbCommand cmd = _dataSource.DBMSContext.ProviderFactory.CreateCommand();
            cmd.Connection = _dataSource.OpenNewConnection();
            cmd.CommandText = strTargetTblTableSelect;
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);

            _rawSchema = reader.GetSchemaTable();
            _rawSchema.TableName = _tableName;
            reader.Close();

            DataRowCollection rows = _rawSchema.Rows;
            _mappedSchema = new Dictionary<string, DataRow>(rows.Count, StringComparer.OrdinalIgnoreCase);
            _columnNameList = new List<string>(rows.Count);
            _keyColumnList = new List<string>();
            _uniqueColumnList = new List<string>();
            _readOnlyColumnIndexes = new List<int>();
            _readOnlyColumnList = new List<string>();
            _lobColumnList = new List<string>();
            _nonLobColumnList = new List<string>();

            string columnName = null;
            int index = 0;
            foreach (DataRow dtRow in rows)
            {
                columnName = dtRow["ColumnName"].ToString();
                _mappedSchema.Add(columnName, dtRow);
                _columnNameList.Add(columnName);

                bool bValueTmp = false;
                if (bool.TryParse(dtRow["IsKey"].ToString(), out bValueTmp) && bValueTmp)
                {
                    _keyColumnList.Add(columnName);
                }
                if (bool.TryParse(dtRow["IsUnique"].ToString(), out bValueTmp) && bValueTmp)
                {
                    _uniqueColumnList.Add(columnName);
                }
                if (bool.TryParse(dtRow["IsReadOnly"].ToString(), out bValueTmp) && bValueTmp)
                {
                    _readOnlyColumnIndexes.Add(index);
                    _readOnlyColumnList.Add(columnName);
                }

                //SqlDbType colDBType = (SqlDbType)Enum.Parse(typeof(SqlDbType), (string)dtRow["DataTypeName"], true);
                //if (colDBType == SqlDbType.Image || colDBType == SqlDbType.NText || colDBType == SqlDbType.Text)
                //    _lobColumnList.Add(columnName);
                //else
                //    _nonLobColumnList.Add(columnName);

                index++;
            }

            _readOnlyColumnIndexes.Sort();
            _readOnlyColumnList.Sort();

            _keyColumnList.Sort();
            _colMapper = new ColumnMapper(this);

            return true;
        }
        #endregion

        #region private: controllers
        private Dictionary<string, DataRow> _mappedSchema;
        private DataTable _rawSchema;
        private ColumnMapper _colMapper;
        private List<string> _columnNameList;
        private List<string> _keyColumnList;
        private List<string> _uniqueColumnList;
        private List<int> _readOnlyColumnIndexes;
        private List<string> _readOnlyColumnList;
        private List<string> _lobColumnList;
        private List<string> _nonLobColumnList;

        private IDataSource _dataSource;
        private string _tableName;
        #endregion
    }
}
