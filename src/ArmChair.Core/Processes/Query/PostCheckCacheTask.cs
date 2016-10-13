namespace ArmChair.Processes.Query
{
    using System.Collections.Generic;
    using InSession;
    using Load;
    using Tasks;

    /// <summary>
    /// when running a query, we will load the object from the database
    /// this task looks at the cached version and uses that, as we assume they has been
    /// a change to the object which we cannot lose.
    /// </summary>
    /// <typeparam name="T">type this task is operating on</typeparam>
    public class PostCheckCacheTask<T> : PipeItemMapTask<T> where T : LoadContext
    {
        private readonly ISessionCache _sessionCache;

        public PostCheckCacheTask(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public override IEnumerable<T> Execute(T ctx)
        {
            var entry = _sessionCache[ctx.Key];
            if (entry == null)
            {
                yield return ctx;
                yield break;
            }

            ctx.Entity = entry.Instance;
            ctx.LoadedFromCache = true;
            yield return ctx;
        }
    }
}