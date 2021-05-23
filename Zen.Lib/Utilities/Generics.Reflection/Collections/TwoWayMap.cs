using System.Collections;
using System.Collections.Generic;

namespace Zen.Utilities.Generics
{
    /// <summary> Bi-direction Map. </summary>
    public sealed class TwowayMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        public TwowayMap()
        {
            _key1map = new Dictionary<T1, T2>();
            _key2map = new Dictionary<T2, T1>();
        }

        public void Add(T1 key1, T2 key2)
        {
            _key1map.Add(key1, key2);
            _key2map.Add(key2, key1);
        }

        public T1 FindT1(T2 key2)
        {
            return _key2map[key2];
        }

        public T2 FindT2(T1 key1)
        {
            return _key1map[key1];
        }

        public void Clear()
        {
            _key1map.Clear();
            _key2map.Clear();
        }

        public bool IsEmpty()
        {
            return (_key1map.Count > 0);
        }

        public int Count()
        {
            return _key1map.Count;
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _key1map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }


        #region private data
        private Dictionary<T1, T2> _key1map;
        private Dictionary<T2, T1> _key2map;
        #endregion
    }
}
