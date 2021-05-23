using System.Data;
using System.Data.Common;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS.Oracle
{
    public sealed class OracleInstance : DataSourceInstance
    {
        public OracleInstance(OracleContext context, string connString)
            : base(context, connString)
        {
        }

        public override IDataProvider<DataTable> GetTableLister()
        {
            return new OracleTableLister(this);
        }

        public IDataProvider<DataTable> GetViewLister()
        {
            return new OracleViewLister(this);
        }
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class OracleTableLister : IDataProvider<DataTable>
    {
        // OWNER, TABLE_NAME, TYPE ['SYSTEM' or 'USER']
        static readonly string[] Restrictions = new string[] { null, null };

        public OracleTableLister(OracleInstance server)
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
        private OracleInstance _server;
        private DbConnection _conn;
        #endregion
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class OracleViewLister : IDataProvider<DataTable>
    {
        // OWNER, VIEW_NAME, TYPE 
        static readonly string[] Restrictions = new string[] { null, null };

        public OracleViewLister(OracleInstance server)
        {
            _server = server;
        }

        public DataTable Fetch()
        {
            if (_conn == null)
                _conn = _server.OpenNewConnection();

            return _conn.GetSchema(SchemaReader.ViewCollection, Restrictions);
        }

        #region private data
        private OracleInstance _server;
        private DbConnection _conn;
        #endregion
    }

}
