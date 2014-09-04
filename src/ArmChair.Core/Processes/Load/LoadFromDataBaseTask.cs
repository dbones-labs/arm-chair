namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using Tasks;

    public class LoadFromDataBaseTask : PipeItemTask<LoadContext>
    {
        private readonly Database _database;

        public LoadFromDataBaseTask(Database database)
        {
            _database = database;
        }

        public override bool CanHandle(LoadContext item)
        {
            return !item.LoadedFromCache;
        }

        public override IEnumerable<LoadContext> Execute(LoadContext item)
        {
            var entity = _database.LoadEntity(item.Key.ToString());
            item.Entity = entity;
            yield return item;
        }
    }
}