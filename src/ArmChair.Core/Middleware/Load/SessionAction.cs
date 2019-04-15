namespace ArmChair.Middleware.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InSession;

    public class SessionAction<T> : IAction<IEnumerable<T>> where T : LoadContext
    {
        private readonly ISessionCache _sessionCache;

        public SessionAction(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task Execute(IEnumerable<T> context, Next<IEnumerable<T>> next)
        {
            var items = context
                .Select(x =>
                {
                    var entry = _sessionCache[x.Key];

                    //not in cache
                    if (entry == null) return x;

                    //the item has been removed inside the session.
                    if (entry.Action == ActionType.Delete)
                    {
                        return null;
                    }

                    //item was cached
                    x.Entity = entry.Instance;
                    x.LoadedFromCache = true;
                    return x;
                })
                .Where(x => x != null) //removed deleted
                .Where(x => !x.LoadedFromCache) //removed cached
                .ToList();

            await next(items);

            foreach (var item in items.Where(x => x.Entity != null))
            {
                var sessionEntry = new SessionEntry()
                {
                    Action = ActionType.Update,
                    Instance = item.Entity,
                    Key = item.Key
                };

                _sessionCache.Attach(sessionEntry);
            }
        }
    }
}