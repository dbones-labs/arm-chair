// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
using System;
using System.Collections.Generic;

namespace ArmChair.Utils
{
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class TypeExtensions
    {
        public static IDictionary<Type, IEnumerable<Type>> TypeHierarchies = new Dictionary<Type, IEnumerable<Type>>();

        public static IEnumerable<Type> GetAllTypes(this Type type)
        {
            //should happen most of the time
            if (TypeHierarchies.ContainsKey(type))
                return TypeHierarchies[type];

            lock (TypeHierarchies)
            {
                //could happen at startup.
                if (TypeHierarchies.ContainsKey(type))
                    return TypeHierarchies[type];

                var types = new List<Type>();
                if (type.BaseType != null)
                {
                    types.AddRange(type.BaseType.GetAllTypes());
                }

                types.Add(type);


                TypeHierarchies.Add(type, types);
                return types;
            }
        }

        public static IDictionary<Type, TypeMeta> TypeMetas = new Dictionary<Type, TypeMeta>();

        public static TypeMeta GetTypeMeta(this Type type)
        {
            TypeMeta meta;
            //should happen most of the time
            if (TypeMetas.TryGetValue(type, out meta)) return meta;

            lock (TypeMetas)
            {
                //could happen at start up
                if (TypeMetas.TryGetValue(type, out meta)) return meta;

                meta = new TypeMeta(type);
                TypeMetas.Add(type, meta);
                return meta;
            }
        }


        public static string ToCamelCase(this string s)
        {
            //Build the titlecase string
            var titlecase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
            //Ensures that there is at-least two characters (so that the Substring method doesn't freak out)
            return (titlecase.Length > 1) ? Char.ToLowerInvariant(titlecase[0]) + titlecase.Substring(1) : titlecase;
        }
    }
}