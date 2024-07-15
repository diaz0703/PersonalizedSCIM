using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.SCIM.WebHostSample.Adicionales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Microsoft.SCIM.WebHostSample.Provider
{
    public class DiccionarioThorDB<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dic;
        public DiccionarioThorDB()
        {
            _dic = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key] { get => _dic[key];
            set => _dic[key] = value; }

        public ICollection<TKey> Keys => _dic.Keys;

        public ICollection<TValue> Values => _dic.Values;

        public int Count => _dic.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TKey key, TValue value)
        {
            _dic.Add(key, value);
            ThorHelperBD.InsertaEntidad(value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        => Add(item.Key, item.Value);

        public void Clear()
        {
            _dic.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        => _dic.Contains(item);

        public bool ContainsKey(TKey key)
        => _dic.ContainsKey(key);   

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _dic.GetEnumerator();

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        => _dic.Remove(item.Key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _dic.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
        => _dic.GetEnumerator();
    }
}
