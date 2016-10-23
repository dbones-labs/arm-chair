namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class AndOrVisitorHandler : HandlerBase<BinaryExpression>
    {
        public override void Handle(BinaryExpression expression, VisitorContext context)
        {
            var isAnd = expression.NodeType == ExpressionType.AndAlso ||
                        expression.NodeType == ExpressionType.And;

            context.Visit(expression.Left);
            context.Visit(expression.Right);

            var right = context.GetResult();
            var left = context.GetResult();

            var @operator = isAnd
                ? "$and"
                : "$or";

            var query = new QueryObject { { @operator, new List<IDictionary<string, object>> { left, right } } };

            context.SetResult(query);
        }

        public override bool CanHandle(BinaryExpression expression)
        {
            var isAnd = expression.NodeType == ExpressionType.AndAlso ||
                        expression.NodeType == ExpressionType.And;
            var isOr = expression.NodeType == ExpressionType.OrElse ||
                       expression.NodeType == ExpressionType.Or;

            var supported = isAnd || isOr;

            return supported;
        }
    }
}