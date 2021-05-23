using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Zen.Utilities.Text;

namespace Zen.DBMS
{
    /// <summary>
    /// Handles csv and other delimited files
    /// Keep it simple. Live on DataTable (data could be constructed manually, not from database)
    /// </summary>
    public sealed class CsvWriter
    {
        public CsvWriter()
        {
        }

        public static void Write(DataTable dataTbl, string filePath)
        {
            if (dataTbl == null)
                return;

            DataRowCollection rows = dataTbl.Rows;
            if (rows.Count < 1)
                return;

            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<string> colNames = new List<string>(dataTbl.Columns.Count);
                foreach (DataColumn cl in dataTbl.Columns)
                    colNames.Add(cl.ColumnName);
                writer.WriteLine(Delimiter.ToString(colNames));

                foreach (DataRow row in rows)
                    writer.WriteLine(Delimiter.ToString((IEnumerable<object>)row.ItemArray));
            }
        }

        public void Read(DataTable dataTable, StreamReader reader)
        {
            if (dataTable == null)
                return;

            dataTable.Clear();

            //DataRowCollection rows = dataTable.Rows;
            //object[] itemArray = null;
            //string docId = null;
            //foreach (KeyValuePair<string, Guid> guidPair in guidList)
            //{
            //    docId = guidPair.Key;
            //    if (_context.SkipErrorRecord && _context.DocIdMgr.HasError(docId))
            //        continue;

            //    itemArray = _docIDRowDataMap[docId];
            //    itemArray[_rowGuidColIndex] = guidPair.Value;

            //    DataRow dtRowTmp = dataTable.NewRow();
            //    dtRowTmp.ItemArray = itemArray;
            //    rows.Add(dtRowTmp);
            //}
        }
    }
}
