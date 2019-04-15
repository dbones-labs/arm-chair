// Copyright 2014 - dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ArmChair.Middleware.Commit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using EntityManagement;
    using Exceptions;
    using Processes.Commit;
    using Processes.Load;
    using Utils;

    public class TransactionAction : IAction<IEnumerable<CommitContext>>
    {
        private readonly ITransactionCoordinator _transactionCoordinator;

        public TransactionAction(ITransactionCoordinator transactionCoordinator)
        {
            _transactionCoordinator = transactionCoordinator;
        }


        public async Task Execute(IEnumerable<CommitContext> context, Next<IEnumerable<CommitContext>> next)
        {
            context = context.ToList();
            using (var txn = _transactionCoordinator.Setup(context.Select(x => x.Entity)))
            {
                try
                {
                    await next(context);
                    txn.CompleteCommit();
                }
                catch (Exception e)
                {
                    txn.Rollback(e);
                    throw;
                }
            }
        }
    }

    public interface ITransactionCoordinator
    {
        ITransaction Setup(IEnumerable<object> items);
    }

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

    public interface ITransaction : IDisposable
    {
        void Init(IEnumerable<TransactionEntry> txnEntries);
        void CompleteCommit();
        void Rollback(Exception causeException);
    }

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


    public class TransactionEntry
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string ActualId { get; set; }
        public string ActualRev { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string TransactionId { get; set; }
    }
}