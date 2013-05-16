namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class PagedListTests
    {
        //// ReSharper disable InconsistentNaming

        [SetUp]
        public void SetUp()
        {
            _source = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
        }


        [TearDown]
        public void TearDown()
        {
        }

        private int[] _source;
        private const int PageSize = 3;

        [TestCase(1, 2)]
        [TestCase(2, 5)]
        [TestCase(4, 9)]
        public void GetEndRecordIndex_ShouldCalculateZeroBasedPageEndIndex(int pageIndex, int endIndex)
        {
            var list = new PagedList<int>(_source, pageIndex, PageSize);
            list.GetEndRecordIndex().Should().Be(endIndex);
        }


        [TestCase(1, 0)]
        [TestCase(2, 3)]
        public void GetStartRecordIndex_ShouldCalculateZeroBasedPageStartIndex(int pageIndex, int startIndex)
        {
            var list = new PagedList<int>(_source, pageIndex, PageSize);
            list.GetStartRecordIndex().Should().Be(startIndex);
        }


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        //// ReSharper restore InconsistentNaming
    }
}