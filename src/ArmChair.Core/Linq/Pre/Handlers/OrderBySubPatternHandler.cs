namespace ArmChair.Linq.Pre.Handlers
{
    using System.Linq;

    public class OrderBySubPatternHandler : SubPatternHandlerBase
    {
        public OrderBySubPatternHandler()
            : base(
                objects => objects.OrderBy(p => p),
                objects => objects.OrderByDescending(p => p))
        {

        }

        public override void Update(ProcessingLinqContext ctx)
        {
            var direction = ctx.CurrentMethod.Name.ComparedTo("OrderBy")
                ? OrderByDirection.Asc
                : OrderByDirection.Desc;

            var order = new OrderBy()
            {
                Direction = direction,
                Expression = ctx.CurrentMethod.Expression.Arguments[1]
            };

            ctx.LinqQuery.AddOrderBy(order);
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return false;
        }
    }
}