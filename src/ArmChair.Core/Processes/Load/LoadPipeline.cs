namespace ArmChair.Processes.Load
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Tasks;
    using Tracking;

    public class LoadPipeline
    {
        private readonly Database _database;
        private readonly IIdManager _idManager;
        private readonly IdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        //custom tasks
        private readonly List<Func<CreateTaskContext, IPipeTask<LoadContext>>> _preLoadTasks = new List<Func<CreateTaskContext, IPipeTask<LoadContext>>>();
        private readonly List<Func<CreateTaskContext, IPipeTask<LoadContext>>> _postLoadTasks = new List<Func<CreateTaskContext, IPipeTask<LoadContext>>>();


        public LoadPipeline(Database database,
            IIdManager idManager,
            IdAccessor idAccessor,
            IRevisionAccessor revisionAccessor)
        {
            _database = database;
            _idManager = idManager;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public void RegisterPreLoadTask(Func<CreateTaskContext, IPipeTask<LoadContext>> createTask)
        {
            _preLoadTasks.Add(createTask);
        }

        public void RegisterPostLoadTask(Func<CreateTaskContext, IPipeTask<LoadContext>> createTask)
        {
            _postLoadTasks.Add(createTask);
        }

        public T LoadOne<T>(object id, ISessionCache sessionCache, ITrackingProvider tracking) where T : class
        {
            return Load<T>(new[] { id }, sessionCache, tracking, new LoadFromDataBaseTask(_database)).FirstOrDefault();
        }

        public IEnumerable<T> LoadMany<T>(IEnumerable ids, ISessionCache sessionCache, ITrackingProvider tracking) where T : class
        {
            return Load<T>(ids, sessionCache, tracking, new LoadManyFromDataBaseTask(_database, _idManager, _idAccessor));
        }

        protected virtual IEnumerable<T> Load<T>(IEnumerable ids, ISessionCache sessionCache, ITrackingProvider tracking, IPipeTask<LoadContext> loadTask) where T : class
        {
            var taskCtx = new CreateTaskContext(_database, _idManager, _revisionAccessor, sessionCache);

            //setup the pipeline
            var tasks = new List<IPipeTask<LoadContext>>();
            tasks.Add(new PreLoadFromSessionTask(sessionCache));
            tasks.AddRange(_preLoadTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(loadTask);
            tasks.AddRange(_postLoadTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new PostSaveToSesionTask(sessionCache));
            tasks.Add(new PostTrackingTask(tracking));

            var pipilineExecutor = tasks.CreatePipeline();

            //setup the load context
            var type = typeof(T);
            var loadContexts = ids.Cast<object>().Select(id =>
            {
                var idKey = _idManager.GetFromId(type, id);
                return new LoadContext()
                {
                    Key = idKey
                };
            });


            var results = pipilineExecutor.Execute(loadContexts);
            return results.Select(x => x.Entity).Cast<T>();
        }
    }
}