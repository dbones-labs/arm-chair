namespace ArmChair.Linq.Transform.Handlers
{
    using System.Linq.Expressions;

    public class NotHandler : HandlerBase<UnaryExpression>
    {
        public override void Handle(UnaryExpression expression, VisitorContext context)
        {
            context.Visit(expression.Operand);
            var val = context.GetResult();

            var result = new QueryObject();
            result.Add("$not", val);

            context.SetResult(result);
        }

        public override bool CanHandle(UnaryExpression expression)
        {
            return expression.NodeType == ExpressionType.Not;
        }
    }
}