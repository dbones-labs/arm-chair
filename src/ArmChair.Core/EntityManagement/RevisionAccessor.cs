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

    /// <summary>
    /// default impl for <see cref="IRevisionAccessor"/>
    /// </summary>
    public class RevisionAccessor : IRevisionAccessor
    {
        private readonly IDictionary<Type, FieldMeta> _typeRevisionFields = new Dictionary<Type, FieldMeta>();
        private bool _allowAutoScanning;
        private Func<Type, string> _namePattern;

        public RevisionAccessor()
        {
            _allowAutoScanning = true;
        }

        //<inheritdoc />
        public void AllowAutoScanningForRevision()
        {
            _allowAutoScanning = true;
        }

        //<inheritdoc />
        public void DisableAutoScanningForRevision()
        {
            _allowAutoScanning = false;
        }

        //<inheritdoc />
        public void SetUpRevisionPattern(Func<Type, string> pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            _allowAutoScanning = true;
            _namePattern = pattern;
        }

        //<inheritdoc />
        public void SetUpRevision(Type type, FieldInfo field)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (field == null) throw new ArgumentNullException(nameof(field));

            var meta = new FieldMeta(field);
            if (_typeRevisionFields.ContainsKey(type))
            {
                _typeRevisionFields[type] = meta;
            }
            else
            {
                _typeRevisionFields.Add(type, meta);
            }
            
        }

        //<inheritdoc />
        public object GetRevision(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            Type type = instance.GetType();
            FieldMeta idField = GetRevisionField(type);
            return idField.GetFieldValueFor(instance);
        }

        //<inheritdoc />
        public void SetRevision(object instance, object id)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            Type type = instance.GetType();
            FieldMeta idField = GetRevisionField(type);
            idField.SetFieldValueOf(instance, id);
        }

        //<inheritdoc />
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
            return $"<{propertyName}>k__BackingField";
        }

        private FieldMeta ScanForRevision(Type type)
        {
            var idPatterns = _namePattern == null
                ? new[] { GetPropertyBackingFieldName("Revision"), GetPropertyBackingFieldName("Rev"), "rev", "_rev", "revision", "_revision", GetPropertyBackingFieldName(type.Name + "Rev"), GetPropertyBackingFieldName(type.Name + "Revision") }
                : new[] { _namePattern(type) };

            return type
                .GetTypeMeta()
                .Fields
                .FirstOrDefault(x => idPatterns.Any(pattern => string.Compare(x.Name, pattern, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

    }


    /// <summary>
    /// extension methods for <see cref="IRevisionAccessor"/>
    /// </summary>
    public static class RevisionAccessorExtensions
    {
        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="fieldName">name of the Revision field</param>
        public static void SetUpRevision<T>(this IRevisionAccessor revisionAccessor, string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            FieldMeta fieldInfo = typeof(T).GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            revisionAccessor.SetUpRevision(typeof(T), fieldInfo?.FieldInfo);
        }

        /*
        public static void SetUpRevision(this IRevisionAccessor revisionAccessor, Type type, string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            FieldMeta fieldInfo = type.GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            revisionAccessor.SetRevision(type, fieldInfo);
        }
        */

        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="field">Revision Field</param>
        public static void SetUpRevision<T>(this IRevisionAccessor revisionAccessor, FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            revisionAccessor.SetUpRevision(typeof(T), field);
        }

        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="property">The Revision property, this will then use the Auto back field</param>
        public static void SetUpRevision<T>(this IRevisionAccessor revisionAccessor, Expression<Func<T, object>> property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            string name = ((MemberExpression)property.Body).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            SetUpRevision<T>(revisionAccessor, backingFieldName);
        }

        private static string GetPropertyBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}