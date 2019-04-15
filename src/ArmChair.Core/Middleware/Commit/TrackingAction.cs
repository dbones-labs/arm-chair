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

namespace ArmChair.Middleware.Commit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InSession;
    using Tracking;

    /// <summary>
    /// filters out entities which do not require updates to the db.
    /// </summary>
    public class TrackingAction : IAction<IEnumerable<CommitContext>>
    {
        private readonly ITrackingProvider _tracking;

        public TrackingAction(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }
        

        public async Task Execute(IEnumerable<CommitContext> context, Next<IEnumerable<CommitContext>> next)
        {
            bool AddOrRemove(CommitContext commitContext) => commitContext.ActionType != ActionType.Update;
            
            bool UpdateHasChanges(CommitContext commitContext) => commitContext.ActionType == ActionType.Update 
                                                                  && _tracking.HasChanges(commitContext.Entity);

            var toCommit = context
                .Where(x => AddOrRemove(x) || UpdateHasChanges(x)).ToList();
            
            
            await next(toCommit);

            
            foreach (var item in toCommit)
            {
                switch (item.ActionType)
                {
                    case ActionType.Add:
                        _tracking.TrackInstance(item.Entity);
                        break;
                    case ActionType.Update:
                        _tracking.Reset(item.Entity);
                        break;
                    case ActionType.Delete:
                        _tracking.CeaseTracking(item.Entity);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}