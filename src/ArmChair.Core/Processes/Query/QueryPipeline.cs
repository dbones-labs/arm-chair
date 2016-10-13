namespace ArmChair.Processes.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Load;
    using Tasks;
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

        //custom tasks
        private readonly List<Func<CreateTaskContext, IPipeTask<QueryContext>>> _postLoadTasks = new List<Func<CreateTaskContext, IPipeTask<QueryContext>>>();

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

        /// <summary>
        /// add a task to be execute after items are loaded from the database
        /// </summary>
        /// <param name="createTask">note this is a function that create your task in a context</param>
        public void RegisterPostLoadTask(Func<CreateTaskContext, IPipeTask<QueryContext>> createTask)
        {
            _postLoadTasks.Add(createTask);
        }

        public IEnumerable<T> Query<T>(MongoQuery query, ISessionCache sessionCache, ITrackingProvider tracking) where T : class
        {
            var taskCtx = new CreateTaskContext(_couchDb, _idManager, _revisionAccessor, sessionCache);
            var queryDb = new MongoQueryFromDataBaseTask(_couchDb, _idManager, _idAccessor);

            //setup the pipeline
            var tasks = new List<IPipeTask<QueryContext>>();
            tasks.Add(queryDb);
            tasks.Add(new PostCheckCacheTask<QueryContext>(sessionCache));
            tasks.AddRange(_postLoadTasks.Select(task => task(taskCtx)));
            tasks.Add(new PostSaveToSesionMapTask<QueryContext>(sessionCache));
            tasks.Add(new PostTrackingMapTask<QueryContext>(tracking));

            var pipilineExecutor = tasks.CreatePipeline();

            //setup the load context
            var type = typeof(T);

            var ctx = new QueryContext()
            {
                Query = query,
                Type = type
            };


            var results = pipilineExecutor.Execute(new [] {ctx});
            return results.Select(x => x.Entity).Where(x => x != null).Cast<T>();
        }
    }
}