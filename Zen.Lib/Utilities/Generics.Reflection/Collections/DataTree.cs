using System.Collections.Generic;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// Each dataNode must have an identifier (key).
    /// </summary>
    public sealed class DataNode<TKey, TData>
    {
        public DataNode(TKey id)
        {
            _id = id;
            _parent = null;
            _children = new Dictionary<TKey, DataNode<TKey, TData>>();
        }

        public TKey Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public TData Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public DataNode<TKey, TData> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public Dictionary<TKey, DataNode<TKey, TData>> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        #region private data
        private TKey _id;
        private TData _data;
        private DataNode<TKey, TData> _parent;
        private Dictionary<TKey, DataNode<TKey, TData>> _children;
        #endregion
    }

    /// <summary>
    /// Simple DataTree with Root only
    /// </summary>
    public interface IDataTree<TKey, TData>
    {
        DataNode<TKey, TData> Root { get; }
    }

    /// <summary>
    /// DataTree
    /// </summary>
    public class DataTree<TKey, TData> : IDataTree<TKey, TData>
    {
        public DataTree()
            : this(null)
        {
        }
        public DataTree(DataNode<TKey, TData> root)
        {
            _root = root;
        }

        public virtual DataNode<TKey, TData> Root
        {
            get { return _root; }
            set { _root = value; }
        }

        #region private data
        private DataNode<TKey, TData> _root;
        #endregion
    }
}