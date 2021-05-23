using System.Collections;
using System.Collections.Generic;

namespace Zen.Utilities.Generics
{
    /// <summary> Multi Map: 1 --> N </summary>
    public sealed class MultiMap<TKey, TValue> : Dictionary<TKey, ICollection<TValue>>
    {
        public MultiMap()
        {
        }
        public MultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
            :base(dictionary)
        {
        }

        public void Add(TKey key, TValue value)
        {
            ICollection<TValue> values = null;
            if (base.TryGetValue(key, out values))
            {
                values.Add(value);
            }
            else
            {
                values = new List<TValue>();
                values.Add(value);
                base.Add(key, values);
            }
        }
    }
}
