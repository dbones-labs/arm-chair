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
    using Commands;
    using Tasks;

    public class LoadFromDataBaseMapTask : PipeItemMapTask<LoadContext>
    {
        private readonly CouchDb _couchDb;

        public LoadFromDataBaseMapTask(CouchDb couchDb)
        {
            _couchDb = couchDb;
        }

        public override bool CanHandle(LoadContext item)
        {
            return !item.LoadedFromCache;
        }

        public override IEnumerable<LoadContext> Execute(LoadContext item)
        {
            var entity = _couchDb.LoadEntity(item.Key.ToString(), item.Type);
            item.Entity = entity;
            yield return item;
        }
    }
}