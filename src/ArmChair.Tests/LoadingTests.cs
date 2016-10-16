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
namespace ArmChair.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using NUnit.Framework;

    public class LoadingTests : TestCase
    {
        [Test]
        public void Load_object_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.GetById<Person>("123");
                Assert.IsNull(person);
            }
        }

        [Test]
        public void Load_objects_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var ids = new List<string> {"123", "12323123"};
                var people = session.GetByIds<Person>(ids);
                Assert.IsFalse(people.Any());
            }
        }


        [Test]
        public void Object_which_does_not_exist_with_some_that_do_exist()
        {

            using (var session = Database.CreateSession())
            {
                for (int i = 0; i < 50; i++)
                {
                    var person = new Person("dave");
                    person.Id = i.ToString();
                    session.Add(person);
                }
                session.Commit();
            }

            IList<Person> people;
            using (var session = Database.CreateSession())
            {
                var ids = new List<string> { "2", "12323123", "3", "1", "hello-world" };
                people = session.GetByIds<Person>(ids).ToList();
            }

            //must insure the order (this is part of the contract).
            Assert.IsTrue(people.Count() == 3);
            Assert.IsTrue(people[0].Id == "2");
            Assert.IsTrue(people[1].Id == "3");
            Assert.IsTrue(people[2].Id == "1");
        }


        [Test]
        public void Handle_empty_list()
        {
            using (var session = Database.CreateSession())
            {
                var ids = new List<string>();
                var people = session.GetByIds<Person>(ids);
                Assert.IsFalse(people.Any());
            }
        }
    }
}