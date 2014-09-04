namespace ArmChair
{
    using System.Collections;
    using System.Collections.Generic;
    using IdManagement;
    using InSession;
    using Processes.Load;
    using Processes.Update;
    using Tracking;

    public class Session : ISession
    {
        private readonly ISessionCache _sessionCache;
        private readonly LoadPipeline _loadPipeline;
        private readonly BulkPipeline _bulkPipeline;
        private readonly IIdManager _idManager;
        private readonly ITrackingProvider _trackingProvider;
        //private ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        public Session(
            LoadPipeline loadPipeline,
            BulkPipeline bulkPipeline,
            IIdManager idManager,
            ITrackingProvider trackingProvider,
            ISessionCache sessionCache
            )
        {
            _sessionCache = sessionCache;
            _loadPipeline = loadPipeline;
            _bulkPipeline = bulkPipeline;
            _idManager = idManager;
            _trackingProvider = trackingProvider;
        }

        public virtual void Dispose()
        {
            //do nothing a the moment.
        }

        public virtual void Add<T>(T instance) where T : class
        {
            var key = _idManager.GetId(instance); //set if they set one.

            if (key == null)
            {
                key = _idManager.GenerateId(typeof(T));
                _idManager.SetId(instance, key.Id);
            }

            var entry = new SessionEntry()
            {
                Action = ActionType.Add,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual void Update<T>(T instance) where T : class
        {
            //allows for attaching exiting objects to this session! 
            //(issue could arise with the rev)
            var key = _idManager.GetId(instance);
            var entry = new SessionEntry()
            {
                Action = ActionType.Update,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual void Remove<T>(T instance) where T : class
        {
            var key = _idManager.GetId(instance);
            var entry = new SessionEntry()
            {
                Action = ActionType.Delete,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual IEnumerable<T> GetByIds<T>(IEnumerable ids) where T : class
        {
            var results = _loadPipeline.LoadMany<T>(ids, _sessionCache, _trackingProvider);
            return results;
        }

        public virtual T GetById<T>(object id) where T : class
        {
            var result = _loadPipeline.LoadOne<T>(id, _sessionCache, _trackingProvider);
            return result;
        }

        public virtual void Commit()
        {
            _bulkPipeline.Process(_sessionCache, _trackingProvider);
        }
    }
}