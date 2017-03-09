namespace ArmChair.Tests.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using Domain.Sample2;
    using NUnit.Framework;

    /// <summary>
    /// Todo: setup indexes before we can test.
    /// </summary>
    [Ignore("require setting up of indexes")]
    public class SortQueryTests : QueryTestCase
    {
        [Test]
        public void Simple_sort()
        {
            List<Animal> results;
            List<Animal> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Animal>().OrderBy(x => x.Name).ToList();
                reference = Query<Animal>().OrderBy(x => x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
        }

        [Test]
        public void Simple_sort_desc()
        {
            Func<IEnumerable<Animal>, List<Animal>> filter =
                items => items.OrderByDescending(x => x.Name).ToList();

            List<Animal> results;
            List<Animal> reference;
            using (var session = Database.CreateSession())
            {
                results = filter(session.Query<Animal>());
                reference = filter(Query<Animal>());
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
        }


        [Test]
        public void Multiple_sorts()
        {
            Func<IEnumerable<Person>, List<Person>> filter =
                items => items.OrderBy(x => x.BirthDate).ThenBy(x => x.Name).ToList();

            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = filter(session.Query<Person>());
                reference = filter(Query<Person>());
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }


        [Test]
        public void Multiple_sorts_desc()
        {
            Func<IEnumerable<Person>, List<Person>> filter =
                items => items.OrderBy(x => x.BirthDate).ThenByDescending(x => x.Name).ToList();

            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = filter(session.Query<Person>());
                reference = filter(Query<Person>());
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }


        [Test]
        public void Multiple_sorts_desc2()
        {
            Func<IEnumerable<Person>, List<Person>> filter =
                items => items.OrderByDescending(x => x.BirthDate).ThenBy(x => x.Name).ToList();

            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = filter(session.Query<Person>());
                reference = filter(Query<Person>());
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }

    }
}