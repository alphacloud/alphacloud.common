#region copyright

// Copyright 2013-2016 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.BoxingAllocation
namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    class PagedListTests
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


        int[] _source;
        const int PageSize = 3;


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


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }


        //// ReSharper restore InconsistentNaming
    }
}