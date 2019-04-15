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
    using Transactions;

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
}