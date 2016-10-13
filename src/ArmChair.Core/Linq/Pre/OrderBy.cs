namespace ArmChair.Linq.Pre
{
    using System.Linq.Expressions;

    public class OrderBy
    {
        public Expression Expression { get; set; }
        public OrderByDirection Direction { get; set; }
    }
}