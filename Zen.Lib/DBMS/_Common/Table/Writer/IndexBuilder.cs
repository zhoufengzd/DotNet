using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Zen.DBMS
{
    using ListFormatter = Zen.Utilities.Text.Delimiter;

    public class IndexAttributes
    {
        public string TableName;
        public string IndexName;
        public List<string> IndexColumns;

        public bool IsUnique = false;
        public bool IsClustered = false;
    }

    public class IndexBuilder
    {
        static readonly string CreateIndexSqlFmt = "CREATE {0}INDEX {1} ON {2} ({3})";
        static readonly string DropIndexSqlFmt = "DROP INDEX {0}.{1}";
        static readonly string ColNameFmt = "[{0}]";
        static readonly string UniqueKwd = "UNIQUE";
        static readonly string ClusteredKwd = "CLUSTERED";

        public IndexBuilder(IndexAttributes indexAttributes, SqlConnection conn)
        {
            _indexAttributes = indexAttributes;
            _connection = conn;
        }

        public string TableName
        {
            get { return _indexAttributes.TableName; }
        }
        public string IndexName
        {
            get { return _indexAttributes.IndexName; }
        }

        public void CreateIndex()
        {
            StringBuilder optAttribute = new StringBuilder();
            if (_indexAttributes.IsUnique)
                optAttribute.AppendFormat("{0} ", UniqueKwd);
            if (_indexAttributes.IsClustered)
                optAttribute.AppendFormat("{0} ", ClusteredKwd);
            string columnList = ListFormatter.ToString(_indexAttributes.IndexColumns, ", ", ColNameFmt);

            string createIndexDDL = string.Format(CreateIndexSqlFmt, optAttribute.ToString(), _indexAttributes.IndexName, _indexAttributes.TableName, columnList);
            using (SqlCommand cmd = new SqlCommand(createIndexDDL, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void DropIndex()
        {
            string dropIndexDDL = string.Format(DropIndexSqlFmt, _indexAttributes.TableName, _indexAttributes.IndexName);
            using (SqlCommand cmd = new SqlCommand(dropIndexDDL, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        #region private methods
        #endregion

        #region private memebers
        private IndexAttributes _indexAttributes;
        private SqlConnection _connection;
        #endregion
    }
}
