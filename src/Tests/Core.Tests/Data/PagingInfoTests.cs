namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;

    using FluentAssertions;

    using NUnit.Framework;

//// ReSharper disable InconsistentNaming

    [TestFixture]
    class PagingInfoTests
    {

        #region Data


        #endregion

        #region Tests

        [TestCase(0, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        public void GetPageNumberForItem_ShouldReturnPageNumberByItemIndex(int itemIndex, int expectedPageNumber)
        {
            new PagingInfo(3).GetPageNumberByIndex(itemIndex).Should().Be(expectedPageNumber);
        }


        #endregion

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {}


        [TearDown]
        public void TearDown()
        {}


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {}


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {}

        #endregion

    }


//// ReSharper restore InconsistentNaming
}
