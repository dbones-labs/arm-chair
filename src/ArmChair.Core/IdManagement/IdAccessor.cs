// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ArmChair.Utils;

namespace ArmChair.IdManagement
{
    public class IdAccessor : IIdAccessor
    {
        private readonly IDictionary<Type, FieldMeta> _typeIdFields = new Dictionary<Type, FieldMeta>();
        private bool _allowAutoScanningForId;
        private Func<Type, string> _idNamePattern;

        public IdAccessor()
        {
            _allowAutoScanningForId = true;
        }

        public void AllowAutoScanningForId()
        {
            _allowAutoScanningForId = true;
        }

        public void SetUpIdPattern(Func<Type, string> pattern)
        {
            _allowAutoScanningForId = true;
            _idNamePattern = pattern;
        }

        public void SetUpId<T>(FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException("field");
            _typeIdFields.Add(typeof(T), new FieldMeta(field));
        }

        public void SetUpId<T>(FieldMeta field)
        {
            if (field == null) throw new ArgumentNullException("field");
            _typeIdFields.Add(typeof(T), field);
        }

        public void SetUpId<T>(string fieldName)
        {
            FieldMeta fieldInfo = typeof(T).GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            SetUpId<T>(fieldInfo);
        }

        public void SetUpId<T>(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            SetUpId<T>(backingFieldName);
        }

        public object GetId(object instance)
        {
            Type type = instance.GetType();
            FieldMeta idField = GetIdField(type);
            return idField.GetFieldValueFor(instance);
        }

        public void SetId(object instance, object id)
        {
            Type type = instance.GetType();
            FieldMeta idField = GetIdField(type);
            idField.SetFieldValueOf(instance, id);
        }

        public FieldMeta GetIdField(Type type)
        {
            lock (_typeIdFields)
            {
                if (!_typeIdFields.ContainsKey(type))
                {
                    if (!_allowAutoScanningForId)
                    {
                        throw new Exception("please setup an Id or allow for auto scanning");
                    }
                    var idField = ScanForId(type);
                    if (idField == null)
                    {
                        return null;
                    }
                    _typeIdFields.Add(type, idField);
                }
            }
            return _typeIdFields[type];
        }

        private string GetPropertyBackingFieldName(string propertyName)
        {
            return string.Format("<{0}>k__BackingField", propertyName);
        }

        private FieldMeta ScanForId(Type type)
        {
            var idPatterns = _idNamePattern == null
                                 ? new[] { GetPropertyBackingFieldName("Id"), GetPropertyBackingFieldName(type.Name+"Id"), "id", "_id" }
                                 : new[] {_idNamePattern(type)};

            return type
                .GetTypeMeta()
                .Fields
                .FirstOrDefault(x => idPatterns.Any(pattern => string.Compare(x.Name, pattern, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

    }
}