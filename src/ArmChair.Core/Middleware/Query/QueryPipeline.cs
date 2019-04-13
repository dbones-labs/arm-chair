namespace ArmChair.Middleware.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Processes.Load;
    using Processes.Query;
    using Tracking;

    /// <summary>
    /// pipeline for querying against couchdb
    /// </summary>
    public class QueryPipeline
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;


        public QueryPipeline(CouchDb couchDb,
            IIdManager idManager,
            IIdAccessor idAccessor,
            IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public virtual async Task<IEnumerable<T>> Query<T>(MongoQuery query, ISessionCache sessionCache, ITrackingProvider tracking)
            where T : class
        {
            
            var pipe = new Middleware<QueryContext, IEnumerable<LoadContext>>();
            pipe.Use(new TrackingAction(tracking));
            pipe.Use(new UpdateCacheAction(sessionCache));
            pipe.Use(new OverrideUsingCacheAction<LoadContext>(sessionCache));
            pipe.Use(new MongoQueryFromDatabaseAction(_couchDb, _idManager, _idAccessor));

            //setup the load context
            var type = typeof(T);

            var ctx = new QueryContext()
            {
                Query = query,
                Type = type
            };

            var results = await pipe.Execute(ctx);
            return results.Select(x => x.Entity).Where(x => x != null).Cast<T>();
        }
    }
}