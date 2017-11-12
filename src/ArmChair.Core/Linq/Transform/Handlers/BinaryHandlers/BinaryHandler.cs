namespace ArmChair.Linq.Transform.Handlers.BinaryHandlers
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    public abstract class BinaryHandler : HandlerBase<BinaryExpression>
    {
        public NameValue GetNameValue(BinaryExpression expression)
        {
            var nameValueExp = new MemberNameEvaluator();
            nameValueExp.Visit(expression);

            return new NameValue
            {
                Member = nameValueExp.Member,
                Constant = nameValueExp.Value
            };
        }
    }

    /// <summary>
    /// this will extract the member and value form a Binary expression.
    /// </summary>
    class MemberNameEvaluator : ExpressionVisitor
    {
        private bool _isParam = false;
        private int _dept = 0;

        public ConstantExpression Value { get; private set; }
        public MemberExpression Member { get; private set; }

        public override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }
            return base.Visit(exp);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var result = base.VisitConstant(node);
            if (!_isParam)
            {
                Value = node;
            }
            return result;

        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _dept++;
            var result = base.VisitMember(node);
            _dept--;
            if (_isParam && _dept == 0)
            {
                Member = node;
                _isParam = false;
            }
            return result;
        }


        protected override Expression VisitParameter(ParameterExpression node)
        {
            _isParam = true;
            return base.VisitParameter(node);
        }
    }
}