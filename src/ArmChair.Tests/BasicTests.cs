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
    using System;
    using Domain;
    using NUnit.Framework;

    public class BasicTests : TestCase
    {
        [Test]
        public void Save_into_database()
        {
            string authorId;

            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);

                authorId = author.Id;

                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(authorId);

                Assert.AreEqual(author.Name, "dave");
            }
        }

    }
}