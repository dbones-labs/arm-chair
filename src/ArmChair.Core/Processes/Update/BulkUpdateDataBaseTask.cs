namespace ArmChair.Processes.Update
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Serialization;
    using Tasks;

    public class BulkUpdateDataBaseTask : IPipeTask<BulkContext>
    {
        private readonly Database _database;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        public BulkUpdateDataBaseTask(Database database, IRevisionAccessor revisionAccessor)
        {
            _database = database;
            _revisionAccessor = revisionAccessor;
        }

        public IEnumerable<BulkContext> Execute(IEnumerable<BulkContext> items)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)

            var entityUpdates = new Dictionary<string, object>();

            var request = new BulkDocsRequest();

            foreach (var bulkContext in items)
            {
                //var entry = new BulkDocRequest();
                //var idKey = _idManager.GetKeyFromId()
                //switch (bulkContext.ActionType)
                //{
                //    case ActionType.Add:
                //        entityAdds.Add(bulkContext.Entity);
                //        break;
                //    case ActionType.Attach:
                //        entityUpdates.Add(bulkContext.Key.ToString(), bulkContext.Entity);
                //        break;
                //    case ActionType.Delete:
                //        entityDeletes.Add(bulkContext.Entity);
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
            }
            //var updatePayLoad = new UpdatePayLoad()
            //{
            //    Creates = entityAdds,
            //    Updates = entityUpdates.Values,
            //    Deletes = entityDeletes
            //};

            //var updates = _database.BulkApplyChanges(updatePayLoad);
            ////TODO:here
            //foreach (var update in updates)
            //{
            //    var entity = entityUpdates[update.Id];
            //    _revisionAccessor.SetRevision(entity, update.Rev);
            //}

            //return items;
            return null;
        }
    }
}