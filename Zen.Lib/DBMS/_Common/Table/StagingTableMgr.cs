using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Zen.DBMS
{
    /// <summary>
    /// Helper class to manage temporary staging table.
    /// </summary>
    public class StagingTableMgr
    {
        #region static readonly strings
        static readonly string CreateTableSqlFmt = @"CREATE TABLE {0} ({1})";
        static readonly string DropTableSqlFmt = @"DROP TABLE {0}";
        static readonly string CleanTableSqlFmt = @"TRUNCATE TABLE {0}";
        #endregion

        public StagingTableMgr(string tableName, string columnDefs, SqlConnection conn)
        {
            _tableName = tableName;
            _columnDefs = columnDefs;
            _connection = conn;
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public void CreateTable()
        {
            string createTableDDL = string.Format(CreateTableSqlFmt, _tableName, _columnDefs);
            using (SqlCommand cmd = new SqlCommand(createTableDDL, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void DropTable()
        {
            string dropTableDDL = string.Format(DropTableSqlFmt, _tableName);
            using (SqlCommand cmd = new SqlCommand(dropTableDDL, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void CleanTable()
        {
            string cleanTableDDL = string.Format(CleanTableSqlFmt, _tableName);
            using (SqlCommand cmd = new SqlCommand(cleanTableDDL, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        #region private memebers
        private string _tableName;
        private string _columnDefs;
        private SqlConnection _connection;
        #endregion
    }
}
