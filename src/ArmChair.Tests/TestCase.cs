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
namespace ArmChair.Tests
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Http;
    using NUnit.Framework;


    [TestFixture]
    public abstract class TestCase
    {
        protected Database Database;
        protected string DbName = "auto_testing";
        protected string DbLocation = "http://document:5984";
        protected WebProxy Proxy = false ? new WebProxy("127.0.0.1", 8888) : null;


        public virtual Database CreateDatabase()
        {
            var conn = new Connection(DbLocation) {Proxy = Proxy};
            return new Database(DbName, conn);
        }

        /// <summary>
        /// while running couch 2.0 preview, we had some tests which would
        /// fail if we ran too fast.
        /// 
        /// this is a hack to slow us down between action and assert.
        /// </summary>
        public virtual void EventallyRun()
        {
            Thread.Sleep(750);
        }

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            OnFixtureSetup();
        }

        protected virtual void OnFixtureSetup() { }

        [SetUp]
        public void Setup()
        {
            DbName = TestContext.CurrentContext.Test.FullName.ToLower().Replace(".","-").Trim();
            EnsureDbIsDeleted();

            //create a test db
            var conn = new Connection(DbLocation) { Proxy = Proxy };
            var createDb = new Request("/:db", HttpVerbType.Put);
            createDb.AddUrlSegment("db", DbName);
            createDb.SetContentType(HttpContentType.Json);
            conn.Execute(createDb, response => { });

            var wait = true;
            var confirmDb = new Request("/:db", HttpVerbType.Get);
            confirmDb.AddUrlSegment("db", DbName);
            confirmDb.SetContentType(HttpContentType.Json);

            EventallyRun();

            while (wait)
            {
                conn.Execute(confirmDb, response =>
                {
                    wait = response.Status != HttpStatusCode.OK;
                });
            }

            Database = CreateDatabase();
            OnSetup();
        }

        protected virtual void OnSetup() { }


        [TearDown]
        public void TearDown()
        {
            OnTearDown();
        }

        protected virtual void OnTearDown() { }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            OnFixtureTearDown();
            EnsureDbIsDeleted();
        }

        private void EnsureDbIsDeleted()
        {
            //delete a test db
            var conn = new Connection(DbLocation) { Proxy = Proxy };

            bool needToDelete = true;

            while (needToDelete)
            {
                var checkDb = new Request("/:db", HttpVerbType.Get);
                checkDb.AddUrlSegment("db", DbName);
                checkDb.SetContentType(HttpContentType.Json);
                conn.Execute(checkDb, response =>
                {
                    needToDelete = response.Status == HttpStatusCode.OK;
                });

                if (needToDelete)
                {
                    var deleteDb = new Request("/:db", HttpVerbType.Delete);
                    deleteDb.AddUrlSegment("db", DbName);
                    deleteDb.SetContentType(HttpContentType.Json);
                    conn.Execute(deleteDb, response => { });
                }
            }

        }


        protected virtual void OnFixtureTearDown() { }
    }
}
