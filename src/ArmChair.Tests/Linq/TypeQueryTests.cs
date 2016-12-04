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
            Console.WriteLine("1");
            List<Cat> results;
            List<Cat> reference;
            using (var session = Database.CreateSession())
            {
                Console.WriteLine("2");
                results = session.Query<Cat>().Where(x => x.RequiresHeatPad == true).ToList();
                reference = Query<Cat>().Where(x => x.RequiresHeatPad == true).ToList();
                Console.WriteLine("3");
            }
            Console.WriteLine("4");
            Assert.IsTrue(results.Count == reference.Count);
            Console.WriteLine("5");
            Assert.IsTrue(results.First().Name == reference.First().Name);
            Console.WriteLine("6");
        }
    }
}