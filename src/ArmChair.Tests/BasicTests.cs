namespace ArmChair.Tests
{
    using Domain;
    using NUnit.Framework;

    public class BasicTests: TestCase
    {
        [Test]
        public void Save_into_database()
        {
            string authorId;

            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                authorId = author.Id;

                session.Commit();    
            }

            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(authorId);

                Assert.AreEqual(author.Name, "dave");
            }
        }


       


    }
}