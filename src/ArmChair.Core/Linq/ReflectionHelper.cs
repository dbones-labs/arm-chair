namespace ArmChair.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

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
            var methods = _type.GetMethods().Where(x => x.Name == methodName);
            return methods;
        }
    }
}