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
        private readonly IdAccessor _idAccessor;
        private readonly ITrackingProvider _trackingProvider;
        //private ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        public Session(
            LoadPipeline loadPipeline,
            BulkPipeline bulkPipeline,
            IIdManager idManager,
            IdAccessor idAccessor,
            ITrackingProvider trackingProvider,
            ISessionCache sessionCache
            )
        {
            _sessionCache = sessionCache;
            _loadPipeline = loadPipeline;
            _bulkPipeline = bulkPipeline;
            _idManager = idManager;
            _idAccessor = idAccessor;
            _trackingProvider = trackingProvider;
        }

        public virtual void Dispose()
        {
            //do nothing a the moment.
        }

        public virtual void Add<T>(T instance) where T : class
        {
            var type = typeof (T);
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof(T), value);

            if (key == null)
            {
                key = _idManager.GenerateId(type);
                _idAccessor.SetId(instance, key.Id);
            }

            var entry = new SessionEntry()
            {
                Action = ActionType.Add,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual void Attach<T>(T instance) where T : class
        {
            //allows for attaching exiting objects to this session! 
            //(issue could arise with the rev)
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof (T), value);
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
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof(T), value);
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