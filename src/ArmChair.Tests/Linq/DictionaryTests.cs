namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample4;
    using NUnit.Framework;

    public class DictionaryTests : QueryTestCase
    {
        [Test]
        public void Contains_key()
        {
            List<Repoisitory> results;
            List<Repoisitory> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Repoisitory>().Where(x => x.Members.ContainsKey("dave")).ToList();
                reference = Query<Repoisitory>().Where(x => x.Members.ContainsKey("dave")).ToList();
            }
            Assert.IsTrue(results.Count == reference.Count);
            Assert.AreEqual(results.First().Name, reference.First().Name);
        }

        [Test]
        public void Does_not_Contains_key()
        {
            List<Repoisitory> results;
            List<Repoisitory> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Repoisitory>().Where(x => !x.Members.ContainsKey("dave")).ToList();
                reference = Query<Repoisitory>().Where(x => !x.Members.ContainsKey("dave")).ToList();
            }
            Assert.IsTrue(results.Count == reference.Count);
            Assert.AreEqual(results.First().Name, reference.First().Name);
        }

        [Test]
        public void Key_value_equals_with_a_non_alpha_key()
        {
            List<Repoisitory> results;
            List<Repoisitory> reference;
            
            var repo = new Repoisitory("grr");
            repo.Members.Add("(45\"£$", Access.Administrator);

            this.ReferenceItems.Add(repo);

            using (var session = Database.CreateSession())
            {
                session.Add(repo);
                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                results = session.Query<Repoisitory>().Where(x => x.Members["(45\"£$"] == Access.Administrator).ToList();
                reference = Query<Repoisitory>().Where(x => x.Members["(45\"£$"] == Access.Administrator).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.AreEqual(results.First().Name, reference.First().Name);
        }
    }
}
