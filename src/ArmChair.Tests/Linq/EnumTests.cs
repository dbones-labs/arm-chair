namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample3;
    using NUnit.Framework;

    public class EnumTests : QueryTestCase
    {
        [Test]
        public void Equals_a_enum()
        {
            List<TodoTask> results;
            List<TodoTask> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<TodoTask>().Where(x => x.Priority == PriorityLevel.Medium).ToList();
                reference = Query<TodoTask>().Where(x => x.Priority == PriorityLevel.Medium).ToList();
            }
            Assert.IsTrue(results.Count == reference.Count);
            Assert.AreEqual(results.First().Description, reference.First().Description);
        }
    }
}