// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
using System;
using System.Configuration;
using System.Reflection;

namespace ArmChair.Utils
{
    public class FieldMeta
    {
        private readonly CallGet _getter;
        private readonly CallSet _setter;
       

        public FieldMeta(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            _getter = fieldInfo.CreateFieldGet();

            Name = fieldInfo.Name;
            Type = fieldInfo.FieldType;
            IsReadOnly = fieldInfo.IsInitOnly;

            if (Name.StartsWith("<", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = Name.IndexOf(">", StringComparison.CurrentCultureIgnoreCase);
                FriendlyName = Name.Substring(1, index - 1);
            }

            if (Name.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
            {
                FriendlyName = Name.Substring(1, Name.Length - 1);
            }

            if (!fieldInfo.IsInitOnly)
            {
                _setter = fieldInfo.CreateFieldSet();
            }
        }

        public Type Type { get; private set; }

        public bool IsReadOnly { get; private set; }

        public string Name { get; private set; }
        
        public string FriendlyName { get; private set; }

        public FieldInfo FieldInfo { get; private set; }

        public object GetFieldValueFor(object instance)
        {
            return _getter(instance);
        }

        public void SetFieldValueOf(object instance, object value)
        {
            if (IsReadOnly)
            {
                throw new SettingsPropertyIsReadOnlyException(Name);
            }
            _setter(instance, value);
        }
    }
}