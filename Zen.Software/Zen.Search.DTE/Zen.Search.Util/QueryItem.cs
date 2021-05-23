using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Zen.Search.Util
{
    public class QueryItem : IQueryItem
    {
        public QueryItem()
        {
        }

        public int NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        public int Parent
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private int _nodeId = -1;
        private int _parentId = -1;
    }

    public class ConnectorItem : QueryItem, IConnectorItem
    {
        public ConnectorItem()
        {
        }

        public int LeftChild
        {
            get { return _leftChild; }
            set { _leftChild = value; }
        }

        public int RightChild
        {
            get { return _rightChild; }
            set { _rightChild = value; }
        }

        public FilterType FilterType
        {
            get { return _filterType; }
            set { _filterType = value; }
        }

        private int _leftChild = -1;
        private int _rightChild = -1;
        private FilterType _filterType = FilterType.And;
    }

    public class LeafItem : QueryItem, ILeafItem
    {
        public LeafItem()
        {
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public string Fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        public string QueryValue
        {
            get { return _queryValue; }
            set { _queryValue = value; }
        }
        public CompareType CompareType
        {
            get { return _compareType; }
            set { _compareType = value; }
        }

        #region private data
        private string _tableName;
        private string _fieldname;
        private string _queryValue;
        private CompareType _compareType = CompareType.Equals;
        #endregion
    }

}
