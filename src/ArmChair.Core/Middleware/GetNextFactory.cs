namespace ArmChair.Middleware
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class GetNextFactory
    {
        private readonly IEnumerator<PipeItem> _enumerator;
        
        public GetNextFactory(IEnumerator<PipeItem> enumerator)
        {
            _enumerator = enumerator;
        }

        public Next<TIn, TOut> GetNext<TIn, TOut>()
        {
            if (!_enumerator.MoveNext())
            {
                return ctx => Task.FromResult(default(TOut));
            }

            var pipedType = _enumerator.Current;

            Task<TOut> Next(TIn ctx)
            {
                var middleware = (IAction<TIn, TOut>) pipedType.GivenInstance;
                return middleware.Execute(ctx, GetNext<TIn, TOut>());
            }

            return Next;

        }
        
        public Next<TContext> GetNext<TContext>()
        {
            if (!_enumerator.MoveNext())
            {
                return ctx => Task.CompletedTask;
            }

            var pipedType = _enumerator.Current;

            Task Next(TContext ctx)
            {
                var middleware = (IAction<TContext>) pipedType.GivenInstance;
                return middleware.Execute(ctx, GetNext<TContext>());
            }

            return Next;

        }
    }
}