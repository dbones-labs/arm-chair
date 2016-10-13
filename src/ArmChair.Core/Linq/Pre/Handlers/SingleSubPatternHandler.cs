namespace ArmChair.Linq.Pre.Handlers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class SingleSubPatternHandler : SubPatternHandlerBase
    {
        public SingleSubPatternHandler()
            : base(objects => objects.Single(), objects => objects.SingleOrDefault())
        {
        }

        public override void Update(ProcessingLinqContext ctx)
        {
            var args = ctx.CurrentMethod.Expression.Arguments;
            if (args.Count == 2)
            {
                ctx.LinqQuery.AddWhereClause(args[1]);
            }

            ctx.LinqQuery.PostProcess = ctx.CurrentMethod.Expression.Method.Name.Contains("Default")
                ? (IPostProcess)new SingleOrDefaultPostProcess()
                : new SinglePostProcess();

            //note this take 2 is to ensure we have a SINGLE returned.
            ctx.LinqQuery.Paging.Take = 2;
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return true;
        }


        class SingleOrDefaultPostProcess : IPostProcess
        {
            public object Execute<T>(IEnumerable<T> items)
            {
                return items.SingleOrDefault();
            }
        }

        class SinglePostProcess : IPostProcess
        {
            public object Execute<T>(IEnumerable<T> items)
            {
                return items.Single();
            }
        }

    }
}