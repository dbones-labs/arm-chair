namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Linq.Expressions;

    public class LessAndGreaterThanHandler : BinaryHandler
    {
        public override void Handle(BinaryExpression expression, VisitorContext context)
        {
            string @operator = null;

            switch (expression.NodeType)
            {
                case ExpressionType.LessThan:
                    @operator = "$lt"; break;
                case ExpressionType.LessThanOrEqual:
                    @operator = "$lte"; break;
                case ExpressionType.GreaterThan:
                    @operator = "$gt"; break;
                case ExpressionType.GreaterThanOrEqual:
                    @operator = "$gte"; break;
            }

            var nameValue = GetNameValue(expression);
            //var name = GetMemberName(nameValue.Member, context);

            var compareObject = new QueryObject { { @operator, nameValue.Constant.Value } };
            var result = CreateQuery(nameValue.Member, compareObject, context);

            context.SetResult(result);
        }


        public override bool CanHandle(BinaryExpression expression)
        {
            var isLessThan = expression.NodeType == ExpressionType.LessThan;
            var isLessThanOrEqual = expression.NodeType == ExpressionType.LessThanOrEqual;
            var isGreaterThan = expression.NodeType == ExpressionType.GreaterThan;
            var isGreaterThanOrEqual = expression.NodeType == ExpressionType.GreaterThanOrEqual;

            var supported = isLessThan || isLessThanOrEqual || isGreaterThan || isGreaterThanOrEqual;

            return supported;
        }
    }
}