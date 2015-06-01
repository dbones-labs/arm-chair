namespace ArmChair.Utils
{
    using System.Collections.Generic;
    using System.Dynamic;

    /// <summary>
    /// defult implementation of a Dynamic object, inherit off this for dyanmic properties
    /// </summary>
    public abstract class Dynamic : DynamicObject
    {
        protected readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string key = GetKey(binder.Name);
            return _dictionary.TryGetValue(key, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            var key = GetKey(binder.Name);
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }

            return true;
        }

        private string GetKey(string key)
        {
            return key.ToLower();
        }
    }
}