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
    using System.Reflection;

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
                if (type.GetTypeInfo().BaseType != null)
                {
                    types.AddRange(type.GetTypeInfo().BaseType.GetAllTypes());
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

        public static Type GetUnderlyingType(this MemberInfo member)
        {
 #if NETSTANDARD1_1
            var fieldInfo = member as FieldInfo;
            if (fieldInfo != null) return fieldInfo.FieldType;
            
            var propertyInfo = member as PropertyInfo;
            if (propertyInfo != null) return propertyInfo.PropertyType;

            var methodInfo = member as MethodInfo;
            if (methodInfo != null) return methodInfo.ReturnType;

            var eventInfo = member as EventInfo;
            if (eventInfo != null) return eventInfo.EventHandlerType;
            
            throw new ArgumentException
            (
                "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
            );
#endif

#if NETSTANDARD1_6
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
#endif
        }

#if NETSTANDARD1_1
        public static Type[] GetGenericArguments(this Type type)
        {
            var info = type.GetTypeInfo();
            return info.GetGenericArguments();
        }
        
        public static Type[] GetGenericArguments(this TypeInfo info)
        {
            return info.IsGenericTypeDefinition 
                ? info.GenericTypeParameters 
                : info.GenericTypeArguments;
        }

        public static IEnumerable<MethodInfo> GetMethods(this TypeInfo info)
        {
            return info.DeclaredMethods;
        }
 #endif

        public static string ToCamelCase(this string s)
        {
            //Build the titlecase string
            
            //var titlecase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
            //Ensures that there is at-least two characters (so that the Substring method doesn't freak out)
            return (s.Length > 1) ? Char.ToLowerInvariant(s[0]) + s.Substring(1) : s;
        }
    }
}