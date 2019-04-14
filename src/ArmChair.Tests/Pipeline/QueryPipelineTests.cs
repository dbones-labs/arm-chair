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
namespace ArmChair.Tests.Pipeline
{
    using System.Linq;
    using Domain.Sample2;
    using Linq;
    using NUnit.Framework;
    using Processes.Query;

    public class QueryPipelineTests : QueryTestCase
    {
        [Test, Ignore("not supported")]
        public void Using_Postload_task()
        {
            var postTask = new PostLoadTask<QueryContext>();

            //Database.Settings.QueryPipeline.RegisterPostLoadTask(ctx => postTask);
            using (var session = Database.CreateSession())
            {
                var cats = session.Query<Cat>().ToList();
            }

            Assert.AreEqual(postTask.FakeCache.Count, 2);
        }
    }


}