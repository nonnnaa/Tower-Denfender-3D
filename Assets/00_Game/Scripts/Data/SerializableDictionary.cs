using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> // Chat GPT hân hạnh tài trợ free
{
    [Serializable]
    public struct Pair
    {
        public TKey key;
        public TValue value;
    }

    [SerializeField] private List<Pair> items = new List<Pair>();
    
    public void Add(TKey key, TValue value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(items[i].key, key))
            {
                items[i] = new Pair { key = key, value = value };
                return;
            }
        }
        items.Add(new Pair { key = key, value = value });
    }
    
    public bool TryGetValue(TKey key, out TValue value)
    {
        foreach (var item in items)
        {
            if (EqualityComparer<TKey>.Default.Equals(item.key, key))
            {
                value = item.value;
                return true;
            }
        }
        value = default;
        return false;
    }
    
    public TValue this[TKey key]
    {
        get
        {
            foreach (var item in items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.key, key))
                    return item.value;
            }
            throw new KeyNotFoundException($"Key {key} not found in SerializableDictionary");
        }
        set
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(items[i].key, key))
                {
                    items[i] = new Pair { key = key, value = value };
                    return;
                }
            }
            items.Add(new Pair { key = key, value = value });
        }
    }
    
    public IEnumerable<TKey> Keys
    {
        get
        {
            foreach (var item in items)
                yield return item.key;
        }
    }
    
    public IEnumerable<TValue> Values
    {
        get
        {
            foreach (var item in items)
                yield return item.value;
        }
    }

    public int Count => items.Count;
    
    public bool Remove(TKey key)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(items[i].key, key))
            {
                items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void Clear()
    {
        items.Clear();
    }
}