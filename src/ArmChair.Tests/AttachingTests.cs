namespace ArmChair.Tests
{
    using System.Linq;
    using Domain;
    using NUnit.Framework;

    public class AttachingTests : TestCase
    {
        [Test]
        public void Simple_Attach()
        {
            Book book;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                book = new Book("Epic book", author);
                session.Add(book);

                session.Commit();
            }

            //we have more information to associate with the book
            using (var session = Database.CreateSession())
            {
                session.Attach(book);

                var contibutor = new Person("bob");
                session.Add(contibutor);

                book.AddContributor(contibutor, ContributorType.Editor);

                session.Commit();
            }


            using (var session = Database.CreateSession())
            {
                var loadedBook = session.GetById<Book>(book.Id);

                Assert.AreEqual(loadedBook.Contributors.Count(), 2);
                Assert.IsTrue(loadedBook.Contributors.Any(x => x.Name == "bob"));
                Assert.IsTrue(loadedBook.Contributors.Any(x => x.Name == "dave"));
            }
        }
    }
}