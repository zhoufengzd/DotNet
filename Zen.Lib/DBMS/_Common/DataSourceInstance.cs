using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS
{
    public interface IDataSource : IDBConnBuilder
    {
        DBMSContext DBMSContext { get; }
        string DataSource { get; }
        List<DataTable> Schemas { get; }

        DbConnection OpenNewConnection();
        bool TestConnection();
        IDataProvider<DataTable> GetTableLister();

        bool Execute(string sql);
        bool Execute(string sql, out DbDataReader reader);
        bool Execute(string sql, out DataTable tbl);
        bool Execute(string sql, out DataSet dtSet);
        bool ExecuteScalar<T>(string sql, out T result);

        string LastErrorMessage { get; }
    }

    /// <summary>
    /// Single DBMS Server instance (live, single threaded)
    ///   Connection Management
    ///   Table browser
    ///   Sql Execution 
    ///   Import / Export
    /// </summary>
    public class DataSourceInstance : IDataSource
    {
        public DataSourceInstance(DBMSContext context, string connString)
        {
            _dbmsCtxt = context;
            _connString = connString;

            LoadControllers();
        }

        public DBMSContext DBMSContext { get { return _dbmsCtxt; } }
        public string DataSource { get { return _dataSource; } }

        #region Connection
        /// <summary>
        /// Raw connection using the default settings (initial catalog)
        /// </summary>
        public virtual DbConnection OpenNewConnection()
        {
            DbConnection conn = _dbmsCtxt.ProviderFactory.CreateConnection();
            conn.ConnectionString = _connString;
            conn.Open();
            return conn;
        }
        public bool TestConnection()
        {
            try
            {
                using (DbConnection conn = OpenNewConnection())
                    conn.Close();
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }

            return true;
        }
        #endregion

        #region Sql execution
        /// <summary> ExecuteNonQuery </summary>
        public bool Execute(string sql)
        {
            try
            {
                using (DbCommand cmd = _dbmsCtxt.ProviderFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = _conMgr.CheckOut();
                    cmd.ExecuteNonQuery();
                    _conMgr.CheckIn(cmd.Connection);
                }

                return true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
        }

        public bool Execute(string sql, out DbDataReader reader)
        {
            reader = null;
            try
            {
                using (DbCommand cmd = _dbmsCtxt.ProviderFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = _conMgr.CheckOut();
                    reader = cmd.ExecuteReader();
                }
                return true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
        }

        public bool Execute(string sql, out DataTable tbl)
        {
            tbl = new DataTable();
            try
            {
                using (DbDataAdapter adapter = _dbmsCtxt.ProviderFactory.CreateDataAdapter())
                {
                    using (DbCommand cmd = _dbmsCtxt.ProviderFactory.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = _conMgr.CheckOut();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(tbl);
                        _conMgr.CheckIn(cmd.Connection);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
        }

        public bool Execute(string sql, out DataSet dtSet)
        {
            dtSet = new DataSet();
            try
            {
                using (DbDataAdapter adapter = _dbmsCtxt.ProviderFactory.CreateDataAdapter())
                {
                    using (DbCommand cmd = _dbmsCtxt.ProviderFactory.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = _conMgr.CheckOut();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtSet);
                        _conMgr.CheckIn(cmd.Connection);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
        }

        public bool ExecuteScalar<T>(string sql, out T result)
        {
            Debug.Assert(Zen.Utilities.TypeInterrogator.IsSingleValueType(typeof(T)));

            result = default(T);
            try
            {
                using (DbCommand cmd = _dbmsCtxt.ProviderFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = _conMgr.CheckOut();
                    result = (T)cmd.ExecuteScalar();
                    _conMgr.CheckIn(cmd.Connection);
                }

                return true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
        }

        public string LastErrorMessage
        {
            get { return _errorMsg; }
        }

        #endregion

        #region Info
        public List<DataTable> Schemas
        {
            get
            {
                DbConnection conn = _conMgr.CheckOut();
                List<DataTable> tbls = SchemaReader.GetAllSchema(conn);
                _conMgr.CheckIn(conn);

                return tbls;
            }
        }

        public virtual IDataProvider<DataTable> GetTableLister()
        {
            return new TableLister(this);
        }
        #endregion

        #region protected functions
        protected void LoadControllers()
        {
            DBLoginInfo info = _dbmsCtxt.CreateDBLoginInfo();
            info.ConnectionString = _connString;
            _dataSource = info.DataSource;
            _conMgr = new ConnPoolMgr(this);
        }
        #endregion

        #region protected data
        protected string _connString;
        protected ConnPoolMgr _conMgr;
        #endregion

        #region private data
        private DBMSContext _dbmsCtxt;
        private string _dataSource;
        private string _errorMsg;
        #endregion
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class TableLister : IDataProvider<DataTable>
    {
        // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE (Platform dependent)
        protected string[] Restrictions = new string[] { null, null, null, null };

        public TableLister(DataSourceInstance server)
        {
            _server = server;
        }

        public DataTable Fetch()
        {
            if (_conn == null)
                _conn = _server.OpenNewConnection();

            return _conn.GetSchema(SchemaReader.TableCollection, Restrictions);
        }

        #region private data
        private DataSourceInstance _server;
        private DbConnection _conn;
        #endregion
    }

}
