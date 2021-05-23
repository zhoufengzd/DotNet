using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Zen.Utilities.Algorithm;

namespace Zen.DBMS.SqlCe
{
    public sealed class SqlCeInstance : SvrInstance
    {
        public SqlCeInstance(string connString)
            : this(new SqlCeContext(), connString)
        {
        }
        public SqlCeInstance(SqlCeContext context, string connString)
            : base(context, connString)
        {
        }

        public void CreateDatabase()
        {
            using (SqlCeEngine engine = new SqlCeEngine(_connString))
            {
                engine.CreateDatabase();
                engine.Dispose();
            }
        }

        public bool Verify()
        {
            bool result = false;
            using (SqlCeEngine engine = new SqlCeEngine(_connString))
            {
                result = engine.Verify(VerifyOption.Enhanced);
                if (!result)
                {
                    try { engine.Upgrade(); }
                    catch (Exception) { };

                    if (!engine.Verify(VerifyOption.Enhanced))
                        engine.Repair(null, RepairOption.RecoverCorruptedRows);

                    result = engine.Verify(VerifyOption.Enhanced);
                }
                engine.Dispose();
            }

            return result;
        }

        public void Upgrade()
        {
            using (SqlCeEngine engine = new SqlCeEngine(_connString))
            {
                engine.Upgrade();
                engine.Dispose();
            }
        }

        public void Compact()
        {
            using (SqlCeEngine engine = new SqlCeEngine(_connString))
                engine.Compact(null);
        }

        public void Shrink()
        {
            using (SqlCeEngine engine = new SqlCeEngine(_connString))
                engine.Shrink();
        }

        public IDataProvider<DataTable> GetTableLister()
        {
            return new SqlCeTableLister(this);
        }

        #region internal functions
        #endregion

        #region private data
        #endregion
    }

    /// <summary>
    /// Provide a list of table / view objects on current server / database
    /// </summary>
    public sealed class SqlCeTableLister : IDataProvider<DataTable>
    {
        // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
        static readonly string[] Restrictions = new string[] { null, null, null, null };

        public SqlCeTableLister(SqlCeInstance server)
        {
            _server = server;
        }

        public DataTable Fetch()
        {
            if (_conn == null)
                _conn = _server.OpenNewConnection();

            DataTable tbl = null;
            _server.Execute(string.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES"), out tbl);
            return tbl;
        }

        #region private data
        private SqlCeInstance _server;
        private DbConnection _conn;
        #endregion
    }

//    public string[] GetTableConstraints(string tablename)
//    {
//        string[] constraints;

//        using (SqlCeConnection conn = new SqlCeConnection(LocalConnectionString))
//        {
//            conn.Open();
//            using (SqlCeCommand cmd = conn.CreateCommand())
//            {
//                cmd.CommandText =
//                 @"SELECT CONSTRAINT_NAME
//   FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
//   WHERE TABLE_NAME=@Name";
//                cmd.Parameters.AddWithValue("@Name", tablename);
//                constraints = PopulateStringList(cmd);
//            }
//        }

//        return constraints;
//    }


}
