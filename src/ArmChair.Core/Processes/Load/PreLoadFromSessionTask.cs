namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using InSession;
    using Tasks;

    public class PreLoadFromSessionMapTask : PipeItemMapTask<LoadContext>
    {
        private readonly ISessionCache _sessionCache;

        public PreLoadFromSessionMapTask(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public override bool CanHandle(LoadContext item)
        {
            return item.Entity == null;
        }

        public override IEnumerable<LoadContext> Execute(LoadContext item)
        {
            var entry = _sessionCache[item.Key];
            if (entry == null)
            {
                yield return item;
            }

            if (entry.Action == ActionType.Delete)
            {
                yield return null;
            }
            item.Entity = entry.Instance;
            item.LoadedFromCache = true;
            yield return item;
        }
    }
}