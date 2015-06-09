// Copyright 2013 - 2015 dbones.co.uk (David Rundle)
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

    public class RevisionAccessor : IRevisionAccessor
    {
        private readonly IDictionary<Type, FieldMeta> _typeRevisionFields = new Dictionary<Type, FieldMeta>();
        private bool _allowAutoScanning;
        private Func<Type, string> _namePattern;

        public RevisionAccessor()
        {
            _allowAutoScanning = true;
        }

        public void AllowAutoScanningForRevision()
        {
            _allowAutoScanning = true;
        }

        public void DisableAutoScanningForRevision()
        {
            _allowAutoScanning = false;
        }

        public void SetUpRevisionPattern(Func<Type, string> pattern)
        {
            _allowAutoScanning = true;
            _namePattern = pattern;
        }

        public void SetUpRevision<T>(FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException("field");
            _typeRevisionFields.Add(typeof(T), new FieldMeta(field));
        }

        public void SetUpId<T>(FieldMeta field)
        {
            if (field == null) throw new ArgumentNullException("field");
            _typeRevisionFields.Add(typeof(T), field);
        }

        public void SetUpRevision<T>(string fieldName)
        {
            FieldMeta fieldInfo = typeof(T).GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            SetUpId<T>(fieldInfo);
        }

        public void SetUpRevision<T>(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            SetUpRevision<T>(backingFieldName);
        }



        public object GetRevision(object instance)
        {
            Type type = instance.GetType();
            FieldMeta idField = GetRevisionField(type);
            return idField.GetFieldValueFor(instance);
        }

        public void SetRevision(object instance, object id)
        {
            Type type = instance.GetType();
            FieldMeta idField = GetRevisionField(type);
            idField.SetFieldValueOf(instance, id);
        }

        public FieldMeta GetRevisionField(Type type)
        {
            lock (_typeRevisionFields)
            {
                if (!_typeRevisionFields.ContainsKey(type))
                {
                    if (!_allowAutoScanning)
                    {
                        throw new Exception("please setup revision field or allow for auto scanning");
                    }
                    var revision = ScanForRevision(type);
                    //if (revision == null)
                    //{
                    //    return null;
                    //}
                    _typeRevisionFields.Add(type, revision);
                }
            }
            return _typeRevisionFields[type];
        }

        private string GetPropertyBackingFieldName(string propertyName)
        {
            return string.Format("<{0}>k__BackingField", propertyName);
        }

        private FieldMeta ScanForRevision(Type type)
        {
            var idPatterns = _namePattern == null
                ? new[] { GetPropertyBackingFieldName("Revision"), GetPropertyBackingFieldName(type.Name + "Revision"), GetPropertyBackingFieldName("Rev"), "rev", "_rev", "revision", "_revision", GetPropertyBackingFieldName(type.Name + "Rev"), GetPropertyBackingFieldName(type.Name + "Revision") }
                : new[] { _namePattern(type) };

            return type
                .GetTypeMeta()
                .Fields
                .FirstOrDefault(x => idPatterns.Any(pattern => string.Compare(x.Name, pattern, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

    }
}