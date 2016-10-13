namespace ArmChair.Linq.Pre.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class TakeSubPatternHandler : SubPatternHandlerBase
    {
        //skip, take, distinct, single, count, any
        private static readonly HashSet<string> AllowedNextStep = new HashSet<string>(new[] { "take", "distinct", "single", "count", "any" });

        public TakeSubPatternHandler()
            : base(objects => objects.Take(1))
        {
            //objects => objects.TakeWhile(p => true)
        }

        public override void Update(ProcessingLinqContext ctx)
        {
            var exp = (ConstantExpression)ctx.CurrentMethod.Expression.Arguments[1];
            ctx.LinqQuery.Paging.Take = Convert.ToInt64(exp.Value);
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return !AllowedNextStep.Contains(ctx.CurrentMethod.Name.ToLower());
        }
    }
}