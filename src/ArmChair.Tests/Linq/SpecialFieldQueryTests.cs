namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using NUnit.Framework;

    public class SpecialFieldQueryTests : QueryTestCase
    {
        [Test]
        public void Query_By_Id()
        {
            List<EntityRoot> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<EntityRoot>().Where(x=>x.Id == "p1").ToList();
            }

            var result = results.Cast<Person>().FirstOrDefault();
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Name == "dave");
        }

        
    }
}