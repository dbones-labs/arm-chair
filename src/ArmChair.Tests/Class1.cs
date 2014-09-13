// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
    using Http;
    using NUnit.Framework;

    [TestFixture]
    class Class1
    {
        [Test]
        public void Test1()
        {
            var db = new Database("test_db", new Connection("http://192.168.1.79:5984"));
            using (var session = db.CreateSession())
            {
                //var p = new Person("dave");
                //session.Add(p);

                //var book = new Book("title", p);
                //session.Add(book);

                var p = session.GetById<Book>("fKvKrp9I5U-rz5VWKo314A");


                //session.Commit();    
            }
            
        }


        [Test]
        public void Test2()
        {

            var db = new Database("test_db", new Connection("http://192.168.1.79:5984"));
            var session = db.CreateSession();
            var p = session.GetById<Book>("fKvKrp9I5U-rz5VWKo314A");


        }


    }
}
