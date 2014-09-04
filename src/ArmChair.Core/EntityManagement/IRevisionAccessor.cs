namespace ArmChair.EntityManagement
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    public interface IRevisionAccessor
    {
        void AllowAutoScanningForRevision();
        void DisableAutoScanningForRevision();
        void SetUpRevisionPattern(Func<Type, string> pattern);
        void SetUpRevision<T>(FieldInfo field);
        void SetUpRevision<T>(string fieldName);
        void SetUpRevision<T>(Expression<Func<T, object>> property);
        object GetRevision(object instance);
        void SetRevision(object instance, object id);
        FieldMeta GetRevisionField(Type type);
    }
}