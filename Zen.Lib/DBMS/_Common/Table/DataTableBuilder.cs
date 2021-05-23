using System;
using System.Data;
using Zen.DBMS.Schema;

namespace Zen.DBMS
{
    /// <summary>
    /// Helper to build a fresh System.Data.DataTable from scratch
    /// </summary>
    public sealed class DataTableBuilder
    {
        /// <summary>
        /// Default: set all column types as string
        /// </summary>
        /// <param name="nColumnCount"></param>
        public static DataTable BuildTable(int nColumnCount, string tableName)
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = tableName;

            int nIndex = 0;
            while (nIndex++ < nColumnCount)
            {
                dataTable.Columns.Add(new DataColumn());
            }
            return dataTable;
        }

        public static DataTable BuildTable(TableSchema tblSchema)
        {
            return BuildTable(tblSchema, false);
        }
        public static DataTable BuildTable(TableSchema tblSchema, bool alwaysAllowDBNull)
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = tblSchema.TableName;

            DataRowCollection colDefs = tblSchema.ColumnDefs.Rows;
            foreach (DataRow dtRow in colDefs)
            {
                DataColumn column = new DataColumn();

                column.ColumnName = dtRow["ColumnName"].ToString();

                column.DataType = (Type)dtRow["DataType"];
                if (column.DataType == typeof(string))
                    column.MaxLength = (int)dtRow["ColumnSize"];

                SqlDbType colDBType = (SqlDbType)Enum.Parse(typeof(SqlDbType), (string)dtRow["DataTypeName"], true);
                if (alwaysAllowDBNull || colDBType == SqlDbType.Timestamp)
                    column.AllowDBNull = true;
                else
                    column.AllowDBNull = (bool)dtRow["AllowDBNull"];

                dataTable.Columns.Add(column);
            }

            return dataTable;
        }
    }
}
