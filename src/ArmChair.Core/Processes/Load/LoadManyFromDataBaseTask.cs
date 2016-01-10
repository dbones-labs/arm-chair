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
namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using Tasks;

    public class LoadManyFromDataBaseTask : IPipeTask<LoadContext>
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;


        public LoadManyFromDataBaseTask(CouchDb couchDb, IIdManager idManager, IIdAccessor idAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
        }

        public IEnumerable<LoadContext> Execute(IEnumerable<LoadContext> items)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)
            var toload = items.Where(x => !x.LoadedFromCache).ToDictionary(loadContext => loadContext.Key.CouchDbId);
            var allDocs = _couchDb.LoadAllEntities(new AllDocsRequest() {Keys = toload.Keys});

            //using the loaded docs, update the context entries.
            foreach (var entity in allDocs.Rows.Select(x=>x.Doc))
            {
                var id = _idAccessor.GetId(entity);
                var key = _idManager.GetFromId(entity.GetType(), id);
                toload[key.CouchDbId].Entity = entity;
            }

            return items;
        }
    }
}