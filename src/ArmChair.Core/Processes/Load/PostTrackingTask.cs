namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using Tasks;
    using Tracking;

    public class PostTrackingMapTask : PipeItemMapTask<LoadContext>
    {
        private readonly ITrackingProvider _tracking;

        public PostTrackingMapTask(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public override bool CanHandle(LoadContext item)
        {
            return !item.LoadedFromCache;
        }

        public override IEnumerable<LoadContext> Execute(LoadContext item)
        {
            _tracking.TrackInstance(item.Entity);
            yield return item;
        }
    }
}