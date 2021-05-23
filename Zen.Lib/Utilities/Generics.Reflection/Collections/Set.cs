using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// Set: by default, it's not sorted.
    /// </summary>
    public class Set<T> : ICollection<T>
    {
        public Set()
        {
        }

        #region Set operations
        /// <summary>
        /// Return unions of this and right set. Current set is not modified.
        /// </summary>
        public Set<T> Union(Set<T> right)
        {
            Set<T> merged = new Set<T>();
            merged.AddRange(this);
            if (right != null)
                merged.AddRange(right);

            return merged;
        }

        /// <summary>
        /// Return intersections of this and right set. Current set is not modified.
        /// </summary>
        public Set<T> Intersect(Set<T> right)
        {
            if (right == null || right.Count < 1 || this.Count < 1)
                return null;

            Set<T> outer = null;
            Set<T> inner = null;
            if (Count < right.Count)
            {
                inner = this;
                outer = right;
            }
            else
            {
                inner = right;
                outer = this;
            }

            Set<T> interSect = new Set<T>();
            foreach (T item in inner)
            {
                if (outer.Contains(item))
                    interSect.Add(item);
            }

            return interSect.Count > 0 ? interSect : null;
        }

        public static Set<T> Intersect(IEnumerable<Set<T>> sets)
        {
            // Build count map so we start with the smallest set as outer set
            Dictionary<int, Set<T>> countMap = new Dictionary<int, Set<T>>();
            foreach (Set<T> set in sets)
                countMap.Add(set.Count, set);

            // Incrementally interset
            Set<T> result = countMap[0];
            for (int i = 1; i < countMap.Count; i++)
            {
                if (result == null || result.Count < 1)
                    break;

                result = result.Intersect(countMap[i]);
            }

            return result;
        }
        #endregion

        #region ICollection implementation
        public int Count
        {
            get { return _dictionary.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(T item)
        {
            if (_dictionary.ContainsKey(item))
            {
                _dictionary[item] = _dictionary[item] + 1;
            }
            else
            {
                _dictionary.Add(item, 1);
                _list.Add(item);
            }
        }
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
                Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _list.Clear();
        }
        public bool Contains(T item)
        {
            return _dictionary.ContainsKey(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int ind = arrayIndex; ind < array.Length; ind++)
                Add(array[ind]);
        }
        public bool Remove(T item)
        {
            bool result = _dictionary.Remove(item);
            if (result)
                _list.Remove(item);

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion

        #region Exposed List interface
        public T this[int index]
        {
            get { return _list[index]; }
        }
        public T[] ToArray()
        {
            return _list.ToArray();
        }
        public void Sort()
        {
            _list.Sort();
        }
        #endregion

        #region private data
        private Dictionary<T, int> _dictionary = new Dictionary<T, int>();
        private List<T> _list = new List<T>();
        #endregion
    }

    /// <summary>
    /// To do: disjoint set?
    /// </summary>
    public sealed class IntSet : ICollection<Int32>
    {
        const int VectorSize = 32; // 32 bit
        public IntSet(int max)
            : this(0, max)
        {
        }
        public IntSet(int min, int max)
        {
            _min = min;
            _max = max;
            _bitVector = new BitVector32[(_max - _min + 1) / VectorSize + 1];
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        #region Set operations
        /// <summary>
        /// Return unions of this and right set. Current set is not modified.
        /// </summary>
        public IntSet Union(IntSet right)
        {
            if (right == null || right.Count < 1)
                return this;

            if (this.Count < 1)
                return right;

            IntSet outer = null, inner = null;
            if (this.Min < right.Count)
            {
                inner = this;
                outer = right;
            }
            else
            {
                inner = right;
                outer = this;
            }

            int min = inner.Min;
            int max = Macros.Max<int>(this.Max, right.Max);
            IntSet merged = new IntSet(min, max);

            return merged;
        }

        /// <summary>
        /// Return intersections of this and right set. Current set is not modified.
        /// </summary>
        //public Set<T> Intersect(Set<T> right)
        //{
        //    if (right == null || right.Count < 1 || this.Count < 1)
        //        return null;

        //    Set<T> outer = null;
        //    Set<T> inner = null;
        //    if (Count < right.Count)
        //    {
        //        inner = this;
        //        outer = right;
        //    }
        //    else
        //    {
        //        inner = right;
        //        outer = this;
        //    }

        //    Set<T> interSect = new Set<T>();
        //    foreach (T item in inner)
        //    {
        //        if (outer.Contains(item))
        //            interSect.Add(item);
        //    }

        //    return interSect.Count > 0? interSect: null;
        //}
        #endregion

        #region ICollection implementation
        public int Count
        {
            get { return _max - _min; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(int item)
        {
            this[item] = true;
        }
        public void AddRange(IEnumerable<int> collection)
        {
            foreach (int item in collection)
                Add(item);
        }

        public void Clear()
        {
            for (int i = 0; i < _bitVector.Length; i++)
                _bitVector[i] = new BitVector32(0);
        }
        public bool Contains(int item)
        {
            return this[item];
        }
        public void CopyTo(int[] array, int arrayIndex)
        {
            for (int ind = arrayIndex; ind < array.Length; ind++)
                Add(array[ind]);
        }
        public bool Remove(int item)
        {
            this[item] = false;
            return true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _bitVector.Length; i++)
            {
                for (int j = 0; j < VectorSize; j++)
                {
                    if (_bitVector[i][j] == true)
                        yield return _min + (i * 32 + j);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion

        private bool this[int item]
        {
            get { item -= _min; return _bitVector[item / VectorSize][item % VectorSize]; }
            set { item -= _min; _bitVector[item / VectorSize][item % VectorSize] = value; }
        }

        private BitVector32 GetVector(ref int item)
        {
            item -= _min;
            return _bitVector[item / VectorSize];
        }

        private BitVector32[] _bitVector;
        private int _min;
        private int _max;
    }

}