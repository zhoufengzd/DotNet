using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Search
{
    /// <summary>
    /// Shared class between indexer & search.
    ///   This class should be self contained.
    ///   Do **NOT** introduce any external dependencies in this class
    ///   May use external config info to decide specialized logic, like skipped columns
    /// </summary>
    public class ColumnValueFormatter
    {
        public const string NULLValue = "A0B6CE5A4E0541c28DDE9B132A65F705";
        public const string Zero = "0", One = "1";
        public const string DateTimeFmt = @"yyyyMMddhhmmss";

        public static string GetFormattedString(string tableName, string columnName, object cellValue)
        {
            // Handle special cases like DocId, RowGuid, DateSend? DocLink?
            if (cellValue is byte[])
                return null;

            string colValue = null;
            if (cellValue == System.DBNull.Value)
                colValue = Zen.Search.Util.ColValueDefs.NULLValue;
            else if (cellValue is DateTime)
                colValue = ((DateTime)cellValue).ToString(DateTimeFmt);
            else if (cellValue is Boolean)
                colValue = ((bool)cellValue) ? One : Zero;
            else
                colValue = (cellValue.ToString()).Replace("\t", " ");

            return colValue;
        }
    }
}
