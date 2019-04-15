namespace ArmChair.Transactions.Couch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;

    public class TransactionCoordinator : ITransactionCoordinator
    {
        private readonly CouchDb _couchDb;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        public TransactionCoordinator(CouchDb couchDb, IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public ITransaction Setup(IEnumerable<object> items)
        {
            var txnEntries = items.Select(x =>
            {
                var type = x.GetType().Name;
                var id = _idAccessor.GetId(x)?.ToString();
                var rev = _revisionAccessor.GetRevision(x)?.ToString();
                var txn = new TransactionEntry()
                {
                    DateTime = DateTime.UtcNow,
                    Id = $"{id}-{type}",
                    ActualId = id,
                    ActualRev = rev,
                    Type = type
                };
                return txn;
            });

            var transaction = new Transaction(_couchDb, _idAccessor, _revisionAccessor);
            try
            {
                transaction.Init(txnEntries);
            }
            catch (Exception e)
            {
                transaction.Rollback(e);
                throw;
            }
            
            return transaction;
        }
    }
}