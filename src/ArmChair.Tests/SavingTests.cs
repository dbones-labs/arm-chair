namespace ArmChair.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using NUnit.Framework;

    public class SavingTests : TestCase
    {
        [Test]
        public void Update_id_and_rev()
        {
            string id;
            string rev;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                id = author.Id;

                session.Commit();

                rev = author.Rev;
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(rev));
        }


        [Test]
        public void Many_saves()
        {
            var people = new List<Person>();
            var loadedPeople = new List<Person>();

            using (var session = Database.CreateSession())
            {
                for (int i = 0; i < 50; i++)
                {
                    var person = new Person("dave");
                    session.Add(person);
                    people.Add(person);
                }
                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                var temp = session.GetByIds<Person>(people.Select(x => x.Id));
                loadedPeople.AddRange(temp);
            }

            Assert.AreEqual(loadedPeople.Count, people.Count, "did not save/load all people");
            Assert.IsTrue(loadedPeople.All(x => people.Any(y => y.Id == x.Id)), "ids where not handled corretly");
            Assert.IsTrue(loadedPeople.All(x => people.Any(y => y.Rev == x.Rev)), "revisions where not handled corretly");
        }
    }



    public class LoadingTests : TestCase
    {
        [Test]
        public void Try_load_object_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.GetById<Person>("123");
                Assert.IsNull(person);
            }
        }


        [Test]
        public void Try_load_objects_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var ids = new List<string> {"123", "12323123"};
                var people = session.GetByIds<Person>(ids);
                Assert.IsFalse(people.Any());
            }
        }



        
    }
}