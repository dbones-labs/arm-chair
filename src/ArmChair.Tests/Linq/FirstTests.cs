namespace ArmChair.Tests.Linq
{
    using System.Linq;
    using Domain.Sample1;
    using NUnit.Framework;

    public class FirstTests : QueryTestCase
    {
        [Test]
        public void First_default_where_name_is_not_in_db()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.Query<Person>().FirstOrDefault(x => x.Name == "Dave");
                Assert.IsNull(person);
            }
        }

        [Test]
        public void First_default_where_name_is_in_db()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.Query<Person>().FirstOrDefault(x => x.Name == "dave");
                Assert.AreEqual(person?.Name, "dave");
            }
        }
    }
}