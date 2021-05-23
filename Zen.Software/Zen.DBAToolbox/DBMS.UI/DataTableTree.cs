using System.Collections.Generic;
using System.Data;
using Zen.Common.Def;
using Zen.DBMS;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.DBAToolbox
{
    using Cell = Pair<int, int>;

    /// <summary>
    /// To allow call back into the table: 
    ///    IDataTableTree --> DataTable [ DataTable Provider] --> Cell
    /// </summary>
    internal class TableLink
    {
        public TableLink()
            : this(null, null)
        {
        }
        public TableLink(IDataTableTree tree, Cell cell)
        {
            _tree = tree;
            _cell = cell;
        }

        public IDataTableTree Tree
        {
            get { return _tree; }
            set { _tree = value; }
        }
        public Cell Cell
        {
            get { return _cell; }
            set { _cell = value; }
        }

        #region private data
        private IDataTableTree _tree;
        private Cell _cell;
        #endregion
    }

    /// <summary>
    /// Support grouping for any DataTable object
    /// </summary>
    public interface IDataTableTree : IDataTree<string, Cell>
    {
        void Refresh();
        void ReGroup(IEnumerable<string> indexColumns, string leafColumn);
        string ToString(Cell location);
    }

    /// <summary>
    /// IDataTableTree Implementation: 
    ///   Responsible to display DataTable as a tree
    /// </summary>
    public class DataTableTree : IDataTableTree
    {
        public DataTableTree(IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
        {
            _tblViewer = new TableViewer(tblProvider);
            _indexColumns = indexColumns;
            _leafColumn = leafColumn;
        }

        public DataNode<string, Cell> Root
        {
            get
            {
                if (_root == null)
                    Refresh();

                return _root;
            }
        }

        public void Refresh()
        {
            _tblViewer.Refresh();
            _leafColOrdinal = _tblViewer.ColOrdinal(_leafColumn);

            _root = _tblViewer.Groupby(_indexColumns, _leafColumn);
        }

        public void ReGroup(IEnumerable<string> indexColumns, string leafColumn)
        {
            _indexColumns = indexColumns;
            _leafColumn = leafColumn;
            _leafColOrdinal = _tblViewer.ColOrdinal(_leafColumn);

            _root = _tblViewer.Groupby(_indexColumns, _leafColumn);
        }

        public virtual string ToString(Cell location)
        {
            if (location.Column == -1)
                return _tblViewer.GetData<string>(location.Row, _leafColOrdinal);
            else
                return _tblViewer.GetData<string>(location);
        }

        #region private data
        protected IEnumerable<string> _indexColumns;
        protected string _leafColumn;
        protected int _leafColOrdinal;

        protected TableViewer _tblViewer;
        protected DataNode<string, Cell> _root;
        #endregion
    }
}
