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
namespace ArmChair.EntityManagement.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    /// <summary>
    /// register information about a class.
    /// </summary>
    public abstract class ClassMap
    {
        protected IList<IndexEntry> _indexes = new List<IndexEntry>();

        protected ClassMap(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// the field which is the ID of this class
        /// </summary>
        public FieldInfo IdField { get; set; }

        /// <summary>
        /// the field which is the revision for this class
        /// </summary>
        public FieldInfo RevisionField { get; set; }

        /// <summary>
        /// the class which this map is for
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// all the indexes to be registered
        /// </summary>
        public IEnumerable<IndexEntry> Indexes => _indexes;

        /// <summary>
        /// set the Id
        /// </summary>
        /// <param name="fieldName">the name of the field</param>
        public void Id(string fieldName)
        {
            FieldMeta fieldMeta = Type.GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            IdField = fieldMeta.FieldInfo;
        }

        /// <summary>
        /// set the revision
        /// </summary>
        /// <param name="fieldName">the name of the field which will be used for storing the revision</param>
        public void Revision(string fieldName)
        {
            FieldMeta fieldMeta = Type.GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            RevisionField = fieldMeta.FieldInfo;
        }

    }

    /// <summary>
    /// register information about a class
    /// </summary>
    /// <typeparam name="T">the class type</typeparam>
    public abstract class ClassMap<T> : ClassMap
    {

        public ClassMap() : base(typeof(T))
        {
        }

        /// <summary>
        /// set the ID
        /// </summary>
        /// <param name="property">the property which will store the Id</param>
        public void Id(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)property.Body).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            Id(backingFieldName);
        }

        /// <summary>
        /// set the revision
        /// </summary>
        /// <param name="property">the property which will store the revision</param>
        public void Revision(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)property.Body).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            Revision(backingFieldName);
        }

        private string GetPropertyBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }

        /// <summary>
        /// Add an index for this class
        /// </summary>
        /// <param name="addIndex">a func which will allow you to setup the index</param>
        public void Index(Action<IndexEntry<T>> addIndex)
        {
            var index = new IndexEntry<T>();
            addIndex(index);
            _indexes.Add(index);
        }
    }
}