namespace ArmChair.Linq.Pre.Handlers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class SkipSubPatternHandler : SubPatternHandlerBase
    {
        //skip, take, distinct, single, count, any
        private static readonly string[] AllowedNextStep = { "skip", "take", "distinct", "single", "count", "any" };

        public SkipSubPatternHandler(): base(objects => objects.Skip(1))
        {
            //cannot support this one
            //objects => objects.SkipWhile(p => true)
        }

        
        public override void Update(ProcessingLinqContext ctx)
        {
            var exp = (ConstantExpression)ctx.CurrentMethod.Expression.Arguments[1];
            ctx.LinqQuery.Paging.Skip += Convert.ToInt64(exp.Value);
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return !AllowedNextStep.Any(x => x.ComparedTo(ctx.CurrentMethod.Name));
        }
    }
}