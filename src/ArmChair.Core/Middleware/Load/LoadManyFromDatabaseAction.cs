namespace ArmChair.Middleware.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using Processes.Load;

    /// <summary>
    /// load a number of items from the database using the ids.
    /// </summary>
    public class LoadManyFromDatabaseAction : IAction<IEnumerable<LoadContext>>
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;

        public LoadManyFromDatabaseAction(CouchDb couchDb, IIdManager idManager, IIdAccessor idAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
        }

        public Task Execute(IEnumerable<LoadContext> context, Next<IEnumerable<LoadContext>> next)
        {
            var items = context.ToList(); //ensure 1 iteration over list. (tasks to run once)

            //we do not want to load items which have been loaded from the cache
            var toLoad = items.Where(x => !x.LoadedFromCache).ToDictionary(loadContext => loadContext.Key.CouchDbId);
            var allDocs = _couchDb.LoadAllEntities(new AllDocsRequest() {Keys = toLoad.Keys});

            //using the loaded docs, update the context entries.
            foreach (var entity in allDocs.Rows.Select(x => x.Doc))
            {
                var id = _idAccessor.GetId(entity);
                var key = _idManager.GetFromId(entity.GetType(), id);
                toLoad[key.CouchDbId].Entity = entity;
            }

            return Task.CompletedTask;
        }
    }
}