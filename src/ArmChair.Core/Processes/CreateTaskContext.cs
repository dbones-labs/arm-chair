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
namespace ArmChair.Processes
{
    using Commands;
    using EntityManagement;
    using IdManagement;
    using InSession;

    /// <summary>
    /// when an instance of a task is created, this class will be avaliable
    /// the purpose is to supply the task with some core functionality
    /// </summary>
    public class CreateTaskContext
    {
        public CreateTaskContext(
            CouchDb couchDb,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor,
            ISessionCache sessionCache)
        {
            SessionCache = sessionCache;
            RevisionAccessor = revisionAccessor;
            IdManager = idManager;
            CouchDb = couchDb;
        }

        /// <summary>
        /// couchdb commands
        /// </summary>
        public CouchDb CouchDb { get; private set; }

        /// <summary>
        /// access to the id manager
        /// </summary>
        public IIdManager IdManager { get; private set; }

        /// <summary>
        /// access to the revision accessor
        /// </summary>
        public IRevisionAccessor RevisionAccessor { get; private set; }

        /// <summary>
        /// the session cache
        /// </summary>
        public ISessionCache SessionCache { get; private set; }
    }
}