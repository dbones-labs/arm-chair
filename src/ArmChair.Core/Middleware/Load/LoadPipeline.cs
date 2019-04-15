namespace ArmChair.Middleware.Load
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Tracking;

    public class LoadPipeline
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        public LoadPipeline(CouchDb couchDb,
            IIdManager idManager,
            IIdAccessor idAccessor,
            IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }


        public async Task<T> LoadOne<T>(object id, ISessionCache sessionCache, ITrackingProvider tracking)
            where T : class
        {
            var result = await Load<T>(new[] {id}, sessionCache, tracking, new LoadFromDatabaseAction(_couchDb));
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<T>> LoadMany<T>(IEnumerable ids, ISessionCache sessionCache, ITrackingProvider tracking)
            where T : class
        {
            return Load<T>(ids, sessionCache, tracking,
                new LoadManyFromDatabaseAction(_couchDb, _idManager, _idAccessor));
        }

        protected virtual async Task<IEnumerable<T>> Load<T>(IEnumerable ids, ISessionCache sessionCache,
            ITrackingProvider tracking, IAction<IEnumerable<LoadContext>> loadTask) where T : class
        {

            //setup the pipeline
            var pipe = new Middleware<IEnumerable<LoadContext>>();
            pipe.Use(new TrackingAction<LoadContext>(tracking));
            pipe.Use(new SessionAction<LoadContext>(sessionCache));
            pipe.Use(loadTask);

            //setup the load context
            var type = typeof(T);
            var loadContexts = ids.Cast<object>().Select(id =>
            {
                var idKey = _idManager.GetFromId(type, id);
                return new LoadContext()
                {
                    Key = idKey,
                    Type = type
                };
            }).ToList();


            await pipe.Execute(loadContexts);
            return loadContexts.Select(x => x.Entity).Where(x => x != null).Cast<T>();
        }
    }
}