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

            if (!fieldInfo.IsInitOnly)
            {
                _setter = fieldInfo.CreateFieldSet();
            }
        }

        public Type Type { get; private set; }

        public bool IsReadOnly { get; private set; }

        public string Name { get; private set; }

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