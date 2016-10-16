﻿// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using Domain;
    using Domain.Sample1;
    using NUnit.Framework;

    public class DeletingTests : TestCase
    {
        [Test]
        public void Simple_delete()
        {
            string id;
            using (var session = Database.CreateSession())
            {
                var author = new Person("dave");
                session.Add(author);
                id = author.Id;
                session.Commit();
            }


            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(id);
                session.Remove(author);
                
                session.Commit();
            }


            using (var session = Database.CreateSession())
            {
                var author = session.GetById<Person>(id);

                Assert.IsNull(author);
            }
        }
    }
}