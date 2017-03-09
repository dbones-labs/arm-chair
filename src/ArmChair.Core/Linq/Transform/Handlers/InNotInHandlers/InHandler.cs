namespace ArmChair.Linq.Transform.Handlers.InNotInHandlers
{
    using System.Collections;
    using System.Linq.Expressions;


    /// <summary>
    /// support the $in syntax IE the name is in ["dave", "chan"]
    /// </summary>
    /// <example>
    /// <code>
    /// session.Query<Person>(person => new []{"dave","chan"}.Any(name => name == person.Name));
    /// </code>
    /// </example>
    public class InHandler: HandlerBase<MethodCallExpression>
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            var list = (IEnumerable)((ConstantExpression)(expression.Arguments[0])).Value;
            var binaryExp = (BinaryExpression) ((LambdaExpression) expression.Arguments[1]).Body;

            var property = binaryExp.Right as MemberExpression ?? (MemberExpression)binaryExp.Left;
            var propertyName = GetMemberName(property, context);

            var query = new QueryObject { { propertyName, new QueryObject { { "$in", list } } } };
            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            if (expression.Method.Name != "Any")
            {
                return false;
            }
            
            var val0 = expression.Arguments[0] as ConstantExpression;
            var val1 = expression.Arguments[1] as LambdaExpression;

            if (val0 == null || val1 == null)
            {
                return false;
            }

            var binaryExp = val1.Body as BinaryExpression;
            if (binaryExp == null || binaryExp.NodeType != ExpressionType.Equal)
            {
                return false;
            }

            var list = val0.Value as IEnumerable;

            return list != null;
        }
    }
}