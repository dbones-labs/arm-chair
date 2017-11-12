namespace ArmChair.Linq.Transform.Handlers
{
    using System.Linq.Expressions;

    /// <summary>
    /// base class for dealing with member expressions.
    /// </summary>
    /// <remarks>
    /// is there a .NET standard 1.1 version for this? 
    /// </remarks>
    public class BooleanEqualityHandler : HandlerBase<UnaryExpression>
    {
        public override bool CanHandle(UnaryExpression expression)
        {
            //note we are looking to exit from this as quick as possible.
            //must be a lambda which returns true and has a memberExpression for a body.
            var isQuote = expression.NodeType == ExpressionType.Quote;
            if (!isQuote) return false;

            var lambda = expression.Operand as LambdaExpression;
            if (lambda == null) return false;

            var isBoolReturn = lambda.ReturnType == typeof(bool);
            if (!isBoolReturn) return false;

            var memberExpression = lambda.Body as MemberExpression;
            return memberExpression != null;

        }

        public override void Handle(UnaryExpression expression, VisitorContext context)
        {
            var memberExpression = (MemberExpression)((LambdaExpression)expression.Operand).Body;

            var name = memberExpression.Member.Name;

            var equal = new QueryObject { { "$eq", true } };
            var result = new QueryObject { { name, equal } };

            context.SetResult(result);

        }
    }
}