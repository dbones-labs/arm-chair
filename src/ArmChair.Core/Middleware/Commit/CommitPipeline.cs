namespace ArmChair.Middleware.Commit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Processes.Commit;
    using Tracking;

    /// <summary>
    /// pipeline which will commit changes to the database
    /// </summary>
    public class CommitPipeline
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IRevisionAccessor _revisionAccessor;

        
        public CommitPipeline(
            CouchDb couchDb,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _revisionAccessor = revisionAccessor;
        }

        
//        /// <summary>
//        /// register any task to be executed after commiting an item.
//        /// </summary>
//        /// <param name="createTask">func which will be used to create the task</param>
//        public void RegisterCommitAction(Func<CreateTaskContext, IAction<IEnumerable<CommitContext>> createAction)
//        {
//            //_postProcessTasks.Add(createTask);
//        }

        public virtual async Task Process(ISessionCache sessionCache, ITrackingProvider tracking)
        {
            //var taskCtx = new CreateTaskContext(_couchDb, _idManager, _revisionAccessor, sessionCache);

            //setup the pipeline
            var pipe = new Middleware<IEnumerable<CommitContext>>();
            pipe.Use(new TrackingAction(tracking));
            pipe.Use(new SessionAction(sessionCache));
            pipe.Use(new CommitToDatabaseAction(_couchDb, _revisionAccessor));
            
            //setup the bulk update context
            var bulkContexts = sessionCache.Entries.Select(entry =>
            {
                var bulkCtx = new CommitContext()
                {
                    ActionType = entry.Action,
                    Entity = entry.Instance,
                    Key = entry.Key
                };
                return bulkCtx;
            });
            
            await pipe.Execute(bulkContexts);
        }
    }
}