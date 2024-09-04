using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omoch.Collections
{
    /// <summary>
    /// シリアライズ化可能なDictionary
    /// インスペクター上で編集できるようリストとして扱う関係でデータを2重に持っているため
    /// 大きなデータにはあまり使わない方がいい
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private List<UnityKeyItem<TKey, TValue>> list = new();
        private Dictionary<TKey, TValue> dictionary;

        public IEnumerable<TKey> Keys
        {
            get
            {
                InitializeIfNeeds();
                return dictionary.Keys;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                InitializeIfNeeds();
                return dictionary.Values;
            }
        }

        public void Add(TKey key, TValue value)
        {
            InitializeIfNeeds();
            dictionary.Add(key, value);
            list.Add(new UnityKeyItem<TKey, TValue> { Key = key, Value = value });
        }

        public TValue this[TKey key]
        {
            get
            {
                InitializeIfNeeds();
                return dictionary[key];
            }
            set
            {
                InitializeIfNeeds();
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        private void InitializeIfNeeds()
        {
            if (dictionary == null)
            {
                dictionary = new();
                foreach (UnityKeyItem<TKey, TValue> item in list)
                {
                    dictionary[item.Key] = item.Value;
                }
            }
        }
    }

    [Serializable]
    internal class UnityKeyItem<K, V>
    {
        public K Key;
        public V Value;
    }
}