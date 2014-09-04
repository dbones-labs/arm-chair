using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArmChair.Tests
{
    using Domain;
    using EntityManagement;
    using Http;
    using IdManagement;
    using InSession;
    using NUnit.Framework;
    using Processes.Load;
    using Processes.Update;
    using Tracking.Shadowing;

    [TestFixture]
    class Class1
    {
        [Test]
        public void Test1()
        {
            //global
            var idAccessor = new IdAccessor();
            var idManager = new SimpleIdManager(idAccessor);
            var revisionAccessor = new RevisionAccessor();
            var database = new Database("test_db", new Connection("http://192.168.1.79:5984/"), revisionAccessor);

            var loadPipeline = new LoadPipeline(database, idManager, revisionAccessor);
            var updatePipeline = new BulkPipeline(database, idManager, revisionAccessor);

            //session level
            var tracker = new ShadowTrackingProvider();
            var sessionCache = new SessionCache();

            var session = new Session(loadPipeline, updatePipeline, idManager, tracker, sessionCache);

            var p = new Person("dave");
            session.Add(p);

            var book = new Book("title", p);
            session.Add(book);

            session.Commit();
        }

    }
}
