namespace ArmChair.Processes.Load
{
    using System.Collections.Generic;
    using Tasks;

    public class LoadFromDataBaseMapTask : PipeItemMapTask<LoadContext>
    {
        private readonly Database _database;

        public LoadFromDataBaseMapTask(Database database)
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