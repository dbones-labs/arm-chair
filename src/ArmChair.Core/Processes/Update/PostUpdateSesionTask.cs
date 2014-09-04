namespace ArmChair.Processes.Update
{
    using System.Collections.Generic;
    using InSession;
    using Tasks;

    public class PostUpdateSesionTask : PipeItemTask<BulkContext>
    {
        private readonly ISessionCache _sessionCache;

        public PostUpdateSesionTask(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public override bool CanHandle(BulkContext item)
        {
            return item.ActionType != ActionType.Update;
        }

        public override IEnumerable<BulkContext> Execute(BulkContext item)
        {
            if (item.ActionType == ActionType.Add)
            {
                var entry = _sessionCache[item.Key];
                entry.Action = ActionType.Update;
            }
            else
            {
                _sessionCache.Remove(item.Key);
            }

            yield return item;
        }
    }
}