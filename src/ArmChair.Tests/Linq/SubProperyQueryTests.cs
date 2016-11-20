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


        [Ignore("in development")]
        [Test]
        public void Collection_object_property()
        {
            List<Book> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>().Where(x => x.Contributors.Any(y=> y.Name == "1st")).ToList();
            }

            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results.First().Id == "bk1");
        }
    }
}