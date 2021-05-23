using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Zen.DBMS.Schema;
using Zen.Utilities.Text;

namespace Zen.DBMS
{
    using Macros = Zen.Utilities.Generics.Macros;

    /// <summary>
    /// Builds up db accessor / stored procedure on any given table
    /// </summary>
    /// 
    class TableSpBuilder
    {
        #region static readonly strings
        static readonly string StoredProcFmt = "CREATE PROCEDURE {0} \n\t( {1} ) \nAS \nBEGIN \n\t{2} \nEND";

        static readonly string UpdateDMLFmt = "UPDATE {0} \n\tSET {1} {2}";
        static readonly string SelectDMLFmt = "SELECT {0} \n\tFROM {1} {2}";
        static readonly string InsertDMLFmt = "INSERT INTO {0} \n\t({1}) \n\tVALUES({2})";
        static readonly string DeleteDMLFmt = "DELETE FROM {0} {1}";

        static readonly string WhereSqlFmt = "\n\tWHERE {0}"; // Optional 'WHERE' clause

        static readonly string ColNameFmt = "[{0}]";
        static readonly string ColNameParamFmt = "@{0}";
        static readonly string ColValueParamFmt = "[{0}] = @{0}";
        static readonly string ColValueParamCaseFmt = "[{0}] = CASE WHEN (@{0} IS NULL) THEN [{0}] ELSE @{0} END";
        #endregion

        public TableSpBuilder()
        {
            _paramBuilder = new ColParamBuilder();
        }

        public string BuildUpdateSpDDL(SqlParameterCollection parameters, string spName, TableAdapterContext ctx)
        {
            StringBuilder sqlParamText = _paramBuilder.AddUpdateParameters(parameters, ctx);

            List<string> updatableValueColumns = new List<string>(ctx.ValueColumns);
            foreach (string colName in ctx.TableSchema.ReadOnlyColumnList)
            {
                updatableValueColumns.Remove(colName);
            }

            string setValFmt = (ctx.Overwrite) ? ColValueParamFmt : ColValueParamCaseFmt;
            string sqlBodyText = BuildUpdateSqlBody(ctx.TableSchema, updatableValueColumns, ctx.KeyColumns, setValFmt);

            return string.Format(StoredProcFmt, spName, sqlParamText, sqlBodyText);
        }

        public string BuildSelectSpDDL(SqlParameterCollection parameters, string spName, TableAdapterContext ctx)
        {
            StringBuilder sqlParamText = _paramBuilder.AddSelectParameters(parameters, ctx);

            string valueClause = Delimiter.ToString<string>(ctx.ValueColumns, ", ", "[{0}]");

            string whereClause = string.Empty;
            if (ctx.SearchFilter != null && ctx.SearchFilter.Count > 0)
                whereClause = string.Format(WhereSqlFmt, Delimiter.ToString<SearchCondition>(ctx.SearchFilter, " AND ", ColValueParamFmt));

            string sqlBodyText = string.Format(SelectDMLFmt, valueClause, ctx.TableSchema.TableName, whereClause);
            return string.Format(StoredProcFmt, spName, sqlParamText, sqlBodyText);
        }

        public string BuildInsertSpDDL(SqlParameterCollection parameters, string spName, TableAdapterContext ctx)
        {
            return null;
            //StringBuilder sqlParamText = _paramBuilder.AddSelectParameters(parameters, ctx);

            //string valueClause = ListFormatter.ToDelimittedString<string>(ctx.ValueColumns, ", ", "[{0}]");
            //string sqlBodyText = string.Format(InsertDMLFmt, ctx.TableSchema.TableName, valueClause);
            //return string.Format(StoredProcFmt, spName, sqlParamText, sqlBodyText);
        }
        #region private functions

        /// <param name="tableName"></param>
        /// <param name="valueColumns">Required to build Set value statement</param>
        /// <param name="keyColumns">Required to build search conditions</param>
        /// <param name="setValFmt">Could erase everything with NULL or update with new value only</param>
        /// <returns></returns>
        private string BuildUpdateSqlBody(TableSchema tblSchema, List<string> valueColumns, List<string> keyColumns, string setValFmt)
        {
            Debug.Assert(valueColumns != null && valueColumns.Count > 0);
            Debug.Assert(keyColumns != null && keyColumns.Count > 0);

            StringBuilder lobBuffer = new StringBuilder();
            StringBuilder nonLobBuffer = new StringBuilder();
            StringBuilder updateSqlBody = new StringBuilder();

            string formatted = null;
            foreach (string valueColumn in valueColumns)
            {
                if (keyColumns.Contains(valueColumn))
                    continue;

                formatted = (setValFmt == null) ? valueColumn : string.Format(setValFmt, valueColumn);
                if (tblSchema.LobColumnList.Contains(valueColumn))
                {
                    if (lobBuffer.Length > 0)
                        lobBuffer.Append(", ");
                    lobBuffer.Append(formatted);
                }
                else
                {
                    if (nonLobBuffer.Length > 0)
                        nonLobBuffer.Append(", ");
                    nonLobBuffer.Append(formatted);
                }
            }

            string whereClause = string.Format(WhereSqlFmt, Delimiter.ToString<string>(keyColumns, " AND ", ColValueParamFmt));

            if (nonLobBuffer.Length > 0)
            {
                updateSqlBody.Append(string.Format(UpdateDMLFmt, tblSchema.TableName, nonLobBuffer.ToString(), whereClause));
                updateSqlBody.AppendLine();
            }
            if (lobBuffer.Length > 0)
            {
                updateSqlBody.Append(string.Format(UpdateDMLFmt, tblSchema.TableName, lobBuffer.ToString(), whereClause));
            }

            return updateSqlBody.ToString();
        }
        #endregion

        #region private data
        private ColParamBuilder _paramBuilder;
        #endregion
    }
    
    /// <summary>
    /// Builds up column parameters
    /// </summary>
    class ColParamBuilder
    {
        public ColParamBuilder()
        {
        }

        public StringBuilder AddSelectParameters(SqlParameterCollection parameters, TableAdapterContext ctx)
        {
            IEnumerable<string> paramColumns = Macros.Merge(ctx.SearchColumns, ctx.ValueColumns);
            return DoAddParameters(parameters, ctx, paramColumns);
        }

        public StringBuilder AddUpdateParameters(SqlParameterCollection parameters, TableAdapterContext ctx)
        {
            IEnumerable<string> paramColumns = Macros.Merge(ctx.KeyColumns, ctx.ValueColumns);
            return DoAddParameters(parameters, ctx, paramColumns);
        }

        public StringBuilder AddDeleteParameters(SqlParameterCollection parameters, TableAdapterContext ctx)
        {
            IEnumerable<string> paramColumns = ctx.SearchColumns;
            return DoAddParameters(parameters, ctx, paramColumns);
        }

        public StringBuilder AddInsertParameters(SqlParameterCollection parameters, TableAdapterContext ctx)
        {
            IEnumerable<string> paramColumns = Macros.Merge(ctx.KeyColumns, ctx.ValueColumns);
            return DoAddParameters(parameters, ctx, paramColumns);
        }

        #region private functions
        private StringBuilder DoAddParameters(SqlParameterCollection parameters, TableAdapterContext ctx, IEnumerable<string> paramColumns)
        {
            parameters.Clear();
            if (paramColumns == null)
                return null;

            StringBuilder sqlParamText = new StringBuilder();
            TableSchema tblSchema = ctx.TableSchema;

            int counter = 0;
            foreach (string colName in paramColumns)
            {
                DataRow dtRow = tblSchema.ColumnDef(colName);
                AddParameter(dtRow, ref parameters, ref sqlParamText, counter, ctx.KeyColumns);
                counter++;
            }

            return (counter > 0)? sqlParamText: null;
        }

        private void AddParameter(DataRow dtRow, ref SqlParameterCollection parameters, ref StringBuilder sqlParamText, int counter, List<string> keyColumns)
        {
            string columnName = dtRow["ColumnName"].ToString();
            SqlDbType colDBType = (SqlDbType)Enum.Parse(typeof(SqlDbType), (string)dtRow["DataTypeName"], true);
            int columnSize = Convert.ToInt32(dtRow["ColumnSize"]);
            
            if (counter != 0)   // the only use of counter is to help format sqlParamText
                sqlParamText.Append(", ");

            string paramName = NameCleaner.Clean("@" + columnName, string.Empty);
            parameters.Add(paramName, colDBType, columnSize, columnName);
            if (colDBType == SqlDbType.NVarChar || colDBType == SqlDbType.VarChar || colDBType == SqlDbType.Binary || colDBType == SqlDbType.VarBinary)
                sqlParamText.AppendFormat("{0} {1}({2})", paramName, colDBType, (columnSize < 4000)? columnSize.ToString(): "max");
            else
                sqlParamText.AppendFormat("{0} {1}", paramName, colDBType);

            // Allow default 'NULL' parameter if it's not the key column
            if (keyColumns != null && !keyColumns.Contains(columnName))
                sqlParamText.Append(" = NULL");
        }
        #endregion
    }


}
