namespace ArmChair.Linq.Pre.Handlers
{
    using System.Linq;

    public class WhereSubPatternHandler : SubPatternHandlerBase
    {
        public WhereSubPatternHandler()
            : base(objects => objects.Where(x => true))
        {
        }

        public override void Update(ProcessingLinqContext ctx)
        {
            ctx.LinqQuery.AddWhereClause(ctx.CurrentMethod.Expression.Arguments[1]);
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return false;
        }
    }
}