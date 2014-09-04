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
            var entities = _database.LoadAllEntities(toload.Keys);

            foreach (var entity in entities)
            {
                var key = _idManager.GetId(entity);
                toload[key.ToString()].Entity = entity;
            }

            return items;
        }
    }
}