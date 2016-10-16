namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using Domain.Sample2;
    using NUnit.Framework;

    public class TypeQueryTests : QueryTestCase
    {
        [Test]
        public void Type_set_via_queryable_type()
        {
            List<Person> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().ToList();
            }

            Assert.IsTrue(results.Count == 2);
        }

        [Test]
        public void Object_type_set_via_queryable_type()
        {
            List<object> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<object>().ToList();
            }

            Assert.IsTrue(results.Count == 6);
        }
    }
}