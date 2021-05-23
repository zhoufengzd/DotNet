using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Zen.DBMS.Schema;

namespace Zen.DBMS
{    
    using CompareType = Zen.Utilities.Algorithm.CompareType;

    public sealed class SearchCondition
    {
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public string SearchValue
        {
            get { return _searchValue; }
            set { _searchValue = value; }
        }
        public CompareType CompareType
        {
            get { return _compareType; }
            set { _compareType = value; }
        }

        public override string ToString()
        {
            const string ColNameParamFmt = @"[{0}] {1} {2} ";
            return string.Format(ColNameParamFmt, _columnName, ToSqlString(_compareType), _searchValue);
        }

        #region private functions
        private string ToSqlString(CompareType ct)
        {
            switch (ct)
            {
                case CompareType.GT: return ">";
                case CompareType.GT_EQL: return ">=";
                case CompareType.EQL: return "=";
                case CompareType.LT_EQL: return "<=";
                case CompareType.LT: return "<";
                default: return string.Empty;
            }
        }
        #endregion

        #region private data
        private string _columnName;
        private string _searchValue;
        private CompareType _compareType;
        #endregion
    }

    /// <summary>
    /// Context paramter to build TableAdapter
    /// </summary>
    public class TableAdapterContext
    {
        //public TableAdapterContext(string tableName, SqlConnection connection)
        //    : this(new TableSchema(connection, tableName), connection)
        //{
        //}
        public TableAdapterContext(TableSchema tblSchema, SqlConnection connection)
        {
            _tblSchema = tblSchema;
            _connection = connection;
        }

        public TableSchema TableSchema
        {
            get { return _tblSchema; }
        }

        public SqlConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Optional column filter
        ///    to allow select / update only columns defined 
        /// Default: all columns included
        ///  </summary>
        public List<string> ValueColumns
        {
            get
            {
                System.Diagnostics.Debug.Assert(_valueColumns == null || _valueColumns.Count <= _tblSchema.ColumnNameList.Count);
                return (_valueColumns != null) ? _valueColumns : _tblSchema.ColumnNameList;
            }
            set
            {
                _valueColumns = value;
                System.Diagnostics.Debug.Assert(_valueColumns != null && _valueColumns.Count > 0);
            }
        }

        public List<string> KeyColumns
        {
            get
            {
                if (_keyColumns == null)
                    _keyColumns = _tblSchema.KeyColumnList;
                return _keyColumns;
            }
            set 
            {
                _keyColumns = value; 
            }
        }

        /// <summary> 
        /// Like 'SELECT ... 
        ///        WHERE [Col1] = value1 and [Col2] > value2'.
        /// May not be key columns
        /// </summary>
        public List<SearchCondition> SearchFilter
        {
            get { return _searchFilter; }
            set { _searchFilter = value; _searchColumns = null; }
        }

        public IEnumerable<string> SearchColumns
        {
            get
            {
                if (_searchColumns == null && _searchFilter != null)
                {
                    _searchColumns = new List<string>(_searchFilter.Count);
                    foreach (SearchCondition sc in _searchFilter)
                        _searchColumns.Add(sc.ColumnName);
                }
                return _searchColumns;
            }
        }

        /// <summary>During update, overwrite existing data or not</summary>
        public bool Overwrite
        {
            get { return _overwrite; }
            set { _overwrite = value; }
        }

        #region private data
        private TableSchema _tblSchema;
        private SqlConnection _connection;

        private List<string> _valueColumns;
        private List<string> _keyColumns;
        private List<string> _searchColumns;
        private List<SearchCondition> _searchFilter;

        private bool _overwrite = true;
        #endregion
    }

    /// <summary>
    /// DataTable Adapter: support 'SELECT, UPDATE, DELETE, INSERT'.
    ///   Interactive mode. For bulk load / update, use TableBulkCopier / TableBulkCopier
    /// </summary>
    public abstract class TableAdapterBase
    {
        public TableAdapterBase(TableAdapterContext context)
        {
            _context = context;
        }

        public DataTable DataTable
        {
            get { BuildDataTable(); return _dataTable; }
        }

        public TableSchema TableSchema
        {
            get { return _context.TableSchema; }
        }

        #region protected functions
        protected abstract void BuildDataTable();

        protected abstract void BuildAdapter();

        protected void SetupAdapterCommands(SqlCommand adapterCmd, string spName, string spDDLsql)
        {
            using (SqlCommand cmd = new SqlCommand(spDDLsql, _context.Connection))
            {
                cmd.ExecuteNonQuery();
            }
            adapterCmd.CommandText = spName;
            adapterCmd.CommandType = CommandType.StoredProcedure;
        }
        #endregion

        #region protected memebers
        protected TableAdapterContext _context;
        protected SqlDataAdapter _dbDataAdapter;
        protected DataTable _dataTable;
        #endregion
    }

    /// <summary>
    /// Editor: support interactively "Select | Update | Delete | Insert"
    /// </summary>
    public sealed class TableAdapterEditor : TableAdapterBase
    {
        public TableAdapterEditor(TableAdapterContext context)
            : base(context)
        {
        }

        public int Fill()
        {
            BuildAdapter();
            return _dbDataAdapter.Fill(this.DataTable);
        }

        #region protected functions
        protected override void BuildDataTable()
        {
            if (_dataTable != null)
                return;

            _dataTable = DataTableBuilder.BuildTable(_context.TableSchema, false);
        }

        protected override void BuildAdapter()
        {
            if (_dbDataAdapter != null)
                return;

            _dbDataAdapter = new SqlDataAdapter();
            _dbDataAdapter.InsertCommand = new SqlCommand();
            _dbDataAdapter.InsertCommand.Connection = _context.Connection;
            _dbDataAdapter.UpdateCommand = new SqlCommand();
            _dbDataAdapter.UpdateCommand.Connection = _context.Connection;
            _dbDataAdapter.SelectCommand = new SqlCommand();
            _dbDataAdapter.SelectCommand.Connection = _context.Connection;
            _dbDataAdapter.DeleteCommand = new SqlCommand();
            _dbDataAdapter.DeleteCommand.Connection = _context.Connection;

            // Builds up select sp
            string spName = TempSqlObjNameGenerator.GetSpName();
            TableSpBuilder builder = new TableSpBuilder();
            string spDDLsql = builder.BuildSelectSpDDL(_dbDataAdapter.SelectCommand.Parameters, spName, _context);
            SetupAdapterCommands(_dbDataAdapter.SelectCommand, spName, spDDLsql);

            // Builds up insert sp
            //spName = TempSqlObjNameGenerator.GetSpName();
            //spDDLsql = builder.BuildInsertSpDDL(_dbDataAdapter.InsertCommand.Parameters, spName, _context);
            //SetupAdapterCommands(_dbDataAdapter.InsertCommand, spName, spDDLsql);
        }
        #endregion
    }

    /// <summary>
    /// BulkUpdater: support Bulk update existing data from external source
    ///   like csv. 
    /// </summary>
    public sealed class TableAdapterBulkUpdater : TableAdapterBase
    {
        public TableAdapterBulkUpdater(TableAdapterContext context)
            : base(context)
        {
        }

        public ILoadEventListener ILoadEventListener
        {
            set { _loadListener = value; }
        }

        public int Update()
        {
            if (_dataTable == null) // nothing to update
                return 0;

            BuildAdapter();
            return _dbDataAdapter.Update(_dataTable);
        }

        #region protected functions
        protected override void BuildDataTable()
        {
            if (_dataTable != null)
                return;

            _dataTable = DataTableBuilder.BuildTable(_context.TableSchema, true);
        }

        protected override void BuildAdapter()
        {
            if (_dbDataAdapter != null)
                return;

            _dbDataAdapter = new SqlDataAdapter();
            _dbDataAdapter.UpdateCommand = new SqlCommand();
            _dbDataAdapter.UpdateCommand.Connection = _context.Connection;
            _dbDataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            _dbDataAdapter.ContinueUpdateOnError = true;

            string spName = TempSqlObjNameGenerator.GetSpName();
            TableSpBuilder builder = new TableSpBuilder();
            string spDDLsql = builder.BuildUpdateSpDDL(_dbDataAdapter.UpdateCommand.Parameters, spName, _context);
            SetupAdapterCommands(_dbDataAdapter.UpdateCommand, spName, spDDLsql);

            _dbDataAdapter.InsertCommand = _dbDataAdapter.UpdateCommand;
            // Empty DataTable ==> Fill data (for update or overwrite)
            // ==> _adapter.Update() ==> call 'InsertCommand' == in fact UpdateCommand called
        }

        private void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.Status == UpdateStatus.ErrorsOccurred)
            {
                if (_loadListener != null)
                    _loadListener.OnRowUpdateError(args.Row, args.Errors.Message);
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        #endregion

        private ILoadEventListener _loadListener;
    }

    /// <summary>
    /// BulkUpdater: support Bulk update existing data from external source
    ///   like csv. 
    /// </summary>
    public sealed class TableAdapterBulkLoader : TableAdapterBase
    {
        public TableAdapterBulkLoader(TableAdapterContext context)
            : base(context)
        {
        }

        public ILoadEventListener ILoadEventListener
        {
            set { _loadListener = value; }
        }

        public int Load()
        {
            if (_dataTable == null || _dataTable.Rows.Count < 1) // nothing to load
                return 0;

            using (SqlBulkCopy bc = new SqlBulkCopy(_context.Connection, SqlBulkCopyOptions.Default, null))
            {
                bc.BatchSize = _dataTable.Rows.Count;
                bc.DestinationTableName = _context.TableSchema.TableName;
                bc.BulkCopyTimeout = 0;

                SqlBulkCopyColumnMappingCollection colMappings = bc.ColumnMappings;
                int columnCount = _context.TableSchema.ColumnNameList.Count;
                for (int colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    if (_context.TableSchema.ReadOnlyColumnIndexes.BinarySearch(colIndex) > -1)
                        continue;

                    colMappings.Add(colIndex, colIndex);
                }

                bc.WriteToServer(_dataTable);
                bc.Close();
            }

            return _dataTable.Rows.Count;
        }

        #region protected functions
        protected override void BuildDataTable()
        {
            if (_dataTable != null)
                return;

            _dataTable = DataTableBuilder.BuildTable(_context.TableSchema, false);
        }

        protected override void BuildAdapter()
        {
            return;
        }

        private void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.Status == UpdateStatus.ErrorsOccurred)
            {
                if (_loadListener != null)
                    _loadListener.OnRowUpdateError(args.Row, args.Errors.Message);
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        #endregion

        private ILoadEventListener _loadListener;
    }

}
