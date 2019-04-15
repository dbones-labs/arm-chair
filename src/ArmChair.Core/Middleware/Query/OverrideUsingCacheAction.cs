namespace ArmChair.Middleware.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InSession;
    using Load;

    /// <summary>
    /// when running a query, we will load the object from the database
    /// this task looks at the cached version and uses that, as we assume they has been
    /// a change to the object which we cannot lose.
    /// </summary>
    /// <typeparam name="T">type this task is operating on</typeparam>
    public class OverrideUsingCacheAction<T> : IAction<QueryContext, IEnumerable<T>> where T : LoadContext
    {
        private readonly ISessionCache _sessionCache;

        public OverrideUsingCacheAction(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }


        public async Task<IEnumerable<T>> Execute(QueryContext context, Next<QueryContext, IEnumerable<T>> next)
        {
            var results = (await next(context)).ToList();

            foreach (var result in results)
            {
                //we favour the cached version over the db in this context.
                var entry = _sessionCache[result.Key];
                if (entry == null) continue;

                result.Entity = entry.Instance;
                result.LoadedFromCache = true;
            }

            return results;
        }
    }
}