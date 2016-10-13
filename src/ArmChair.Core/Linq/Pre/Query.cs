namespace ArmChair.Linq.Pre
{
    using System.Collections;
    using System.Linq.Expressions;

    public class Query
    {
        private readonly MethodCallExpression _subQueryExpression;
        private readonly Expression _fullExpression;

        public Query(MethodCallExpression subQueryExpression, Expression fullExpression)
        {
            _subQueryExpression = subQueryExpression;
            _fullExpression = fullExpression;
            Expression = null;
        }

        public Expression Expression { get; private set; }

        public void RewriteSource(IEnumerable newSource)
        {
            var temp = ReplaceSourceVisitor.Eval(_fullExpression, _subQueryExpression, newSource);
            Expression = temp;
        }
    }
}