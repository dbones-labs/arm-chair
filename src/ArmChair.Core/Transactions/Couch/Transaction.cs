namespace ArmChair.Transactions.Couch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using Exceptions;
    using Utils;

    public class Transaction : ITransaction
    {
        private readonly CouchDb _couchDb;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;
        private IList<BulkDocResponse> _results;
        private Dictionary<string, TransactionEntry> _txnEntries;
        private volatile bool _txnCleanedUp = false;
        private readonly string _id = ShortGuid.NewGuid();

        public Transaction(CouchDb couchDb, IIdAccessor idAccessor,  IRevisionAccessor revisionAccessor)
        {
            _couchDb = couchDb;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public void Init(IEnumerable<TransactionEntry> txnEntries)
        {
            _txnEntries = new Dictionary<string, TransactionEntry>();
            foreach (var txnEntry in txnEntries)
            {
                txnEntry.TransactionId = _id;
                _txnEntries.Add(txnEntry.ActualId, txnEntry);
            }
            
            var now = DateTime.UtcNow;

            bool HasExpired(DateTime time) => new TimeSpan(now.Ticks - time.Ticks).TotalSeconds > 5;

            var existingTransactionEntries = _couchDb
                .LoadAllEntities(
                    new AllDocsRequest()
                    {
                        Keys = _txnEntries.Values.Select(x=> x.Id) //checking against transaction key for documents
                    })
                .Rows
                .Select(x => x.Doc)
                .Cast<TransactionEntry>()
                .ToList();

            var hasTransactionLockConflict = existingTransactionEntries.Any(x => HasExpired(x.DateTime));

            if (hasTransactionLockConflict)
            {
                throw new TransactionException(existingTransactionEntries.Select(x => x.Id).ToList());
            }

            var payload = new BulkDocsRequest()
            {
                Docs = _txnEntries.Values.Select(
                    txn => new BulkDocRequest()
                    {
                        Id = txn.Id,
                        Content = txn
                    })
            };

            _results = _couchDb.BulkApplyChanges(payload).ToList();
            
            //now we have a txn (lock) we confirm the target docs are still on the same rev
            var conflictingDocs = _couchDb.LoadAllEntities(
                    new AllDocsRequest()
                    {
                        Keys = _txnEntries.Values.Select(x=> x.ActualId)
                    })
                .Rows
                .Select(x => x.Doc)
                .Select(x => new
                {
                    Id= _idAccessor.GetId(x).ToString(),
                    Rev = _revisionAccessor.GetRevision(x)?.ToString()
                })
                .Where(x =>
                {
                    if (!_txnEntries.TryGetValue(x.Id, out var entry)) return false;
                    return entry.ActualRev != x.Rev;
                })
                .ToList();

            if (conflictingDocs.Any())
            {
                throw new AggregateException(conflictingDocs.Select(x => new ConflictException(x.Id, x.Rev, "conflict", "stale reference")));
            }
        }

        public void CompleteCommit()
        {
            if (_txnCleanedUp) return;
            if (_results == null) return;

            var payload = new BulkDocsRequest()
            {
                Docs = _results
                    .Where(x => x.Error == null)
                    .Select(txn => new BulkDocRequest()
                    {
                        Id = txn.Id,
                        Rev = txn.Rev,
                        Delete = true
                    })
            };

            _couchDb.BulkApplyChanges(payload);

            _txnCleanedUp = true;
        }

        public void Rollback(Exception causeException)
        {
            try
            {
                CompleteCommit();
            }
            catch (Exception e)
            {
                throw new RollbackException(e, causeException);
            }
        }

        public void Dispose()
        {
            CompleteCommit();
        }
    }
}