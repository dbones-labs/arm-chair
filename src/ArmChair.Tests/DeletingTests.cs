namespace ArmChair.Tests
{
    using Domain;
    using NUnit.Framework;

    public class DeletingTests : TestCase
    {
        [Test]
        public void Simple_delete()
        {
            string id;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);
                id = author.Id;
                session.Commit();
            }


            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(id);
                session.Remove(author);
                
                session.Commit();
            }


            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(id);

                Assert.IsNull(author);
            }
        }
    }
}