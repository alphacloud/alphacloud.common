#region copyright

// Copyright 2013 Alphacloud.Net
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
namespace Core.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    [TestFixture]
    class CollectionExtensionsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }


        [Test]
        public void CreateDictionary_KeepLastDuplicate_Should_KeepLastValueForDuplicateKey()
        {
            var dups = new[] {
                new KeyValuePair<int, string>(1, "First"),
                new KeyValuePair<int, string>(1, "Last")
            };

            var res = dups.CreateDictionary(k => k.Key, v => v.Value, DuplicateKeyHandling.KeepLast);
            res[1].Should().Be("Last");
        }


        [Test]
        public void CreateDictionary_Should_CreateDictionaryFromSourceSequence()
        {
            var seq = new[] {1, 2, 3};

            var dic = seq.CreateDictionary(k => k, v => v.ToString(CultureInfo.InvariantCulture),
                DuplicateKeyHandling.KeepLast);

            dic.Should().ContainKeys(1, 2, 3);
            dic.Should().ContainValues("1", "2", "3");
        }


        [Test]
        public void CreateDictonary_ErrorOnDuplicateKey_Should_FailIfDuplicateKeyExists()
        {
            var dups = new[] {
                new KeyValuePair<int, string>(1, "First"),
                new KeyValuePair<int, string>(1, "Last")
            };

            Action a = () => dups.CreateDictionary(k => k.Key, v => v.Value, DuplicateKeyHandling.Error);

            a.ShouldThrow<ArgumentException>();
        }


        [Test]
        public void CreateDictonary_KeepFirstDuplicate_Should_KeepValueOfFirstDuplicateKey()
        {
            var dups = new[] {
                new KeyValuePair<int, string>(1, "First"),
                new KeyValuePair<int, string>(1, "Last")
            };

            var res = dups.CreateDictionary(k => k.Key, v => v.Value, DuplicateKeyHandling.KeepFirst);
            res[1].Should().Be("First");
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