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
    using Tasks;
    using Tracking;

    public class PostTrackingMapTask<T> : PipeItemMapTask<T> where T: LoadContext
    {
        private readonly ITrackingProvider _tracking;

        public PostTrackingMapTask(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public override bool CanHandle(T item)
        {
            return !item.LoadedFromCache;
        }

        public override IEnumerable<T> Execute(T item)
        {
            if (item.Entity != null)
            {
                _tracking.TrackInstance(item.Entity);    
            }
            yield return item;
        }
    }
}