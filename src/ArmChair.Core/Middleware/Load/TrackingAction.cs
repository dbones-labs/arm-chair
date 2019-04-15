namespace ArmChair.Middleware.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Tracking;

    public class TrackingAction<T> : IAction<IEnumerable<T>> where T : LoadContext
    {
        private readonly ITrackingProvider _tracking;

        public TrackingAction(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }

        public async Task Execute(IEnumerable<T> context, Next<IEnumerable<T>> next)
        {
            var items = context.ToList();

            await next(items);

            foreach (var item in items)
            {
                if (item.LoadedFromCache) continue;
                if (item.Entity == null) continue;
                _tracking.TrackInstance(item.Entity);
            }
        }
    }
}