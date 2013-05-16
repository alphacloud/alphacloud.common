namespace Core.Tests.Reflection
{
    using System;
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class DelegateAdjusterTests
    {
        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }

        private class A
        {
        }


        private class B : A
        {
        }


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        [Test]
        public void Action_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Action<B> b = bp => bp.ToString();
            var upcast = DelegateAdjuster.CastArgument<A, B>(x => b(x));
            upcast(new B());

            Action baseTypePassed = () => upcast(new A());
            baseTypePassed.ShouldThrow<InvalidCastException>();
        }


        [Test]
        public void Function_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Func<B, string> fu = (B a) => a.ToString();

            var f = DelegateAdjuster.CastArgument<A, B, string>(x => fu(x));

            var result = f(new B());
            result.Should().Be(typeof (B).ToString());

            Action passA = () => result = f(new A());
            passA.ShouldThrow<InvalidCastException>();
        }
    }
}