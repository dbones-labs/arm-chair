namespace ArmChair.Processes.Update
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Tasks;
    using Tracking;

    public class BulkPipeline
    {
        private readonly Database _database;
        private readonly IIdManager _idManager;
        private readonly IRevisionAccessor _revisionAccessor;

        //custom tasks
        private readonly List<Func<CreateTaskContext, IPipeTask<BulkContext>>> _preProcessTasks = new List<Func<CreateTaskContext, IPipeTask<BulkContext>>>();
        private readonly List<Func<CreateTaskContext, IPipeTask<BulkContext>>> _postProcessTasks = new List<Func<CreateTaskContext, IPipeTask<BulkContext>>>();


        public BulkPipeline(
            Database database,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor)
        {
            _database = database;
            _idManager = idManager;
            _revisionAccessor = revisionAccessor;
        }

        public void RegisterPreProcessTask(Func<CreateTaskContext, IPipeTask<BulkContext>> createTask)
        {
            _preProcessTasks.Add(createTask);
        }

        public void RegisterPostLoadTask(Func<CreateTaskContext, IPipeTask<BulkContext>> createTask)
        {
            _postProcessTasks.Add(createTask);
        }

        public virtual void Process(ISessionCache sessionCache, ITrackingProvider tracking)
        {
            var taskCtx = new CreateTaskContext(_database, _idManager, _revisionAccessor, sessionCache);

            //setup the pipeline
            var tasks = new List<IPipeTask<BulkContext>>();
            tasks.Add(new PreUpdateFilterTrackingTask(tracking));
            tasks.AddRange(_preProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new BulkUpdateDataBaseTask(_database, _revisionAccessor));
            tasks.AddRange(_postProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new PostUpdateSesionTask(sessionCache));
            tasks.Add(new PostUpdateTrackingTask(tracking));

            var pipilineExecutor = tasks.CreatePipeline();

            //setup the bulk update context
            var bulkContexts = sessionCache.Entries.Select(entry =>
            {
                var bulkCtx = new BulkContext()
                {
                    ActionType = entry.Action,
                    Entity = entry.Instance,
                    Key = entry.Key
                };
                return bulkCtx;
            });
            

            pipilineExecutor.Execute(bulkContexts);
        }
    }
}