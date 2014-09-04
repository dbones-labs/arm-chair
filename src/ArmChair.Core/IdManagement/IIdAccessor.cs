using System;
using System.Linq.Expressions;
using System.Reflection;
using ArmChair.Utils;

namespace ArmChair.IdManagement
{
    public interface IIdAccessor
    {
        void AllowAutoScanningForId();
        void SetUpIdPattern(Func<Type, string> pattern);
        void SetUpId<T>(FieldInfo field);
        void SetUpId<T>(string fieldName);
        void SetUpId<T>(Expression<Func<T, object>> property);
        object GetId(object instance);
        void SetId(object instance, object id);
        FieldMeta GetIdField(Type type);
    }
}