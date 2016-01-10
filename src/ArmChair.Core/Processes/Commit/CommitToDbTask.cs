// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using InSession;
    using Tasks;

    public class CommitToDbTask : IPipeTask<CommitContext>
    {
        private readonly CouchDb _couchDb;
        private readonly IRevisionAccessor _revisionAccessor;

        public CommitToDbTask(CouchDb couchDb, IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _revisionAccessor = revisionAccessor;
        }

        public IEnumerable<CommitContext> Execute(IEnumerable<CommitContext> items)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)

            var entityUpdates = new Dictionary<string, object>();
            var docRequests = new List<BulkDocRequest>();

            foreach (var bulkContext in items)
            {
                var entry = new BulkDocRequest
                {
                    Content = bulkContext.Entity,
                    Id = bulkContext.Key.CouchDbId
                };
                
                switch (bulkContext.ActionType)
                {
                    case ActionType.Add:
                        break;
                    case ActionType.Update:
                        break;
                    case ActionType.Delete:
                        entry.Rev = (string)_revisionAccessor.GetRevision(bulkContext.Entity);
                        entry.Delete = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                docRequests.Add(entry);
                entityUpdates.Add(entry.Id, bulkContext.Entity);
            }

            var request = new BulkDocsRequest {Docs = docRequests};

            var updates = _couchDb.BulkApplyChanges(request);
            //update any revisions!
            foreach (var update in updates)
            {
                var entity = entityUpdates[update.Id];
                _revisionAccessor.SetRevision(entity, update.Rev);
            }

            return items;
            
        }
    }
}