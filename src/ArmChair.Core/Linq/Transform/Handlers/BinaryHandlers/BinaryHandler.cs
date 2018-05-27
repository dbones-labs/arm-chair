namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Linq.Expressions;

    public abstract class BinaryHandler : HandlerBase<BinaryExpression>
    {
        public NameValue GetNameValue(BinaryExpression expression)
        {
            var nameValueExp = new MemberNameEvaluator();
            nameValueExp.Visit(expression);

            return new NameValue
            {
                Member = nameValueExp.Property,
                Constant = nameValueExp.Value,
            };
        }
    }
}