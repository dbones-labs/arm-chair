// Copyright 2014 - dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ArmChair.Tests.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using EntityManagement.Config;
    using NUnit.Framework;
    using EntityManagement;

    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Id(x => x.Id);
            Revision(x => x.Rev);

            Index(idx => { idx.Field(x => x.Name); });

            Index(idx => { idx.Field(x => x.BirthDate); });
        }
    }

    public class CustomThingMap : ClassMap<CustomThing>
    {
        public CustomThingMap()
        {
            Id(x => x.EpicId);
            Revision(x => x.IAmARevision);
        }
    }

    public class CustomThing
    {
        public string EpicId { get; set; }
        public string IAmARevision { get; set; }
        public string Name { get; set; }
    }


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
    }
}