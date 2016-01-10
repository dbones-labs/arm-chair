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
        public void Try_load_object_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var person = session.GetById<Person>("123");
                Assert.IsNull(person);
            }
        }

        [Test]
        public void Try_load_objects_which_does_not_exist()
        {
            using (var session = Database.CreateSession())
            {
                var ids = new List<string> {"123", "12323123"};
                var people = session.GetByIds<Person>(ids);
                Assert.IsFalse(people.Any());
            }
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