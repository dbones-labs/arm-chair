namespace ArmChair.Processes.Query
{
    using InSession;
    using Load;
    using Tasks.BySingleItem;

    /// <summary>
    /// when running a query, we will load the object from the database
    /// this task looks at the cached version and uses that, as we assume they has been
    /// a change to the object which we cannot lose.
    /// </summary>
    /// <typeparam name="T">type this task is operating on</typeparam>
    public class PostCheckCacheTask<T> : TaskOnItem<T> where T : LoadContext
    {
        private readonly ISessionCache _sessionCache;

        public PostCheckCacheTask(ISessionCache sessionCache, IItemIterator<T> itemIterator = null) : base(itemIterator)
        {
            _sessionCache = sessionCache;
        }

        public override T Execute(T ctx)
        {
            //we favior the cached verson over the db in this context.
            var entry = _sessionCache[ctx.Key];
            if (entry == null) return ctx;

            ctx.Entity = entry.Instance;
            ctx.LoadedFromCache = true;

            return ctx;
        }
    }
}