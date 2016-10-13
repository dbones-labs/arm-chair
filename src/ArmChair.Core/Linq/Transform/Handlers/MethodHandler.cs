namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;

    public abstract class MethodHandler : HandlerBase<MethodCallExpression>
    {
        protected Type GetDeclaringType(MethodCallExpression expression)
        {
            var method = expression.Method;
            return method.DeclaringType;
        }

        protected string GetMethodName(MethodCallExpression expression)
        {
            var method = expression.Method;
            return method.Name;
        }
    }
}