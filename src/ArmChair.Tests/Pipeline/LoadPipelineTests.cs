namespace ArmChair.Tests.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Sample2;
    using Linq;
    using NUnit.Framework;
    using Processes.Load;
    using Tasks.BySingleItem;

    public class LoadPipelineTests : QueryTestCase
    {
        [Test]
        public void Using_Preload_task()
        {
            IEnumerable<Cat> results;
            Database.Settings.LoadPipeline.RegisterPreLoadTask(ctx => new PreLoadTask());
            using (var session = Database.CreateSession())
            {
                results = session.GetByIds<Cat>(new[] {"c1", "c2"});
            }

            Assert.AreEqual(results.Count(), 1, "should have filted out c2");
            Assert.AreEqual(results.ElementAt(0).Id, "c1");
        }

        [Test]
        public void Using_Postload_task()
        {
            var postTask = new PostLoadTask<LoadContext>();

            Database.Settings.LoadPipeline.RegisterPostLoadTask(ctx => postTask);
            using (var session = Database.CreateSession())
            {
                session.GetByIds<Cat>(new[] {"c1", "c2"});
                session.GetByIds<Cat>(new[] {"c1", "c2"});
            }

            Assert.AreEqual(postTask.FakeCache.Count, 2);
        }
    }


    /// <summary>
    /// NOTE this can be used with both the load and query pipelines.
    /// </summary>
    public class PostLoadTask<T> : TaskOnItem<T> where T : LoadContext
    {
        public IList<string> FakeCache = new List<string>();

        public override T Execute(T item)
        {
            if (!item.LoadedFromCache)
            {
                FakeCache.Add(item.Key.Id.ToString());
            }
            return item;
        }
    }

    public class PreLoadTask : TaskOnItem<LoadContext>
    {
        public override LoadContext Execute(LoadContext item, Action skip)
        {
            if (item.Key.Id == "c2")
            {
                skip();
                return null;
            }
            return item;
        }
    }
}