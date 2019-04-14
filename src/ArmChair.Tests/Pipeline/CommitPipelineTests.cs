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
    using System.Collections.Generic;
    using Domain.Sample2;
    using InSession;
    using Linq;
    using NUnit.Framework;
    using Processes.Commit;
    using Tasks.BySingleItem;

    public class CommitPipelineTests: QueryTestCase
    {
        [Test, Ignore("not supported")]
        public void Using_Pre_task()
        {
            string id;
            //Database.Settings.CommitPipeline.RegisterPreCommitTask(ctx => new PreCommitTask());
            using (var session = Database.CreateSession())
            {
                var cat = new Cat
                {
                    Name = "whiskers",
                    RequiresHeatPad = true
                };
                session.Add(cat);
                id = cat.Id;

                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                var cat = session.GetById<Cat>(id);
                Assert.AreEqual("Kitty -> whiskers", cat.Name);
            }

        }

        [Test, Ignore("not supported")]
        public void Using_Post_task()
        {
            var postTask = new PostCommitTask();

            Cat update;
            Cat delete;
            Cat add;

            //Database.Settings.CommitPipeline.RegisterPreCommitTask(ctx => postTask);
            using (var session = Database.CreateSession())
            {
                update = new Cat
                {
                    Name = "whiskers",
                    RequiresHeatPad = true
                };
                delete = new Cat
                {
                    Name = "snowflake"
                };

                session.Add(update);
                session.Add(delete);
                session.Commit();
            }

            using (var session = Database.CreateSession())
            {
                session.Attach(update);
                session.Attach(delete);

                add = new Cat()
                {
                    Name = "pumpkin"
                };
                update.RequiresHeatPad = false;

                session.Add(add);
                session.Remove(delete);

                session.Commit();
            }

            Assert.AreEqual(postTask.FakeIndex.Count, 2);
            Assert.AreEqual(postTask.FakeIndex["pumpkin"], add.Id);
            Assert.AreEqual(postTask.FakeIndex["whiskers"], update.Id);

        }
    }


    public class PostCommitTask : TaskOnItem<CommitContext>
    {
        public IDictionary<string, string> FakeIndex = new Dictionary<string, string>();

        public override CommitContext Execute(CommitContext item)
        {
            var cat = item.Entity as Cat;
            if (cat == null)
            {
                return item;
            }

            switch (item.ActionType)
            {
                case ActionType.Add:
                    FakeIndex.Add(cat.Name, item.Key.Id.ToString());
                    break;
                case ActionType.Update:
                //    FakeIndex[cat.Name] = item.Key.Id.ToString();
                    break;
                default:
                    FakeIndex.Remove(cat.Name);
                    break;
            }

            return item;
        }
    }





    public class PreCommitTask : TaskOnItem<CommitContext>
    {
        public override CommitContext Execute(CommitContext item)
        {
            var cat = item.Entity as Cat;
            if (cat == null) return item;

            cat.Name = $"Kitty -> {cat.Name}";
            return item;
        }
    }
}