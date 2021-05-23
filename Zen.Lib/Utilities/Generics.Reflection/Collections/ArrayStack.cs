using System.Collections.Generic;
using System.Collections;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// Stack with index accessor
    /// </summary>
    public sealed class ArrayStack<T>: IEnumerable<T>
    {
        public ArrayStack(int maxCount)
        {
            _container = new List<T>(maxCount);
        }

        public void Push(T item)
        {
            _container.Insert(0, item);
        }
        public T Pop()
        {
            T data = _container[0];
            _container.RemoveAt(0);

            return data;
        }
        public T Peek()
        {
            return _container[0];
        }

        public void Set(T item, int location)
        {
            _container[location] = item;
        }
        public void SetLast(T item)
        {
            if (_container.Count > 0)
                _container[_container.Count - 1] = item;
            else
                _container.Add(item);
        }

        public int Count 
        {
            get { return _container.Count; } 
        }
        public void Clear()
        {
            _container.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _container.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public T[] ToArray()
        {
            return _container.ToArray();
        }

        #region private data
        private List<T> _container;
        #endregion
    }
}
