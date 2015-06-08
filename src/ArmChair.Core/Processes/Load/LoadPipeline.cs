// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair.Processes.Load
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Tasks;
    using Tracking;

    public class LoadPipeline
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        //custom tasks
        private readonly List<Func<CreateTaskContext, IPipeTask<LoadContext>>> _preLoadTasks = new List<Func<CreateTaskContext, IPipeTask<LoadContext>>>();
        private readonly List<Func<CreateTaskContext, IPipeTask<LoadContext>>> _postLoadTasks = new List<Func<CreateTaskContext, IPipeTask<LoadContext>>>();


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

        /// <summary>
        /// add a task to be executed before items are loaded from the database.
        /// </summary>
        /// <param name="createTask">note this is a function that create your task in a context</param>
        public void RegisterPreLoadTask(Func<CreateTaskContext, IPipeTask<LoadContext>> createTask)
        {
            _preLoadTasks.Add(createTask);
        }

        /// <summary>
        /// add a task to be execute after items are loaded from the database
        /// </summary>
        /// <param name="createTask">note this is a function that create your task in a context</param>
        public void RegisterPostLoadTask(Func<CreateTaskContext, IPipeTask<LoadContext>> createTask)
        {
            _postLoadTasks.Add(createTask);
        }

        public T LoadOne<T>(object id, ISessionCache sessionCache, ITrackingProvider tracking) where T : class
        {
            return Load<T>(new[] { id }, sessionCache, tracking, new LoadFromDataBaseMapTask(_couchDb)).FirstOrDefault();
        }

        public IEnumerable<T> LoadMany<T>(IEnumerable ids, ISessionCache sessionCache, ITrackingProvider tracking) where T : class
        {
            return Load<T>(ids, sessionCache, tracking, new LoadManyFromDataBaseTask(_couchDb, _idManager, _idAccessor));
        }

        protected virtual IEnumerable<T> Load<T>(IEnumerable ids, ISessionCache sessionCache, ITrackingProvider tracking, IPipeTask<LoadContext> loadTask) where T : class
        {
            var taskCtx = new CreateTaskContext(_couchDb, _idManager, _revisionAccessor, sessionCache);

            //setup the pipeline
            var tasks = new List<IPipeTask<LoadContext>>();
            tasks.Add(new PreLoadFromSessionMapTask(sessionCache));
            tasks.AddRange(_preLoadTasks.Select(task => task(taskCtx)));
            tasks.Add(loadTask);
            tasks.AddRange(_postLoadTasks.Select(task => task(taskCtx)));
            tasks.Add(new PostSaveToSesionMapTask(sessionCache));
            tasks.Add(new PostTrackingMapTask(tracking));

            var pipilineExecutor = tasks.CreatePipeline();

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
            });


            var results = pipilineExecutor.Execute(loadContexts);
            return results.Select(x => x.Entity).Where(x => x != null).Cast<T>();
        }
    }
}