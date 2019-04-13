// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

namespace IQToolkit
{
    using System;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo member, object instance)
        {

#if NETSTANDARD1_1
            {
                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(instance);
                }
            }
            
            {
                var propertyInfo = member as PropertyInfo;
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(instance);
                }
            }
            throw new Exception($"sorry this member is not supported: {member.Name}");
#endif
            
#if NETSTANDARD1_6 || NET452
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetValue(instance, null);
                case MemberTypes.Field:
                    return ((FieldInfo)member).GetValue(instance);
                default:
                    throw new InvalidOperationException();
            }
#endif
        }

        public static void SetValue(this MemberInfo member, object instance, object value)
        {

#if NETSTANDARD1_1
            {
                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(instance, value);
                    return;
                }
            }
            
            {
                var propertyInfo = member as PropertyInfo;
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(instance, value);
                    return;
                }
            }
            throw new Exception($"sorry this member is not supported: {member.Name}");
#endif
            
#if NETSTANDARD1_6           
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    var pi = (PropertyInfo)member;
                    pi.SetValue(instance, value, null);
                    break;
                case MemberTypes.Field:
                    var fi = (FieldInfo)member;
                    fi.SetValue(instance, value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
#endif
        }
    }
}