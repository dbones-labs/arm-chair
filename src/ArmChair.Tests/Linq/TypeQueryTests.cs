namespace ArmChair.Tests.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            Assert.IsTrue(results.Count == Query<Person>().Count());
        }

        [Test]
        public void Object_type_set_via_queryable_type()
        {
            List<object> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<object>().ToList();
            }

            Assert.IsTrue(results.Count == ReferenceItems.Count);
        }

        [Test]
        public void Inherited_type_property()
        {
            List<Cat> results;
            List<Cat> reference;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Cat>().Where(x => x.RequiresHeatPad == true).ToList();
                reference = Query<Cat>().Where(x => x.RequiresHeatPad == true).ToList();
            }

            Assert.IsTrue(results.Count == reference.Count);
            Assert.IsTrue(results.First().Name == reference.First().Name);
        }
    }
}