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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    public abstract class ClassMap
    {
        protected ClassMap(Type type)
        {
            Type = type;
        }

        public FieldInfo IdField { get; set; }
        public FieldInfo RevisionField { get; set; }
        public Type Type { get; }

        public void Id(string fieldName)
        {
            FieldMeta fieldMeta = Type.GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            IdField = fieldMeta.FieldInfo;
        }

        public void Revision(string fieldName)
        {
            FieldMeta fieldMeta = Type.GetTypeMeta().Fields.FirstOrDefault(x => x.Name == fieldName);
            RevisionField = fieldMeta.FieldInfo;
        }

    }

    public abstract class ClassMap<T> : ClassMap
    {
        
        public ClassMap() : base(typeof(T))
        {
        }

        public void Id(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            Id(backingFieldName);
        }

        public void Revision(Expression<Func<T, object>> property)
        {
            string name = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name;
            string backingFieldName = GetPropertyBackingFieldName(name);
            Revision(backingFieldName);
        }

        private string GetPropertyBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}