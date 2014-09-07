namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using InSession;
    using Tasks;

    public class PostSaveToSesionMapTask : PipeItemMapTask<LoadContext>
    {
        private readonly ISessionCache _sessionCache;

        public PostSaveToSesionMapTask(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public override bool CanHandle(LoadContext item)
        {
            return !item.LoadedFromCache;
        }

        public override IEnumerable<LoadContext> Execute(LoadContext item)
        {
            var sessionEntry = new SessionEntry()
            {
                Action = ActionType.Update,
                Instance = item.Entity,
                Key = item.Key
            };

            _sessionCache.Attach(sessionEntry);
            yield return item;
        }
    }
}