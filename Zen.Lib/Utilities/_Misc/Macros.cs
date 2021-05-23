using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Zen.Utilities.Generics
{
    // using Macros = Zen.Utilities.Generics.Macros; 

    /// <summary>
    /// Generic & simple macros to guard coding style.
    ///   Complicated logic should be placed under 'Zen.Utilities.Algorithm'
    /// </summary>
    public static class Macros
    {
        public static T SafeGet<T>(ref T obj) where T : new()
        {
            if (obj == null)
                obj = new T();

            return obj;
        }

        public static TValue SafeGet<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            //Debug.Assert(dictionary != null);
            if (dictionary == null)
                return defaultValue;

            TValue objValue = defaultValue;
            if (!dictionary.TryGetValue(key, out objValue))
            {
                dictionary.Add(key, defaultValue);
                return defaultValue;
            }

            return objValue;
        }

        public static object SafeGet(IDictionary dictionary, object key, object defaultValue)
        {
            //Debug.Assert(dictionary != null);
            if (dictionary == null)
                return defaultValue;

            if (!dictionary.Contains(key))
            {
                dictionary.Add(key, defaultValue);
                return defaultValue;
            }

            return dictionary[key];
        }

        public static TValue SafeGetNew<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue: class, new()
        {
            Debug.Assert(dictionary != null);
            if (dictionary == null)
                return new TValue();

            TValue objValue = null;
            if (!dictionary.TryGetValue(key, out objValue))
            {
                objValue = new TValue();
                dictionary.Add(key, objValue);
                return objValue;
            }

            return objValue;
        }

        public static void SafeAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue val)
        {
            Debug.Assert(dictionary != null);
            if (dictionary == null)
                return;

            if (dictionary.ContainsKey(key))
                dictionary[key] = val;
            else
                dictionary.Add(key, val);
        }

        public static void SafeAdd(IDictionary dictionary, object key, object val)
        {
            Debug.Assert(dictionary != null);
            if (dictionary == null)
                return;

            if (dictionary.Contains(key))
                dictionary[key] = val;
            else
                dictionary.Add(key, val);
        }


        /// <summary>return merged sorted collection </summary>
        public static IEnumerable<T> Merge<T>(IEnumerable<T> t1, IEnumerable<T> t2)
        {
            Set<T> merged = new Set<T>();
            if (t1 != null)
                merged.AddRange(t1);
            if (t2 != null)
                merged.AddRange(t2);

            return merged;
        }

        public static T Min<T>(T t1, T t2) where T: IComparable<T>
        {
            return (t1.CompareTo(t2) > 0) ? t2 : t1;
        }

        public static T Max<T>(T t1, T t2) where T : IComparable<T>
        {
            return (t1.CompareTo(t2) > 0) ? t1 : t2;
        }
    }
}
