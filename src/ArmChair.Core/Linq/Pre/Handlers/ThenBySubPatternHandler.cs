namespace ArmChair.Linq.Pre.Handlers
{
    public class ThenBySubPatternHandler : SubPatternHandlerBase
    {
        public ThenBySubPatternHandler()
            : base(
                "ThenBy",
                "ThenByDescending")
        {

        }

        public override void Update(ProcessingLinqContext ctx)
        {
            var direction = ctx.CurrentMethod.Name.ComparedTo("ThenBy")
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