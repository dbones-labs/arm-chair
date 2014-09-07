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
            var docRequests = new List<BulkDocRequest>();

            foreach (var bulkContext in items)
            {
                var entry = new BulkDocRequest {Id = bulkContext.Key.CouchDbId};
                
                switch (bulkContext.ActionType)
                {
                    case ActionType.Add:
                        break;
                    case ActionType.Update:
                        entityUpdates.Add(bulkContext.Key.ToString(), bulkContext.Entity);
                        break;
                    case ActionType.Delete:
                        entry.Rev = (string)_revisionAccessor.GetRevision(bulkContext.Entity);
                        entry.Delete = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                docRequests.Add(entry);
                entityUpdates.Add(entry.Id, bulkContext.Entity);
            }

            var request = new BulkDocsRequest {Docs = docRequests};

            var updates = _database.BulkApplyChanges(request);
            //update any revisions!
            foreach (var update in updates)
            {
                var entity = entityUpdates[update.Id];
                _revisionAccessor.SetRevision(entity, update.Rev);
            }

            return items;
            
        }
    }
}