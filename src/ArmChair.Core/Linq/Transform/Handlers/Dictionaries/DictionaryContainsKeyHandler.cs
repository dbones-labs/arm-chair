namespace ArmChair.Linq.Transform.Handlers.Dictionaries
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// dictionary.ContainsKey("hello")  => { "hello" : 123 }, version simple impl.
    /// </summary>
    public class DictionaryContainsKeyHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            //var name = GetMemberName((MemberExpression)expression.Object, context);
            
            var exists = new QueryObject { { "$exists", true } };
            //var query = new QueryObject { { $"{name}.{cValue.Value.ToString()}", exists } };
            var query = CreateQuery(expression, exists, context);
            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            var isCollection = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(GetDeclaringType(expression));
            var isMethod = GetMethodName(expression).Equals("ContainsKey");

            return isCollection && isMethod;
        }
    }


    public class IndexHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            
            //var query = new QueryObject { { $"{name}.{cValue.Value.ToString()}", null } };
            var query = CreateQuery(expression, null, context);

            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            var isCollection = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(GetDeclaringType(expression));
            var isMethod = GetMethodName(expression).Equals("get_Item");

            return isCollection && isMethod;
        }


    }
}