namespace Core.Tests.Reflection
{
    using System;

    using Alphacloud.Common.Core.Reflection;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    class DelegateAdjusterTests
    {
        #region Tests

        [Test]
        public void Action_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Action<B> b = bp => bp.ToString();
            Action<A> upcast = DelegateAdjuster.CastArgument<A, B>(x => b(x));
            upcast(new B());

            Action baseTypePassed = () => upcast(new A());
            baseTypePassed.ShouldThrow<InvalidCastException>();
        }


        [Test]
        public void Function_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Func<B, string> fu = (B a) => a.ToString();

            Func<A, string> f = DelegateAdjuster.CastArgument<A, B, string>(x => fu(x));

            var result = f(new B());
            result.Should().Be(typeof (B).ToString());

            Action passA = () => result = f(new A());
            passA.ShouldThrow<InvalidCastException>();
        }

        #endregion

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {}


        [TearDown]
        public void TearDown()
        {}

        #endregion

        #region Helper methods

        class A
        {}


        class B : A
        {}


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {}


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {}

        #endregion
    }
}
