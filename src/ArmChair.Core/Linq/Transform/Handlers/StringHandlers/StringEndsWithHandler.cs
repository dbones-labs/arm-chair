namespace ArmChair.Linq.Transform.Handlers.StringHandlers
{
    using System;
    using System.Linq.Expressions;

    public class StringEndsWithHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            //var name = GetMemberName((MemberExpression)expression.Object, context);

            var regex = new QueryObject { { "$regex", $"{cValue.Value}$" } };
            var query = CreateQuery(expression.Object, regex, context);

            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            var isString = GetDeclaringType(expression) == typeof(string);
            var isMethod = GetMethodName(expression).Equals("EndsWith");

            return isString && isMethod;
        }
    }
}