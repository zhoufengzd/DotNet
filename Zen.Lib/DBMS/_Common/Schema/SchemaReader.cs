using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Zen.DBMS.Schema
{
    public class SchemaReader
    {
        public const string CollectionName = "CollectionName";

        public const string TableCollection = "Tables";
        public const string ViewCollection = "Views";
        public const string ColumnCollection = "Columns";

        public static List<string> GetSchemaNames(DbConnection conn)
        {
            return (new TableViewer(conn.GetSchema())).GetData<string>(CollectionName);
        }

        public static List<DataTable> GetAllSchema(DbConnection conn)
        {
            List<string> colNames = GetSchemaNames(conn);
            List<DataTable> schemas = new List<DataTable>(colNames.Count);
            foreach (string name in colNames)
            {
                try
                {
                    schemas.Add(conn.GetSchema(name));
                }
                catch (Exception)
                {
                }
            }

            return schemas;
        }

        public static DataTable GetSchema(DbConnection conn, string collectionName, string[] restrictions)
        {
            return conn.GetSchema(collectionName, restrictions);
        }
    }
}
