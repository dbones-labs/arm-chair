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
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using Http;
    using NUnit.Framework;


    /// <summary>
    /// base for all tests should be this class
    /// </summary>
    [TestFixture]
    public abstract class TestCase
    {
        protected Database Database;
        protected string DbName = "auto_testing";
        protected string DbLocation = "http://document:5984";
        protected WebProxy Proxy = false ? new WebProxy("127.0.0.1", 8081) : null;
        //127.0.0.1:8081
        //8888

        public virtual Database CreateDatabase()
        {
            var conn = new Connection(DbLocation);
            conn.SetupConfig(cfg => cfg.Proxy = Proxy);
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
            var conn = new Connection(DbLocation);
            var createDb = new Request("/:db", HttpMethod.Put);
            createDb.AddUrlSegment("db", DbName);
            createDb.SetContentType(ContentType.Json);
            using (var response = conn.Execute(createDb))
            {
                Debug.WriteLine(response.Status);
            };

            var wait = true;
            var confirmDb = new Request("/:db", HttpMethod.Get);
            confirmDb.AddUrlSegment("db", DbName);
            confirmDb.SetContentType(ContentType.Json);

            EventallyRun();

            while (wait)
            {
                using(var response = conn.Execute(confirmDb))
                {
                    wait = response.Status != HttpStatusCode.OK;
                }
            }

            Database = CreateDatabase();
            OnSetup();
        }

        protected virtual void OnSetup() { }


        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            EnsureDbIsDeleted();
        }

        protected virtual void OnTearDown() { }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            OnFixtureTearDown();
        }

        private void EnsureDbIsDeleted()
        {
            //delete a test db
            var conn = new Connection(DbLocation);

            bool needToDelete = true;

            while (needToDelete)
            {
                var checkDb = new Request("/:db", HttpMethod.Get);
                checkDb.AddUrlSegment("db", DbName);
                checkDb.SetContentType(ContentType.Json);
                using(var response = conn.Execute(checkDb))
                {
                    needToDelete = response.Status == HttpStatusCode.OK;
                };

                if (needToDelete)
                {
                    var deleteDb = new Request("/:db", HttpMethod.Delete);
                    deleteDb.AddUrlSegment("db", DbName);
                    deleteDb.SetContentType(ContentType.Json);
                    using (var response = conn.Execute(deleteDb)) { };
                }
            }

        }


        protected virtual void OnFixtureTearDown() { }
    }
}
