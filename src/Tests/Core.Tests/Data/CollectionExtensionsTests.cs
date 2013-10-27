namespace Core.Tests.Data
{
    using System.Collections.Generic;
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    [TestFixture]
    internal class CollectionExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
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
        public void RemoveFirst_MatchFound_ShouldRemove_FirstOccurence()
        {
            var l = new List<int> {1, 2, 3, 1};
            l.RemoveFirst(i => i == 1).Should().BeTrue();
            l.Count.Should().Be(3);
            l[2].Should().Be(1);
        }


        [Test]
        public void RemoveFirst_MatchNotFound_ShouldReturnFalse()
        {
            var l = new List<int> {1, 2, 3};
            l.RemoveFirst(i => i == 5).Should().BeFalse();
        }


        [Test]
        public void TakePage_ShouldTakeSpecifiedPage()
        {
            var seq = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var res = seq.TakePage(1, 3);
            res.Should().BeEquivalentTo(new[] {1, 2, 3});
        }
    }


    //// ReSharper restore InconsistentNaming
}