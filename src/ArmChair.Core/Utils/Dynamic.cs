// Copyright 2014 - dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;

    /// <summary>
    /// defult implementation of a Dynamic object, inherit off this for dyanmic properties
    /// </summary>
    public abstract class Dynamic : DynamicObject, IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public void SetKey(string name, object value)
        {
            var key = GetKey(name);
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string key = GetKey(binder.Name);
            return _dictionary.TryGetValue(key, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            SetKey(binder.Name, value);
            return true;
        }


        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public object this[string index]
        {
            get
            {
                object result;
                return _dictionary.TryGetValue(index, out result) ? result : null;
            }
            set
            {
                SetKey(index, value);
            }
        }

        public ICollection<string> Keys => _dictionary.Keys;
        public ICollection<object> Values => _dictionary.Values;

        protected virtual string GetKey(string key)
        {
            return key;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public override string ToString()
        {
            return _dictionary.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item.Key);
        }

        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;
    }
}