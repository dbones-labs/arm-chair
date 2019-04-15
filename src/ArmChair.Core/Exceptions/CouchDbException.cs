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

namespace ArmChair.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class BulkException : Exception
    {
        public IEnumerable<CouchDbException> Exceptions { get; }

        public BulkException(IEnumerable<CouchDbException> exceptions)
        {
            Exceptions = exceptions;
        }
    }

    public class RollbackException : Exception
    {
        public Exception RollbackCause { get; }
        public Exception TransactionCause { get; }

        public RollbackException(Exception rollbackCause, Exception transactionCause)
        {
            RollbackCause = rollbackCause;
            TransactionCause = transactionCause;
        }
    }
    
    public class TransactionException : Exception
    {
        public List<string> Ids { get; }

        public TransactionException(List<string> ids)
        {
            Ids = ids;
        }
    }

    public class CouchDbException : Exception
    {
        public string Id { get; }
        public string Rev { get; }
        public string Error { get; }
        public string Reason { get; }

        
        
        public CouchDbException(string id, string rev, string error, string reason)
        {
            Id = id;
            Rev = rev;
            Error = error;
            Reason = reason;
        }    
    }

    public class ConflictException : CouchDbException
    {
        public ConflictException(string id, string rev, string error, string reason) : base(id, rev, error, reason)
        {
        }
    }
}