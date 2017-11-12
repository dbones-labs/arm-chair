// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

namespace IQToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;

    /// <summary>
    /// Type related helper methods
    /// </summary>
    public static class TypeHelper
    {        
        public static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            var info = seqType.GetTypeInfo();
            if (info.IsGenericType)
            {
                foreach (Type arg in info.GenericTypeParameters)
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.GetTypeInfo().IsAssignableFrom(info))
                    {
                        return ienum;
                    }
                }
            }
            IEnumerable<Type> ifaces = info.ImplementedInterfaces;
            if (ifaces != null && ifaces.Any())
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }
            if (info.BaseType != null && info.BaseType != typeof(object))
            {
                return FindIEnumerable(info.BaseType);
            }
            return null;
        }

        public static Type GetSequenceType(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        public static Type GetElementType(Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            if (ienum == null) return seqType;
            return ienum.GetTypeInfo().GenericTypeParameters[0];
        }

        public static bool IsNullableType(Type type)
        {
            return type != null && type.GetTypeInfo().IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullAssignable(Type type)
        {
            return !type.GetTypeInfo().IsValueType || IsNullableType(type);
        }

        public static Type GetNonNullableType(Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetTypeInfo().GenericTypeParameters[0];
            }
            return type;
        }

        public static Type GetNullAssignableType(Type type)
        {
            if (!IsNullAssignable(type))
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }

        public static ConstantExpression GetNullConstant(Type type)
        {
            return Expression.Constant(null, GetNullAssignableType(type));
        }

        public static Type GetMemberType(MemberInfo mi)
        {
            var fi = mi as FieldInfo;
            if (fi != null) return fi.FieldType;
            var pi = mi as PropertyInfo;
            if (pi != null) return pi.PropertyType;
            var ei = mi as EventInfo;
            if (ei != null) return ei.EventHandlerType;
            var meth = mi as MethodInfo; // property getters really
            if (meth != null) return meth.ReturnType;
            return null;
        }

        public static object GetDefault(Type type)
        {
            bool isNullable = !type.GetTypeInfo().IsValueType || IsNullableType(type);
            if (!isNullable)
                return Activator.CreateInstance(type);
            return null;
        }

        public static bool IsReadOnly(MemberInfo member)
        {
            
#if NETSTANDARD1_1
            var fieldInfo = member as FieldInfo;
            if (fieldInfo != null)
            {
                return (fieldInfo.Attributes & FieldAttributes.InitOnly) != 0;
            }
            
            var propertyInfo = member as PropertyInfo;
            if (propertyInfo != null)
            {
                return !propertyInfo.CanWrite || propertyInfo.SetMethod == null;
            }
            return true;
#endif

#if NETSTANDARD1_6   
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return (((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != 0;
                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)member;
                    return !propertyInfo.CanWrite || propertyInfo.GetSetMethod() == null;
                default:
                    return true;
            }
#endif
        }
        
#if NETSTANDARD1_1
        static readonly Type _sbyte = typeof(sbyte);
        static readonly Type _int16 = typeof(Int16);
        static readonly Type _int32 = typeof(Int32);
        static readonly Type _int64 = typeof(Int64);
        static readonly Type _byte = typeof(byte);
        static readonly Type _uInt16 = typeof(UInt16);
        static readonly Type _uInt32 = typeof(UInt32);
        static readonly Type _uInt64 = typeof(UInt64);
        static readonly HashSet<Type> ints = new HashSet<Type>()
        {
            _sbyte, _int16, _int32, _int64, _byte, _uInt16, _uInt32, _uInt64
        };
#endif
        
        public static bool IsInteger(Type type)
        {
 #if NETSTANDARD1_1
            var nnType = GetNonNullableType(type);
            return ints.Contains(nnType);            
 #endif
            
 #if NETSTANDARD1_6
            var nnType = GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
#endif
        }
    }
}