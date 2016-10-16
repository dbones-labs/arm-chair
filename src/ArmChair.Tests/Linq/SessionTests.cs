namespace ArmChair.Tests.Linq
{
    using System.Linq;
    using Domain;
    using NUnit.Framework;

    public class SessionTests : TestCase
    {
        [Test]
        public void Item_already_in_context()
        {
            using (var session = Database.CreateSession())
            {
                var dave = new Person("dave") { Id = "1" };
                session.Add(dave);

                var chan = new Person("chan") { Id = "2" };
                session.Add(chan);

                session.Commit();
            }

            {
                Person dave;
                Person dave2;

                using (var session = Database.CreateSession())
                {
                    //note we want to make a change
                    dave = session.GetById<Person>("1");
                    dave.Name = "David";

                    //note the db still has the value as dave, at this point
                    dave2 = session.Query<Person>().First(x => x.Name == "dave");
                }

                Assert.IsTrue(dave == dave2);
            }
        }
    }
}