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
namespace ArmChair.Tests.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample1;
    using Exceptions;
    using NUnit.Framework;

    public class SavingTests : TestCase
    {
        [Test]
        public void Update_id_and_rev()
        {
            string id;
            string rev;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                id = author.Id;

                session.Commit();

                rev = author.Rev;
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(rev));
        }


        [Test]
        public void Many_saves()
        {
            var people = new List<Person>();
            var loadedPeople = new List<Person>();

            using (var session = Database.CreateSession())
            {
                for (int i = 0; i < 50; i++)
                {
                    var person = new Person("dave");
                    session.Add(person);
                    people.Add(person);
                }
                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                var temp = session.GetByIds<Person>(people.Select(x => x.Id));
                loadedPeople.AddRange(temp);
            }

            Assert.AreEqual(loadedPeople.Count, people.Count, "did not save/load all people");
            Assert.IsTrue(loadedPeople.All(x => people.Any(y => y.Id == x.Id)), "ids where not handled correctly");
            Assert.IsTrue(loadedPeople.All(x => people.Any(y => y.Rev == x.Rev)), "revisions where not handled correctly");
        }

        [Test]
        public void Save_nothing()
        {
            using (var session = Database.CreateSession())
            {
                session.Commit();
            }

            Assert.Pass();
        }
        
        
        [Test]
        public void Concurrancy_Load_and_save()
        {
            string id;
            string rev;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                id = author.Id;

                session.Commit();

                rev = author.Rev;
            }
            
            using (var session1 = Database.CreateSession())
            using (var session2 = Database.CreateSession())
            {
                var p1 = session1.GetById<Person>(id);
                var p2 = session2.GetById<Person>(id);
                var p3 = new Person("chan");
                session2.Add(p3);

                p1.Name = "bob";
                p2.Name = "bobby";
                
                session1.Commit();

                var result = Assert.Throws<BulkException>(() => session2.Commit(), "should have a conflict");
                Assert.IsTrue(result.Exceptions.Any());
                var ex = result.Exceptions.First();
                Assert.IsTrue(ex is ConflictException);
                Assert.AreEqual(ex.Id, id);
            }

        }

    }
}