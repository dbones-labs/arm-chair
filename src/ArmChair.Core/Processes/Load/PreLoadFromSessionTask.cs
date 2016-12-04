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
    using System;
    using InSession;
    using Tasks.BySingleItem;

    public class PreLoadFromSessionMapTask : TaskOnItem<LoadContext>
    {
        private readonly ISessionCache _sessionCache;

        public PreLoadFromSessionMapTask(ISessionCache sessionCache, IItemIterator<LoadContext> iterator = null) : base(iterator)
        {
            _sessionCache = sessionCache;
        }

        public override LoadContext Execute(LoadContext item, Action skip)
        {
            //should not be the case
            if (item.Entity != null) return item;

            var entry = _sessionCache[item.Key];

            //not in cache
            if (entry == null) return item;

            //the item has been removed inside the session.
            if (entry.Action == ActionType.Delete)
            {
                skip();
                return null;
            }

            item.Entity = entry.Instance;
            item.LoadedFromCache = true;

            return item;
        }
    }
}