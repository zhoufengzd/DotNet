using System.Collections;
using System.Collections.Generic;
using Zen.DBMS.Schema;

namespace Zen.DBMS
{
    public class TableCell : ICell
    {
        public TableCell()
        {
            _strValue = string.Empty;
        }
        public TableCell(string strValue)
        {
            _strValue = strValue;
        }

        public void FromString(string strValue)
        {
            _strValue = strValue;
        }
        public override string ToString()
        {
            return _strValue;
        }

        #region private data
        private string _strValue;
        #endregion
    }

    public class SimpleContainerBase<T> : ISimpleContainer<T>
    {
        public int Size()
        {
            return _container.Count;
        }
        public void Clear()
        {
            _container.Clear();
        }
        public void Reserve(int lCount)
        {
            _container.Capacity = lCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        protected List<T> _container = new List<T>();
    }

    public class TableRow : SimpleContainerBase<ICell>, IRow
    {
        public TableRow()
        {
        }

        public void Push_back(ICell element)
        {
            _container.Add(element);
        }
        public ICell At(int lColIndex)
        {
            return _container[lColIndex];
        }
    }

    public class TableRowSet : SimpleContainerBase<IRow>, IRowSet
    {
        public TableRowSet()
        {
        }

        public void Push_back(IRow dataRow)
        {
            _container.Add(dataRow);
        }
        public IRow At(int lRowIndex)
        {
            return _container[lRowIndex];
        }
        public ICell At(int lRowIndex, int lColIndex)
        {
            return (At(lRowIndex)).At(lColIndex);
        }
        public void SetMetaData(int lColIndex, ColumnMeta colMeta)
        {
            if (_ColumnMetaList == null)
                _ColumnMetaList = new List<ColumnMeta>();

            if (_ColumnMetaList.Count <= lColIndex)
            {
                for (int i = _ColumnMetaList.Count; i <= lColIndex; i++)
                    _ColumnMetaList.Add(null);
            }

            _ColumnMetaList[lColIndex] = colMeta;
        }
        #region private data
        private List<ColumnMeta> _ColumnMetaList;
        #endregion
    }

}
