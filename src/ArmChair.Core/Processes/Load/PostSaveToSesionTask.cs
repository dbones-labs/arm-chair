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
    using InSession;
    using Tasks.BySingleItem;

    public class PostSaveToSesionMapTask<T> : TaskOnItem<T> where T: LoadContext
    {
        private readonly ISessionCache _sessionCache;

        public PostSaveToSesionMapTask(ISessionCache sessionCache, IItemIterator<T> iterator = null) : base(iterator)
        {
            _sessionCache = sessionCache;
        }

        public override T Execute(T item)
        {
            if (item.LoadedFromCache) return item;
            if (item.Entity == null) return item;

            var sessionEntry = new SessionEntry()
            {
                Action = ActionType.Update,
                Instance = item.Entity,
                Key = item.Key
            };
            
            _sessionCache.Attach(sessionEntry);

            return item;
        }
    }
}