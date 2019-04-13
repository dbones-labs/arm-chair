namespace ArmChair.Middleware.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Processes.Load;
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

            items.Where(x => !x.LoadedFromCache)
                .Where(x => x.Entity != null);

            foreach (var item in items)
            {
                _tracking.TrackInstance(item.Entity);
            }
        }
    }
}