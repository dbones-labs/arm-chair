namespace ArmChair.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ReflectionHelper
    {
        public virtual string GetMemberName(
            MemberExpression memberExpression,
            Func<Type, string, string> getActualName = null)
        {
            var prefixExpression = memberExpression.Expression as MemberExpression;
            var name = memberExpression.Member.Name;

            //note this should be at the root level
            if (prefixExpression == null)
            {
                return getActualName == null ? name : getActualName(memberExpression.Member.DeclaringType, name);
            }
            //as we are not at the root level, we should not need to see if there is an id field,
            var prefix = GetMemberName(prefixExpression);
            return string.Join(".", prefix, name);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReflectionHelper<T>
    {
        private Type _type = typeof(T);

        public MethodInfo GetSupportedMethod(Expression<Func<T, object>> expression)
        {
            UnaryExpression temp = expression.Body as UnaryExpression;

            return temp != null
                ? ((MethodCallExpression)temp.Operand).Method
                : ((MethodCallExpression)expression.Body).Method;
        }

        public IEnumerable<MethodInfo> GetSupportedMethods(Expression<Func<T, object>> expression, bool includeAllOverrides = true)
        {
            var methodName = ((MethodCallExpression)((UnaryExpression)expression.Body).Operand).Method.Name;
            var methods = _type.GetTypeInfo().GetMethods().Where(x => x.Name == methodName);
            return methods;
        }
    }
}