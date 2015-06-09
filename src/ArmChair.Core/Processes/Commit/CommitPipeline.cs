// Copyright 2013 - 2015 dbones.co.uk (David Rundle)
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
namespace ArmChair.Processes.Commit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Tasks;
    using Tracking;

    public class CommitPipeline
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IRevisionAccessor _revisionAccessor;

        //custom tasks
        private readonly List<Func<CreateTaskContext, IPipeTask<CommitContext>>> _preProcessTasks = new List<Func<CreateTaskContext, IPipeTask<CommitContext>>>();
        private readonly List<Func<CreateTaskContext, IPipeTask<CommitContext>>> _postProcessTasks = new List<Func<CreateTaskContext, IPipeTask<CommitContext>>>();


        public CommitPipeline(
            CouchDb couchDb,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _revisionAccessor = revisionAccessor;
        }

        public void RegisterPreCommitTask(Func<CreateTaskContext, IPipeTask<CommitContext>> createTask)
        {
            _preProcessTasks.Add(createTask);
        }

        public void RegisterPostCommitTask(Func<CreateTaskContext, IPipeTask<CommitContext>> createTask)
        {
            _postProcessTasks.Add(createTask);
        }

        public virtual void Process(ISessionCache sessionCache, ITrackingProvider tracking)
        {
            var taskCtx = new CreateTaskContext(_couchDb, _idManager, _revisionAccessor, sessionCache);

            //setup the pipeline
            var tasks = new List<IPipeTask<CommitContext>>();
            tasks.Add(new PreCommitFilterTrackingTask(tracking));
            tasks.AddRange(_preProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new CommitToDbTask(_couchDb, _revisionAccessor));
            tasks.AddRange(_postProcessTasks.Select(preLoadTask => preLoadTask(taskCtx)));
            tasks.Add(new PostCommitSesionTask(sessionCache));
            tasks.Add(new PostCommitTrackingTask(tracking));

            var pipilineExecutor = tasks.CreatePipeline();

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
            

            pipilineExecutor.Execute(bulkContexts);
        }
    }
}