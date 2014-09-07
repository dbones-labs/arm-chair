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
            tasks.Add(new PreUpdateFilterTrackingMapTask(tracking));
            tasks.AddRange(_preProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new BulkUpdateDataBaseTask(_database, _revisionAccessor));
            tasks.AddRange(_postProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new PostUpdateSesionMapTask(sessionCache));
            tasks.Add(new PostUpdateTrackingMapTask(tracking));

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