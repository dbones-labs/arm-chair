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
    using InSession;
    using Tasks.BySingleItem;

    /// <summary>
    /// once an item has been commited, we will update its session context to update.
    /// </summary>
    public class PostCommitSesionTask : TaskOnItem<CommitContext>
    {
        private readonly ISessionCache _sessionCache;

        public PostCommitSesionTask(ISessionCache sessionCache, IItemIterator<CommitContext> iterator = null) : base(iterator)
        {
            _sessionCache = sessionCache;
        }

        public override CommitContext Execute(CommitContext item)
        {
            switch (item.ActionType)
            {
                case ActionType.Update:
                    return item;
                case ActionType.Add:
                    var entry = _sessionCache[item.Key];
                    entry.Action = ActionType.Update;
                    break;
                default:
                    _sessionCache.Remove(item.Key);
                    break;
            }

            return item;
        }
    }
}