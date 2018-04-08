namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using NUnit.Framework;


    public class StringQueryTests : QueryTestCase
    {
        [Test]
        public void Not_equal_to_dave_should_return_eveyone_but_dave()
        {
            List<Person> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().Where(x => x.Name != "dave").ToList();
            }

            Assert.AreEqual(results.Count, 4);
            Assert.IsTrue(results.All(x => x.Name != "dave"));
        }

        [Test]
        public void Ends_with()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.Title.EndsWith("couchdb")).ToList();
                referenceResults = Query<Book>().Where(x => x.Title.EndsWith("couchdb")).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

        [Test]
        public void Starts_with()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.Title.StartsWith("using")).ToList();
                referenceResults = Query<Book>().Where(x => x.Title.StartsWith("using")).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }


        [Test]
        public void Contains()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.Title.Contains("epic")).ToList();
                referenceResults = Query<Book>().Where(x => x.Title.Contains("epic")).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }


    }
}