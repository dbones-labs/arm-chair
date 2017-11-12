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
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// contains some helpful information about a type, should speed up the app
    /// </summary>
    public class TypeMeta
    {
        private readonly Type _type;
        private readonly TypeInfo _typeInfo;
        private readonly IEnumerable<Type> _allTypes;
        private readonly IDictionary<string, FieldMeta> _valueFields = new Dictionary<string, FieldMeta>();
        private readonly IDictionary<string, FieldMeta> _allFields = new Dictionary<string, FieldMeta>();
        private readonly bool _isAbstract;
        private readonly Func<object> _ctor;

        /// <summary>
        /// create a typemeta from the type
        /// </summary>
        /// <param name="type">the type to get some meta info from</param>
        public TypeMeta(Type type)
        {
            //Need to test the inherited field getter/setter
            //try and process all this once
            _type = type;
            _typeInfo = type.GetTypeInfo();


            _allTypes = type.GetAllTypes();
            var fields = _allTypes
                .Where(t => t != typeof(object))
#if NETSTANDARD1_1
                .SelectMany(t => t.GetTypeInfo().DeclaredFields)
                .Where(x => !x.IsStatic);
#endif
#if NETSTANDARD1_6
                .SelectMany(t => t.GetTypeInfo().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
#endif
            _isAbstract = _typeInfo.IsAbstract;
            if (!IsAbstract)
            {
                //look only at the direct type
                var ctor =
#if NETSTANDARD1_1
                    _typeInfo.DeclaredConstructors.Where(x => !x.IsStatic)
#endif
#if NETSTANDARD1_6
                    _typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
#endif
                        .FirstOrDefault(x => !x.GetParameters().Any());

                if (ctor != null)
                {
                    //create the delegate 
                    var lambda = Expression.Lambda<Func<object>>(Expression.New(ctor));
                    _ctor = lambda.Compile();
                }
            }

            foreach (var meta in fields.Select(field => new FieldMeta(field)))
            {
                if (meta.Type.GetTypeInfo().IsValueType || meta.Type == typeof(string))
                {
                    _valueFields.Add(meta.Name, meta);
                }
                _allFields.Add(meta.Name, meta);
            }
        }

        /// <summary>
        /// ctor for the type (note this is the parameterless ctor)
        /// </summary>
        public Func<object> Ctor => _ctor;

        /// <summary>
        /// denotes if the type is abstract
        /// </summary>
        public bool IsAbstract => _isAbstract;

        /// <summary>
        /// all the types in the inheritance hierarchy
        /// </summary>
        public IEnumerable<Type> AllTypes => _allTypes;

        /// <summary>
        /// all the value type fields this type has
        /// </summary>
        public IEnumerable<FieldMeta> ValueFields => _valueFields.Values;

        /// <summary>
        /// all the fields this type has
        /// </summary>
        public IEnumerable<FieldMeta> Fields => _allFields.Values;

        /// <summary>
        /// the actual type
        /// </summary>
        public Type Type => _type;
    }
}