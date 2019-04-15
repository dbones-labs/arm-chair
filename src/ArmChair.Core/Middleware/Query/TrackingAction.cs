namespace ArmChair.Middleware.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Load;
    using Tracking;

    public class TrackingAction : IAction<QueryContext, IEnumerable<LoadContext>>
    {
        private readonly ITrackingProvider _tracking;

        public TrackingAction(ITrackingProvider tracking)
        {
            _tracking = tracking;
        }
        
        public async Task<IEnumerable<LoadContext>> Execute(QueryContext context,
            Next<QueryContext, IEnumerable<LoadContext>> next)
        {
            var items = (await next(context)).ToList();
            
            foreach (var item in items)
            {
                if (item.LoadedFromCache) continue;

                _tracking.TrackInstance(item.Entity);
            }

            return items;
        }
    }
}