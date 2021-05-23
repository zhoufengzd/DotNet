using System;
using System.Collections.Generic;
using System.Data;
using Zen.Common.Def;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.DBMS
{
    using Cell = Pair<int, int>;
    
    /// <summary>
    /// Reinvent the 'LINQ' wheel? :-)
    /// </summary>
    public sealed class TableViewer
    {
        /// <summary> No refresh supported. </summary>
        public TableViewer(DataTable dataTbl)
        {
            System.Diagnostics.Debug.Assert(dataTbl != null);
            _dataTbl = dataTbl;
        }
        /// <summary> To support refresh data table as needed. </summary>
        public TableViewer(IDataProvider<DataTable> tblProvider)
        {
            _dataProvider = tblProvider;
        }

        public DataTable DataTable
        {
            get
            {
                if (_dataProvider != null && _dataTbl == null)
                    FetchTable();
                return _dataTbl;
            }
        }

        public int ColOrdinal(string colName)
        {
            return DataTable.Columns.IndexOf(colName);
        }

        public List<T> GetData<T>(string colName)
        {
            int colOrdinal = DataTable.Columns.IndexOf(colName);
            return (colOrdinal < 0) ? null : GetData<T>(colOrdinal);
        }
        public List<T> GetData<T>(int colOrdinal)
        {
            DataRowCollection rows = DataTable.Rows;
            int rowCount = rows.Count;

            List<T> columnData = new List<T>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                object cell = rows[i][colOrdinal];
                if (cell != DBNull.Value)
                    columnData.Add((T)cell);
            }

            return columnData;
        }
        public T GetData<T>(Cell location)
        {
            DataRow row = DataTable.Rows[location.Row];
            object cell = row[location.Column];

            if (cell == null || cell == DBNull.Value)
                return default(T);

            return (T)cell;
        }
        public T GetData<T>(int rowIndex, int columnOrdinal)
        {
            DataRow row = DataTable.Rows[rowIndex];
            object cell = row[columnOrdinal];

            if (cell == null || cell == DBNull.Value)
                return default(T);

            return (T)cell;
        }

        /// <summary>
        /// Will refresh only if tblProvider is provided in the constructor
        /// </summary>
        public void Refresh()
        {
            System.Diagnostics.Debug.Assert(_dataProvider != null);
            if (_dataProvider != null)
            {
                _dataTbl = _dataProvider.Fetch();
                _grouper = null;
            }
        }

        // Group by + Order by
        public DataNode<string, Cell> Groupby(IEnumerable<string> indexColumns, string leafColumn)
        {
            if (_grouper == null)
                _grouper = new TableGrouper(_dataTbl);

            return _grouper.GroupBy(indexColumns, leafColumn);
        }

        // Search + Order by
        public Set<int> Search()
        {
            return null;
        }

        // Show / hide some nodes
        public DataNode<string, Cell> Filter()
        {
            return null;
        }

        #region private functions
        private void FetchTable()
        {
            _dataTbl = _dataProvider.Fetch();
            _grouper = new TableGrouper(_dataTbl);
        }
        #endregion

        #region private data
        private IDataProvider<DataTable> _dataProvider = null;
        private TableGrouper _grouper;
        private DataTable _dataTbl;
        #endregion
    }
}
