namespace ArmChair.Linq.Transform.Handlers
{
    using System.Linq.Expressions;

    public abstract class BinaryHandler : HandlerBase<BinaryExpression>
    {
        public NameValue GetNameValue(BinaryExpression expression)
        {
            var member = expression.Left as MemberExpression;
            var value = expression.Right as ConstantExpression;
            if (member == null)
            {
                member = (MemberExpression)expression.Right;
                value = (ConstantExpression)expression.Left;
            }
            return new NameValue() { Member = member, Constant = value };
        }
    }
}