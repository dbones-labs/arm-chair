namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using NUnit.Framework;

    public class CollectionQueryTests : QueryTestCase
    {
        [Test]
        public void Property_is_in_sequence()
        {
            List<Person> results;
            using (var session = Database.CreateSession())
            {
                var names = new[] { "dave", "chan" };
                results = session.Query<Person>().Where(x => names.Any(y => y == x.Name)).ToList();
            }

            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results.Any(x => x.Name == "dave"));
        }


        [Test]
        public void Property_is_in_sequence_2()
        {
            List<Person> results;
            using (var session = Database.CreateSession())
            {
                var names = new[] { "dave", "chan" };
                results = session.Query<Person>().Where(x => names.Any(y => x.Name == y)).ToList();
            }

            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results.Any(x => x.Name == "dave"));
        }

        [Test]
        public void Property_is_not_in_sequence()
        {
            List<Person> results;
            List<Person> expected;
            using (var session = Database.CreateSession())
            {
                var names = new[] { "dave", "chan" };
                results = session.Query<Person>().Where(x => names.All(y => x.Name != y)).ToList();
                expected = Query<Person>().Where(x => names.All(y => x.Name != y)).ToList();
            }

            Assert.IsTrue(results.Count == expected.Count);
            Assert.IsTrue(results.All(x => x.Name != "dave"));
        }


    }
}