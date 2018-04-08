namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using NUnit.Framework;

    public class NumberOperatorQueryTests : QueryTestCase
    {
        [Test]
        public void Greater_than_query()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.NumberOfPages > 50).ToList();
                referenceResults = Query<Book>().Where(x => x.NumberOfPages > 50).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

        [Test]
        public void Greater_than_or_equal_query()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.NumberOfPages >= 50).ToList();
                referenceResults = Query<Book>().Where(x => x.NumberOfPages >= 50).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

        [Test]
        public void Less_than_or_equal_query()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.NumberOfPages <= 51).ToList();
                referenceResults = Query<Book>().Where(x => x.NumberOfPages <= 51).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

        [Test]
        public void Less_than_query()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.NumberOfPages < 51).ToList();
                referenceResults = Query<Book>().Where(x => x.NumberOfPages < 51).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

        [Test]
        public void Equals_query()
        {
            List<Book> results;
            List<Book> referenceResults;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.NumberOfPages == 51).ToList();
                referenceResults = Query<Book>().Where(x => x.NumberOfPages == 51).ToList();
            }

            Assert.AreEqual(results.Count, referenceResults.Count);
        }

    }
}