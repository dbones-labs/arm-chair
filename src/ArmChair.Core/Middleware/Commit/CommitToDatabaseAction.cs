namespace ArmChair.Middleware.Commit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using InSession;

    /// <summary>
    /// this will send the correct command which will update the database
    /// </summary>
    public class CommitToDatabaseAction : IAction<IEnumerable<CommitContext>>
    {
        private readonly CouchDb _couchDb;
        private readonly IRevisionAccessor _revisionAccessor;

        public CommitToDatabaseAction(CouchDb couchDb, IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _revisionAccessor = revisionAccessor;
        }

        public Task Execute(IEnumerable<CommitContext> items, Next<IEnumerable<CommitContext>> next)
        {
            items = items.ToList(); //ensure 1 iteration over list. (tasks to run once)

            //do not call the db if we have no items to transact on.
            if (!items.Any()) return Task.CompletedTask;

            //setup the items for the database commit.
            var entityUpdates = new Dictionary<string, object>();
            var docRequests = new List<BulkDocRequest>();

            foreach (var bulkContext in items)
            {
                var entry = new BulkDocRequest
                {
                    Content = bulkContext.Entity,
                    Id = bulkContext.Key.CouchDbId
                };

                switch (bulkContext.ActionType)
                {
                    case ActionType.Add:
                        break;
                    case ActionType.Update:
                        break;
                    case ActionType.Delete:
                        entry.Rev = (string) _revisionAccessor.GetRevision(bulkContext.Entity);
                        entry.Delete = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                docRequests.Add(entry);
                entityUpdates.Add(entry.Id, bulkContext.Entity);
            }

            //apply the commit
            var request = new BulkDocsRequest {Docs = docRequests};
            var updates = _couchDb.BulkApplyChanges(request);

            //update any revisions!
            foreach (var update in updates)
            {
                var entity = entityUpdates[update.Id];
                _revisionAccessor.SetRevision(entity, update.Rev);
            }
            
            return Task.CompletedTask;

        }
    }
}