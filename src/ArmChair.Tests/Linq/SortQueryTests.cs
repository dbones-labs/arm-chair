namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using Domain.Sample2;
    using Domain.Sample3;
    using Mapping;
    using NUnit.Framework;

    /// <summary>
    /// Todo: setup indexes before we can test.
    /// </summary>
    public class SortQueryTests : QueryTestCase
    {
        [Test]
        public void Simple_sort()
        {
            var idx = new IndexEntry();
            idx.Index.Add("name", Order.Asc);
            Database.Index.Create(idx);

            List<Animal> results;
            List<Animal> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Animal>().Where(x=> x.Name.Contains("o")).OrderBy(x => x.Name).ToList();
                reference = Query<Animal>().Where(x=> x.Name.Contains("o")).OrderBy(x => x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
        }

        [Test]
        public void Simple_sort_desc()
        {
            var idx = new IndexEntry();
            idx.Index.Add("name");
            Database.Index.Create(idx);

            List<Animal> results;
            List<Animal> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Animal>().OrderByDescending(x=>x.Name).ToList();
                reference = Query<Animal>().OrderByDescending(x=>x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
        }


        [Test]
        public void Enum_sort()
        {
            Database.Register(new []{ new TaskClassMap() });

            List<TodoTask> results;
            List<TodoTask> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<TodoTask>().OrderBy(x => x.Priority).ToList();
                reference = Query<TodoTask>().OrderBy(x => x.Priority).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Description == reference.First().Description);
            Assert.IsTrue(results.Skip(1).First().Description == reference.Skip(1).First().Description);
        }

        [Test]
        public void Enum_sort_descending()
        {
            Database.Register(new[] { new TaskClassMap() });

            List<TodoTask> results;
            List<TodoTask> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<TodoTask>().OrderByDescending(x => x.Priority).ToList();
                reference = Query<TodoTask>().OrderByDescending(x => x.Priority).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Description == reference.First().Description);
            Assert.IsTrue(results.Skip(1).First().Description == reference.Skip(1).First().Description);
        }



        [Test]
        public void Multiple_sorts()
        {
            Database.Index.Create<Person>(idx =>
            {
                idx.Field(p => p.BirthDate);
                idx.Field(p => p.Name);
            });


            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().OrderBy(x => x.BirthDate).ThenBy(x => x.Name).ToList();
                reference = Query<Person>().OrderBy(x => x.BirthDate).ThenBy(x => x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }


        [Test]
        public void Multiple_sorts_desc()
        {
            var idx = new IndexEntry();
            idx.Field("birthDate");
            idx.Field("name");
            Database.Index.Create(idx);


            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().OrderByDescending(x => x.BirthDate).ThenByDescending(x => x.Name).ToList();
                reference = Query<Person>().OrderByDescending(x => x.BirthDate).ThenByDescending(x => x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }


        [Test]
        [Ignore("Couchdb: Sorts currently only support a single direction for all fields.")]
        public void Multiple_sorts_desc2()
        {
            var idx = new IndexEntry();
            idx.Index.Add("birthDate");
            idx.Index.Add("name", Order.Desc);
            Database.Index.Create(idx);


            List<Person> results;
            List<Person> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().OrderByDescending(x => x.BirthDate).ThenBy(x => x.Name).ToList();
                reference = Query<Person>().OrderByDescending(x => x.BirthDate).ThenBy(x => x.Name).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Assert.IsTrue(results.Skip(1).First().Name == reference.Skip(1).First().Name);
            Assert.IsTrue(results.Skip(2).First().Name == reference.Skip(2).First().Name);
        }

    }
}