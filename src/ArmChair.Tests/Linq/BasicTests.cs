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
namespace ArmChair.Tests.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using NUnit.Framework;


    public class BasicTests : TestCase
    {
        [Test]
        public void First_default_where_name_is_not_in_db()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.Query<Person>().FirstOrDefault(x => x.Name == "Dave");
                Assert.IsNull(person);
            }
        }

        [Test]
        public void Single_where_name_equals()
        {

            using (var session = Database.CreateSession())
            {
                for (int i = 0; i < 50; i++)
                {
                    var person = new Person("dave");
                    session.Add(person);
                }

                for (int i = 0; i < 5; i++)
                {
                    var person = new Person("chan");
                    person.Id = i.ToString();
                    session.Add(person);
                }

                session.Commit();
            }

            IList<Person> people;
            using (var session = Database.CreateSession())
            {
                people = session.Query<Person>().Where(x=> x.Name == "chan").ToList();
            }

            //must insure the order (this is part of the contract).
            Assert.IsTrue(people.Count() == 5);
            Assert.IsTrue(people.Any(x => x.Id == "0"));
            Assert.IsTrue(people.Any(x => x.Id == "1"));
            Assert.IsTrue(people.Any(x => x.Id == "2"));
            Assert.IsTrue(people.Any(x => x.Id == "3"));
            Assert.IsTrue(people.Any(x => x.Id == "4"));

        }

    }
}