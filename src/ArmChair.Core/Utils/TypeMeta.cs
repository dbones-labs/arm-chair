using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ArmChair.Utils
{
    public class TypeMeta
    {
        private readonly Type _type;
        private readonly IEnumerable<Type> _allTypes = new List<Type>();
        private readonly IDictionary<string, FieldMeta> _valueFields = new Dictionary<string, FieldMeta>();
        private readonly IDictionary<string, FieldMeta> _allFields = new Dictionary<string, FieldMeta>();
        private readonly bool _isAbstract;
        private readonly Func<object> _ctor;


        public TypeMeta(Type type)
        {
            //Need to test the inherited field getter/setter
            //try and process all this once
            _type = type;
            _allTypes = type.GetAllTypes();
            var fields = _allTypes
                .Where(t => t != typeof(object))
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));

            _isAbstract = _type.IsAbstract;
            if (!IsAbstract)
            {
                //look only at the direct type
                var ctor = _type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
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
                if (meta.Type.IsValueType || meta.Type == typeof(string))
                {
                    _valueFields.Add(meta.Name, meta);
                }
                _allFields.Add(meta.Name, meta);
            }

        }

        public Func<object> Ctor { get { return _ctor; } }
        public bool IsAbstract { get { return _isAbstract; } }
        public IEnumerable<Type> AllTypes { get { return _allTypes; } }
        public IEnumerable<FieldMeta> ValueFields { get { return _valueFields.Values; } }
        public IEnumerable<FieldMeta> Fields { get { return _allFields.Values; } }
        public Type Type { get { return _type; } }
    }
}