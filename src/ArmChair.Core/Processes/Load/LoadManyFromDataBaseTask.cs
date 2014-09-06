namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using System.Linq;
    using IdManagement;
    using Tasks;

    public class LoadManyFromDataBaseTask : IPipeTask<LoadContext>
    {
        private readonly Database _database;
        private readonly IIdManager _idManager;


        public LoadManyFromDataBaseTask(Database database, IIdManager idManager)
        {
            _database = database;
            _idManager = idManager;
        }

        public IEnumerable<LoadContext> Execute(IEnumerable<LoadContext> items)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)
            var toload = items.Where(x => !x.LoadedFromCache).ToDictionary(loadContext => loadContext.Key.ToString());
            var allDocs = _database.LoadAllEntities(new AllDocsRequest() {Keys = toload.Keys});

            //only interested in the doc
            foreach (var entity in allDocs.Rows.Select(x=>x.Doc))
            {
                var key = _idManager.GetId(entity);
                toload[key.ToString()].Entity = entity;
            }

            return items;
        }
    }
}