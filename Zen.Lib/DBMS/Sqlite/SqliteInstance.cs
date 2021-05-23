using System.Data;
using System.Data.Common;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS.Sqlite
{
    public sealed class SqliteInstance : SvrInstance
    {
        public SqliteInstance(SqliteContext context, string connString)
            : base(context, connString)
        {
        }

        public IDataProvider<DataTable> GetTableLister()
        {
            return new SqliteTableLister(this);
        }

        #region internal functions
        #endregion

        #region private data
        #endregion
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class SqliteTableLister : IDataProvider<DataTable>
    {
        // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
        static readonly string[] Restrictions = new string[] { null, null, null, null };

        public SqliteTableLister(SqliteInstance server)
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
        private SqliteInstance _server;
        private DbConnection _conn;
        #endregion
    }
}
