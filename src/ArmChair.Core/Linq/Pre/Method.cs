namespace ArmChair.Linq.Pre
{
    using System.Linq.Expressions;

    public class Method
    {
        public Method(MethodCallExpression expression)
        {
            Name = expression.Method.Name;
            Expression = expression;
        }

        public string Name { get; set; }
        public MethodCallExpression Expression { get; set; }
    }
}