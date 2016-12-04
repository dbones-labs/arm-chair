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
    using InSession;
    using Tasks.BySingleItem;
    using Tracking;

    /// <summary>
    /// update the tracking on post of commiting.
    /// </summary>
    public class PostCommitTrackingTask : TaskOnItem<CommitContext>
    {
        private readonly ITrackingProvider _tracking;

        public PostCommitTrackingTask(ITrackingProvider tracking, IItemIterator<CommitContext> iterator = null) : base(iterator)
        {
            _tracking = tracking;
        }

        public override CommitContext Execute(CommitContext item)
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

            return item;
        }
    }
}