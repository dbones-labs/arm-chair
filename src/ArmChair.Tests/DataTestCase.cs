namespace ArmChair.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using Domain.Sample2;
    using Domain.Sample3;
    using Domain.Sample4;
    using Domain.Sample5;

    /// <summary>
    /// provides some known data in the database.
    /// </summary>
    public class DataTestCase : TestCase
    {
        public List<object> ReferenceItems { get; set; }

        public IEnumerable<T> Query<T>()
        {
            return ReferenceItems.Where(x => x is T).Cast<T>();
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            KnownTestDataGenericTestData();
        }

        public virtual void KnownTestDataGenericTestData()
        {
            ReferenceItems = new List<object>();
            using (var session = Database.CreateSession())
            {

                //add some domain 1
                var dave = new Person("dave") { Id = "p1", BirthDate = new DateTime(1982,1,1) };
                var chan = new Person("chan") { Id = "p2", BirthDate = new DateTime(1983, 1, 1) };
                var pam = new Person("pam") { Id = "p3", BirthDate = new DateTime(1972, 1, 1) };
                var john = new Person("john") { Id = "p4", BirthDate = new DateTime(1972, 1, 1) };
                var max = new Person("max") { Id = "p5", BirthDate = new DateTime(1972, 1, 1) };

                var book = new Book("using couchdb", dave, 50) { Id = "b1" };
                book.AddEdition(new Edition("2nd", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(33) });
                book.AddEdition(new Edition("1st", EditionType.Electronic) { ReleaseDate = DateTime.Now });
                book.AddEdition(new Edition("1st", EditionType.HardBack) { ReleaseDate = DateTime.Now.AddDays(3) });
                book.AddEdition(new Edition("preview", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(-10) });

                var book2 = new Book("being awesome", dave, 51) { Id = "b2" };
                book2.AddEdition(new Edition("1st", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(50) });
                book2.AddEdition(new Edition("preview", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(-30) });
                book2.AddContributor(pam, ContributorType.Editor);
                book2.AddContributor(chan, ContributorType.CoAuthor);

                var book3 = new Book("being epic with couchdb", chan, 100) { Id = "b3" };
                book3.AddEdition(new Edition("1st", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(-10) });

                //add some domain 2
                var leeloo = new Cat { Id = "c1", Name = "leeloo", RequiresHeatPad = true };
                var robbie = new Cat { Id = "c2", Name = "robbie", RequiresHeatPad = false };
                var bonnie = new Dog { Id = "d1", Name = "bonnie", NumberOfWalksPerDay = 2 };
                var starfire = new Dog { Id = "d2", Name = "starfire", NumberOfWalksPerDay = 1 };

                var booking = new KennelBooking() { Animal = leeloo, Start = DateTime.Now, End = DateTime.Now.AddDays(2), Id = "bk1" };

                //domain 3
                var todo1 = new TodoTask("write an armchair example", PriorityLevel.Medium) {Id = "t1" };
                var todo2 = new TodoTask("inital convert armchair to support .net core", PriorityLevel.High) {Id = "t2", IsComplete = true };

                //domain 4
                var repo = new Repoisitory("armchair") { Members = new Dictionary<string, Access>() { { "dave", Access.Administrator }, { "bob", Access.Contributor } } };
                var repo2 = new Repoisitory("awesome-source") { Members = new Dictionary<string, Access>() { { "bob", Access.Administrator } } };

                //domain 5
                var serviceEntry1 = new ServiceEntry { Id = "se1", Name = "SqlServer", Description = "a sql server database", Tags = new List<string>(new []{"Database", "Sql", "Relational"})};
                var serviceEntry2 = new ServiceEntry { Id = "se2", Name = "CouchDb", Description = "a document server database", Tags = new List<string>(new []{"Database", "Document" })};
                var serviceEntry3 = new ServiceEntry { Id = "se3", Name = "PostgreSql", Description = "a relational and document server database", Tags = new List<string>(new []{"Database", "Document", "Relational", "Sql" })};
                
                ReferenceItems.Add(dave);
                ReferenceItems.Add(chan);
                ReferenceItems.Add(pam);
                ReferenceItems.Add(john);
                ReferenceItems.Add(max);
                ReferenceItems.Add(book);
                ReferenceItems.Add(book2);
                ReferenceItems.Add(book3);
                ReferenceItems.Add(leeloo);
                ReferenceItems.Add(robbie);
                ReferenceItems.Add(bonnie);
                ReferenceItems.Add(starfire);
                ReferenceItems.Add(booking);
                ReferenceItems.Add(todo1);
                ReferenceItems.Add(todo2);
                ReferenceItems.Add(repo);
                ReferenceItems.Add(repo2);
                ReferenceItems.Add(serviceEntry1);
                ReferenceItems.Add(serviceEntry2);
                ReferenceItems.Add(serviceEntry3);

                session.AddRange(ReferenceItems.Cast<EntityRoot>());
                session.Commit();
            }
        }
    }
}