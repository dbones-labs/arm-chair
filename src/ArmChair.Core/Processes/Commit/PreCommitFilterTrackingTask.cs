﻿// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
    using System.Collections.Generic;
    using InSession;
    using Tasks;
    using Tracking;

    public class PreCommitFilterTrackingTask : PipeItemMapTask<CommitContext>
    {
        private readonly ITrackingProvider _tracking;

        public PreCommitFilterTrackingTask(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public override bool CanHandle(CommitContext item)
        {
            return item.ActionType == ActionType.Update;
        }

        public override IEnumerable<CommitContext> Execute(CommitContext item)
        {
            //filter out update items which have no changes.
            if (_tracking.HasChanges(item))
            {
                yield return item;
            }
        }
    }
}