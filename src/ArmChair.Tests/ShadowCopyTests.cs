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
    using System;
    using Domain;
    using Utils.Copying;
    using NUnit.Framework;
    using Utils.Comparing;


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
