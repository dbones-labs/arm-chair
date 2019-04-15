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

namespace ArmChair.Middleware.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using Load;


    /// <summary>
    /// query the db using the mongo query object
    /// </summary>
    public class MongoQueryFromDatabaseAction : IAction<QueryContext, IEnumerable<LoadContext>>
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;

        public MongoQueryFromDatabaseAction(CouchDb couchDb, IIdManager idManager, IIdAccessor idAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
        }

        public Task<IEnumerable<LoadContext>> Execute(QueryContext context,
            Next<QueryContext, IEnumerable<LoadContext>> next)
        {
            var ctx = context;
            var q = ctx.Query;
            var query = new MongoQueryRequest
            {
                Selector = q.Selector,
                Limit = q.Limit,
                Skip = q.Skip,
                Sort = q.Sort
            };

            var allDocs = _couchDb.MongoQuery(query);

            //using the loaded docs, update the context entries.
            var results =
                (from entity in allDocs.Docs
                    let id = _idAccessor.GetId(entity)
                    let key = _idManager.GetFromId(entity.GetType(), id)
                    select new LoadContext()
                    {
                        Entity = entity,
                        Key = key,
                        Type = entity.GetType()
                    }).ToList();

            return Task.FromResult((IEnumerable<LoadContext>) results);
        }
    }
}