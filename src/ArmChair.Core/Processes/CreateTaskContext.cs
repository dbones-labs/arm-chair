// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
namespace ArmChair.Processes
{
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;

    public class CreateTaskContext
    {
        public CreateTaskContext(CouchDb couchDb,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor,
            ISessionCache sessionCache)
        {
            SessionCache = sessionCache;
            RevisionAccessor = revisionAccessor;
            IdManager = idManager;
            CouchDb = couchDb;
        }

        public CouchDb CouchDb { get; private set; }
        public IIdManager IdManager { get; private set; }
        public IRevisionAccessor RevisionAccessor { get; private set; }
        public ISessionCache SessionCache { get; private set; }
    }
}