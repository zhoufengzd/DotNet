using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS.SqlServer
{
    public sealed class SqlSvrInstance : DataSourceInstance
    {
        public SqlSvrInstance(string connString)
            : base(new SqlSvrContext(), connString)
        {
        }

        public SqlSvrInstance(SqlSvrContext context, string connString)
            : base(context, connString)
        {
        }

        public IEnumerable<string> Databases
        {
            get 
            {
                if (_databaseNames == null)
                {
                    DbConnection conn = _conMgr.CheckOut();
                    _databaseNames = SqlSvrSchemaReader.GetDatabaseNames(conn);
                    _conMgr.CheckIn(conn);
                }
                return _databaseNames; 
            }
        }

        public Dictionary<string, DataTable> Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        //public void ChangeDatabase(string databaseName)
        //{
        //    _currentDatabase = databaseName;
        //}
        //public override DbConnection OpenNewConnection()
        //{
        //    DbConnection conn = DBMSContext.ProviderFactory.CreateConnection();
        //    conn.ConnectionString = _connString;
        //    conn.Open();
        //    return conn;
        //}

        public IDataProvider<DataTable> GetTableLister(string databaseName)
        {
            return new SqlSvrTableLister(this, databaseName);
        }

        #region internal functions
        private void GetTables()
        {
            // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
            string[] restrictions = new string[] { null, null, null, null };

            _tables = new Dictionary<string, DataTable>();
            DbConnection conn = _conMgr.CheckOut();
            foreach (string db in _databaseNames)
            {
                conn.ChangeDatabase(db);
                DataTable tbl = conn.GetSchema(SchemaReader.TableCollection, restrictions);

                if (tbl.Rows.Count > 0)
                    _tables.Add(db, tbl);
            }
            _conMgr.CheckIn(conn);
        }
        #endregion

        #region private data
        private List<string> _databaseNames;
        private Dictionary<string, DataTable> _tables;
        private string _currentDatabase;
        #endregion
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class SqlSvrTableLister : IDataProvider<DataTable>
    {
        // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
        static readonly string[] Restrictions = new string[] { null, null, null, null };

        public SqlSvrTableLister(SqlSvrInstance server, string database)
        {
            _server = server;
            _database = database;
        }

        public DataTable Fetch()
        {
            if (_conn == null)
                _conn = _server.OpenNewConnection();

            _conn.ChangeDatabase(_database);
             return _conn.GetSchema(SchemaReader.TableCollection, Restrictions);
        }

        #region private data
        private SqlSvrInstance _server;
        private string _database;
        private DbConnection _conn;
        #endregion
    }
}
