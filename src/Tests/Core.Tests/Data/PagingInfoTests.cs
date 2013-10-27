namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

//// ReSharper disable InconsistentNaming

    [TestFixture]
    internal class PagingInfoTests
    {
        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }

        [TestCase(0, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        public void GetPageNumberForItem_ShouldReturnPageNumberByItemIndex(int itemIndex, int expectedPageNumber)
        {
            new PagingInfo(3).GetPageNumberByIndex(itemIndex).Should().Be(expectedPageNumber);
        }


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }
    }


//// ReSharper restore InconsistentNaming
}