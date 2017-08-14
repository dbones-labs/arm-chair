namespace ArmChair.Tests.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using Domain.Sample3;
    using EntityManagement;
    using EntityManagement.Config;
    using NUnit.Framework;

    public class MappingTest : DataTestCase
    {
        [Test]
        public void Register_classMap()
        {
            //note this mappings are the only types which armchair will know about
            //that is when it queries, note that by convention it will still save any
            //document.
            Database.Register(new ClassMap[] {new CustomThingMap(), new PersonMap()});

            using (var session = Database.CreateSession())
            {
                session.Add(new CustomThing() {Name = "hmmmmmm"});
                session.Commit();
            }

            List<object> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<object>().ToList();
            }

            Assert.AreEqual(results.Count, Query<Person>().Count() + 1);
        }

        [Test]
        public void Register_type()
        {
            Database.Register(new[] {typeof(Person), typeof(CustomThing)});

            //as we are not using a class map
            Database.Settings.IdAccessor.SetUpId<CustomThing>(x => x.EpicId);
            Database.Settings.RevisionAccessor.SetUpRevision<CustomThing>(x => x.IAmARevision);

            using (var session = Database.CreateSession())
            {
                session.Add(new CustomThing() {Name = "hmmmmmm"});
                session.Commit();
            }

            List<object> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<object>().ToList();
            }

            Assert.AreEqual(results.Count, Query<Person>().Count() + 1);
        }


        /*
        [Test]
        public void Generated_index_names_should_be_the_same_every_time_they_are_generated()
        {
            var todoMappings = new List<TaskClassMap>();

            for (int i = 0; i < 100; i++)
            {
                var map = new TaskClassMap();
                todoMappings.Add(map);
            }

            var referenceMap = todoMappings[0];
            var value0 = referenceMap.Indexes.ElementAt(0).Name;
            var value1 = referenceMap.Indexes.ElementAt(1).Name;
            var value2 = referenceMap.Indexes.ElementAt(2).Name;

            //gevin lists are not garrenteed order, they normally are.
            Assert.IsTrue(todoMappings.All(x => x.Indexes.ElementAt(0).Name == "-1781928368"));
            Assert.IsTrue(todoMappings.All(x => x.Indexes.ElementAt(1).Name == "-223751922"));
            Assert.IsTrue(todoMappings.All(x => x.Indexes.ElementAt(2).Name == "771041031"));

        }
        */

    }
}