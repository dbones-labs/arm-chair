namespace ArmChair.Linq.Transform.Handlers
{
    using System.Linq.Expressions;

    public class NameValue
    {
        public Expression Member { get; set; }
        public ConstantExpression Constant { get; set; }
    }
}