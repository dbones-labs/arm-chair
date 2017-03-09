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
namespace ArmChair.Tests.Mongo
{
    using System.Collections.Generic;
    using System.Linq;
    using ArmChair.Linq;
    using Domain.Sample1;
    using Newtonsoft.Json;
    using NUnit.Framework;

    public class BasicTests : DataTestCase
    {
        [Test]
        public void Find_book_by_edition_using_queryobject()
        {
            List<Book> results;
            List<Book> expected;
            using (var session = Database.CreateSession())
            {
                results = session.Query<Book>(new MongoQuery()
                {
                    Selector = new QueryObject()
                    {{
                        "Editions", new QueryObject()
                        {{
                            "$elemMatch", new QueryObject()
                            {{
                                "Name", new QueryObject()
                                {{
                                    "$eq", "2nd"
                                }}
                            }}
                        }}
                    }}
                }).ToList();
                expected = Query<Book>().Where(x => x.Editions.Any(y => y.Name == "2nd")).ToList();
            }

            Assert.IsTrue(results.Count == expected.Count);
            Assert.IsTrue(results.Any(x => x.Id == expected.First().Id));
        }


        [Test]
        public void Find_book_by_edition_using_json()
        {
            List<Book> results;
            List<Book> expected;
            using (var session = Database.CreateSession())
            {
                var json = "{ \n" +
                           "    \"editions\": { \n" +
                           "        \"$elemMatch\": { \n" +
                           "            \"name\": { \n" +
                           "                \"$eq\": \"2nd\" \n" +
                           "            } \n" +
                           "        } \n" +
                           "    } \n" +
                           "} \n";

                //note the camel case. (as we do no use the IDictionary at each level on the deseialise)
                var queryObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                results = session.Query<Book>(new MongoQuery()
                {
                    Selector = queryObject
                }).ToList();
                expected = Query<Book>().Where(x => x.Editions.Any(y => y.Name == "2nd")).ToList();
            }

            Assert.IsTrue(results.Count == expected.Count);
            Assert.IsTrue(results.Any(x => x.Id == expected.First().Id));
        }

        [Test]
        public void Skip_and_Take()
        {
            List<object> results;
            using (var session = Database.CreateSession())
            {
                results = session.Query<object>(new MongoQuery()
                {
                    Selector = new QueryObject(),
                    Skip = 5,
                    Limit = 3
                }).ToList();
            }

            Assert.IsTrue(results.Count == 3);
        }


    }
}