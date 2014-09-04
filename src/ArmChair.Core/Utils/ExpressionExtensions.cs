using System.Linq.Expressions;
using System.Reflection;

namespace ArmChair.Utils
{
    public static class ExpressionExtensions
    {
        public static CallGet CreateFieldGet(this FieldInfo field)
        {
            ParameterExpression target = Expression.Parameter(typeof(object), "target");

            MemberExpression member = Expression.Field(Expression.Convert(target, field.DeclaringType), field);

            Expression<CallGet> lambda = Expression.Lambda<CallGet>(
                Expression.Convert(member, typeof(object)), target);

            return lambda.Compile();
        }


        public static CallSet CreateFieldSet(this FieldInfo field)
        {
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            MemberExpression member = Expression.Field(Expression.Convert(target, field.DeclaringType), field);
            BinaryExpression assign = Expression.Assign(member, Expression.Convert(value, field.FieldType));

            Expression<CallSet> lambda = Expression.Lambda<CallSet>(
                Expression.Convert(assign, typeof(object)), target, value);

            return lambda.Compile();
        }
    }
}
