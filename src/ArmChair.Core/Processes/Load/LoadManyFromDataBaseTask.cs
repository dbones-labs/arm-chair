namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using IdManagement;
    using Tasks;

    public class LoadManyFromDataBaseTask : IPipeTask<LoadContext>
    {
        private readonly Database _database;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;


        public LoadManyFromDataBaseTask(Database database, IIdManager idManager, IIdAccessor idAccessor)
        {
            _database = database;
            _idManager = idManager;
            _idAccessor = idAccessor;
        }

        public IEnumerable<LoadContext> Execute(IEnumerable<LoadContext> items)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)
            var toload = items.Where(x => !x.LoadedFromCache).ToDictionary(loadContext => loadContext.Key.CouchDbId);
            var allDocs = _database.LoadAllEntities(new AllDocsRequest() {Keys = toload.Keys});

            //using the loaded docs, update the context entries.
            foreach (var entity in allDocs.Rows.Select(x=>x.Doc))
            {
                var id = _idAccessor.GetId(entity);
                var key = _idManager.GetFromId(entity.GetType(), id);
                toload[key.CouchDbId].Entity = entity;
            }

            return items;
        }
    }
}