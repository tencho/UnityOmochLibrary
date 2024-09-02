using System.Collections.Generic;
using System.Linq;

namespace Omoch.Collections
{
    public class OrderedDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionaty;
        private readonly List<TKey> keys;

        public OrderedDictionary()
        {
            dictionaty = new();
            keys = new();
        }

        public void Add(TKey key, TValue value)
        {
            dictionaty.Add(key, value);
            keys.Add(key);
        }

        public void Remove(TKey key)
        {
            dictionaty.Remove(key);
            keys.Remove(key);
        }

        public void Clear()
        {
            dictionaty.Clear();
            keys.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return dictionaty.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return dictionaty.ContainsValue(value);
        }

        public IEnumerable<TValue> Values => keys.Select(key => dictionaty[key]);
        public IEnumerable<TKey> Keys => keys;

        public TValue this[TKey key]
        {
            get => dictionaty[key];
            set
            {
                if (dictionaty.ContainsKey(key))
                {
                    dictionaty[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }
    }
}
