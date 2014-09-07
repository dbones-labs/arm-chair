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
    using System.Runtime.Serialization.Formatters;
    using Domain;
    using EntityManagement;
    using Http;
    using IdManagement;
    using InSession;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Processes.Load;
    using Processes.Update;
    using Serialization.Newton;
    using Tracking.Shadowing;

    [TestFixture]
    class Class1
    {
        [Test]
        public void Test1()
        {
            //externals
            var settings = new JsonSerializerSettings
            {
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new ContractResolver()
            };

            //global
            var idAccessor = new IdAccessor();
            var idManager = new ShortStringIdManager();
            var revisionAccessor = new RevisionAccessor();
            var serializer = new Serializer(settings, idAccessor, revisionAccessor);

            var database = new Database("test_db", new Connection("http://192.168.1.79:5984"), serializer);

            var loadPipeline = new LoadPipeline(database, idManager, idAccessor, revisionAccessor);
            var updatePipeline = new BulkPipeline(database, idManager, revisionAccessor);

            //session level
            var tracker = new ShadowTrackingProvider();
            var sessionCache = new SessionCache();

            var session = new Session(loadPipeline, updatePipeline, idManager, idAccessor, tracker, sessionCache);

            //var p = new Person("dave");
            //session.Add(p);

            //var book = new Book("title", p);
            //session.Add(book);

            var p = session.GetById<Book>("fKvKrp9I5U-rz5VWKo314A");


            //session.Commit();
        }

    }
}
