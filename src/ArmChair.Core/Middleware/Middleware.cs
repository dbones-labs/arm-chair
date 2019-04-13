namespace ArmChair.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Middleware<TContext>
    {
        private readonly List<PipeItem> _pipedTypes = new List<PipeItem>();

        public void Use(IAction<TContext> action)
        {
            _pipedTypes.Add(new PipeItem(action));
        }


        public async Task Execute(TContext context)
        {
            var enumerator = _pipedTypes.GetEnumerator();
            var factory = new GetNextFactory(enumerator);
            await factory.GetNext<TContext>()(context);
        }
    }
    
    
    public class Middleware<TIn, TOut>
    {
        private readonly List<PipeItem> _pipedTypes = new List<PipeItem>();

        public void Use(IAction<TIn, TOut> action)
        {
            _pipedTypes.Add(new PipeItem(action));
        }


        public async Task<TOut> Execute(TIn context)
        {
            var enumerator = _pipedTypes.GetEnumerator();
            var factory = new GetNextFactory(enumerator);
            return await factory.GetNext<TIn, TOut>()(context);
        }
    }
}