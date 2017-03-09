namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using Domain.Sample2;
    using NUnit.Framework;


    public class SubProperyQueryTests : QueryTestCase
    {
        [Test]
        public void Single_object_property()
        {
            List<KennelBooking> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<KennelBooking>().Where(x => x.Animal.Name == "leeloo").ToList();
            }

            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results.First().Id == "bk1");
        }


        [Test]
        public void Collection_object_property()
        {
            List<Book> results;
            List<Book> expected;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.Editions.Any(y=> y.Name == "2nd")).ToList();
                expected = Query<Book>().Where(x => x.Editions.Any(y => y.Name == "2nd")).ToList();
            }

            Assert.IsTrue(results.Count == expected.Count);
            Assert.IsTrue(results.Any(x=> x.Id == expected.First().Id));
        }
    }
}