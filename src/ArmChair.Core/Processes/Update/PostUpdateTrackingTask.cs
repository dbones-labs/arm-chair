﻿namespace ArmChair.Processes.Update
{
    using System.Collections.Generic;
    using InSession;
    using Tasks;
    using Tracking;

    public class PostUpdateTrackingMapTask : PipeItemMapTask<BulkContext>
    {
        private readonly ITrackingProvider _tracking;

        public PostUpdateTrackingMapTask(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public override bool CanHandle(BulkContext item)
        {
            return item.ActionType != ActionType.Delete;
        }

        public override IEnumerable<BulkContext> Execute(BulkContext item)
        {
            if (item.TrackingRequiresReset)
            {
                _tracking.Reset(item.Entity);
            }
            else
            {
                _tracking.TrackInstance(item.Entity);
            }
            yield return item;
        }
    }
}