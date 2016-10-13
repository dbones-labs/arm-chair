namespace ArmChair.Linq.Pre.Handlers
{
    using System.Collections.Generic;
    using System.Linq;

    public class FirstSubPatternHandler : SubPatternHandlerBase
    {
        public FirstSubPatternHandler()
            : base(objects => objects.First(), objects => objects.FirstOrDefault())
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
                ? (IPostProcess) new FirstOrDefaultPostProcess()
                : new FirstPostProcess();

            ctx.LinqQuery.Paging.Take = 1;
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return true;
        }


        class FirstOrDefaultPostProcess : IPostProcess
        {
            public object Execute<T>(IEnumerable<T> items)
            {
                return items.FirstOrDefault();
            }
        }

        class FirstPostProcess : IPostProcess
        {
            public object Execute<T>(IEnumerable<T> items)
            {
                return items.First();
            }
        }

    }


    
}