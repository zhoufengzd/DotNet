using System;
using System.Collections.Generic;
using System.Text;
using Zen.DBMS.Schema;
using System.Data.Common;
using System.Data;

namespace Zen.DBMS.SqlServer
{
    public sealed class SqlSvrSchemaReader : SchemaReader
    {
        public static readonly string DatabaseCollection = "Databases";
        public static readonly string BaseTableCollection = "BASE TABLE";
        public static readonly string ViewCollection = "VIEW";

        public static List<string> GetDatabaseNames(DbConnection conn)
        {
            const string DBNameColumn = "DATABASE_NAME";
            return (new TableViewer(conn.GetSchema(DatabaseCollection))).GetData<string>(DBNameColumn);
        }

        /// <summary>
        /// Warning: it's caller's responsibility to change the initial catalog (e.g., database)
        ///   before querying table names
        /// </summary>
        public static List<DBObjectIdentifier> GetTableList(DbConnection conn, string owner)
        {
            // TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE ['BASE TABLE' or 'VIEW']
            string[] restrictions = new string[] { null, owner, null, null };

            DataTable dt = conn.GetSchema(TableCollection, restrictions);
            DataRowCollection rows = dt.Rows;
            int rowCount = rows.Count;

            List<DBObjectIdentifier> names = new List<DBObjectIdentifier>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                DBObjectIdentifier obj = new DBObjectIdentifier();
                obj.Type = DBObjectEnum.Table;
                obj.Name = rows[i]["TABLE_NAME"].ToString();
                obj.Database = rows[i]["TABLE_CATALOG"].ToString();
                obj.Owner = rows[i]["TABLE_SCHEMA"].ToString();
                obj.SubType = rows[i]["TABLE_TYPE"].ToString();
                names.Add(obj);
            }

            return names;
        }

        public static List<ColumnMeta> GetColumnNames(DbConnection conn, DBObjectIdentifier parentTable)
        {
            string[] restrictions = new string[] { null, null, parentTable.Name };

            DataTable dt = conn.GetSchema(ColumnCollection, restrictions);
            DataRowCollection rows = dt.Rows;
            int rowCount = rows.Count;

            List<ColumnMeta> names = new List<ColumnMeta>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                string columnType = rows[i]["DATA_TYPE"].ToString();
                ColumnMeta col = new ColumnMeta(rows[i]["COLUMN_NAME"].ToString(), DataTypeEnum.Unspecified, null);
                //col.DataType.NativeDataType = 
                // no native type exposed from this
                names.Add(col);
            }

            return names;
        }
    }
}
