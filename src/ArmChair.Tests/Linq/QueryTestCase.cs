namespace ArmChair.Tests.Linq
{
    using System;
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using Domain.Sample2;
    using EntityManagement.Config;

    public abstract class QueryTestCase : TestCase
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            SetupMaps();
            KnownTestDataGenericTestData();
        }

        public virtual void SetupMaps()
        {
            var types = typeof(Person).Assembly.GetTypes().Where(x => typeof(EntityRoot).IsAssignableFrom(x)).ToList();
            Database.Settings.Register(types);
        }
       

        public virtual void KnownTestDataGenericTestData()
        {
            using (var session = Database.CreateSession())
            {

                //add some domain 1
                var dave = new Person("dave") { Id = "p1" };
                var chan = new Person("chan") { Id = "p2" };

                var book = new Book("using couchdb", dave) { Id = "b1" };
                book.AddEdition(new Edition("1st", EditionType.Electronic) { ReleaseDate = DateTime.Now });
                book.AddEdition(new Edition("1st", EditionType.HardBack) { ReleaseDate = DateTime.Now.AddDays(3) });
                book.AddEdition(new Edition("preview", EditionType.Electronic) { ReleaseDate = DateTime.Now.AddDays(-10) });

                //add some domain 2
                var lealoo = new Cat { Id = "c1", Name = "Lealoo", RequiresHeatPad = true };
                var bonnie = new Dog { Id = "d1", Name = "Bonnie", NumberOfWalksPerDay = 2 };

                var booking = new KennelBooking() { Animal = lealoo, Start = DateTime.Now, End = DateTime.Now.AddDays(2), Id = "bk1" };

                session.Add(dave);
                session.Add(chan);
                session.Add(book);
                session.Add(lealoo);
                session.Add(bonnie);
                session.Add(booking);
                session.Commit();
            }
        }
    }
}