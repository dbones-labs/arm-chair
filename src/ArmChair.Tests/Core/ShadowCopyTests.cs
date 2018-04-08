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
namespace ArmChair.Tests.Core
{
    using System;
    using Domain.Sample1;
    using Domain.Sample2;
    using NUnit.Framework;
    using Utils.Comparing;
    using Utils.Copying;

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
            var b = new Book("This is a test", p, 170);
            b.AddEdition(new Edition("10th year limited Edition", EditionType.HardBack) { ReleaseDate = DateTime.Now });
            b.AddContributor(p2, ContributorType.Editor);


            var sut = new ShadowCopier();

            var result = sut.Copy(b);

            Assert.IsTrue(new Comparer().AreEqual(b, result));
        }

        [Test]
        public void Poly_object_initial_copy_test()
        {
            var booking = new KennelBooking() { Id = "123" };
            var cat = new Cat() { Name = "Whiskers", RequiresHeatPad = true };
            booking.Animal = cat;

            var sut = new ShadowCopier();

            var result = sut.Copy(booking);

            Assert.IsTrue(new Comparer().AreEqual(booking, result));
        }


        [Test]
        public void Poly_object_copy_2_objects_test()
        {
            var catBooking = new KennelBooking() { Id = "123" };
            var cat = new Cat() { Name = "Whiskers", RequiresHeatPad = true };
            catBooking.Animal = cat;

            var dogBooking = new KennelBooking() { Id = "123" };
            var dog = new Dog() { Name = "Skippy", NumberOfWalksPerDay = 2};
            dogBooking.Animal = dog;

            var sut = new ShadowCopier();

            var result1 = sut.Copy(catBooking);
            var result2 = sut.Copy(dogBooking);

            Assert.IsTrue(new Comparer().AreEqual(catBooking, result1));
            Assert.IsTrue(new Comparer().AreEqual(dogBooking, result2));
            Assert.IsFalse(new Comparer().AreEqual(catBooking, result2));
        }



        [Test]
        public void Poly_object_post_change_test()
        {
            var booking = new KennelBooking() { Id = "123" };
            var cat = new Cat() { Name = "Whiskers", RequiresHeatPad = true };
            booking.Animal = cat;

            var sut = new ShadowCopier();

            var result = sut.Copy(booking);
            cat.RequiresHeatPad = false;

            Assert.IsFalse(new Comparer().AreEqual(booking, result));
        }


    }
}
