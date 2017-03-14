namespace ArmChair.Tests.Linq
{
    using ArmChair.Tests.Domain.Sample1;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DateTests : QueryTestCase
    {

        [Test]
        public void Filter_greater_than()
        {
            List<Person> results;
            List<Person> expected;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Person>().Where(x => x.BirthDate > new DateTime(1980, 1, 1)).ToList();
                expected = Query<Person>().Where(x => x.BirthDate > new DateTime(1980, 1, 1)).ToList();
            }

            Assert.IsTrue(results.Count == expected.Count);
            Assert.IsTrue(results.Any(x => x.Id == expected.First().Id));
        }


    }
}
