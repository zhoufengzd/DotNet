using System.Data;
using System.Data.Common;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS.DB2
{
    public sealed class DB2Instance : DataSourceInstance
    {
        public DB2Instance(DBMSContext context, string connString)
            : base(context, connString)
        {
        }

        public override IDataProvider<DataTable> GetTableLister()
        {
            return new DB2TableLister(this);
        }
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class DB2TableLister : IDataProvider<DataTable>
    {
        // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
        static readonly string[] Restrictions = new string[] { null, null, null, null };

        public DB2TableLister(DB2Instance server)
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
        private DB2Instance _server;
        private DbConnection _conn;
        #endregion
    }
}
