namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Linq.Expressions;

    /// <summary>
    /// this will extract the member and value form a Binary expression.
    /// </summary>
    internal class MemberNameEvaluator : ExpressionVisitor
    {
        public ConstantExpression Value { get; private set; }
        public Expression Property { get; private set; }

        public override Expression Visit(Expression exp)
        {
            return exp == null ? null : base.Visit(exp);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left is ConstantExpression)
            {
                Value = (ConstantExpression)node.Left;
                Property = node.Right;
            }
            else
            {
                Value = (ConstantExpression)node.Right;
                Property = node.Left;
            }
    
            return node;
        }
    }
}