namespace ArmChair.Linq.Transform.Handlers.CollectionHandlers
{
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;


    public class AnyHandler : MethodHandler
    {
        public override void Handle(MethodCallExpression expression, VisitorContext context)
        {
            context.Visit(expression.Arguments[1]);
            var matchCriteria = context.GetResult();
            
            var regex = new QueryObject { { "$elemMatch", matchCriteria } };
            var query = CreateQuery(expression.Arguments[0], regex, context);

            context.SetResult(query);
        }

        public override bool CanHandle(MethodCallExpression expression)
        {
            if (expression.Method.Name != "Any")
            {
                return false;
            }

            var val0 = expression.Arguments[0] as MemberExpression;
            var val1 = expression.Arguments[1] as LambdaExpression;

            if (val0 == null || val1 == null)
            {
                return false;
            }

            var prop = val0.Member as PropertyInfo;
            return prop != null && typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(prop.PropertyType.GetTypeInfo());
        }
    }
}