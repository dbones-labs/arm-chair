namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Linq.Expressions;

    public class EqualityHandler : BinaryHandler
    {
        public override void Handle(BinaryExpression expression, VisitorContext context)
        {
            var nameValue = GetNameValue(expression);
            var @operator = expression.NodeType == ExpressionType.Equal
                ? "$eq"
                : "$ne";

            var equal = new QueryObject { { @operator, nameValue.Constant.Value } };
            var result = CreateQuery(nameValue.Member, equal, context);

            context.SetResult(result);
        }

        public override bool CanHandle(BinaryExpression expression)
        {
            var isBoolean = expression.NodeType == ExpressionType.Equal
                || expression.NodeType == ExpressionType.NotEqual;

            return isBoolean;
            //return isBoolean && expression.Left is MemberExpression;
        }
    }


}