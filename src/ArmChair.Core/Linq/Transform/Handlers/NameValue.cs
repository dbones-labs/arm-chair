namespace ArmChair.Linq.Transform.Handlers
{
    using System.Linq.Expressions;

    public class NameValue
    {
        public MemberExpression Member { get; set; }
        public ConstantExpression Constant { get; set; }
    }
}