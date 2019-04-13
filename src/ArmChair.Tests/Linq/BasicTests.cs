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
    using Domain.Sample1;
    using NUnit.Framework;


    public class BasicTests : TestCase
    {
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
        
        [Test]
        public void load_item_twice_in_separate_sessions_it_be_the_same()
        {

            using (var session = Database.CreateSession())
            {   
                var person = new Person("chan");
                session.Add(person);
                session.Commit();
            }

            Person p1;
            Person p2;
            using (var session = Database.CreateSession())
            {
                p1 = session.Query<Person>().FirstOrDefault(x => x.Name == "chan");
                session.Commit();
            }
            
            using (var session = Database.CreateSession())
            {
                p2 = session.Query<Person>().FirstOrDefault(x => x.Name == "chan");
                session.Commit();
            }
            
            Assert.IsFalse(p1 == p2, "these are the same object but difference instances");
            Assert.AreEqual(p1.Name, p2.Name);
            Assert.AreEqual(p1.Id, p2.Id);
            Assert.AreEqual(p1.Rev, p2.Rev);
        }
        
    }
}