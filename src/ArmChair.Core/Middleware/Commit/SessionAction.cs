namespace ArmChair.Middleware.Commit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InSession;

    /// <summary>
    /// updates the cache accordingly on a db commit.
    /// </summary>
    public class SessionAction : IAction<IEnumerable<CommitContext>>
    {
        private readonly ISessionCache _sessionCache;

        public SessionAction(ISessionCache sessionCache) 
        {
            _sessionCache = sessionCache;
        }

        public async Task Execute(IEnumerable<CommitContext> context, Next<IEnumerable<CommitContext>> next)
        {
            var items = context.ToList();
            
            await next(items);

            foreach (var item in items)
            {
                switch (item.ActionType)
                {
                    case ActionType.Update:
                        continue;
                    case ActionType.Add:
                        var entry = _sessionCache[item.Key];
                        entry.Action = ActionType.Update;
                        break;
                    default:
                        _sessionCache.Remove(item.Key);
                        break;
                }
            }
        }
    }
}