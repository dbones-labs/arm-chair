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
namespace ArmChair
{
    using Commands;
    using EntityManagement;
    using Http;
    using IdManagement;
    using Processes.Commit;
    using Processes.Load;
    using Serialization;
    using Serialization.Newton;

    public class Settings
    {
        public Settings(string databaseName, Connection connection)
        {
            //global
            IdAccessor = new IdAccessor();
            IdManager = new ShortGuidIdManager();
            RevisionAccessor = new RevisionAccessor();
            Serializer = new Serializer(IdAccessor, RevisionAccessor);

            CouchDb = new CouchDb(databaseName, connection, Serializer);

            LoadPipeline = new LoadPipeline(CouchDb, IdManager, IdAccessor, RevisionAccessor);
            CommitPipeline = new CommitPipeline(CouchDb, IdManager, RevisionAccessor);
            
        }


        public IIdAccessor IdAccessor { get; private set; }
        public IIdManager IdManager { get; private set; }
        public IRevisionAccessor RevisionAccessor { get; private set; }
        public ISerializer Serializer { get; private set; }

        public LoadPipeline LoadPipeline { get; private set; }
        public CommitPipeline CommitPipeline { get; private set; }
        public CouchDb CouchDb { get; private set; }


    }
}