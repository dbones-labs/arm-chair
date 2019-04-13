namespace ArmChair.Middleware.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InSession;
    using Processes.Load;
    using Processes.Query;

    public class UpdateCacheAction : IAction<QueryContext, IEnumerable<LoadContext>>
    {
        private readonly ISessionCache _sessionCache;

        public UpdateCacheAction(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }
        
        public async Task<IEnumerable<LoadContext>> Execute(QueryContext context,
            Next<QueryContext, IEnumerable<LoadContext>> next)
        {
            var items = (await next(context)).ToList();
            
            foreach (var item in items)
            {
                if (item.LoadedFromCache) continue;
                if (item.Entity == null) continue;
                
                var sessionEntry = new SessionEntry()
                {
                    Action = ActionType.Update,
                    Instance = item.Entity,
                    Key = item.Key
                };

                _sessionCache.Attach(sessionEntry);
            }

            return items;
        }
    }
}