namespace ArmChair.EntityManagement
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Utils;

    /// <summary>
    /// stores some information about the aggregate roots. alllowing us to apply some querying.
    /// </summary>
    public interface ITypeManager
    {
        /// <summary>
        /// register a type we are interested in
        /// </summary>
        /// <param name="type">type that we need to know about</param>
        void Register(Type type);

        /// <summary>
        /// get all the types which inheret off a type
        /// </summary>
        /// <param name="type">the type on which we will look at what inherits off</param>
        /// <returns>a set of inherited types, note this will include the type which is passed in</returns>
        HashSet<Type> Implementation(Type type);
    }

    public class TypeManager : ITypeManager
    {
        private readonly IDictionary<Type, HashSet<Type>> _lookup = new ConcurrentDictionary<Type, HashSet<Type>>();

        public void Register(Type type)
        {
            if (_lookup.ContainsKey(type)) return;

            var directSet = new HashSet<Type>();
            directSet.Add(type);
            _lookup.Add(type, directSet);

            var meta = type.GetTypeMeta();
            foreach (var baseType in meta.AllTypes)
            {
                if (baseType == type) continue;

                HashSet<Type> set;
                if (!_lookup.TryGetValue(baseType, out set))
                {
                    Register(baseType);
                    set = _lookup[baseType];
                }

                set.Add(type);
            }
        }

        public HashSet<Type> Implementation(Type type)
        {
            HashSet<Type> set = null;
            if (!_lookup.TryGetValue(type, out set))
            {
                set = new HashSet<Type> {type};
            }
            return set;

        }
    }
}