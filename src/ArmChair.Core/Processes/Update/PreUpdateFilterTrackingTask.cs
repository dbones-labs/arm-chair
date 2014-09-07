namespace ArmChair.Processes.Update
{
    using System.Collections.Generic;
    using InSession;
    using Tasks;
    using Tracking;

    public class PreUpdateFilterTrackingMapTask : PipeItemMapTask<BulkContext>
    {
        private readonly ITrackingProvider _tracking;

        public PreUpdateFilterTrackingMapTask(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public override bool CanHandle(BulkContext item)
        {
            return item.ActionType == ActionType.Update;
        }

        public override IEnumerable<BulkContext> Execute(BulkContext item)
        {
            //filter out items which have no changes.
            if (_tracking.HasChanges(item))
            {
                yield return item;
            }
        }
    }
}