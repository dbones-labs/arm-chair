namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Linq.Expressions;

    public class EqualityHandler : BinaryHandler
    {
        public override void Handle(BinaryExpression expression, VisitorContext context)
        {
            var nameValue = GetNameValue(expression);
            var name = GetMemberName(nameValue.Member, context);

            var @operator = expression.NodeType == ExpressionType.Equal
                ? "$eq"
                : "$ne";

            var equal = new QueryObject { { @operator, nameValue.Constant.Value } };
            var result = new QueryObject { { name, equal } };

            context.SetResult(result);
        }

        public override bool CanHandle(BinaryExpression expression)
        {
            return expression.NodeType == ExpressionType.Equal
                || expression.NodeType == ExpressionType.NotEqual;
        }
    }


}