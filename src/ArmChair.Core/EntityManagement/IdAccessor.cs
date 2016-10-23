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
namespace ArmChair.EntityManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    public class IdAccessor : IIdAccessor
    {
        private readonly IDictionary<Type, FieldMeta> _typeIdFields = new Dictionary<Type, FieldMeta>();
        private bool _allowAutoScanning;
        private Func<Type, string> _namePattern;

        public IdAccessor()
        {
            _allowAutoScanning = true;
        }

        public void AllowAutoScanningForId()
        {
            _allowAutoScanning = true;
        }

        public void SetUpIdPattern(Func<Type, string> pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            _allowAutoScanning = true;
            _namePattern = pattern;
        }

        public void SetUpId<T>(FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            _typeIdFields.Add(typeof(T), new FieldMeta(field));
        }

        public void SetUpId(Type type, FieldInfo field)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (field == null) throw new ArgumentNullException(nameof(field));
            _typeIdFields.Add(type, new FieldMeta(field));
        }

        public void SetUpId<T>(FieldMeta field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            _typeIdFields.Add(typeof(T), field);
        }

        public void SetUpId<T>(string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            FieldMeta fieldInfo = typeof(T).GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            SetUpId<T>(fieldInfo);
        }

        public void SetUpId<T>(Expression<Func<T, object>> property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            string name = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            SetUpId<T>(backingFieldName);
        }

        public object GetId(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            Type type = instance.GetType();
            FieldMeta idField = GetIdField(type);
            return idField.GetFieldValueFor(instance);
        }

        public void SetId(object instance, object id)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            Type type = instance.GetType();
            FieldMeta idField = GetIdField(type);
            idField.SetFieldValueOf(instance, id);
        }

        public FieldMeta GetIdField(Type type)
        {
            //try and return asap;
            FieldMeta meta;
            if (_typeIdFields.TryGetValue(type, out meta)) return meta;

            lock (_typeIdFields)
            {
                //check again, if no autoscanning, then return null
                if (_typeIdFields.TryGetValue(type, out meta) || !_allowAutoScanning) return meta;

                //allow the store of null;
                var idField = ScanForId(type);
                _typeIdFields.Add(type, idField);
                return idField;
            }
        }

        private string GetPropertyBackingFieldName(string propertyName)
        {
            return string.Format("<{0}>k__BackingField", propertyName);
        }

        private FieldMeta ScanForId(Type type)
        {
            var idPatterns = _namePattern == null
                                 ? new[] { GetPropertyBackingFieldName("Id"), GetPropertyBackingFieldName(type.Name+"Id"), "id", "_id" }
                                 : new[] {_namePattern(type)};

            return type
                .GetTypeMeta()
                .Fields
                .FirstOrDefault(x => idPatterns.Any(pattern => string.Compare(x.Name, pattern, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

    }
}