using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmChair.Castle;
using ArmChair.Tests.Domain;
using ArmChair.Utils.Copying;
using NUnit.Framework;
using ArmChair.Utils.Comparing;

namespace ArmChair.Tests
{
    public class CastleDocumentifyTests
    {
        [Test]
        public void SimpleCopy()
        {
            //var p = new Person("Dave") { Id = 1337 };
            //var sut = new Documentify();

            //var result = sut.AggregateRoot(p);
            //var document = (IDocument) result;
            //document.CouchDbId = "123";
            //document.CouchDbVersion = "1";

            //Assert.IsTrue(result is IDocument);
            //Assert.IsTrue(result is Person);
            //Assert.AreEqual(document.CouchDbId, "123");
            //Assert.AreEqual(document.CouchDbVersion, "1");

        }


        [Test]
        public void LittleMoreComplexCopy()
        {
            //var p = new Person("Dave") { Id = 1337 };
            //var p2 = new Person("Ben") { Id = 1332 };
            //var b = new Book("This is a test", p);
            //b.AddEdition(new Edition("10th year limited Edition", EditionType.HardBack) { ReleaseDate = DateTime.Now });
            //b.AddContributor(p2, ContributorType.Editor);
            //var sut = new Documentify();

            //var result = sut.AggregateRoot(b);
            //var document = (IDocument)result;
            //document.CouchDbId = "123";
            //document.CouchDbVersion = "1";

            //Assert.IsTrue(result is IDocument);
            //Assert.IsTrue(result is Book);
            //Assert.AreEqual(document.CouchDbId, "123");
            //Assert.AreEqual(document.CouchDbVersion, "1");
            
        }
    }


    [TestFixture]
    public class ShadowCopyTests
    {
        [Test]
        public void SimpleCopy()
        {
            var p = new Person("Dave") { Id = "1337" };
            var sut = new ShadowCopier();

            var result = sut.Copy(p);

            Assert.IsTrue(new Comparer().AreEqual(p, result));
        }


        [Test]
        public void LittleMoreComplexCopy()
        {
            var p = new Person("Dave") { Id = "1337" };
            var p2 = new Person("Ben") { Id = "1332" };
            var b = new Book("This is a test", p);
            b.AddEdition(new Edition("10th year limited Edition", EditionType.HardBack) { ReleaseDate = DateTime.Now });
            b.AddContributor(p2, ContributorType.Editor);


            var sut = new ShadowCopier();
            var result = sut.Copy(b);
            Assert.IsTrue(new Comparer().AreEqual(b, result));
        }
    }
}
