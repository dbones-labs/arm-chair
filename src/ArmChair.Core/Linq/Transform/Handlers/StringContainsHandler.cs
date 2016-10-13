namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;

    public class StringContainsHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            var name = GetMemberName((MemberExpression)expression.Object);

            var regex = new QueryObject { { "$regex", $"/{cValue}/" } };
            var query = new QueryObject { { name, regex } };

            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            var isString = GetDeclaringType(expression) == typeof(string);
            var isMethod = GetMethodName(expression).Equals("Contains");

            return isString && isMethod;
        }
    }

    public class StringStartsWithHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            var name = GetMemberName((MemberExpression)expression.Object);

            var regex = new QueryObject { { "$regex", $"^{cValue}" } };
            var query = new QueryObject { { name, regex } };

            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            var isString = GetDeclaringType(expression) == typeof(string);
            var isMethod = GetMethodName(expression).Equals("StartsWith");

            return isString && isMethod;
        }
    }

    public class StringEndsWithHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var cValue = expression.Arguments[0] as ConstantExpression;
            if (cValue == null)
            {
                throw new NotSupportedException("requires a parameter");
            }

            var name = GetMemberName((MemberExpression)expression.Object);

            var regex = new QueryObject { { "$regex", $"{cValue}$" } };
            var query = new QueryObject { { name, regex } };

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